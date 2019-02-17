using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Halloween : Anim_PlayerControl
{
    private bool isTakeDamage = false;

    public override void TakeDamage()
    {
        if (isTakeDamage)
        {
            return;
        }
        isTakeDamage = true;
        GetComponent<Animator>().SetTrigger("Take Damage");
        Invoke("Init",0.5f);
    }

    private void Init()
    {
        isTakeDamage = false;
    }
}
