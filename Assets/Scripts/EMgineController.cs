using UnityEngine;
using AgentSimulation;
using EMgine;
using static AgentSimulation.ProtoWorldStateViewT;

public class EMgineController : MonoBehaviour
{
    // Displaying Intensity Values in Unity
    public double JoyIntensityValue = 0.0d;
    public double SadnessIntensityValue = 0.0d;
    public double FearIntensityValue = 0.0d;
    public double AngerIntensityValue = 0.0d;
    public double DisgustIntensityValue = 0.0d;
    public double AcceptanceIntensityValue = 0.0d;
    public double SurpriseIntensityValue = 0.0d;



    // EMgine joy example initial setup
    static AgentSimItemSlot full_slot = new AgentSimItemSlot(true);
    static AgentSimItemSlot empty_slot = new AgentSimItemSlot(false);

    // goal is to have fill world
    static AgentSimItemSlot[] goal_inventory_state =
            new AgentSimItemSlot[] { full_slot, full_slot, full_slot };

    static ProtoWorldStateViewT goal_World =
        new ProtoWorldStateViewT(goal_inventory_state);
    static GoalT<AgentSimItemSlot> goal =
        new GoalT<AgentSimItemSlot>(goal_World, 1.0d);
    static ProtoWorldStateViewT currentWorld =
        new ProtoWorldStateViewT(new AgentSimItemSlot[]
        { empty_slot, empty_slot, empty_slot });

    // 0 distance threshold
    static ProtoDistanceChangeCausedByEvent epsilonJ =
        new ProtoDistanceChangeCausedByEvent(
            new ProtoWorldEventT(new int[] { 0, 0, 0 }));

    static EmIntensityT joyIntensity = new EmIntensityT(0.0d);
    static (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distPrev,
        WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT distNow,
        WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT distDelta)?
            joy;

    // animation 
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Current Joy intensity: " + JoyIntensityValue);
    }


    public void UpdateEMgine(int[] updateEvent)
    {
        ProtoWorldEventT inventoryChange = new ProtoWorldEventT(updateEvent);
        joy = EmGenFunLib<AgentSimItemSlot>.GenerateJoy(goal,
            currentWorld, inventoryChange, epsilonJ); // calculating joy

        if (joy != null)  // if passes all the threshold conditions
        {
            (WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT
                    previousDistance,
                WorldStateViewT<AgentSimItemSlot>.DistBtwWorldStateViewsT
                    newDistance,
                WorldStateViewT<AgentSimItemSlot>.DistChgBtwWorldStateViewsT
                    distanceChange) = joy ?? default;

            EmIntensityT.EmIntensityChgT intensityDelta =
                EmIntensityCalcLib<AgentSimItemSlot>.CalcJoyIntensity(goal,
                    distanceChange);

            // update and overwrite the value if changed
            joyIntensity = joyIntensity.UpdateWithChg(intensityDelta);

            // updates the animator and display
            animator.SetFloat("Joy Intesnity",
                (float)joyIntensity.ToReal());
            JoyIntensityValue = joyIntensity.ToReal();
            Debug.Log("Current Joy intensity: " + JoyIntensityValue);

        }

        // updating the current world
        currentWorld = (ProtoWorldStateViewT)currentWorld.ApplyEvent(
               inventoryChange);
    }
}