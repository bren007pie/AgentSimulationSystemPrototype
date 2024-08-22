/* =======================================================================
 * World View Generic Abstract Class
 * -----------------------------------------------------------------------
 * The WorldState Implementation Class is for providing an implementation for the ProtoWorldStateViewT type with subtypes
 *  ProtoWorldEventT, ProtoDistanceBetweenWorldStates, and distanceChangeCausedByEvent derived from
 *  EMgine.WorldStateViewT<S>, EMgine.WorldStateViewT<S>.WorldEventT, EMgine.WorldStateViewT<S>.DistBtwWorldStateViewsT,
 *  and EMgine.WorldStateViewT<S>.DistChgBtwWorldStateViewsT respectively but in the AgentSimulation 
 *  namespace and with a AgentSimItemSlot as the type variable <S>. 
 *  Pretty much coppied from EmgineWorldT.cs with some variable name changes for readability.
 * =======================================================================
 * Relies on: ---
 * =======================================================================
 * Notes: Equals() Method follows the conventions 
 * 
 *          nextElement > y returns c > 0
 *          nextElement = y returns c = 0
 *          nextElement < y returns c < 0
 * 
 * =======================================================================
 */


using System;  // IComparable<T>, IEquatable<T>, IConvertible, IFormattable
using System.Collections.Generic;
using System.Linq;
using EMgine;  // EMgine.WorldStateViewT<S> and subtypes

namespace AgentSimulation
{
    /* ===================================================
     * AgentSim Implemented World Type
    * ===================================================
    */

    /// <summary>
    /// World State for Agent Simulation System, implementing Emgine WorldStateViewT.
    /// </summary
    /// <seealso cref=
    ///     "EMgine.WorldStateViewT&lt;AgentSimulation.AgentSimItemSlot&gt;" />
    public class ProtoWorldStateViewT : WorldStateViewT<AgentSimItemSlot>
    {
        /// <summary>The inventory state of the slots.</summary>
        private readonly AgentSimItemSlot[] _inventoryState;

        // constructor
        /// <summary>Initializes a new instance 
        /// of the <see cref="ProtoWorldStateViewT" /> class.</summary>
        /// <remarks>The main type for the world.</remarks>
        /// <param name="inventoryState">State of the inventory.</param>
        public ProtoWorldStateViewT(AgentSimItemSlot[] 
            inventoryState) => _inventoryState = inventoryState;

        /* ===================================================
         * Accessors
         * ===================================================
        */

        /// <summary>
        /// Applies the event to the world and returns new world.
        /// </summary>
        /// <remarks>Does checks, zip compares with 
        ///   bool.compareTo(bool), and fold sums those compares.</remarks>
        /// <param name="worldChange">The world change.</param>
        /// <returns>World after return change.</returns>
        public override WorldStateViewT<AgentSimItemSlot> 
                ApplyEvent(WorldEventT worldChange)
        {
            // casting to inherited type
            ProtoWorldEventT worldChangeEvent = (ProtoWorldEventT)worldChange; 
            // Guard checking for equal sizes of inventories and slot changes
            CheckInventorySize(this.GetInventorySize(), 
                worldChangeEvent.GetSize());
            CheckSlotChanges(this.GetInventory(), 
                worldChangeEvent.GetChanges());
            // Using a functional zip to combine the event with the inventory
            AgentSimItemSlot[] new_inventory_state = 
                Extensions.ZipUpdateInventory(this.GetInventory(), 
                    worldChangeEvent.GetChanges());
            ProtoWorldStateViewT new_World = 
                new ProtoWorldStateViewT(new_inventory_state);
            return new_World;
        }


        /// <summary>
        /// Indirection creator for ProtoDistanceChangeCausedByEvent.
        /// </summary>
        /// <param name="worldChangeEvent">The world change event.</param>
        /// <remarks> Use this to create distances.</remarks>
        /// <seealso cref=""/>
        /// <returns>Distance caused by event.</returns>
        public override DistChgBtwWorldStateViewsT 
            CalcChangeCausedBy(WorldEventT worldChangeEvent)
        {
            ProtoWorldEventT convertedEvent = 
                (ProtoWorldEventT)worldChangeEvent;
            CheckSlotChanges(this.GetInventory(), 
                convertedEvent.GetChanges()); // guard for changing slots
            // reusing class constructor to calculate the new change
            ProtoDistanceChangeCausedByEvent distanceChange = 
                new ProtoDistanceChangeCausedByEvent(convertedEvent);
            CheckInventorySize(this.GetInventorySize(), 
                distanceChange.GetEventSize());
            return distanceChange;
        }



