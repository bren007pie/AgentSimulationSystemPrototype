/* =======================================================================================================
 * Data Classes: AgentSimItemSlot 
 * -------------------------------------------------------------------------------------------------------
 * These classes implement the WorldStateViewT, WorldEventT, DistBtwWorldStateViewsT, and 
 * DistChgBtwWorldStateViewsT abstract classes by defining a type for the S generic type in
 * WorldStateViewT. Pretty much coppied from EMgineWorldData.cs with some variable name changes for readability.
 * =======================================================================================================
 * Relies on: ---
 * =======================================================================================================
 */

using System; // IComparable, IEquatable<T>

namespace AgentSimulation
{
    /* ===================================================
    * AgentSimWorldData Interface for defining different
    * classes that are recognized as World Data by
    * ProtoWorldT (satisfies type parameter S).
    * ===================================================
    */
    public interface AgentSimWorldData : IComparable
    {

    }

    /* ===================================================
    * Boolean Item state Data
    * Describes whether an item is in the monitored 
    * slot or not. True is item here, false is not here.
    * ===================================================
    */

    public class AgentSimItemSlot : AgentSimWorldData, IEquatable<AgentSimItemSlot>, IComparable<AgentSimItemSlot>
    {
        private readonly bool _boolean;
        public AgentSimItemSlot(bool b) => _boolean = b;
        public bool GetData() => _boolean;
        public int CompareTo(bool b) => _boolean.CompareTo(b);

        /// <summary>
        /// Returns a bool on whether slot change is invalid. 
        /// </summary>
        /// <remarks>
        /// Adding to a full slot (true + 1) and removing from an empty slot (false - 1) are both invalid.
        /// </remarks>
        /// <param name="slotChange">The slot change.</param>
        /// <returns>True if invalid change, false if valid change.</returns>
        public bool InvalidSlotChange(int slotChange)
        {
            return (this.GetData(), slotChange) switch
            {
                (true, > 0) => true, // error, adding to a full slot
                (true, < 0) => false, // removing from a full slot
                (false, > 0) => false, // adding to an empty 
                (false, < 0) => true, // error, removing from an empty slot
                _ => false, // no state change, (_, 0)
            };
        }

        /// <summary>
        /// Changes the slot returning a new value. 
        /// Assumes already checked for invalid changes from <see cref="AgentSimulation.ProtoWorldStateViewT.CheckSlotChanges(AgentSimItemSlot[], int[])" />
        /// </summary>
        /// <param name="slotChange">The slot change.</param>
        /// <remarks>Copy pasted switch statement, probably could be reduced <seealso cref="InvalidSlotChange(int)"/>.</remarks>
        /// <returns>New slot after change</returns>
        public AgentSimItemSlot SlotChange(int slotChange)
        {
            return (this.GetData(), slotChange) switch
            {
                (true, < 0) => new AgentSimItemSlot(false), // removing from a full slot
                (false, > 0) => new AgentSimItemSlot(true), // adding to an empty 
                _ => new AgentSimItemSlot(this.GetData()), // no state change, (_, 0)
            };
        }


#nullable enable // nullable enable envoirnment: https://stackoverflow.com/questions/55492214/the-annotation-for-nullable-reference-types-should-only-be-used-in-code-within-a
        public int CompareTo(object? obj)  
        {
            if (obj == null) return 1;
            bool b = (bool)obj;
            return CompareTo(b);
        }
#nullable disable

        public int CompareTo(AgentSimItemSlot other_slot)  
        {
            if (other_slot == null) return 1;
            bool b = other_slot.GetData();
            return CompareTo(b);
        }

#nullable enable
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            try
            {
                AgentSimItemSlot b = (AgentSimItemSlot)obj;
                return Equals(b);
            }
            catch { return false; }
        }
#nullable disable

        // had to add this override for bool type because it kept misdetecting types to compare to
        public bool Equals(bool b)
        {
            return (_boolean == b);
        }

        
        public bool Equals(AgentSimItemSlot other_slot)
        {
            if (other_slot == null) return false;
            if (_boolean == other_slot.GetData()) return true; // if two slots equal true
            else return false; // if two slots not equal false
        }

        public override int GetHashCode() => _boolean.GetHashCode();


    }


}