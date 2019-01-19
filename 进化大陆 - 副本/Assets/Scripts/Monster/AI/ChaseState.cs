using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Common;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : FSMState
{
    private bool sendAttackRequest=false;
    private bool isAttack = false;
    private GameObject npc;
    private Rigidbody npcRd;
    private float attackDis = 3f;
    private GameFacade gameFacade;
    private GameObject afterPlayer;

    public ChaseState(GameObject npc)
    {
        gameFacade = GameObject.Find("GameFacade").GetComponent<GameFacade>();
        stateID = StateID.Chase;
        this.npc = npc;
        npcRd = npc.GetComponent<Rigidbody>();
    }
    public override void DoBeforeEntering(GameObject player)
    {
        targetPlayer = player;
        navAgent.destination = targetPlayer.transform.position;
        Debug.Log("Entering state " + ID);
    }

    public override void DoBeforeLeaving()
    {
        fsm.targetPlayer = this.targetPlayer;
        navAgent.speed = 3.5f;
    }

    public override void DoUpdate()
    {
        CheckTransition();
        ChaseMove();
        if (sendAttackRequest)
        {
            gameFacade.SendRequest(RequestCode.Game,ActionCode.MonsterAttack, afterPlayer.GetComponent<RoleData>().UserID.ToString());
            navAgent.speed = 3.5f;
            sendAttackRequest = false;
            isAttack = false;
        }
    }
    private void CheckTransition()
    {
        if (targetPlayer==null|| Vector3.Distance(targetPlayer.transform.position, npc.transform.position) > 10||
            targetPlayer.gameObject.tag=="Finish")
        {
            npc.GetComponentInChildren<NPCControl>().playerList.Remove(targetPlayer);
            this.targetPlayer = null;
            fsm.PerformTransition(Transition.LostPlayer);
        }
    }

    

    private void ChaseMove()
    {
        if (targetPlayer==null)
        {
            return;
        }
        navAgent.destination = targetPlayer.transform.position;
        if (Vector3.Distance(npc.transform.position, targetPlayer.transform.position) > attackDis)
        {
            return;
        }
        else
        {
            
            if (!isAttack)
            {
                Attack();
            }
            
            
        }
        //
        //Debug.Log(11);
        //Debug.Log(npc.gameObject.name);
        //npcRd.velocity = npc.transform.forward * 5;
        //Vector3 targetposition = player.transform.position;
        //targetposition.y = npc.transform.position.y;
        //npc.transform.LookAt(targetposition);
        //npcRd.velocity = new Vector3(0, 0, 1) * 5;
    }

    private void Attack()
    {
        isAttack = true;
        npc.transform.LookAt(targetPlayer.transform.position);
        navAgent.speed = 0;
        npc.GetComponent<Animator>().SetTrigger("Attack");
        Thread newThread = new Thread(AfterAttack);
        afterPlayer = targetPlayer;
        newThread.Start();
        //Debug.Log(targetPlayer.GetComponent<RoleData>().UserID);
        //gameFacade.MonsterAttack(npc.GetComponent<MonsterData>().number, targetPlayer.GetComponent<RoleData>().UserID);
        //navAgent.speed = 3.5f;
    }

    private void AfterAttack()
    {
        
        Thread.Sleep(1000);
        sendAttackRequest = true;
    }
}
