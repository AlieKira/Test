using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源池
/// </summary>
[Serializable]
public class GameObjectPool
{
    [SerializeField]
    public string name;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int maxAmount;

    [NonSerialized]
    private List<GameObject> goList = new List<GameObject>();

    /// <summary>
    /// 表示从资源池中获取一个实例
    /// </summary>
    public GameObject GetInst()
    {
        foreach (GameObject go in goList)
        {
            if (go.activeInHierarchy == false)
            {
                go.SetActive(true);
                return go;
            }
        }

        if (goList.Count >= maxAmount)
        {
            return null;
        }

        GameObject temp = GameObject.Instantiate(prefab) as GameObject;
        temp.transform.Find("Rig").gameObject.AddComponent<CollisionDetection>();
        temp.AddComponent<NPCControl>();
        temp.AddComponent<MonsterData>();
        goList.Add(temp);
        return temp;
    }
}