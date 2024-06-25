/* =======================================================================================================
 * Time Generic Implementation Class
 * -------------------------------------------------------------------------------------------------------
 * The Time Implementation Class is for providing an implementation for the Time type and Delta Time subtypes 
 * of EMgine.TimeT<T> and EMgine.TimeT<T>.DeltaTimeT respectively but in the AgentSimulation namespace with a
 * float as the type variable <T>. Pretty much coppied from EmgineTimeT.cs with some variable name changes for
 * readability.
 * =======================================================================================================
 * Relies on: ---
 * =======================================================================================================
 * Notes: CompareTo() Method follows the conventions 
 * 
 *          x > y returns c > 0
 *          x = y returns c = 0
 *          x < y returns c < 0
 *          
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * TimeT<float>
 * -------------------------------------------------------------------------------------------------------
 * -------------------------------------------------------------------------------------------------------
 * TimeT<float>.DeltaTimeT
 * -------------------------------------------------------------------------------------------------------
 * =======================================================================================================
 */


using System;  // IComparable<T>, IEquatable<T>, IConvertible, IFormattable
using EMgine;  // TimeT<T>, TimeT.Deltatime

namespace AgentSimulation
{
    /* ===================================================
     * AgentSim Implemented Time Type
     * ===================================================
     */

    public class ProtoTimeT : TimeT<float>
    {

        /* State Variables */
        private readonly float _timeseconds;  // Based on Unity's Time.time "public static float time"

        /* Constructor */
        public ProtoTimeT(float time_in_seconds)
        {
            if (time_in_seconds < 0)
            {
                throw new ArgumentException("Time cannot be negative zero.", nameof(time_in_seconds));
            }
            _timeseconds = time_in_seconds;
        }

        

        /* Public Accessors */
        public override bool IsAfter(TimeT<float> other_time)
        {
            // need to cast on these to get access to private variable
            ProtoTimeT converted_other_time = (ProtoTimeT)other_time; 
            return _timeseconds > converted_other_time.GetTime();
        }

        public override DeltaTimeT Difference(TimeT<float> other_time)
        {
            ProtoTimeT converted_other_time = (ProtoTimeT)other_time;
            return new ProtoDeltaTimeT(_timeseconds - converted_other_time.GetTime());
        }

        /* Private Accessors */
        private float GetTime() => _timeseconds; // this is how you oneline a function with lambda functions

        ///* ===================================================
        // * AgentSim Implemented Delta Time Type
        // * ===================================================
        // */
        public class ProtoDeltaTimeT : DeltaTimeT
        {
            /* State Variables */
            private readonly float _deltatimeseconds;

            /* Constructor */
            public ProtoDeltaTimeT(float delta_time_in_seconds)
            {
                _deltatimeseconds = delta_time_in_seconds;
            }


            /* Public Accessors */
            // Adds two DeltaTimeT elements
            public override DeltaTimeT SumWith(DeltaTimeT other_delta)
            {
                ProtoDeltaTimeT converted_other_delta = (ProtoDeltaTimeT)other_delta;
                return new ProtoDeltaTimeT(_deltatimeseconds + converted_other_delta.GetDelta());
            }

            // Subtracts two DeltaTimeTs
            public override DeltaTimeT Difference(DeltaTimeT other_delta)
            {
                ProtoDeltaTimeT converted_other_delta = (ProtoDeltaTimeT)other_delta;
                return new ProtoDeltaTimeT(_deltatimeseconds - converted_other_delta.GetDelta());
            }

            // Multiplies this DeltaTimeT with a real value
            public override DeltaTimeT MultiplyByValue(double multiplier) => new ProtoDeltaTimeT(_deltatimeseconds * (float)multiplier);
            // ISSUE: This is not a templated type,
            //      EMgine interface should have multiplier be generic so user can enter whatever they want 

            // Compare this DeltaTimeT to dt, follows CompareTo() method, see note in header
            public override int CompareTo(DeltaTimeT other_delta)
            {
                ProtoDeltaTimeT converted_other_delta = (ProtoDeltaTimeT)other_delta;
                return _deltatimeseconds.CompareTo(converted_other_delta.GetDelta());
            }


          
            public override bool IsNegative() => _deltatimeseconds < 0; // Returns True if this DeltaTimeT is less than zero
            public override bool IsZero() => _deltatimeseconds == 0; // Returns True if this DeltaTimeT is equal to zero


            public override float GetAsValue() => _deltatimeseconds; // Get delta time as a computable value

            // makes coppies to allow pass by value
            public override DeltaTimeT Copy()
            {
                ProtoDeltaTimeT copied_deltatime = new(_deltatimeseconds);
                return copied_deltatime;
            }

            /* Private Accessors */
            private float GetDelta() => _deltatimeseconds;

            // Would be nice to Implement Operator Overloads for all these
        }
    }

}






