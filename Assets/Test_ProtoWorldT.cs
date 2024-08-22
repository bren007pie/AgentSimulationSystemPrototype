using System;
using NUnit.Framework;
using UnityEngine;
using AgentSimulation;
using static AgentSimulation.ProtoWorldStateViewT;


public class Test_ProtoWorldT
{
    // let's make an inventory and try to fill a world with an inventory
    readonly AgentSimItemSlot full_slot = new AgentSimItemSlot(true);
    readonly AgentSimItemSlot empty_slot = new AgentSimItemSlot(false);


    [Test]
    public void TestMakeInventories()
    {
        // Making Boolean item slots (whether has the inventory or not)
        AgentSimItemSlot item_slot1 = new AgentSimItemSlot(true);
        AgentSimItemSlot item_slot2 = new AgentSimItemSlot(false);
        AgentSimItemSlot item_slot3 = new AgentSimItemSlot(false);

        Assert.That(item_slot1.Equals(true));
        Assert.That(item_slot2.Equals(false));
        Assert.That(item_slot3.Equals(false));

        // making base inventory
        AgentSimItemSlot[] inventory_state = new AgentSimItemSlot[] { item_slot1, item_slot2, item_slot3 };

        // making base world object
        ProtoWorldStateViewT World = new ProtoWorldStateViewT(inventory_state);

        Assert.That(World.Equals(World));

        // Making new inventory
        AgentSimItemSlot[] goal_inventory_state = new AgentSimItemSlot[] { item_slot1, item_slot1, item_slot1 };

        ProtoWorldStateViewT goal_World = new ProtoWorldStateViewT(goal_inventory_state);

        Assert.False(World.Equals(goal_World));


    }

    [Test]
    public void ApplyWorldEvent()
    {

        // making base inventory
        AgentSimItemSlot[] inventory_state = new AgentSimItemSlot[]
              { new AgentSimItemSlot(true),
                new AgentSimItemSlot(false),
                new AgentSimItemSlot(false) };

        ProtoWorldStateViewT World = new ProtoWorldStateViewT(inventory_state);

        int[] inventory_change_state = new int[] { 0, 1, 1 };

        ProtoWorldStateViewT.ProtoWorldEventT inventory_change_event =
            new ProtoWorldStateViewT.ProtoWorldEventT(inventory_change_state);

        ProtoWorldStateViewT new_World = (ProtoWorldStateViewT)World.ApplyEvent(inventory_change_event);

        AgentSimItemSlot[] goal_State = new AgentSimItemSlot[]
                { new AgentSimItemSlot(true),
                new AgentSimItemSlot(true),
                new AgentSimItemSlot(true) };

        ProtoWorldStateViewT goal_World =
            new ProtoWorldStateViewT(goal_State);

        Assert.That(new_World.Equals(goal_World));

    }

