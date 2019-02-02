using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerExitHandler, IPointerDownHandler,IPointerEnterHandler
{
    public InventoryManager inventoryMng;

    private void Start()
    {
        //gameFacade = GameObject.Find("GameFacade").GetComponent<GameFacade>();
        //inventoryMng = gameFacade.GetInventoryMng();
    }

    public GameObject itemPrefab;
    /// <summary>
    /// 把item放在自身下面
    /// 如果自身下面已经有item了，amount++
    /// 如果没有 根据itemPrefab去实例化一个item，放在下面
    /// </summary>
    /// <param name="item"></param>
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(itemPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }


    /// <summary>
    /// 得到当前物品槽存储的物品类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    /// <summary>
    /// 得到物品的id
    /// </summary>
    /// <returns></returns>
    public int GetItemId()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
    }

    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;//当前的数量大于等于容量
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (inventoryMng.IsBuyItem)
        {
            return;
        }

        if (!inventoryMng.isUseItem&&!inventoryMng.IsSellItem && !inventoryMng.IsSellItem)
        {
            EventCenter.Broadcast(EventType.HideToolTip);
        }
            //格子不为空
            if (transform.childCount > 0)
            {
                ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
                if (inventoryMng.IsSellItem)
                {
                    this.transform.parent.parent.GetComponentInParent<KnapsackPanel>().SendMessage("SellItem", currentItem);
                }
                //点击了“使用”按钮
                else if (inventoryMng.isUseItem)
                {
                    //格子有装备或武器，则装备上
                    if (currentItem.Item is Equipment || currentItem.Item is Weapon)
                    {
                        currentItem.ReduceAmount(1);
                        Item tempItem = currentItem.Item;
                        if (currentItem.Amount <= 0)
                        {
                            DestroyImmediate(currentItem.gameObject);
                        }
                        inventoryMng.characterPanel.PutOn(tempItem);
                    }
                    //格子里的为消耗品，则为角色对应血量
                    if (currentItem.Item is Consumable)
                    {
                        Consumable consumable = (Consumable)currentItem.Item;
                        currentItem.ReduceAmount(1);
                        if (currentItem.Amount <= 0)
                        {
                            DestroyImmediate(currentItem.gameObject);
                        }
                        EventCenter.Broadcast(EventType.RecoverHp, consumable.HP);
                    }
                }
                //手上已经有物品
                else if (inventoryMng.IsPickedItem == true)
                {
                    //手上物品与格子物品相同
                    if (currentItem.Item.ID == inventoryMng.PickedItem.Item.ID)
                    {
                        if (currentItem.Item.Capacity > currentItem.Amount)
                        {
                            int amountRemain = currentItem.Item.Capacity - currentItem.Amount;
                            if (amountRemain >= inventoryMng.PickedItem.Amount)
                            {
                                currentItem.SetAmount(currentItem.Amount + inventoryMng.PickedItem.Amount);
                                inventoryMng.RemoveItem(inventoryMng.PickedItem.Amount);
                            }
                            else
                            {
                                currentItem.SetAmount(currentItem.Amount + amountRemain);
                                inventoryMng.RemoveItem(amountRemain);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    //手上物品与格子物品不同，交换物品
                    else
                    {
                        Item tempItem = currentItem.Item;
                        int amount = currentItem.Amount;
                        currentItem.SetItem(inventoryMng.PickedItem.Item, inventoryMng.PickedItem.Amount);
                        inventoryMng.PickedItem.SetItem(tempItem, amount);
                    }
                }
                //手上没有物品且没点过按钮
                else
                {
                    inventoryMng.PickupItem(currentItem.Item, currentItem.Amount);
                    Destroy(currentItem.gameObject);
                }
            }
            //格子为空
            else
            {
                //手上有物品，则将物品放入格子中
                if (inventoryMng.IsPickedItem == true)
                {
                    for (int i = 0; i < inventoryMng.PickedItem.Amount; i++)
                    {
                        this.StoreItem(inventoryMng.PickedItem.Item);
                    }
                    inventoryMng.RemoveItem(inventoryMng.PickedItem.Amount);
                }
                else
                {
                    return;
                }
            }

        ////处于商店界面
        //else
        //{
        //    //格子不为空
        //    if (transform.childCount > 0)
        //    {
        //        Debug.Log(1);
        //        ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
        //        Debug.Log(currentItem.Item.Name);
        //        //点击了“出售”按键,出售一件物品
        //        if (inventoryMng.IsSellItem)
        //        {
        //            this.transform.parent.parent.GetComponentInParent<KnapsackPanel>().SendMessage("SellItem", currentItem);
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}
    }

    //public virtual void OnPointerUp(PointerEventData eventData)
    //{
    //    if (eventData.button!= PointerEventData.InputButton.Left)
    //    {
    //        return;
    //    }
    //    //手上无物品
    //    if (inventoryMng.IsPickedItem==false)
    //    {
    //        //格子不为空
    //        if (transform.childCount>0)
    //        {
    //            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
    //         //   gameFacade.ShowToolTip(currentItem.Item.GetToolTipText());
    //        }
    //        else
    //        {
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (inventoryMng==null)
        {
            Debug.Log(this.gameObject.name);
            Debug.Log("null");
        }
        if (inventoryMng.isUseItem||inventoryMng.IsSellItem||inventoryMng.IsBuyItem||inventoryMng.IsPickedItem)return;
            if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            EventCenter.Broadcast(EventType.ShowToolTip,toolTipText,Vector2.zero);
        }

    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryMng.isUseItem || inventoryMng.IsSellItem || inventoryMng.IsBuyItem) return;
        if (transform.childCount > 0)
            EventCenter.Broadcast(EventType.HideToolTip);
    }
}
