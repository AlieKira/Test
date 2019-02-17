using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    protected RoleData roleData;
    protected Animator animator;
    protected Vector3 mTargetPos;
    protected Transform attackTarget;
    protected Touch touch;
    protected float attackDis;
    public float originDis = 0.2f;
    public bool isAttack = false;
    public bool isAttacking = false;
    protected bool isSyncAttack = false;

    protected virtual void Start()
    {
        roleData = GetComponent<RoleData>();
        animator = GetComponent<Animator>();
        mTargetPos = transform.position;
        attackDis = transform.GetComponent<RoleData>().attackDis;
    }


    protected void FixedUpdate()
    {
        if (isAttacking)
        {
            return;
        }

        Vector3 mScreenPos = new Vector3();
#if UNITY_EDITOR
        if (Input.GetMouseButton(1) && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
        {
            mScreenPos = Input.mousePosition;
#else
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
         return;
        } 
        if (Input.touchCount > 0 && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
            touch = Input.touches[0];
            //获取屏幕坐标
            mScreenPos = touch.position;
#endif

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
                    //if (isAttack)
                    //{
                    //    isAttack = false;
                    //}
                }
                //点击目标为敌人或怪物
                if (mHit.collider.gameObject.tag == "Monster" || mHit.collider.gameObject.tag == "Enemy")
                {
                    //if (isAttacking == true)
                    //{
                    //    return;
                    //}
                    attackTarget = mHit.collider.gameObject.transform.parent;

                    //stopDis = attackDis;
                    //让主角面朝目标坐标并向目标移动
                    transform.LookAt(attackTarget.transform.position);
                    mTargetPos = mHit.collider.gameObject.transform.position;
                    float enemyDistance = Vector3.Distance(transform.position, mHit.collider.gameObject.transform.position);

                    if (enemyDistance <= attackDis)
                    {
                        mTargetPos = transform.position;
                        //播放攻击动画
                        Attack();
                    }
                    else
                    {
                        transform.gameObject.GetComponent<Animator>().SetBool("Run", true);
                        isAttack = true;
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
                //if (isAttacking)
                //{
                //    return;
                //}
                if (Vector3.Distance(transform.position, attackTarget.transform.position) > attackDis)
                {
                    transform.LookAt(attackTarget.transform.position);
                    this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 6;
                    //transform.Translate(Vector3.forward * Time.deltaTime * 6);
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
                    this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 6;
                    // transform.Translate(Vector3.forward * Time.deltaTime * 6);
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
        mTargetPos = this.transform.position;
        if (attackTarget == null || attackTarget.tag == "Finish")
        {
            mTargetPos = this.transform.position;
            animator.SetBool("Melee Right Attack 02", false);
            return;
        }
        isAttack = true;
        isAttacking = true;
        transform.gameObject.GetComponent<Animator>().SetBool("Run", false);
        animator.SetBool("Melee Right Attack 02", true);
        Invoke("AfterAttack", 1);
    }

    protected virtual void AfterAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Right Attack 02"))
        {
            if (Vector3.Distance(transform.position, attackTarget.transform.position) <= attackDis)
            {
                EventCenter.Broadcast<GameObject>(EventType.MonsterTakeDamage, attackTarget.gameObject);
                EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_WeaponAttack);
            }
        }
        isAttack = false;
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isAttack && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetTrigger("Take Damage");

        }
        roleData.reduceHP(damage);
    }
}