    [Test]
    public void ApplyWorldEvent_TestAllCases()
    {

        // Testing all six cases of the case statement + null + wrong size
        //Switch (Slot value, change)
        //1. (true, 0) => true
        //2. (false, 0) => false
        //3. (true, 1) => Error, adding to full slot
        //4. (true, -1) => false
        //5. (false, 1) => true
        //6. (false, -1) => Error, removing from empty slot
        //7. size 1 world + size 2 event => Error, wrong world size
        //8. 0 size world + 0 sized event => 0 sized world

        // Testing non-error cases 1,2,4,5
        AgentSimItemSlot[] inventory_state_1 = new AgentSimItemSlot[]
            { full_slot, empty_slot, full_slot, empty_slot }; // [T,F,T,F]

        AgentSimItemSlot[] inventory_state_1_post_event_check =
            new AgentSimItemSlot[]
            { full_slot, empty_slot, empty_slot, full_slot}; // [T,F,F,T]

        ProtoWorldStateViewT World_state_1 =
            new ProtoWorldStateViewT(inventory_state_1);

        ProtoWorldStateViewT World_state_1_post_event_check =
            new ProtoWorldStateViewT(inventory_state_1_post_event_check);

        int[] state_change_1 = new int[] { 0, 0, -1, 1 };
        ProtoWorldEventT event_1 = new ProtoWorldEventT(state_change_1);

        ProtoWorldStateViewT new_World_state_1 =
            (ProtoWorldStateViewT)World_state_1.ApplyEvent(event_1);

        Assert.That(new_World_state_1.Equals(World_state_1_post_event_check));

        // Testing two error cases 3,6
        AgentSimItemSlot[] inventory_state_2 =
            new AgentSimItemSlot[] { full_slot };
        ProtoWorldStateViewT World_state_2 =
            new ProtoWorldStateViewT(inventory_state_2);
        int[] state_change_2 = new int[] { 1 };
        ProtoWorldEventT event_2 =
            new ProtoWorldEventT(state_change_2);

        // Using ananomys delegate to check exceptions because
        // it makes sense to me to run code blocks
        // https://docs.nunit.org/articles/nunit/writing-tests/assertions/classic-assertions/Assert.Throws.html
        InvalidOperationException exception_1 =
            Assert.Throws<InvalidOperationException>
                (delegate { World_state_2.ApplyEvent(event_2); });
        Debug.Log(exception_1);

        AgentSimItemSlot[] inventory_state_3 =
            new AgentSimItemSlot[] { empty_slot };
        ProtoWorldStateViewT World_state_3 =
            new ProtoWorldStateViewT(inventory_state_3);
        int[] state_change_3 = new int[] { -1 };
        ProtoWorldEventT event_3 =
            new ProtoWorldEventT(state_change_3);

        InvalidOperationException exception_2 =
            Assert.Throws<InvalidOperationException>
                (delegate { World_state_3.ApplyEvent(event_3); });
        Debug.Log(exception_2);


        // Testing Wrong GetSize case 7
        AgentSimItemSlot[] inventory_state_4 =
            new AgentSimItemSlot[] { empty_slot, full_slot };
        ProtoWorldStateViewT World_state_4 =
            new ProtoWorldStateViewT(inventory_state_4);
        int[] state_change_4 = new int[] { 1 };
        ProtoWorldEventT event_4 = new ProtoWorldEventT(state_change_4);

        IndexOutOfRangeException exception_3 =
            Assert.Throws<IndexOutOfRangeException>
               (delegate { World_state_4.ApplyEvent(event_4); });
        Debug.Log(exception_3);

        // Testing null world/event case 8
        AgentSimItemSlot[] inventory_state_5 =
            new AgentSimItemSlot[] { };
        ProtoWorldStateViewT World_state_5 =
            new ProtoWorldStateViewT(inventory_state_5);
        int[] state_change_5 = new int[] { };
        ProtoWorldEventT event_5 = new ProtoWorldEventT(state_change_5);

        // world state is equal to itself post
        // update because empty world and event
        Assert.That(World_state_5.Equals(World_state_5.ApplyEvent(event_5)));

    }

