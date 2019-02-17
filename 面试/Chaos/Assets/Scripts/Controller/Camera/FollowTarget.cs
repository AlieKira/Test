using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public GameObject target;
    public Vector3 offset = new Vector3(0, 7.2f, 8.5f);

    private bool isUpdate = false;

    public void StartUpdate(GameObject target)
    {
        this.target = target;
        isUpdate = true;
    }

    public void StopUpdate()
    {
        isUpdate = false;
    }

    void FixedUpdate()
    {
        if (!isUpdate)
        {
            return;
        }
        transform.position = target.transform.position + offset;
    }
}
