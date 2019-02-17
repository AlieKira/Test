using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Archer : Anim_PlayerControl
{
    private GameObject attackTarget;
    private Transform leftHandTrans;
    private GameObject arrow;
    private bool isAttack = false;

    void Start()
    {
        leftHandTrans = transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigLArm1/ArrowPos");
        arrow = Resources.Load<GameObject>("Arrow");
    }



    private void OnTriggerStay(Collider collider)
    {
        if (isAttack)
        {
            return;
        }
        if (collider.gameObject.tag=="Monster")
        {
            attackTarget = collider.gameObject;
            transform.LookAt(collider.gameObject.transform);
            isAttack = true;
            GetComponent<Animator>().SetBool("Crossbow Shoot Attack", true);
            Invoke("Shoot", 0.4f);
        }
    }

    private void Shoot()
    {
        Vector3 targetPos = attackTarget.transform.parent.position;
        GetComponent<Animator>().SetBool("Crossbow Shoot Attack", false);
        targetPos.y = transform.position.y;
        Vector3 dir = targetPos - leftHandTrans.position;
        GameObject a = GameObject.Instantiate(arrow, leftHandTrans.position, Quaternion.LookRotation(dir));
        a.AddComponent<Arrow>().isPlay=false;
        Invoke("Init", 0.9f);
    }

    private void Init()
    {
        isAttack = false;
    }
}