    [Test]
    public void TestWorldDistanceChange()
    {
        AgentSimItemSlot[] inventory_1 = new AgentSimItemSlot[] { full_slot, empty_slot, empty_slot };
        ProtoWorldStateViewT world_1 = new ProtoWorldStateViewT(inventory_1);

        // make to goal inventory
        ProtoWorldEventT event_1 = new ProtoWorldEventT(new int[] { 0, 1, 1 });
        // remove current item
        ProtoWorldEventT event_2 = new ProtoWorldEventT(new int[] { -1, 0, 0 });
        // remove and add
        ProtoWorldEventT event_3 = new ProtoWorldEventT(new int[] { -1, 0, 1 });
        // no change
        ProtoWorldEventT event_4 = new ProtoWorldEventT(new int[] { 0, 0, 0 });

        // checking the changes_distances sum to the right values
        ProtoDistanceChangeCausedByEvent change_1 = (ProtoDistanceChangeCausedByEvent) world_1.CalcChangeCausedBy(event_1); // 2, positive 
        ProtoDistanceChangeCausedByEvent change_2 = (ProtoDistanceChangeCausedByEvent) world_1.CalcChangeCausedBy(event_2); // -1, negative
        ProtoDistanceChangeCausedByEvent change_3 = (ProtoDistanceChangeCausedByEvent) world_1.CalcChangeCausedBy(event_3); // 0, zero sum
        ProtoDistanceChangeCausedByEvent change_4 = (ProtoDistanceChangeCausedByEvent) world_1.CalcChangeCausedBy(event_4); // 0 zero


        Assert.AreEqual(change_1.ToReal(), 2.0d); // d suffix for double
        Assert.AreEqual(change_2.ToReal(), -1.0d);
        Assert.AreEqual(change_3.ToReal(), 0.0d);
        Assert.AreEqual(change_4.ToReal(), 0.0d);

        // checking the changes_distances difference method works
        // There are 6 subtraction cases to check
        //  (but are absolute values so doesn't turn negative anymore)
        // 1. positive - 0 = positive
        // 2. postive - positive = 0
        // 3. 0 - positive = negative
        // 4. 0 - 0 = 0
        // 5. big positive - small positive = small positive, e.g. 2 - 1 = 1
        // 6. small positive - big positive = small negative, e.g. 1 - 2 = -1

        // 2 - 0 = 2 
        ProtoDistanceChangeCausedByEvent change_diff_1 = (ProtoDistanceChangeCausedByEvent)change_1.Difference(change_4);
        // 2 - 0 = 2 
        ProtoDistanceChangeCausedByEvent change_diff_2 = (ProtoDistanceChangeCausedByEvent)change_1.Difference(change_3);
        // 0 - 2 = -2
        ProtoDistanceChangeCausedByEvent change_diff_3 = (ProtoDistanceChangeCausedByEvent)change_4.Difference(change_1);
        // 0 - 0 = 0
        ProtoDistanceChangeCausedByEvent change_diff_4 = (ProtoDistanceChangeCausedByEvent)change_4.Difference(change_4);
        // 2 - -1 = 3
        ProtoDistanceChangeCausedByEvent change_diff_5 = (ProtoDistanceChangeCausedByEvent)change_1.Difference(change_2);
        // -1 - 2 = -3 
        ProtoDistanceChangeCausedByEvent change_diff_6 = (ProtoDistanceChangeCausedByEvent)change_2.Difference(change_1);

        Assert.AreEqual(change_diff_1.ToReal(), 2.0d);
        Assert.AreEqual(change_diff_2.ToReal(), 2.0d);
        Assert.AreEqual(change_diff_3.ToReal(), -2.0d);
        Assert.AreEqual(change_diff_4.ToReal(), 0.0d);
        Assert.AreEqual(change_diff_5.ToReal(), 3.0d);
        Assert.AreEqual(change_diff_6.ToReal(), -3.0d);


        // Compare To testing 3 cases

        int compare_1 = change_1.CompareTo(change_2); // 2.compareTo(-1) = 1
        int compare_2 = change_1.CompareTo(change_1); // 2.compareTo(2) = 0
        int compare_3 = change_2.CompareTo(change_1); // -1.compareTo(2) = -1

        Assert.AreEqual(compare_1, 1);
        Assert.AreEqual(compare_2, 0);
        Assert.AreEqual(compare_3, -1);
    }

