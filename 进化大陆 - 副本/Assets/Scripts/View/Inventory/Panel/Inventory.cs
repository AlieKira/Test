using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : BasePanel
{
    protected Slot[] slotList;

    private float targetAlpha = 1f;

    private float smoothing = 4;

    protected CanvasGroup canvasGroup;

    public InventoryManager inventoryMng;

    public virtual void Start()
    {
        view = GameObject.Find("View").GetComponent<View>();
        slotList = GetComponentsInChildren<Slot>();
        foreach (Slot temp in slotList)
        {
            temp.GetComponent<Slot>().inventoryMng = inventoryMng;
        }
        canvasGroup = GetComponent<CanvasGroup>();
        this.gameObject.SetActive(false);
    }

    //public virtual void OnStart()
    //{
    //    slotList = GetComponentsInChildren<Slot>();
    //    foreach (Slot temp in slotList)
    //    {
    //        temp.GetComponent<Slot>().inventoryMng = inventoryMng;
    //    }
    //    canvasGroup = GetComponent<CanvasGroup>();
    //}

    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < .01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }



    public bool StoreItem(int id)
    {
        Item item = uiMng.view.GetItemById(id);
        return StoreItem(item);
    }

    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("要存储的物品的id不存在");
            return false;
        }
        if (item.Capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreItem(item);//把物品存储到这个空的物品槽里面
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(item);
            if (slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 这个方法用来找到一个空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    private Slot FindSameIdSlot(Item item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemId() == item.ID && slot.IsFilled() == false)
            {
                return slot;
            }
        }
        return null;
    }

    //public void Show()
    //{
    //    this.gameObject.SetActive(true);
    //}
    //public void Hide()
    //{
    //    inventoryMng.Hide();
    //}
    //public void DisplaySwitch()
    //{
    //    if (targetAlpha == 0) 
    //    {
    //        Show();
    //    }
    //    else
    //    {
    //        Hide();
    //    }
    //}

    #region Pre_Treatment

    public override void OnEnter()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        this.gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        this.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        this.gameObject.SetActive(false);
    }

    #endregion


    #region save and load
    public virtual void SaveInventory()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                sb.Append(itemUI.Item.ID + "," + itemUI.Amount + "-");
            }
            else
            {
                sb.Append("0-");
            }
        }
        PlayerPrefs.SetString(this.gameObject.name, sb.ToString());
    }

    //public virtual void LoadInventory()
    //{
    //    if (PlayerPrefs.HasKey(this.gameObject.name) == false) return;
    //    string str = PlayerPrefs.GetString(this.gameObject.name);
    //    print(str);
    //    string[] itemArray = str.Split('-');
    //    for (int i = 0; i < itemArray.Length - 1; i++)
    //    {
    //        string itemStr = itemArray[i];
    //        if (itemStr != "0")
    //        {
    //            print(itemStr);
    //            string[] temp = itemStr.Split(',');
    //            int id = int.Parse(temp[0]);
    //            Item item = inventoryMng.GetItemById(id);
    //            int amount = int.Parse(temp[1]);
    //            for (int j = 0; j < amount; j++)
    //            {
    //                slotList[i].StoreItem(item);
    //            }
    //        }
    //    }
    //}
    #endregion
}
