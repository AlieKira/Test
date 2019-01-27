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


    public override void Start()
    {
        base.Start();
        if (inventoryMng == null) 
        {
            Debug.Log("null");
        }
        //knapsackPanel = transform.Find("KnapsackPanel").GetComponent<KnapsackPanel>();
        //knapsackPanel.inventoryMng = inventoryMng;
        //knapsackPanel.OnStart();
        propertyText = transform.Find("PropertyPanel/Text").GetComponent<Text>();
        CloseButton = transform.Find("CloseButton").GetComponent<Button>();
        CloseButton.onClick.AddListener(OnCloseClick);
        inventoryMng.characterPanel = this;
        this.gameObject.SetActive(false);
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
                }
                else
                {
                    equipmentSlot.StoreItem(item);
                }
                break;
            }
        }
        if (exitItem != null)
            view.knapsackPanel.StoreItem(exitItem);

        UpdatePropertyText();
    }

    public override void OnResume()
    {
        base.OnEnter();
        roleData = uiMng.view.GetRoleData();
        UpdatePropertyText();
    }

    public void PutOff(Item item)
    {
        if (view==null)
        {
            Debug.Log(1);
        }
        if (view.knapsackPanel == null)
        {
            Debug.Log(2);
        }
        if (item == null)
        {
            Debug.Log(3);
        }
        view.knapsackPanel.StoreItem(item);
        UpdatePropertyText();
    }

    private void UpdatePropertyText()
    {
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
        GameManager.Instance.UpdateAttri(strength,intellect,agility,stamina,attack);
    }



    private void OnCloseClick()
    {
        if (uiMng == null)
        {
            uiMng = GameObject.Find("View").GetComponent<View>().uiMng;
        }
        inventoryMng.InitBool();
        view.HideToolTip();
        uiMng.HideInventory(UIPanelType.CharacterPanel);
    }
}
