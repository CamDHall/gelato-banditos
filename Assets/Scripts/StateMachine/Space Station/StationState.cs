using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Station AI/StationState")]
public class StationState : ScriptableObject {

    public StationAction[] actions;
    public StationTransition[] transitions;

	public void UpdateState(StationController controller)
    {
        CheckTransitions(controller);
        DoActions(controller);
    }

    void DoActions(StationController controller)
    {
        if (!controller.aiActive) return;
        foreach(StationAction action in actions)
        {
            action.Act(controller);
        }
    }

    public void CheckTransitions(StationController controller)
    {
        foreach(StationTransition transition in transitions)
        {
            int val = transition.decision.StandingDecision(controller);
            if (val == 0)
            {
                controller.TransitionToState(transition.gaurdState);
            }
            else if (val < 0)
            {
                controller.TransitionToState(transition.attackstate);
            }
            else
            {
                controller.TransitionToState(transition.receiveState);
            }
        }
    }
}
