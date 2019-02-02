using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Anim_Monster : MonoBehaviour
{
    private GameObject attackTarget;
    private bool isAttack = false;
    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (attackTarget==null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, attackTarget.transform.position) > 2)
        {
            if (navAgent.speed == 0)
            {
                navAgent.speed = 3.5f;
            }

            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Forward"))
            {
                GetComponent<Animator>().SetBool("Forward", true);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("Forward", false);
            navAgent.speed = 0;
        }
    }

    private void OnTriggerStay(Collider collider)
    {

        if (isAttack)
        {
            return;
        }
        if (collider.gameObject.tag == "Player")
        {
            attackTarget = collider.gameObject;
            navAgent.destination = attackTarget.transform.position;
            transform.LookAt(collider.gameObject.transform);
            isAttack = true;
            GetComponent<Animator>().SetTrigger("Attack");
            collider.gameObject.GetComponent<Anim_PlayerControl>().TakeDamage();
            Thread newThread = new Thread(AfterAttack);
            newThread.Start();
        }
    }

    private void AfterAttack()
    {
        Thread.Sleep(1000);
        isAttack = false;
    }
}
