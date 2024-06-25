/* =======================================================================================================
 * Copyright (c) 2023 G. M. Smith and J. Carette
 * Released under the BSD 3-Clause license
 * .NET Framework 4.8.04084 \ .NET Standard Library 2.0.3 \ .NET Core Platforms 1.1.0 \ 
 * NuGet Package Manager 6.4.0 \ Windows 10.0.19044
 * =======================================================================================================
 * Emotion Intensity Function Module
 * -------------------------------------------------------------------------------------------------------
 * The Emotion Intensity Function Library collects methods for calculating emotion intensity changes. Each 
 * emotion in EmotionTypesT (see EmotionStateT.cs) has its own function to make it easier to use, reuse, 
 * and/or modify them individually. 
 * 
 * The functions are application-specific, as they are modelled on specific emotion theories and EMgine 
 * assumptions.
 * =======================================================================================================
 * Relies on: EmIntensityT, WorldT, GoalT (Type of S must match)
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * EmGenFunLib
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 89
 * Cyclomatic Complexity: 9
 * Depth of Inheritance: 1
 * Class Coupling: 10
 * Lines of Source Code: 79
 * Lines of Executable Code: 9
 * =======================================================================================================
 */

using System;

namespace EMgine
{
    public static class EmIntensityCalcLib<S> where S : IComparable<S>, IEquatable<S>
    {
        /* ---------------------------------------------------
        * ************* JOY *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcJoyIntensity(GoalT<S> g, WorldStateViewT<S>.DistChgBtwWorldStateViewsT dDelta)
        {
            return new EmIntensityT.EmIntensityChgT(Math.Abs(dDelta.ToReal()) * g.GetGoalImportance());
        }

        /* ---------------------------------------------------
        * ************* SADNESS *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcSadnessIntensity()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* FEAR *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcFearIntensity(GoalT<S> g, WorldStateViewT<S>.DistChgBtwWorldStateViewsT dDelta)
        {
            return new EmIntensityT.EmIntensityChgT(dDelta.ToReal() * g.GetGoalImportance());
        }

        public static EmIntensityT.EmIntensityChgT CalcFearIntensity(GoalT<S> g, GoalT<S> gLost, WorldStateViewT<S>.DistChgBtwWorldStateViewsT dDelta)
        {
            return new EmIntensityT.EmIntensityChgT(dDelta.ToReal() * (gLost.GetGoalImportance() / g.GetGoalImportance()));
        }

        /* ---------------------------------------------------
        * ************* ANGER *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcAngerIntensity()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* DISGUST *************
        * ---------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcDisgustIntensity(GoalT<S> g, WorldStateViewT<S>.DistBtwWorldStateViewsT d)
        {
            return new EmIntensityT.EmIntensityChgT(d.ToReal() * g.GetGoalImportance());
        }

        /* ---------------------------------------------------
        * ************* ACCEPTANCE *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcAcceptanceIntensity()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* INTEREST *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcInterestIntensity()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* SURPRISE *************
        * ----------------------------------------------------
        */
        public static EmIntensityT.EmIntensityChgT CalcSurpriseIntensity(double discrDelta, EmIntensityT.EmIntensityChgT maxDelta)
        {
            return new EmIntensityT.EmIntensityChgT(discrDelta * maxDelta.ToReal());
        }
    }
}
