/* =======================================================================================================
 * Attention ADT
 * -------------------------------------------------------------------------------------------------------
 * The Attention ADT is for defining how much attention an entity has spent on some entity x in discrete 
 * time steps.
 * 
 * The stepSize state variable is for utility such that users can modify how many steps each ``attending'' 
 * increment should take. It is allowed to be zero to mimic spending no ``attending'' steps to x.
 * 
 * AttentionT can have child classes. All methods that change its internal state are marked "virtual" so 
 * that users can define their own implementations of them.
 * =======================================================================================================
 * Relies on: TimeT<T>.DeltaTimeT (TimeT.cs)
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 81
 * Cyclomatic Complexity: 22
 * Depth of Inheritance: 1
 * Class Coupling: 8
 * Lines of Source Code: 100
 * Lines of Executable Code: 32
 * =======================================================================================================
 */

using System;

namespace EMgine
{
    public class AttentionT<T> where T : IComparable<T>, IEquatable<T>, IConvertible, IFormattable
    {
        /* State Variables */
        private int _stepsAttended;
        private TimeT<T>.DeltaTimeT _stepSize;

        /* ---------------------------------------------------
         * METHODS
         * ---------------------------------------------------
         */

        /* Constructor */
        public AttentionT(int stepsAttended, TimeT<T>.DeltaTimeT stepSize)
        {
            _stepSize = null;
            UpdateAttendStepSize(stepSize);

            if (_stepSize != null) UpdateStepsAttended(stepsAttended);
            else _stepsAttended = -1;
        }

        /* Accessors */
        public int CompareStepsAttended(AttentionT<T> at) => _stepsAttended.CompareTo(at._stepsAttended);
        public int CompareStepSize(AttentionT<T> at) => _stepSize.CompareTo(at.GetAttendStepSize());
        public TimeT<T>.DeltaTimeT GetTimeAttended() => _stepSize.MultiplyByValue(_stepsAttended);

        public AttentionT<T> AbsoluteDifference(AttentionT<T> at)
        {
            TimeT<T>.DeltaTimeT thisAttention = GetTimeAttended();
            TimeT<T>.DeltaTimeT thatAttention = at.GetTimeAttended();
            TimeT<T>.DeltaTimeT diff;

            if (thisAttention.CompareTo(thatAttention) < 0) diff = thatAttention.Difference(thisAttention);
            else diff = thisAttention.Difference(thatAttention);

            if (!diff.IsNegative() && !diff.IsZero()) return new AttentionT<T>(1, diff);
            else return null;
        }

        /* Mutators */
        public virtual void UpdateAttendStepSize(TimeT<T>.DeltaTimeT stepSize)
        {
            /* 
             * 2022-12-13: Added a null check for nullable DeltaTimeT type
             * so that it is possible to recover from stepSize < 0
             * Found By: ConstructAttentionNegativeStepSize
             *           ConstructAttentionNullStepSize
             *           UpdateAttendStepSizeNegative
             *           UpdateAttendStepSizeNull
             */
            if (stepSize == null)
            {
                Utility.PrintMsg("E-A_NULL_STEPSIZE", Array.Empty<string>());
            }
            else if (stepSize.IsNegative())
            {
                Utility.PrintMsg("W-A_NEGATIVE_STEPSIZE", Array.Empty<string>());
                _stepSize = stepSize.MultiplyByValue(0);
            }
            else
            {
                _stepSize = stepSize;
            }
        }
        public virtual void IncrementStepsAttended() => UpdateStepsAttended(1);
        public virtual void DecrementStepsAttended() => UpdateStepsAttended(-1);
        public virtual void UpdateStepsAttendedBy(int steps) => UpdateStepsAttended(steps);
        public virtual void ResetStepsAttendedToZero() => _stepsAttended = 0;

        /* Unsafe/Debugging */
        public virtual int GremlinGetStepsAttended() => GetStepsAttended();
        public virtual TimeT<T>.DeltaTimeT GremlinGetAttendStepSize() => GetAttendStepSize();

        /* Utility */
        private int GetStepsAttended() => _stepsAttended;
        private TimeT<T>.DeltaTimeT GetAttendStepSize() => _stepSize;
        private void UpdateStepsAttended(int stepsAttended)
        {
           /* 
            * 2022-12-15: Incrementing _stepsAttended past the min value 
            * of int caused integer overflow. Added a try-catch block with 
            * checked keyword to recover from it, added new warning message
            * Found By: UpdateStepsAttendedByPositive
            */
            try
            {
                checked { _stepsAttended += stepsAttended; }
                if (_stepsAttended < 0)
                {
                    Utility.PrintMsg("W-A_NEGATIVE_ATTENTION", Array.Empty<string>());
                    _stepsAttended = 0;
                }
            }
            catch (OverflowException)
            {
                Utility.PrintMsg("W-A_OVERFLOW", Array.Empty<string>());
                _stepsAttended = int.MaxValue;
            }
        }
    }
}