        /// <summary>
        /// Indirection creator for ProtoDistanceBetweenWorldStates.
        /// </summary>
        /// <remarks> Use this to create distances.</remarks>
        /// <param name="otherWorld">The other world.</param>
        /// <returns>Distance between this world state and 
        ///     the OtherWorld state.</returns>
        public override DistBtwWorldStateViewsT 
            CalcDistanceTo(WorldStateViewT<AgentSimItemSlot> otherWorld)
        {
            // reusing class constructor to calculate the new change
            ProtoDistanceBetweenWorldStates distanceChange = 
                new ProtoDistanceBetweenWorldStates(this, otherWorld);
            CheckInventorySize(this.GetInventorySize(), 
               distanceChange.GetInventorySize());
            return distanceChange;
        }

        /// <summary>
        /// Determines whether [is mutually exclusive of] 
        ///     [the specified other world].
        /// </summary>
        /// <remarks>
        /// Not needed for joy example but would just 
        ///     require a simple equality check
        // Bassically is one state a substate of the other? 
        // I.e. can you be in two states at once? No, you
        //  either have the same inventory or not
        // In which case if they're not the same world state
        //  they would be mutually exclusive 
        /// </remarks>
        /// <param name="otherWorld">The other world.</param>
        /// <returns>
        ///   <c>true</c> if [is mutually exclusive of] 
        ///   [the specified other world]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool 
            IsMutuallyExclusiveOf(WorldStateViewT<AgentSimItemSlot> otherWorld)
        {

            throw new NotImplementedException();
        }

        // For comparing if two world states are equal,
        // loops over all elements & checks for subtypes
        // based on EMgineWorldT.cs
        public override bool 
            Equals(WorldStateViewT<AgentSimItemSlot> otherWorld)
        {
            ProtoWorldStateViewT other_world_converted = 
                (ProtoWorldStateViewT)otherWorld;
            int inventorySize = this.GetInventorySize();

            if (otherWorld == null || inventorySize != 
                other_world_converted.GetInventory().Length) return false;
            else if (otherWorld == this) return true; 
            else
            {
                bool eq;
                for (int i = 0; i < inventorySize; i++)
                {
                    eq = false;
                    for (int j = 0; j < inventorySize; j++)
                    {
                        if (this.GetInventory()[i].Equals(
                            other_world_converted.GetInventory()[i]))
                        {
                            eq = true;
                            break;
                        }
                    }
                    if (!eq) return false;
                }
                return true;
            }
        }


        // considered making equals for AgentSimItemSlot[] and bool[]
        // comparisons but then would expose underlying "secret"

        /// <summary>
        /// Equalses the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode(); 
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        {
            return base.ToString();
        }

        /// <summary>
        /// Internal getter for inventory state
        /// </summary>
        /// <returns>Inventory state</returns>
        private AgentSimItemSlot[] GetInventory() => _inventoryState;

        /// <summary>
        /// Internal getter for inventory size
        /// </summary>
        /// <returns>Inventory size</returns>
        private int GetInventorySize() => _inventoryState.Length;



        /* ===================================================
         * Subtypes: ProtoWorldEventT, ProtoDistanceBetweenWorldStates, 
         *  and distanceChangeCausedByEvent
         * ===================================================
         */

        /* ===================================================
        * Secret: Is an integer array coresponding to each 
        *   invnetory slot position
        * Positive numbers are adding and negative subtracting
        * E.g. If one item added from third index: [0, 0, 1]
        * E.g. If one item removed from third index: [0, 0, -1]
        * ===================================================
        */

        /// <summary>
        /// World event change, i.e. inventory state change.
        /// </summary>
        /// <seealso cref=
        /// "EMgine.WorldStateViewT&lt;AgentSimulation.AgentSimItemSlot&gt;.WorldEventT" />
        public class ProtoWorldEventT : WorldEventT
        {

            /// <summary>
            /// Change in inventory bassically just an array change
            /// </summary>
            /// <remarks>
            // Chose int array so that could represent an item being
            // removed, unchanged, and added for each item slot
            /// </remarks>
            private readonly int[] _inventoryChange;


