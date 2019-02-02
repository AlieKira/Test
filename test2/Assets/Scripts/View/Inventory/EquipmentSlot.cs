using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public Equipment.EquipmentType equipType;
    public Weapon.WeaponType wpType;


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log(2);
            if (inventoryMng.IsPickedItem == false && transform.childCount > 0)
            {
                Debug.Log(3);
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                Item itemTemp = currentItemUI.Item;
                DestroyImmediate(currentItemUI.gameObject);
                //脱掉放到背包里面
                transform.parent.parent.SendMessage("PutOff", itemTemp);
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 手上有 东西
        //当前装备槽 有装备
        //无装备
        // 手上没 东西
        //当前装备槽 有装备 
        //无装备  不做处理
        bool isUpdateProperty = false;
        if (inventoryMng.IsPickedItem == true)
        {
            //手上有东西的情况
            ItemUI pickedItem = inventoryMng.PickedItem;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();//当前装备槽里面的物品

                if (IsRightItem(pickedItem.Item))
                {
                    inventoryMng.PickedItem.Exchange(currentItemUI);
                    isUpdateProperty = true;
                }
            }
            else
            {
                if (IsRightItem(pickedItem.Item))
                {
                    this.StoreItem(inventoryMng.PickedItem.Item);
                    inventoryMng.RemoveItem(1);
                    isUpdateProperty = true;
                }

            }
        }
        else
        {
            //手上没东西的情况
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                inventoryMng.PickupItem(currentItemUI.Item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);
                isUpdateProperty = true;
            }
        }
        if (isUpdateProperty)
        {
            transform.parent.parent.SendMessage("UpdatePropertyText");
        }
    }

    //判断item是否适合放在这个位置
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)(item)).EquipType == this.equipType) ||
                    (item is Weapon && ((Weapon)(item)).WpType == this.wpType))
        {
            return true;
        }
        return false;
    }
}
