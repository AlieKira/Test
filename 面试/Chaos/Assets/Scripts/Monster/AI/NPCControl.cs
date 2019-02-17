using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : MonoBehaviour
{
    public GameObject targetPlayer=null;
    public List<GameObject> playerList;
    private MonsterData monsterData;
    private FSMSystem fsm;
    private NavMeshAgent navAgent;

    private Vector3 originalPos;

    private GameObject[] waypoints;

    // Use this for initialization
    void Start()
    {
       // originalPos = this.gameObject.transform.position;
        playerList =new List<GameObject>();
        monsterData = this.gameObject.GetComponent<MonsterData>();
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        InitFSM();
    }

    /// <summary>
    /// 初始化状态机
    /// </summary>
    public void InitFSM()
    {
        fsm = new FSMSystem();

        PatrolState patrolState = new PatrolState(this.gameObject.transform.position, this.gameObject);
        patrolState.AddTransition(Transition.SawPlayer, StateID.Chase);
        patrolState.SetNavAgent(navAgent);

        ChaseState chaseState = new ChaseState(this.gameObject);
        chaseState.AddTransition(Transition.LostPlayer, StateID.Patrol);
        chaseState.SetNavAgent(navAgent);
        this.transform.GetComponent<SphereCollider>().enabled = true;



        fsm.AddState(patrolState);
        fsm.AddState(chaseState);

        fsm.Start(StateID.Patrol);
    }
    void FixedUpdate()
    {
        fsm.CurrentState.DoUpdate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag=="Player"|| collider.gameObject.tag == "Enemy")
        {
            playerList.Add(collider.gameObject);
            float i = 0;
            GameObject originPlayer = targetPlayer;
            foreach (GameObject temp in playerList)
            {
                float x = Vector3.Distance(this.gameObject.transform.position, collider.gameObject.transform.position);
                if (x > i)
                {
                    i = x;
                    targetPlayer = collider.gameObject;
                }
            }

            if (originPlayer != targetPlayer)
            {
                fsm.CurrentState.SetTargetPlayer(targetPlayer);
            }
        } 
    }

    private void OnTriggerExit(Collider collider)
    {
        playerList.Remove(collider.gameObject);
        float i = 0;
        GameObject originPlayer = targetPlayer;
        if (collider.gameObject==targetPlayer)
        {
            targetPlayer = null;
            foreach (GameObject temp in playerList)
            {
                float x = Vector3.Distance(this.gameObject.transform.position, temp.transform.position);
                if (x > i)
                {
                    i = x;
                    targetPlayer = temp;
                }
            }
        }

        //if (PlayerList.Count<=0)
        //{
        //    targetPlayer = null;
        //}
        if (originPlayer != targetPlayer)
        {
            fsm.CurrentState.SetTargetPlayer(targetPlayer);
        }

    }

    public void TakeDamage(int damage)
    {
         transform.GetComponent<MonsterData>().reduceHP(damage);
    }

    public void ClearTarget()
    {
        targetPlayer = null;
        if (playerList!=null)
        {
            playerList.Clear();
        }

    }
}
