/* =======================================================================================================
 * Copyright (c) 2023 G. M. Smith and J. Carette
 * Released under the BSD 3-Clause license
 * .NET Framework 4.8.04084 \ .NET Standard Library 2.0.3 \ .NET Core Platforms 1.1.0 \ 
 * NuGet Package Manager 6.4.0 \ Windows 10.0.19044
 * =======================================================================================================
 * World View Generic Abstract Class
 * -------------------------------------------------------------------------------------------------------
 * The World View Generic Abstract Class is for providing a specification for the World State types. Users
 * provide the implementation so it is tailored for their needs.
 * =======================================================================================================
 * Relies on: ---
 * =======================================================================================================
 * Notes: Equals() Method follows the conventions 
 * 
 *          x > y returns c > 0
 *          x = y returns c = 0
 *          x < y returns c < 0
 * 
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * WorldStateViewT<S>
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 4
 * Depth of Inheritance: 1
 * Class Coupling: 3
 * Lines of Source Code: 80
 * Lines of Executable Code: 0
 * -------------------------------------------------------------------------------------------------------
 * WorldStateViewT<S>.WorldEventT
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 1
 * Depth of Inheritance: 1
 * Class Coupling: 0
 * Lines of Source Code: 10
 * Lines of Executable Code: 0
 * -------------------------------------------------------------------------------------------------------
 * WorldStateViewT<S>.DistBtwWorldStateViewsT
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 4
 * Depth of Inheritance: 1
 * Class Coupling: 1
 * Lines of Source Code: 19
 * Lines of Executable Code: 0
 * -------------------------------------------------------------------------------------------------------
 * WorldStateViewT<S>.DistChgBtwWorldStateViewsT
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 3
 * Depth of Inheritance: 1
 * Class Coupling: 0
 * Lines of Source Code: 15
 * Lines of Executable Code: 0
 * =======================================================================================================
 */

using System;

namespace EMgine
{
    /* =====================================================
     * World State View Type
     * =====================================================
     */

    /* 
     * 2022-12-06: Declaring Time and DeltaTime as interfaces did not have the intended
     * behaviour when implementing them with concrete values and there was no explicitly
     * enforced DeltaTime dependency on Time. Since this also affects the World State
     * types, they were also switched to abstract classes and WorldEventT, 
     * DistBtwWorldStateViewsT, and DistChgBtwWorldStateViewsT were nested in 
     * WorldStateViewT.
     * Found By: Test_ITime_Implement_Exists
     *           Test_IDeltaTime_Implement_Exists
     */
    public abstract class WorldStateViewT<S>
    {
        // Returns a new WorldStateViewT resulting from the application of 
        // WorldEventT e to this WorldStateViewT
        public abstract WorldStateViewT<S> ApplyEvent(WorldEventT e);

        // Returns the difference/distance between this WorldStateViewT and
        // another WorldStateViewT w as a distance between them
        public abstract DistBtwWorldStateViewsT CalcDistanceTo(WorldStateViewT<S> w);

        // Returns the magnitude of distance change that WorldEventT e causes in
        // this WorldStateViewT
        public abstract DistChgBtwWorldStateViewsT CalcChangeCausedBy(WorldEventT e);

        // Returns True if this WorldStateViewT and WorldStateViewT w are
        // the "same" as determined by the user
        public abstract bool Equals(WorldStateViewT<S> w);

        /* =====================================================
         * World Event Type
         * =====================================================
         */
        public abstract class WorldEventT
        {
            // Returns the probability of this event happening
            // as a value in [0,1]
            public abstract double GetEventProbability();
        }

        /* =====================================================
         * Distance Between Two World State Views Type
         * =====================================================
         */
        public abstract class DistBtwWorldStateViewsT
        {
            // Compare this DistBtwWorldStateViewsT to otherDist
            public abstract int CompareTo(DistBtwWorldStateViewsT otherDist);

            // Returns the difference between this DistBtwWorldStateViewsT and
            // DistBtwWorldStateViewsT otherDist
            public abstract DistChgBtwWorldStateViewsT Difference(DistBtwWorldStateViewsT otherDist);

            // Returns True if DistBtwWorldStateViewsT is a finite value
            // (i.e. an infinite distance cannot be traversed)
            public abstract bool IsFinite();

            public abstract double ToReal();
        }

        /* =====================================================
         * Change in Distance Between Two World State Views Type
         * =====================================================
         */
        public abstract class DistChgBtwWorldStateViewsT
        {
            // Compare this DistChgBtwWorldStateViewsT to DistChgBtwWorldStateViewsT otherDistChg
            public abstract int CompareTo(DistChgBtwWorldStateViewsT otherDistChg);

            // Returns the difference between this DistChgBtwWorldStateViewsT and
            // DistChgBtwWorldStateViewsT chg
            public abstract DistChgBtwWorldStateViewsT Difference(DistChgBtwWorldStateViewsT chg);

            public abstract double ToReal();
        }
    }
}