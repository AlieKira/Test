using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : FSMState
{
    private Vector3 targetPos;
    private GameObject npc;
    private Rigidbody npcRd;
    



    public PatrolState(Vector3 pos, GameObject npc)
    {
        
        stateID = StateID.Patrol;
        targetPos = pos;
        this.npc = npc;
        npcRd = npc.GetComponent<Rigidbody>();
    }

    public override void DoBeforeEntering(GameObject player)
    {
        navAgent.destination = targetPos;
        targetPlayer = player;
        Debug.Log("Entering state " + ID);
    }

    public override void DoBeforeLeaving()
    {
        fsm.targetPlayer = this.targetPlayer;
    }

    public override void DoUpdate()
    {
        CheckTransition();
        PatrolMove();

    }
    //检查转换条件
    private void CheckTransition()
    {
        if (targetPlayer!=null)
        {
            fsm.PerformTransition(Transition.SawPlayer);
        }
        //if (Vector3.Distance(player.transform.position, npc.transform.position) < 5)
        //{
        //    fsm.PerformTransition(Transition.SawPlayer);
        //}
    }
    private void PatrolMove()
    {
        if (Vector3.Distance(npc.transform.position, targetPos) < 0.5)
        {
            navAgent.speed=0;
        }
        else
        {
            navAgent.speed = 3.5f;
        }


        
        //npcRd.velocity = npc.transform.forward * 3;
        //targetPos.y = npc.transform.position.y;
        //npc.transform.LookAt(targetPos);

    }
}