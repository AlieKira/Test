using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MagicBall:AttackTool
{
    private int speed = 5;
    private Rigidbody rgd;
    private GameObject effect;
    public bool isPlay = true;

    void Start()
    {
        rgd = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rgd.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Monster" || other.gameObject.tag == "Terrain")
        {
            if (isPlay)
            {
                EventCenter.Broadcast(EventType.PlayNormalSound, Audios.Sound_Burst);
            }
            effect = Resources.Load<GameObject>("Effect/NovaRed");
            Instantiate(effect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
}

