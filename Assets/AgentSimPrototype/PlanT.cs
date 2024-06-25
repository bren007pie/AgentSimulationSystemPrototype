/* -------------------------------------------------------------------------------------------------------
 * Plan Generic ADT
 * -------------------------------------------------------------------------------------------------------
 * The Plan Generic ADT is for defining entity plans $\plantype$ as a sequence of world states. Each world 
 * state in the sequence is a ``step'' in the plan achievable by a world event that alters the current 
 * world state to a plan ``step''.
 * 
 * The Plan ADT does NOT use GoalT(T) directly so that it can work for any target world state, which might 
 * not be part of a GoalT(T) structure. It is possible to check if a plan moves towards achieving a goal 
 * by passing the goal state into the IsPlanUseful method.
 * -------------------------------------------------------------------------------------------------------
 * Relies on: IWorld (Type of S must match)
 * -------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;

namespace EMgine
{
    public class PlanT<S> where S : IComparable<S>, IEquatable<S>
    {
        /* State Variables */
        private List<WorldStateViewT<S>> plan;

        /* ---------------------------------------------------
        * METHODS
        * ---------------------------------------------------
        */

        /* Constructor */
        public PlanT(List<WorldStateViewT<S>> p) { plan = p; }

        /* Accessors */
        public List<WorldStateViewT<S>> GetPlan() { return plan; }
        public int GetNumStatesInPlan() { return plan.Count; }

        public bool IsPlanUseful(WorldStateViewT<S> targetState)
        {
            bool useful = false;
            WorldStateViewT<S>.DistBtwWorldStateViewsT start = plan[0].CalcDistanceTo(targetState);
            WorldStateViewT<S>.DistBtwWorldStateViewsT end = plan[GetNumStatesInPlan() - 1].CalcDistanceTo(targetState);

            if (end.CompareTo(start) < 0) useful = true;

            return useful;
        }

        public bool IsPlanFeasible(List<WorldStateViewT<S>.WorldEventT> worldEvents)
        {
            bool feasible = true;
            WorldStateViewT<S> tmp;

            if (worldEvents.Count < GetNumStatesInPlan() - 1)
            {
                Utility.PrintMsg("E-P_TOO_FEW_EVENTS", Array.Empty<string>());
                return !feasible;
            }

            for (int i = 0; i < GetNumStatesInPlan() - 1; i++)
            {
                for (int j = 0; j < worldEvents.Count; j++)
                {
                    tmp = plan[i].ApplyEvent(worldEvents[j]);
                    // We didn't reach the next plan step, try next event
                    if (!tmp.Equals(plan[i+1])) feasible = false;
                }
                // No element in worldEvents moves us to the next plan step
                if (!feasible) return feasible;
            }
            return feasible;
        }

        /* Mutators */
        public void UpdatePlanAt(int i, WorldStateViewT<S> s)
        {
            if (i < 0 || i >= GetNumStatesInPlan())
            {
                Utility.PrintMsg("E-P_OUT_OF_BOUNDS", new string[] { i.ToString(), GetNumStatesInPlan().ToString() });
                return;
            }

            plan.Insert(i, s);
        }
    }
}
