using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Action[] fixedActions;
    //public Transition[] transitions;
    
    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        //CheckTransitions(controller);
    }

    public void UpdateFixedState(StateController controller)
    {
        DoFixedActions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    void DoFixedActions(StateController controller)
    {
        for (int i = 0; i < fixedActions.Length; i++)
        {
            fixedActions[i].Act(controller);
        }
    }
}
