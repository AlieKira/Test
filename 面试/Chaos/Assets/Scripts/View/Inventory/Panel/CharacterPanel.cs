using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : Inventory
{
    private Text propertyText;

    private RoleData roleData;

    private Button CloseButton;

    //public KnapsackPanel knapsackPanel;


    public virtual void OnStart()
    {
        base.OnStart();
        inventoryMng.characterPanel = this;
        propertyText = transform.Find("PropertyPanel/Text").GetComponent<Text>();
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
    }


    public void PutOn(Item item)
    {
        Item exitItem = null;
        foreach (Slot slot in slotList)
        {
            EquipmentSlot equipmentSlot = (EquipmentSlot)slot;
            if (equipmentSlot.IsRightItem(item))
            {
                if (equipmentSlot.transform.childCount > 0)
                {
                    ItemUI currentItemUI = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    exitItem = currentItemUI.Item;
                    currentItemUI.SetItem(item, 1);
                    inventoryMng.knapsackPanel.StoreItem(exitItem);
                }
                else
                {
                    equipmentSlot.StoreItem(item);
                }
                break;
            }
        }
        if (exitItem != null)
            //view.knapsackPanel.StoreItem(exitItem); //TODO

            UpdatePropertyText();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        OnStart();
    }

    public override void OnResume()
    {
        base.OnEnter();
        roleData = Model.Instance.GetRoleData();
        UpdatePropertyText();
    }

    public void PutOff(Item item)
    {
        inventoryMng.knapsackPanel.StoreItem(item);
        UpdatePropertyText();
    }

    private void UpdatePropertyText()
    {
        if (roleData==null)
        {
            roleData = Model.Instance.GetRoleData();
        }
        int strength = 0, intellect = 0, agility = 0, stamina = 0, attack = 0;
        foreach (Slot slot in slotList)
        {

            //EquipmentSlot slot = (EquipmentSlot) temp;
            if (slot.transform.childCount > 0)
            {
                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().Item;
                if (item is Equipment)
                {
                    Equipment e = (Equipment)item;
                    strength += e.Strength;
                    intellect += e.Intellect;
                    agility += e.Agility;
                    stamina += e.Stamina;
                }
                else if (item is Weapon)
                {
                    attack += ((Weapon)item).Damage;
                }
            }
        }
        strength += roleData.BasicStrength;
        intellect += roleData.BasicIntellect;
        agility += roleData.BasicAgility;
        stamina += roleData.BasicStamina;
        attack += roleData.BasicAttack;
        string text = string.Format("力量：{0}\n\n智力：{1}\n\n敏捷：{2}\n\n体力：{3}\n\n攻击力：{4} ", strength, intellect, agility, stamina, attack);
        propertyText.text = text;
        EventCenter.Broadcast(EventType.UpdateAttri, strength, intellect, agility, stamina, attack);
    }

    public void ClearItem()
    {
        foreach (Slot temp in slotList)
        {
            if (temp.transform.childCount != 0)
            {
                Destroy(temp.transform.GetChild(0).gameObject);
            }
        }
    }

    //private void OnCloseClick()
    //{
    //    if (uiMng == null)
    //    {
    //        uiMng = GameObject.Find("View").GetComponent<View>().uiMng;
    //    }
    //    inventoryMng.InitBool();
    //    view.HideToolTip();
    //    //uiMng.HideInventory(UIPanelType.CharacterPanel);
    //}

    private void OnCloseClick()
    {
        //inventoryMng.InitBool();
        //view.HideToolTip();
        if (inventoryMng.IsPickedItem)
        {
            return;
        }
        OnClick();
        EventCenter.Broadcast(EventType.HidePanel, UIPanelType.CharacterPanel);
        //uiMng.HideInventory(UIPanelType.CharacterPanel);
    }
}
