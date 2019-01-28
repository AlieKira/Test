using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private RoleData roleData;
    private Animator animator;
    private Vector3 mTargetPos;
    private Transform attackTarget;
    private float attackDis = 3;
    public float originDis = 0.2f;
    public bool isAttack = false;
    public bool isAttacking = false;
    private bool isSyncAttack = false;

    private void Start()
    {
        roleData = GetComponent<RoleData>();
        animator = GetComponent<Animator>();
        mTargetPos = transform.position;
    }


    protected void FixedUpdate()
    {

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
                    if (isAttack)
                    {
                        isAttack = false;
                    }
                }
                //点击目标为敌人或怪物
                if (mHit.collider.gameObject.tag == "Monster" || mHit.collider.gameObject.tag == "Enemy")
                {
                    if (isAttacking == true)
                    {
                        return;
                    }

                    attackTarget = mHit.collider.gameObject.transform.parent;
                    isAttack = true;
                    //stopDis = attackDis;
                    //让主角面朝目标坐标并向目标移动
                    transform.LookAt(attackTarget.transform.position);
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
            if (isAttack)
            {
                if (isAttacking)
                {
                    return;
                }
                if (Vector3.Distance(transform.position, attackTarget.transform.position) > attackDis)
                {
                    transform.LookAt(attackTarget.transform.position);
                    transform.Translate(Vector3.forward * Time.deltaTime * 6);
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, mTargetPos) > originDis)
                {
                    transform.LookAt(mTargetPos);
                    transform.Translate(Vector3.forward * Time.deltaTime * 6);
                }
                else
                {

                    mTargetPos = transform.position;
                    animator.SetBool("Run", false);
                }
            }
        }
    }

    protected virtual void Attack()
    {
        if (attackTarget==null||attackTarget.tag=="Finish")
        {
            mTargetPos = this.transform.position;
            animator.SetBool("Melee Right Attack 02", false);
            isAttack = false;
            return;
        }
        isAttacking = true;
        transform.gameObject.GetComponent<Animator>().SetBool("Run", false);
        animator.SetBool("Melee Right Attack 02", true);
        Invoke("AfterAttack", 1);
    }

    protected virtual void AfterAttack()
    {
        animator.SetBool("Melee Right Attack 02", false);
        if (attackTarget == null || attackTarget.tag == "Finish")
        {
            return;
        }
        GameManager.Instance.PlayNormalSound(Audios.Sound_ShootPerson);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Right Attack 02"))
        {
            if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDis)
            {
                GameManager.Instance.MonsterTakeDamage(attackTarget.gameObject);
            }
        }

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isAttack)
        {
            animator.SetTrigger("Take Damage");
            
        }
        roleData.reduceHP(damage);
    }
}
