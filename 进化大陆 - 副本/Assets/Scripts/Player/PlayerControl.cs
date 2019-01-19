using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    private RoleData roleData;
    private Animator animator;
    private Vector3 mTargetPos;
    private GameObject attackTarget;
    public float attackDis = 3;
    public float originDis = 0.2f;
    public float stopDis = 0.2f;
    public bool isAttack = false;
    public bool isAttacking = false;
    public AttackRequest attackRequest;
    private bool isSyncAttack = false;

    private void Start()
    {
        roleData = GetComponent<RoleData>();
        animator = GetComponent<Animator>();
        attackRequest = GetComponent<AttackRequest>();
        mTargetPos = transform.position;
    }


    void Update()
    {
        if (roleData.isLocal == false)
        {
            return;
        }
        //按下鼠标右键时
        if (Input.GetMouseButton(1) && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                                      animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
        {
            isAttack = false;
            //获取屏幕坐标
            Vector3 mScreenPos = Input.mousePosition;
            //定义射线
            Ray mRay = Camera.main.ScreenPointToRay(mScreenPos);
            RaycastHit mHit;
            //判断射线是否击中地面
            if (Physics.Raycast(mRay, out mHit))
            {
                if (mHit.collider.gameObject.tag == "Terrain")
                {
                    //获取目标坐标
                    mTargetPos.x = mHit.point.x;
                    mTargetPos.y = transform.position.y;
                    mTargetPos.z = mHit.point.z;
                    //让主角面朝目标坐标并向目标移动
                    transform.LookAt(mTargetPos);
                    //播放奔跑动画
                    transform.gameObject.GetComponent<Animator>().SetBool("Run", true);

                }
                //点击目标为敌人或怪物
                if (mHit.collider.gameObject.tag == "Monster" || mHit.collider.gameObject.tag == "Enemy")
                {
                    if (isAttacking == true)
                    {
                        return;
                    }

                    attackTarget = mHit.collider.gameObject;
                    isAttack = true;
                    stopDis = attackDis;
                    //获取目标坐标
                    mTargetPos.x = mHit.point.x;
                    mTargetPos.y = transform.position.y;
                    mTargetPos.z = mHit.point.z;
                    //让主角面朝目标坐标并向目标移动
                    transform.LookAt(mTargetPos);
                    mTargetPos = mHit.collider.gameObject.transform.position;
                    float enemyDistance = Vector3.Distance(transform.position, mHit.collider.gameObject.transform.position);

                    if (enemyDistance <= attackDis)
                    {
                        //播放攻击动画
                        Attack();
                        mTargetPos = transform.position;
                    }
                    else
                    {
                        transform.gameObject.GetComponent<Animator>().SetBool("Run", true);
                    }

                }
            }
        }

        if (transform.position == mTargetPos)
        {
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (Vector3.Distance(transform.position, mTargetPos) > stopDis)
            {
                mTargetPos.y = transform.position.y;
                transform.LookAt(mTargetPos);
                transform.Translate(Vector3.forward * 0.1F);
            }
            else
            {
                if (isAttack == true)
                {
                    Attack();
                    mTargetPos = transform.position;
                    stopDis = originDis;
                    isAttack = false;
                }
                else
                {
                    mTargetPos = transform.position;
                    animator.SetBool("Run", false);
                }
            }
        }

        if (isSyncAttack==true)
        {
            SyncAttack();
            isSyncAttack = false;
        }

    }

    private void Attack()
    {
        transform.gameObject.GetComponent<Animator>().SetBool("Run", false);
        isAttacking = true;
        animator.SetBool("Melee Right Attack 02", true);
        Invoke("AfterAttack", 1);
    }

    private void AfterAttack()
    {
        animator.SetBool("Melee Right Attack 02", false);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Right Attack 02"))
        {
            stopDis = originDis;
            if (attackTarget.tag == "Monster")
            {
                attackRequest.SendRequest(attackTarget.transform.GetComponent<MonsterData>().Number, false);
            }
            else
            {
                attackRequest.SendRequest(attackTarget.transform.GetComponent<RoleData>().UserID, true);
            }
        }

        isAttacking = false;
    }

    private void SyncAttack()
    {
        transform.gameObject.GetComponent<Animator>().SetBool("Run", false);
        isAttacking = true;
        animator.SetBool("Melee Right Attack 02", true);
        Invoke("SyncAfterAttack", 1);
    }

    private void SyncAfterAttack()
    {
        animator.SetBool("Melee Right Attack 02", false);
        
    }

    public void TakeDamage()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Right Attack 02"))
        {
            animator.SetTrigger("Take Damage");
        }
    }

    public void OnAttackResponse()
    {
        isSyncAttack = true;
    }
}
