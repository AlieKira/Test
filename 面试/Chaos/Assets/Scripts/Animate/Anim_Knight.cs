using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Knight : Anim_PlayerControl
{

    private GameObject attackTarget;
    private Transform leftHandTrans;
    private GameObject arrow;
    private bool isAttack = false;


    private void OnTriggerStay(Collider collider)
    {

        if (isAttack)
        {
            return;
        }
        if (collider.gameObject.tag == "Monster")
        {
            isAttack = true;
            attackTarget = collider.gameObject;
            transform.LookAt(collider.gameObject.transform);
            isAttack = true;
            GetComponent<Animator>().SetBool("Melee Right Attack 02", true);
            Invoke("Init", 0.5f);
        }
    }

    private void Init()
    {
        isAttack = false;
    }

    private void TakeDamage()
    {
        //DoNothing
    }
}