    [Test]
    public void TestDistanceBetweenWorldStates()
    {
        AgentSimItemSlot[] inventory_1 = new AgentSimItemSlot[] { full_slot, empty_slot, empty_slot };
        ProtoWorldStateViewT world_1 = new ProtoWorldStateViewT(inventory_1);

        AgentSimItemSlot[] inventory_2 = new AgentSimItemSlot[] { full_slot, full_slot, empty_slot };
        ProtoWorldStateViewT world_2 = new ProtoWorldStateViewT(inventory_2);

        AgentSimItemSlot[] inventory_3 = new AgentSimItemSlot[] { empty_slot, empty_slot, empty_slot };
        ProtoWorldStateViewT world_3 = new ProtoWorldStateViewT(inventory_3);

        AgentSimItemSlot[] goal_inventory = new AgentSimItemSlot[] { full_slot, full_slot, full_slot };
        ProtoWorldStateViewT goal_world = new ProtoWorldStateViewT(goal_inventory);
        // testing the Distance caluclation

        ProtoDistanceBetweenWorldStates distance_1 = (ProtoDistanceBetweenWorldStates) world_1.CalcDistanceTo(goal_world); // -2
        ProtoDistanceBetweenWorldStates distance_2 = (ProtoDistanceBetweenWorldStates) world_2.CalcDistanceTo(goal_world); //-1
        ProtoDistanceBetweenWorldStates distance_3 = (ProtoDistanceBetweenWorldStates) world_3.CalcDistanceTo(goal_world); // -3
        ProtoDistanceBetweenWorldStates distance_4 = (ProtoDistanceBetweenWorldStates) goal_world.CalcDistanceTo(goal_world); //0 

        Assert.That(distance_1.Equals(new ProtoDistanceBetweenWorldStates(world_3, world_2))); //- 2
        Assert.That(distance_2.Equals(new ProtoDistanceBetweenWorldStates(world_1, world_2))); // -1
        Assert.That(distance_3.Equals(new ProtoDistanceBetweenWorldStates(world_3, goal_world))); // -3
        Assert.That(distance_4.Equals(new ProtoDistanceBetweenWorldStates(world_3, world_3))); // 0


        // Testing the CompareTo Function
        int distance_compare_1 = distance_1.CompareTo(distance_2); // 2.compareTo(1) => -1
        int distance_compare_2 = distance_2.CompareTo(distance_1); // 1.compareTo(2) => 1
        int distance_compare_3 = distance_3.CompareTo(distance_4); // 3.compareTo(0) => -1
        int distance_compare_4 = distance_4.CompareTo(distance_3); // 0.compareTo(3) => +1
        int distance_compare_5 = distance_4.CompareTo(distance_4); // 0.compareTo(0) => 0
        int distance_compare_6 = distance_4.CompareTo(distance_1); // 0.compareTo(2) => 1

        Assert.AreEqual(distance_compare_1, -1);
        Assert.AreEqual(distance_compare_2, 1);
        Assert.AreEqual(distance_compare_3, -1);
        Assert.AreEqual(distance_compare_4, 1);
        Assert.AreEqual(distance_compare_5, 0);
        Assert.AreEqual(distance_compare_6, 1);

        // Testing Difference
        // checking the changes_distances difference method works
        // There are 6 subtraction cases to check 
        // 1. positive - 0 = positive
        // 2. postive - positive = 0
        // 3. 0 - positive = negative
        // 4. 0 - 0 = 0
        // 5. big positive - small positive =
        //      small positive, e.g. 2 - 1 = 1
        // 6. small positive - big positive =
        //      small negative, e.g. 1 - 2 = -1


        ProtoDistanceChangeCausedByEvent distance_diff_1 = (ProtoDistanceChangeCausedByEvent) distance_1.Difference(distance_4); // -2 - 0 = -2
        ProtoDistanceChangeCausedByEvent distance_diff_2 = (ProtoDistanceChangeCausedByEvent) distance_1.Difference(distance_1); // -2 - -2 = 0
        ProtoDistanceChangeCausedByEvent distance_diff_3 = (ProtoDistanceChangeCausedByEvent) distance_4.Difference(distance_1); // 0 - -2 = 2
        ProtoDistanceChangeCausedByEvent distance_diff_4 = (ProtoDistanceChangeCausedByEvent) distance_4.Difference(distance_4); // 0 - 0 = 0
        ProtoDistanceChangeCausedByEvent distance_diff_5 = (ProtoDistanceChangeCausedByEvent) distance_1.Difference(distance_2); // -2 - -1 = -1
        ProtoDistanceChangeCausedByEvent distance_diff_6 = (ProtoDistanceChangeCausedByEvent) distance_2.Difference(distance_1); // -1 - -2 = 1

        Assert.AreEqual(distance_diff_1.ToReal(), -2.0d);
        Assert.AreEqual(distance_diff_2.ToReal(), 0.0d);
        Assert.AreEqual(distance_diff_3.ToReal(), 2.0d);
        Assert.AreEqual(distance_diff_4.ToReal(), 0.0d);
        Assert.AreEqual(distance_diff_5.ToReal(), -1.0d);
        Assert.AreEqual(distance_diff_6.ToReal(), 1.0d);

    }

