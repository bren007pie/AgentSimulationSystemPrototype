/* =======================================================================================================
 * Copyright (c) 2023 G. M. Smith and J. Carette
 * Released under the BSD 3-Clause license
 * .NET Framework 4.8.04084 \ .NET Standard Library 2.0.3 \ .NET Core Platforms 1.1.0 \ 
 * NuGet Package Manager 6.4.0 \ Windows 10.0.19044
 * =======================================================================================================
 * Goal ADT
 * -------------------------------------------------------------------------------------------------------
 * The Goal ADT defines the goal type $\goaltype$ and a helper type for specifying a goal's type as 
 * Self-Preservation and/or Gustatory.
 *
 * The Goal data type contains a World State View representing the entity's desired world state, a real 
 * value denoting the perceived importance of achieving/maintaining the desired goal state, and a sequence 
 * of Boolean values marking the goal as Self-Preservation, Gustatory, both, or neither.
 * 
 * Goal importance is non-negative, where a value of zero (0) means that the goal is not important to the 
 * entity and should not be considered.
 * 
 * This design is application-agnostic and can exist independently of EMgine iff the World Type Module or 
 * equivalent are available.
 * =======================================================================================================
 * Relies on: WorldT (Type of S must match)
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * GoalTypeT
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 100
 * Cyclomatic Complexity: 1
 * Depth of Inheritance: 1
 * Class Coupling: 0
 * Lines of Source Code: 9
 * Lines of Executable Code: 0
 * -------------------------------------------------------------------------------------------------------
 * GoalT<S>
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 81
 * Cyclomatic Complexity: 17
 * Depth of Inheritance: 1
 * Class Coupling: 12
 * Lines of Source Code: 84
 * Lines of Executable Code: 27
 * =======================================================================================================
 */

using System;
using System.Collections.Generic;

namespace EMgine
{
    /* ---------------------------------------------------
     * Goal Types Type
     * ---------------------------------------------------
     */
    public enum GoalTypeT
    {
        SelfPreservation,
        Gustatory
    }

    /* ---------------------------------------------------
     * Goal Type
     * ---------------------------------------------------
     */
    public class GoalT<S> where S : IComparable<S>, IEquatable<S>
    {
        /* Constants */
        private const bool _GOAL_TYPE_DEFAULT = false;

        /* State Variables */
        private WorldStateViewT<S> _goalState;
        private double _importance;
        private readonly bool[] _gType;

        /* Utility Variables */
        private static readonly int _NUM_GOAL_TYPES = Enum.GetNames(typeof(GoalTypeT)).Length;

        /* ---------------------------------------------------
        * METHODS
        * ---------------------------------------------------
        */

        /* Constructors */
        public GoalT(WorldStateViewT<S> g, double impt)
        {
            ChangeGoalState(g);
            AssignImportance(impt);
            _gType = new bool[_NUM_GOAL_TYPES];
            for (int i = 0; i < _NUM_GOAL_TYPES; i++) _gType[i] = _GOAL_TYPE_DEFAULT;
        }

        public GoalT(WorldStateViewT<S> g, double impt, GoalTypeT[] gTypes)
        {
            ChangeGoalState(g);
            AssignImportance(impt);
            _gType = new bool[_NUM_GOAL_TYPES];
            ChangeGoalTypes(gTypes);
        }

        /* Accessors */
        public WorldStateViewT<S> GetGoalState() => _goalState;
        public double GetGoalImportance() => _importance;
        public GoalTypeT[] GetGoalTypes()
        {
            List<GoalTypeT> gTypes = new List<GoalTypeT>();

            foreach (GoalTypeT g in Enum.GetValues(typeof(GoalTypeT)))
                if (_gType[(int)g]) gTypes.Add(g);

            return gTypes.ToArray();
        }

        public bool IsSatisfiedBy(WorldStateViewT<S> worldState) => _goalState.Equals(worldState);
        public WorldStateViewT<S>.DistBtwWorldStateViewsT Dist2Goal(WorldStateViewT<S> worldState) => worldState.CalcDistanceTo(_goalState);
        public WorldStateViewT<S>.DistChgBtwWorldStateViewsT Dist2GoalChg(WorldStateViewT<S> worldState, WorldStateViewT<S>.WorldEventT worldEvent)
        {
            WorldStateViewT<S>.DistBtwWorldStateViewsT currentDist2Goal = Dist2Goal(worldState);
            WorldStateViewT<S>.DistBtwWorldStateViewsT potentialDist2Goal = Dist2Goal(worldState.ApplyEvent(worldEvent));

            return currentDist2Goal.Difference(potentialDist2Goal);
        }

        /* Mutators */
        public void ChangeGoalState(WorldStateViewT<S> g) => _goalState = g;
        public void ChangeImportance(double impt) => AssignImportance(impt);
        public void ChangeGoalTypes(GoalTypeT[] gTypes)
        {
            for (int ty = 0; ty < gTypes.Length; ty++) _gType[(int)gTypes[ty]] = true;
        }

        /* Utility */
        private void AssignImportance(double impt)
        {
            if (impt < 0)
            {
                Utility.PrintMsg("W-G_IMPORTANCE_IS_NEGATIVE", Array.Empty<string>());
                _importance = 0;
            }
            else
            {
                _importance = impt;
            }
        }
    }
}
