using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ArcherControl:PlayerControl
{
    private Transform leftHandTrans;
    private GameObject arrow;

    protected override void Start()
    {
        base.Start();
        leftHandTrans = transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigLArm1/ArrowPos");
        arrow = Resources.Load<GameObject>("Arrow");
    }

    protected override void Attack()
    {
        mTargetPos = this.transform.position;
        if (attackTarget == null || attackTarget.tag == "Finish")
        {

            animator.SetBool("Crossbow Shoot Attack", false);
            return;
        }
        mTargetPos = this.transform.position;
        animator.SetBool("Run", false);
        animator.SetBool("Crossbow Shoot Attack", true);
        Invoke("Shoot",0.5f);
        isAttack = true;
        isAttacking = true;
    }

    private void Shoot()
    {
        mTargetPos = this.transform.position;
        Vector3 targetPos = attackTarget.position;
        targetPos.y = transform.position.y;
        Vector3 dir = targetPos - transform.position;
        GameObject a= GameObject.Instantiate(arrow, leftHandTrans.position, Quaternion.LookRotation(dir));
        a.AddComponent<Arrow>().damage=GetComponent<RoleData>().Damage;
        Invoke("init",0.5f);
        EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_ArrowShoot);
    }

    private void init()
    {
        isAttack = false;
        isAttacking = false;
    }
}
