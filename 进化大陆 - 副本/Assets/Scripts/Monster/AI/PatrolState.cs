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
        navAgent.speed = 3.5f;
        navAgent.destination = targetPos;
        targetPlayer = player;
    }

    public override void DoBeforeLeaving()
    {
        fsm.targetPlayer = this.targetPlayer;
        npc.GetComponent<Animator>().SetBool("Forward", false);
    }

    public override void DoUpdate()
    {
        PatrolMove();
        CheckTransition();

    }
    //检查转换条件
    private void CheckTransition()
    {
        if (targetPlayer != null)
        {
            fsm.PerformTransition(Transition.SawPlayer);
        }
    }

    private void PatrolMove()
    {
        if (npc.transform.position == targetPos)
        {
            return;
        }
        if (Vector3.Distance(npc.transform.position, targetPos) > 0.2f)
        {
            if (!npc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Forward"))
            {
                npc.GetComponent<Animator>().SetBool("Forward", true);
            }
        }
        else
        {
            npc.GetComponent<Animator>().SetBool("Forward", false);
            npc.transform.position = targetPos;
        }
    }
}