using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAIHugState : CreatureAIState
{

    public CreatureAIHugState(CreatureAI creatureAI) : base(creatureAI) { }


    public override void BeginState()
    {
        creatureAI.SetColor(Color.red);
    }

    public override void UpdateState()
    {
        if (creatureAI.GetTarget() != null)
        {
            //creatureAI.myCreature.MoveCreatureToward(creatureAI.GetTarget().transform.position);
            creatureAI.myCreature.transform.position += creatureAI.myCreature.transform.forward * 10.0f;
            Debug.Log("MoveCreatureTorward Called.");

        }
        else
        {
            creatureAI.ChangeState(creatureAI.investigateState);
        }

    }
}