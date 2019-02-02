using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CollisionDetection:MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag!="AttackTool")
        {
            return;
        }
        transform.parent.GetComponent<MonsterData>().reduceHP(other.gameObject.GetComponent<AttackTool>().damage);

    }
}