            /// <summary>
            /// Initializes a new instance of the <see 
            ///     cref="ProtoWorldEventT"/> class.
            /// </summary>
            /// <remarks> Should bring in applyEvent into this 
            ///     subtype</remarks>
            /// <param name="inventory_change">The inventory 
            /// change slots.</param>
            public ProtoWorldEventT(int[] inventory_change)
            {
                _inventoryChange = inventory_change;
            }


            // Accessors

            /// <summary>
            /// Getter for the size of the event
            /// </summary>
            /// <remarks>Should be same size as the 
            ///     inventory of the world state.</remarks>
            /// <returns>Integer size.</returns>
            internal int GetSize()
            {
                return _inventoryChange.Length;
            }

            /// <summary>
            /// Returns the value at that index
            /// </summary>
            /// <param name="index"></param>
            /// <returns>Change value at that index.</returns>
            internal int At(int index)
            {
                return (int)_inventoryChange[index];
            }


            /// <summary>
            /// Getter for the Event Array
            /// </summary>
            /// <remarks>Used only for methods within parent class. 
            /// </remarks>
            /// <returns>The changes array.</returns>
            internal int[] GetChanges()
            {
                return _inventoryChange;
            }

            public override double GetEventProbability()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }

        }


        public class ProtoDistanceBetweenWorldStates : 
            DistBtwWorldStateViewsT
        {

            /// <summary> Magnitude of the change, 
            /// number of steps to get to distance. </summary>
            private readonly int _distance;
            /// <summary> Size of the World inventory that was taken in, 
            /// used to check only comparing similar sized worlds/events. 
            /// </summary>
            private readonly int _inventorySize;


            // Secret: Does a CompareTo for each slot (bassically making
            // an event) then AbsLeftFolds into a distance

            /// <summary>
            /// Initializes a new instance of the <see cref=
            /// "ProtoDistanceBetweenWorldStates"/> class.
            /// </summary>
            /// <remarks>Encapsulates logic of measuring distance 
            /// between worlds.
            /// Order Matters: left.compareTo(right).</remarks>
            /// <param name="leftWorld">The left world.</param>
            /// <param name="rightWorld">The right world.</param>
            public ProtoDistanceBetweenWorldStates(
                WorldStateViewT<AgentSimItemSlot> leftWorld, 
                WorldStateViewT<AgentSimItemSlot> rightWorld)
            {
                ProtoWorldStateViewT convertedLeftWorld = 
                    (ProtoWorldStateViewT)leftWorld;
                ProtoWorldStateViewT convertedRightWorld = 
                    (ProtoWorldStateViewT)rightWorld;

                CheckInventorySize(convertedLeftWorld.GetInventorySize(), 
                    convertedRightWorld.GetInventorySize());
                _inventorySize = convertedLeftWorld.GetInventorySize();

                int[] slotDifference = Extensions.ZipSlotCompare(
                    convertedLeftWorld.GetInventory(), 
                    convertedRightWorld.GetInventory());

                _distance = Extensions.FoldLeft(slotDifference); // get the distance
            }


            /// <summary>
            /// Standard CompareTo to another distance.
            /// </summary>
            /// <remarks>
            /// fst.CompareTo(snd) Method follows the conventions: 
            /// fst > snd returns 1 ,
            /// fst = snd returns 0 ,
            /// fst < snd returns -1 .
            /// </remarks>
            /// <param name="otherDistance">The other distance.</param>
            /// <returns>Comparison int.</returns>
            public override int CompareTo(DistBtwWorldStateViewsT
                otherDistance)
            {
                ProtoDistanceBetweenWorldStates convertedDistance =
                    (ProtoDistanceBetweenWorldStates)otherDistance;

                CheckInventorySize(this.GetInventorySize(),
                    convertedDistance.GetInventorySize());

                // returning the difference
                return this.GetDistance().CompareTo(convertedDistance.GetDistance());

            }


            /// <summary>
            /// Determines whether this instance is finite.
            /// </summary>
            /// <remarks>
            /// I like the name IsAttainainable
            /// Bassically would check if there is a state that could not be 
            ///     reached. 
            /// In my case events could reach any state so will always be true
            /// assuming "true" means distance will always be finite
            /// Will not assume so will leave as not_implemented
            /// </remarks>
            /// <returns>
            ///   <c>true</c> if this instance is finite; otherwise, <c>false</c>.
            /// </returns>
            /// <exception cref="NotImplementedException"></exception>
            public override bool IsFinite()
            {
                throw new NotImplementedException();
                // return true; // should always be a finite array
            }


            /// <summary>
            /// Checks equality between this distance and another one.
            /// </summary>
            /// <param name="otherDistance">The other distance.</param>
            /// <returns>If equal.</returns>
            public bool Equals(DistBtwWorldStateViewsT otherDistance)
            {
                ProtoDistanceBetweenWorldStates convertedDistance = 
                    (ProtoDistanceBetweenWorldStates)otherDistance;

                CheckInventorySize(this.GetInventorySize(), 
                    convertedDistance.GetInventorySize());

                return this.GetDistance().Equals(
                    convertedDistance.GetDistance());

            }

            /// <summary>
            /// Difference between this distance and otherDistance.
            /// </summary>
            /// <remarks>Not sure conceptually why returns 
            ///     DistanceCausedByEvent but
            /// that's what's in the interface. Also takes abs value 
            ///     because distances can't be negaitve.
            /// </remarks>
            /// <param name="otherDistance">The other distance.</param>
            /// <returns>Changed caused by event distance.</returns>
            public override DistChgBtwWorldStateViewsT 
                Difference(DistBtwWorldStateViewsT otherDistance)
            {
                ProtoDistanceBetweenWorldStates convertedDistance = 
                    (ProtoDistanceBetweenWorldStates)otherDistance;

                CheckInventorySize(this.GetInventorySize(), 
                    convertedDistance.GetInventorySize());

                int difference = this.GetDistance() - 
                    convertedDistance.GetDistance();

                return new ProtoDistanceChangeCausedByEvent(
                    this.GetInventorySize(), difference);
            }

            /// <summary>
            /// Returns distance as real number.
            /// </summary>
            /// <returns>Distance as a real</returns>
            public override double ToReal()
            {
                return (double)this.GetDistance();
            }

            /// <summary>
            /// Getter for Inventory size.
            /// </summary>
            /// <remarks>Not private so outside functions can 
            /// check size.</remarks>
            /// <returns>Inventory Size</returns>
            internal int GetInventorySize()
            {
                return this._inventorySize;
            }

            /// <summary>
            /// Private getter for distance.
            /// </summary>
            /// <returns>Distance</returns>
            private int GetDistance()
            {
                return _distance;
            }

            /// <summary>
            /// Gets the hash code.
            /// </summary>
            /// <returns>Hash code.</returns>
            public override int GetHashCode() 
            {
                return base.GetHashCode();  
            }

            /// <summary>
            /// Converts to string.
            /// </summary>
            /// <returns>Object Stringified</returns>
            public override string ToString()
            {
                return base.ToString();
            }

            /// <summary>
            /// Equalses the specified object.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>If objects are equal.</returns>
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

        }


