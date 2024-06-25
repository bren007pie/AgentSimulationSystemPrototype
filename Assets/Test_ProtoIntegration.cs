using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AgentSimulation;
using EMgine;
using static AgentSimulation.ProtoWorldStateViewT;
using System.Xml.Linq;

public class Test_ProtoIntegration
{
    // repeating variables
    static AgentSimItemSlot full_slot = new AgentSimItemSlot(true);
    static AgentSimItemSlot empty_slot = new AgentSimItemSlot(false);

    static AgentSimItemSlot[] goal_inventory_state = new AgentSimItemSlot[] { full_slot, full_slot, full_slot };
    static ProtoWorldStateViewT goal_World = new ProtoWorldStateViewT(goal_inventory_state);
    static GoalT<AgentSimItemSlot> goal = new GoalT<AgentSimItemSlot>(goal_World, 1.0d);
    static ProtoWorldStateViewT currentWorld = new ProtoWorldStateViewT(new AgentSimItemSlot[] { full_slot, empty_slot, empty_slot });


    [Test]
    public void Make_a_Goal()
    {
        

        Assert.That(goal.GetGoalState().Equals(goal_World));
        Assert.That(goal.IsSatisfiedBy(goal_World));

        ProtoDistanceBetweenWorldStates dist_1 = (ProtoDistanceBetweenWorldStates)goal.Dist2Goal(goal_World); // 0
        ProtoWorldEventT remove_event = new ProtoWorldEventT(new int[] { -1, -1, -1 }); // -3
        ProtoDistanceChangeCausedByEvent dist_2 = (ProtoDistanceChangeCausedByEvent)goal.Dist2GoalChg(goal_World, remove_event); // -3

        Assert.That(dist_1.Equals(new ProtoDistanceBetweenWorldStates(goal_World, goal_World)));
        Assert.AreEqual(dist_2.ToReal(), 3.0d);
    }

    [Test]
    public void CalcJoyIntesnity()
    {
        // okay, in this example the intensity goes up when we add something and make it closer to the goal
        ProtoWorldEventT event1 = new ProtoWorldEventT(new int[]{ 0, 1, 0 });

        ProtoDistanceChangeCausedByEvent dDelta1 = (ProtoDistanceChangeCausedByEvent)currentWorld.CalcChangeCausedBy(event1);

        EmIntensityT.EmIntensityChgT intensityDelta1 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, dDelta1);
        // DΔ = change the game world event makes to the world

        EmIntensityT joyIntensity = new EmIntensityT(1.0d); // 1 seems to be a good base value to add or subtract joy intensity
        EmIntensityT joyIntensityPrime1 =  joyIntensity.UpdateWithChg(intensityDelta1);

        Assert.AreEqual(joyIntensityPrime1.ToReal(), 1.1d);

        // Joy only goes up, removing something only makes it further. 
        ProtoWorldEventT event2 = new ProtoWorldEventT(new int[] { -1, 0, 0 });
        ProtoDistanceChangeCausedByEvent dDelta2 = (ProtoDistanceChangeCausedByEvent)currentWorld.CalcChangeCausedBy(event2);
        EmIntensityT.EmIntensityChgT intensityDelta2 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, dDelta2);
        EmIntensityT joyIntensityPrime2 = joyIntensity.UpdateWithChg(intensityDelta2);

        Assert.AreEqual(joyIntensityPrime2.ToReal(), 1.1d);
    }

    [Test]
    public void GenerateJoy()
    {
        ProtoWorldStateViewT noItems = new ProtoWorldStateViewT(new AgentSimItemSlot[] { empty_slot, empty_slot, empty_slot });
        ProtoWorldEventT event1 = new ProtoWorldEventT(new int[] { 0, 0, 0 });
        ProtoWorldEventT event2 = new ProtoWorldEventT(new int[] { 0, 1, 0 });
        ProtoWorldEventT event3 = new ProtoWorldEventT(new int[] { 1, 1, 0 });
        ProtoWorldEventT event4 = new ProtoWorldEventT(new int[] { 1, 1, 1 });
        /// < param name = "epsilonJ" > Threshold value, 0 distance.</param>
        ProtoDistanceChangeCausedByEvent epsilonJ = new ProtoDistanceChangeCausedByEvent(event1); // 0 distance threshold

        EmIntensityT joyIntensity = new EmIntensityT(0.0d);
        EmIntensityT joyIntensity1Prime = new EmIntensityT(0.0d);
        EmIntensityT joyIntensity2Prime = new EmIntensityT(0.0d);
        EmIntensityT joyIntensity3Prime = new EmIntensityT(0.0d);
        EmIntensityT joyIntensity4Prime = new EmIntensityT(0.0d);


        // Use the GenerateJoy function from EmGenFunLib then feed in the Ddelta (aka 3rd tuple value goalDistChg)
        // epsilonJ is a threshold distance to see how far to elicit Joy, maybe should just be zero to trigger for now?
        (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distNow, WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distDelta)? joy1;
        (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distNow, WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distDelta)? joy2;
        (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distNow, WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distDelta)? joy3;
        (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distNow, WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distDelta)? joy4;


        joy1 = EmGenFunLib<AgentSimItemSlot>.GenerateJoy(goal, noItems, event1, epsilonJ);
        joy2 = EmGenFunLib<AgentSimItemSlot>.GenerateJoy(goal, noItems, event2, epsilonJ);
        joy3 = EmGenFunLib<AgentSimItemSlot>.GenerateJoy(goal, noItems, event3, epsilonJ);
        joy4 = EmGenFunLib<AgentSimItemSlot>.GenerateJoy(goal, noItems, event4, epsilonJ);


        // the way to make this into a loop (without a nullable value) is to overwrite the value of joy
        if (joy1 != null)  // if passes all the threshold conditions
        {
            (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT previousDistance,
                WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT newDistance,
                WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distanceChange) = joy1 ?? default;

            EmIntensityT.EmIntensityChgT intensityDelta1 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, distanceChange);
            joyIntensity1Prime = joyIntensity.UpdateWithChg(intensityDelta1); // update the value if changed

        }
        Assert.AreEqual(joy1, null);

        if (joy2 != null)  // if passes all the threshold conditions
        {
            (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT previousDistance,
                WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT newDistance,
                WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distanceChange) = joy2 ?? default;

            EmIntensityT.EmIntensityChgT intensityDelta2 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, distanceChange);
            joyIntensity2Prime = joyIntensity.UpdateWithChg(intensityDelta2); // update the value if changed

            (previousDistance, newDistance, distanceChange) = joy3 ?? default;
            EmIntensityT.EmIntensityChgT intensityDelta3 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, distanceChange);
            joyIntensity3Prime = joyIntensity.UpdateWithChg(intensityDelta3); // update the value if changed

            (previousDistance, newDistance, distanceChange) = joy4 ?? default;
            EmIntensityT.EmIntensityChgT intensityDelta4 = EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal, distanceChange);
            joyIntensity4Prime = joyIntensity.UpdateWithChg(intensityDelta4); // update the value if changed


        }

        Assert.AreEqual(joyIntensity.ToReal(), 0.0d);
        Assert.AreEqual(joyIntensity1Prime.ToReal(), 0.0d);
        Assert.That(joyIntensity2Prime.ToReal() > 1);
        Assert.That(joyIntensity3Prime.ToReal() > 2);
        Assert.That(joyIntensity4Prime.ToReal() > 3);

    }



}