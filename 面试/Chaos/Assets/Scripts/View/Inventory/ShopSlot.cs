using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class ShopSlot : Slot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryMng.IsBuyItem==false)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left && inventoryMng.IsPickedItem == false)
        {
            if (transform.childCount > 0)
            {
                Item currentItem = transform.GetChild(0).GetComponent<ItemUI>().Item;
                transform.parent.parent.SendMessage("BuyItem", currentItem);
            }
        }
    }
}
