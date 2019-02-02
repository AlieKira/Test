using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Wizard : Anim_PlayerControl
{

    private GameObject attackTarget;
    private Transform handTrans;
    private GameObject arrow;
    private bool isAttack = false;

    void Start()
    {
        handTrans = transform.Find("RigPelvis/RigSpine1/RigSpine2/RigRibcage/RigRArm1/BallPos");
        arrow = Resources.Load<GameObject>("TorchFireRed");
    }



    private void OnTriggerStay(Collider collider)
    {
        if (isAttack)
        {
            return;
        }
        if (collider.gameObject.tag == "Monster")
        {
            attackTarget = collider.gameObject;
            transform.LookAt(collider.gameObject.transform);
            isAttack = true;
            GetComponent<Animator>().SetBool("Cast Spell", true);
            Invoke("Shoot", 0.6f);
        }
    }

    private void Shoot()
    {
        Vector3 targetPos = attackTarget.transform.parent.position;
        GetComponent<Animator>().SetBool("Cast Spell", false);
        targetPos.y = transform.position.y;
        Vector3 dir = targetPos - transform.position;
        GameObject a = GameObject.Instantiate(arrow, handTrans.position, Quaternion.LookRotation(dir));
        a.AddComponent<MagicBall>().isPlay = false;
        Invoke("Init", 0.6f);
    }

    private void Init()
    {
        isAttack = false;
    }
}