    [Test]
    public void TestUtilityFunctions()
    {
        // Fold Sum
        int[] array_1 = new int[] { 1, 2, 3, 4, 5 };
        int[] array_2 = new int[] { -1, -2, -3, -4, -5 };

        int abs_sum_1 = Extensions.FoldLeft(array_1);
        int abs_sum_2 = Extensions.FoldLeft(array_2);

        Assert.AreEqual(abs_sum_1, 15);
        Assert.AreEqual(abs_sum_2, -15);

        // AllValidChanges
        AgentSimItemSlot[] inventory1 =
            new AgentSimItemSlot[] { full_slot, empty_slot, empty_slot };

        int[] changes1 = new int[] { -1, 1, 1 }; // T
        int[] changes2 = new int[] { 1, 0, 0 };  // F
        int[] changes3 = new int[] { 0, -1, -1 }; // F
        int[] changes4 = new int[] { 0, 0, 0 }; // T

        bool change1 =
            Extensions.AllValidChanges(inventory1, changes1); // T
        bool change2 =
            Extensions.AllValidChanges(inventory1, changes2); // F
        bool change3 =
            Extensions.AllValidChanges(inventory1, changes3); // F
        bool change4 =
            Extensions.AllValidChanges(inventory1, changes4); // T

        Assert.AreEqual(change1, true);
        Assert.AreEqual(change2, false);
        Assert.AreEqual(change3, false);
        Assert.AreEqual(change4, true);

        // Update Inventory
        int[] changes5 = new int[] { 0, 1, 1 }; // T
        AgentSimItemSlot[] inventory2 =
            Extensions.ZipUpdateInventory(inventory1, changes1);
        AgentSimItemSlot[] inventory3 =
            Extensions.ZipUpdateInventory(inventory1, changes4);
        AgentSimItemSlot[] inventory5 =
            Extensions.ZipUpdateInventory(inventory1, changes5);

        Assert.AreEqual(inventory2,
            new AgentSimItemSlot[] { empty_slot, full_slot, full_slot });
        Assert.AreEqual(inventory3, inventory1);
        Assert.AreEqual(inventory5,
            new AgentSimItemSlot[] { full_slot, full_slot, full_slot });

        // Comparing World Slots
        int[] compare1 =
            Extensions.ZipSlotCompare(inventory1, inventory2); // 1, -1, -1
        int[] compare2 =
            Extensions.ZipSlotCompare(inventory1, inventory5); // 0, -1, -1
        int[] compare3 =
            Extensions.ZipSlotCompare(inventory1, inventory3); // 0, 0, 0

        Assert.AreEqual(compare1, new int[] { 1, -1, -1 });
        Assert.AreEqual(compare2, new int[] { 0, -1, -1 });
        Assert.AreEqual(compare3, new int[] { 0, 0, 0 });

    }

}