        /* ===================================================
        * Secret is that it will be an absolute value integer 
        *   fold/sum of the event array
        * This means if low number of changes smaller distance
        * E.g. Distance change caused by event [1, 1, 1] => 3  
        * E.g. More Examples: [-1, 1, 1] => 3, [0, -1, 0] => 1
        * ===================================================
        */
        /// <summary>
        /// Change in Distance (delta distance/inventory changes) 
        ///     caused by an event.
        /// </summary>
        /// <remarks>
        /// I.e. the  distance/change diffence of the event. 
        /// Changed name to make it clear that it's a DISTANCE.
        /// i.e.smaller distances mean shorter distances and closer worlds.
        /// Therefore larger distances mean further distances and 
        ///     further worlds.
        /// </remarks>
        /// <seealso cref=
        /// "EMgine.WorldStateViewT&lt;AgentSimulation.AgentSimItemSlot&gt;.DistChgBtwWorldStateViewsT" />
        public class ProtoDistanceChangeCausedByEvent : DistChgBtwWorldStateViewsT
        {
            /// <summary> Magnitude of the change, number of steps to 
            ///     get to distance. </summary>
            private readonly int _distance;
            /// <summary> Size of the event that was taken in, used to 
            /// check only comparing similar sized worlds/events. </summary>
            private readonly int _eventSize;

