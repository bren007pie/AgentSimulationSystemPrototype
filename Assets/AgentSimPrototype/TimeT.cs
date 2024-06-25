/* =======================================================================================================
 * Copyright (c) 2023 G. M. Smith and J. Carette
 * Released under the BSD 3-Clause license
 * .NET Framework 4.8.04084 \ .NET Standard Library 2.0.3 \ .NET Core Platforms 1.1.0 \ 
 * NuGet Package Manager 6.4.0 \ Windows 10.0.19044
 * =======================================================================================================
 * Time Generic Abstract Class
 * -------------------------------------------------------------------------------------------------------
 * The Time Generic Abstract Class is for providing a specification for the Time and Delta Time types. 
 * Users provide the implementation so it is tailored for their needs.
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
 * TimeT<T>
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 2
 * Depth of Inheritance: 1
 * Class Coupling: 5
 * Lines of Source Code: 50
 * Lines of Executable Code: 0
 * -------------------------------------------------------------------------------------------------------
 * TimeT<T>.DeltaTimeT
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 8
 * Depth of Inheritance: 1
 * Class Coupling: 0
 * Lines of Source Code: 30
 * Lines of Executable Code: 0
 * =======================================================================================================
 */

using System;

namespace EMgine
{
    /* ===================================================
     * Time Type
     * ===================================================
     */

    /* 
     * 2022-12-06: Declaring Time and DeltaTime as interfaces did not have the intended
     * behaviour when implementing them with concrete values and there was no explicitly
     * enforced DeltaTime dependency on Time. Switched them to abstract classes and
     * nested DeltaTime in Time.
     * Found By: Test_ITime_Implement_Exists
     *           Test_IDeltaTime_Implement_Exists
     */
    public abstract class TimeT<T> where T : IComparable<T>, IEquatable<T>, IConvertible, IFormattable
    {
        /* Accessors */
        public abstract bool IsAfter(TimeT<T> t); // Returns True this TimeT comes after t
        public abstract DeltaTimeT Difference(TimeT<T> t); // Returns the difference between this TimeT and t

        /* ===================================================
         * Delta Time Type
         * ===================================================
         */
        public abstract class DeltaTimeT
        {
            /* Accessors */
            public abstract DeltaTimeT SumWith(DeltaTimeT dt); // Adds two DeltaTimeT elements
            public abstract DeltaTimeT Difference(DeltaTimeT dt); // Subtracts two IDeltaTimeTs
            public abstract DeltaTimeT MultiplyByValue(double v); // Multiplies this DeltaTimeT with a real value
            public abstract int CompareTo(DeltaTimeT dt); // Compare this DeltaTimeT to dt
            public abstract bool IsNegative(); // Returns True if this DeltaTimeT is less than zero
            public abstract bool IsZero(); // Returns True if this DeltaTimeT is equal to zero
            /* 
             * 2022-12-22: Need access to the internal value of DeltaTime objects for 
             * mathematical manipulation. Adding new method signature that returns the
             * internal value that has some unknown type T.
             * Found By: DecayingIntensities (Test_EmIntensityDecayT)
             */
            public abstract T GetAsValue(); // Get delta time as a computable value
            /* 
             * 2023-01-01: Some methods need to manipulate DeltaTimeT objects, which
             * could cause unintended changes due to pass-by-reference. Copies must
             * be made before using them to prevent this. Copies are identical to the
             * original DeltaTimeT object in terms of the values assigned to class
             * variables.
             * Found By:  (Test_EmIntensityDecayT)
             */
            public abstract DeltaTimeT Copy();
        }
    }
}
