/* =======================================================================================================
 * Copyright (c) 2023 G. M. Smith and J. Carette
 * Released under the BSD 3-Clause license
 * .NET Framework 4.8.04084 \ .NET Standard Library 2.0.3 \ .NET Core Platforms 1.1.0 \ 
 * NuGet Package Manager 6.4.0 \ Windows 10.0.19044
 * =======================================================================================================
 * Emotion Generation Module
 * -------------------------------------------------------------------------------------------------------
 * The functions are application-specific, as they are modelled on specific emotion theories and EMgine 
 * assumptions.
 * =======================================================================================================
 * Relies on: WorldT, GoalT (Type of S must match)
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * EmGenFunLib
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 80
 * Cyclomatic Complexity: 14
 * Depth of Inheritance: 1
 * Class Coupling: 12
 * Lines of Source Code: 107
 * Lines of Executable Code: 23
 * =======================================================================================================
 */

using System;
using System.Collections.Generic;

namespace EMgine
{
    public static class EmGenFunLib<S> where S : IComparable<S>, IEquatable<S>
    {
        /* ---------------------------------------------------
        * ************* JOY *************
        * ----------------------------------------------------
        */
        public static (WorldStateViewT<S>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<S>.DistBtwWorldStateViewsT distNow, WorldStateViewT<S>.DistChgBtwWorldStateViewsT distDelta)?
            GenerateJoy(GoalT<S> g, WorldStateViewT<S> sPrev, WorldStateViewT<S>.WorldEventT sDelta, WorldStateViewT<S>.DistChgBtwWorldStateViewsT epsilonJ)
        {
            (WorldStateViewT<S>.DistBtwWorldStateViewsT current, WorldStateViewT<S>.DistBtwWorldStateViewsT eventChange, WorldStateViewT<S>.DistChgBtwWorldStateViewsT dist2GoalChg)? joy = null;

            WorldStateViewT<S>.DistChgBtwWorldStateViewsT goalDistChg = g.Dist2GoalChg(sPrev, sDelta);

            if (Math.Abs(goalDistChg.ToReal()).CompareTo(epsilonJ.ToReal()) > 0)
            {
                WorldStateViewT<S>.DistBtwWorldStateViewsT currentDist = g.Dist2Goal(sPrev);
                WorldStateViewT<S>.DistBtwWorldStateViewsT newDist = g.Dist2Goal(sPrev.ApplyEvent(sDelta));

                if (Math.Abs(currentDist.ToReal()).CompareTo(Math.Abs(newDist.ToReal())) > 0) joy = (currentDist, newDist, goalDistChg);
            }

            return joy;
        }

        /* ---------------------------------------------------
        * ************* SADNESS *************
        * ----------------------------------------------------
        */
        public static void GenerateSadness()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* FEAR *************
        * ----------------------------------------------------
        */
        public static void GenerateFear()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* ANGER *************
        * ----------------------------------------------------
        */
        public static void GenerateAnger()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* DISGUST *************
        * ---------------------------------------------------
        */
        public static (WorldStateViewT<S>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<S>.DistBtwWorldStateViewsT distNow, WorldStateViewT<S>.DistChgBtwWorldStateViewsT distDelta)?
            GenerateDisgust(GoalT<S> g, WorldStateViewT<S> sPrev, WorldStateViewT<S>.WorldEventT sDelta, WorldStateViewT<S>.DistBtwWorldStateViewsT epsilonDS, WorldStateViewT<S>.DistBtwWorldStateViewsT epsilonDN)
        {
            (WorldStateViewT<S>.DistBtwWorldStateViewsT distPrev, WorldStateViewT<S>.DistBtwWorldStateViewsT distNow, WorldStateViewT<S>.DistChgBtwWorldStateViewsT distDelta)? disgust = null;

            bool isGustatory = false;
            GoalTypeT[] gTypes = g.GetGoalTypes();
            for (int t = 0; t < gTypes.Length; t++)
            {
                if (gTypes[t] == GoalTypeT.Gustatory)
                {
                    isGustatory = true;
                    break;
                }
            }

            if (isGustatory)
            {
                WorldStateViewT<S>.DistBtwWorldStateViewsT distPrev = g.Dist2Goal(sPrev);
                if (distPrev.CompareTo(epsilonDS) <= 0)
                {
                    WorldStateViewT<S>.DistBtwWorldStateViewsT distNow = g.Dist2Goal(sPrev.ApplyEvent(sDelta));
                    if (distNow.CompareTo(epsilonDS) > 0)
                    {
                        WorldStateViewT<S>.DistChgBtwWorldStateViewsT goalDistChg = g.Dist2GoalChg(sPrev, sDelta);
                        if (Math.Abs(goalDistChg.ToReal()).CompareTo(epsilonDN) > 0)
                        {
                            disgust = (distPrev, distNow, goalDistChg);
                        }
                    }
                }
            }
            
            return disgust;
        }

        /* ---------------------------------------------------
        * ************* ACCEPTANCE *************
        * ----------------------------------------------------
        */
        public static void GenerateAcceptance()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* INTEREST *************
        * ----------------------------------------------------
        */
        public static void GenerateInterest()
        {
            throw new NotImplementedException();
        }

        /* ---------------------------------------------------
        * ************* SURPRISE *************
        * ----------------------------------------------------
        */
        public static void GenerateSurprise()
        {
            throw new NotImplementedException();
        }
    }
}