            /// <summary>
            /// Initializes a new instance of the <see cref=
            /// "ProtoDistanceChangeCausedByEvent"/> class.
            /// </summary>
            /// <param name="worldChangeEvent">The world change event.</param>
            public ProtoDistanceChangeCausedByEvent(
                WorldEventT worldChangeEvent)
            {
                // check removes aren't invalid
                ProtoWorldEventT converted_event = (ProtoWorldEventT)
                    worldChangeEvent;
                _eventSize = converted_event.GetSize();

                // Using fold instead of for loop for readability
                _distance = 
                    Extensions.FoldLeft(converted_event.GetChanges());

            }

            /// <summary>
            /// Private constructor for settings things internally.
            /// Initializes a new instance of the 
            /// <see cref="ProtoDistanceChangeCausedByEvent"/> class.
            /// </summary>
            /// <remarks>Used for difference method to return without using setters.</remarks>
            /// <param name="eventSize">Size of the event.</param>
            /// <param name="distance">The distance.</param>
            internal ProtoDistanceChangeCausedByEvent(
                int eventSize, int distance)
            {
                _eventSize = eventSize;
                _distance = distance;
            }

            /// <summary>
            /// Compares two distances using the standard CompareTo method.
            /// </summary>
            /// <remarks> 
            /// fst.CompareTo(snd) Method follows the conventions: 
            /// fst > snd returns 1 ,
            /// fst = snd returns 0 ,
            /// fst < snd returns -1 .
            /// </remarks>
            /// <param name="distanceChange">The other distance change.</param>
            /// <returns>Comparison int.</returns>
            public override int CompareTo(DistChgBtwWorldStateViewsT distanceChange)
            {
                ProtoDistanceChangeCausedByEvent convertedDistance = 
                    (ProtoDistanceChangeCausedByEvent)distanceChange;
                CheckInventorySize(this.GetEventSize(), 
                    convertedDistance.GetEventSize());

                return this.GetDistance().CompareTo(
                    convertedDistance.GetDistance());

            }


            /// <summary>
            /// Difference between two distances without modifying either.
            /// </summary>
            /// <remarks>Absolute value of distance taken because 
            /// must be positive. </remarks>
            /// <param name="distanceChanged">The other distance changed.
            /// </param>
            /// <returns>New distance that's the difference of this 
            /// - distanceChanged. </returns>
            public override DistChgBtwWorldStateViewsT 
                Difference(DistChgBtwWorldStateViewsT distanceChanged)
            {
                ProtoDistanceChangeCausedByEvent convertedDistance = 
                    (ProtoDistanceChangeCausedByEvent)distanceChanged;

                CheckInventorySize(this.GetEventSize(), 
                    convertedDistance.GetEventSize());

                int difference = this.GetDistance() - 
                    convertedDistance.GetDistance();

                // made private constructor just for this


                return new ProtoDistanceChangeCausedByEvent(
                    this.GetEventSize(), difference);

            }


            /// <summary>
            /// Returns distance as real number.
            /// </summary>
            /// <returns>Distance as a real</returns>
            public override double ToReal()
            {
                return (double)this.GetDistance();
            }

            /// <summary>
            /// Getter for event array size.
            /// </summary>
            /// <remarks>Not private because other ProtoWorldT 
            ///     functions use to check size.</remarks>
            /// <returns>Event array size.</returns>
            internal int GetEventSize()
            {
                return _eventSize;
            }

            /// <summary>
            /// Internal getter for distance value.
            /// </summary>
            /// <returns>Distance value.</returns>
            private int GetDistance()
            {
                return this._distance;
            }

            /// <summary>
            /// Equalses the specified object.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            /// <summary>
            /// Checks equality between this distance and another one.
            /// </summary>
            /// <param name="otherDistance">The other distance.</param>
            /// <returns>If equal.</returns>
            public bool Equals(DistChgBtwWorldStateViewsT distanceChanged)
            {
                ProtoDistanceChangeCausedByEvent convertedDistance = 
                    (ProtoDistanceChangeCausedByEvent)distanceChanged;

                CheckInventorySize(this.GetEventSize(), 
                    convertedDistance.GetEventSize());

                return this.GetDistance().Equals(
                    convertedDistance.GetDistance());

            }

            /// <summary>
            /// Gets the hash code.
            /// </summary>
            /// <returns>Base Hashcode.</returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Converts to string.
            /// </summary>
            /// <returns>Base ToString</returns>
            public override string ToString()
            {
                return base.ToString();
            }

        }

