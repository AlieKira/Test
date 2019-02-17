using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : FSMState
{
    private bool sendAttackRequest = false;
    private bool isAttack = false;
    private GameObject npc;
    private Rigidbody npcRd;
    private float attackDis = 3f;
    private GameObject afterPlayer;
    private bool isSetSpeed = false;
    private bool isAfterAttack = false;
    private NPCControl control;

    public ChaseState(GameObject npc)
    {
        stateID = StateID.Chase;
        this.npc = npc;
        npcRd = npc.GetComponent<Rigidbody>();
    }
    public override void DoBeforeEntering(GameObject player)
    {
        navAgent.speed = 3.5f;
        targetPlayer = player;
        navAgent.destination = targetPlayer.transform.position;
    }

    public override void DoBeforeLeaving()
    {
        fsm.targetPlayer = this.targetPlayer;

    }

    public override void DoUpdate()
    {
        CheckTransition();
        ChaseMove();
        if (isAfterAttack)
        {
            if (targetPlayer==null)
            {
                return;
            }
            if (Vector3.Distance(npc.transform.position, targetPlayer.transform.position) <= attackDis)
            {
                EventCenter.Broadcast(EventType.RoleTakeDamage);
            }

            isAfterAttack = false;
        }
    }
    private void CheckTransition()
    {
        if (targetPlayer == null || Vector3.Distance(targetPlayer.transform.position, npc.transform.position) > 10 ||
            targetPlayer.gameObject.tag == "Finish")
        {
            npc.GetComponentInChildren<NPCControl>().playerList.Remove(targetPlayer);
            this.targetPlayer = null;
            fsm.PerformTransition(Transition.LostPlayer);
        }
    }



    private void ChaseMove()
    {
        if (targetPlayer == null)
        {
            return;
        }

        navAgent.destination = targetPlayer.transform.position;
        if (Vector3.Distance(npc.transform.position, targetPlayer.transform.position) > attackDis)
        {
            if (navAgent.speed == 0)
            {
                navAgent.speed = 3.5f;
            }

            if (!npc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Forward"))
            {
                npc.GetComponent<Animator>().SetBool("Forward", true);
            }
        }
        else
        {

            if (isAttack == false)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        navAgent.speed = 0;
        npc.GetComponent<Animator>().SetBool("Forward", false);
        isAttack = true;
        npc.transform.LookAt(targetPlayer.transform.position);
        npc.GetComponent<Animator>().SetTrigger("Attack");
        Thread newThread = new Thread(AfterAttack);
        afterPlayer = targetPlayer;
        newThread.Start();
    }

    private void AfterAttack()
    {
        Thread.Sleep(1000);
        isAttack = false;
        isAfterAttack = true;
    }
}
