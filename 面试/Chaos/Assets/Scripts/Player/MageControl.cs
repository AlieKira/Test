using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class MageControl : PlayerControl
{
    private Transform leftHandTrans;
    private GameObject ball;

    protected override void Start()
    {
        base.Start();
        leftHandTrans = transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/BallPos");
        ball = Resources.Load<GameObject>("TorchFireRed");
    }

    protected override void Attack()
    {
        mTargetPos = this.transform.position;
        if (attackTarget == null || attackTarget.tag == "Finish")
        {

            animator.SetBool("Cast Spell", false);
            return;
        }
        mTargetPos = this.transform.position;
        animator.SetBool("Run", false);
        animator.SetBool("Cast Spell", true);
        Invoke("Shoot", 0.9f);
        isAttack = true;
        isAttacking = true;
    }

    private void Shoot()
    {
        Vector3 targetPos = attackTarget.position;
        targetPos.y = transform.position.y;
        Vector3 dir = targetPos - transform.position;
        if (leftHandTrans == null)
        {
            Debug.Log("null");
        }
        GameObject a = GameObject.Instantiate(ball, leftHandTrans.position, Quaternion.LookRotation(dir));
        a.AddComponent<MagicBall>().damage = GetComponent<RoleData>().Damage;
        isAttack = false;
        isAttacking = false;
        EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_FireBallShoot);
    }
}
