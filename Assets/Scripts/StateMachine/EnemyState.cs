using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/State")]
public class EnemyState : ScriptableObject {

    public Action[] actions;
    public Action[] fixed_actions;

	public void UpdateState(StateController controller)
    {
        DoActions(controller);
    }

    private void DoActions(StateController controller)
    {
        for(int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    public void FixedActions(StateController controller)
    {
        for(int i = 0; i < fixed_actions.Length; i++)
        {
            fixed_actions[i].Act(controller);
        }
    }
}