        /// <summary>
        /// Checks the sizes of the inventory are the same.
        /// Should be used whenever two things inventories interact.
        /// </summary>
        /// <param name="inventoryOneSize">Size of  inventory one.</param>
        /// <param name="inventoryTwoSize">Size of  inventory two.</param>
        /// <remarks>Order of sizes does not matter.</remarks>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">
        ///     Sizes of inventories do not match.</exception>
        private static void CheckInventorySize(
            int inventoryOneSize, int inventoryTwoSize)
        {

            if (inventoryOneSize != inventoryTwoSize)
            {
                // using index out of range because is
                // what would happen if size mismatch
                throw new 
                    IndexOutOfRangeException("Sizes of inventories do not match.");
            }
        }

        /// <summary>
        /// Checks for invalid slot changes changes.
        /// Should be used whenever an event and inventory interact.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="changes">The changes.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Invalid slot change. E.g. adding to a full slot or 
        /// removing from an empty one.
        /// Using InvalidOperation because trying to do an illegal slot change.
        /// </exception>
        private static void CheckSlotChanges(
            AgentSimItemSlot[] inventory, int[] changes)
        {
            if (!Extensions.AllValidChanges(inventory, changes))
            {
                throw new 
                    InvalidOperationException("Invalid slot change.");
            }
        }

    }

    /// <summary>
    /// Extension utility class for common used functions.
    /// Almost all functions use functional programming 
    /// concepts from IEnumerable.
    /// </summary>
    /// <see href=
    /// "https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable?view=net-7.0"/>
    public static class Extensions
    {
        /// <summary>
        /// Does the left fold (sum) of each element of an int array.
        /// </summary>
        /// <remarks>
        /// Could have used Sum(IEnumerable<Int32>) but want to keep 
        ///     just incase function changes.
        /// </remarks>
        /// <param name="sumArray">The array to be summed.</param>
        /// <returns></returns>
        public static int FoldLeft(this IEnumerable<int> sumArray)
        {
            return sumArray.Aggregate(0, (totalSum, nextElement) => 
                nextElement + totalSum);
        }

        /// <summary>
        /// Checks whether all changes are allowed. 
        /// </summary>
        /// <remarks>
        /// Probably some way to do this in one function without a 
        ///     zip then an all.
        /// Also sorry for one line, was having casting issues.
        /// </remarks>
        /// <param name="inventory">The inventory slots.</param>
        /// <param name="changes">The changes.</param>
        /// <returns>True if all valid changes, false if there is 
        ///     at least one invalid change.</returns>
        public static bool 
            AllValidChanges(IEnumerable<AgentSimItemSlot> inventory, 
                IEnumerable<int> changes)
        {
            return inventory.Zip<AgentSimItemSlot, int, bool>(changes, 
                   (slot, change) => slot.InvalidSlotChange(
                        change)).All<bool>(invalidChange => 
                            invalidChange.Equals(false));
        }
        /// <summary>
        /// Zips to update both inventories the inventory. 
        /// Assumes CheckSlotChanges have been called <see cref=
        /// "ProtoWorldStateViewT.CheckSlotChanges(AgentSimItemSlot[], int[])"/>
        /// </summary>
        /// <seealso cref="AllValidChanges(IEnumerable{AgentSimItemSlot}, 
        ///     IEnumerable{int})"/>
        /// <param name="inventory">The inventory.</param>
        /// <param name="changes">The changes.</param>
        /// <returns>New Inventory.</returns>
        public static AgentSimItemSlot[] 
            ZipUpdateInventory(IEnumerable<AgentSimItemSlot> inventory, 
                IEnumerable<int> changes)
        {
            return inventory.Zip<AgentSimItemSlot, int, 
                AgentSimItemSlot>(changes, (slot, change) => 
                slot.SlotChange(change)).ToArray<AgentSimItemSlot>();
        }

        /// <summary>
        /// Zip compares the slotsSlots the compare.
        /// </summary>
        /// <param name="leftInventory">The left inventory.</param>
        /// <param name="rightInventory">The right inventory.</param>
        /// <returns></returns>
        public static int[] ZipSlotCompare(IEnumerable<AgentSimItemSlot> 
            leftInventory, IEnumerable<AgentSimItemSlot> rightInventory)
        {
            return leftInventory.Zip<AgentSimItemSlot, AgentSimItemSlot, 
                int>(rightInventory, (leftSlot, rightSlot) => 
                    leftSlot.CompareTo(rightSlot)).ToArray<int>();
        }

    }

}