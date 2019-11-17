using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackMgr : MonoBehaviour
{
    private static KnapsackMgr _instacne = null;
    public static KnapsackMgr Instance
    {
        get
        {
            if (_instacne == null)
            {
                GameObject go = GameObject.Find("KnapsackPanel(Clone)");
                if (go == null)
                {
                    UIMgr.Instance.OpenPanel(PanelType.KnapsackPanel);
                    _instacne = GameObject.Find("KnapsackPanel(Clone)").GetComponent<KnapsackMgr>();
                    UIMgr.Instance.ClosePanel();
                }
                else
                {
                    _instacne = go.GetComponent<KnapsackMgr>();
                }
            }
            return _instacne;
        }
    }

    public Slot[] slots;

    /// <summary>
    /// 添加
    /// </summary>
    public void AddItem(int id, int count)
    {
        Slot slot = FindSlot(id);
        if (slot == null)
        {
            slot = FindEmptySlot();
            CreateItem(id, slot, count);
        }
        else
        {
            slot.transform.GetChild(0).GetComponent<ItemUI>().AddCount(count);
        }
    }

    /// <summary>
    /// 查找物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindSlot(int id)
    {
        foreach (Slot s in slots)
        {
            if (s.transform.childCount > 0)
            {
                ItemUI itemUI = s.transform.GetChild(0).GetComponent<ItemUI>();
                if (itemUI.Crop.ID == id)
                {
                    return s;
                }
            }
        }
        return null;
    }

    private Slot FindEmptySlot()
    {
        foreach (var s in slots)
        {
            if (s.transform.childCount == 0)
            {
                return s;
            }
        }
        return null;
    }

    private void CreateItem(int id, Slot slot, int count = 1)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), slot.transform);
        ItemUI itemUI = go.GetComponent<ItemUI>();
        itemUI.CreateItem(InventoryMgr.Instacne.GetCropById(id), count);
        slot.SetOwner(PanelType.KnapsackPanel);
        slot.Crop = InventoryMgr.Instacne.GetCropById(id);
    }

    public void SubItem(int id, int count = 1)
    {
        ItemUI itemUI = FindItemUIById(id);
        if (itemUI==null)
        {
            Debug.LogError("背包中没有该物品! id = " + id);
            return;
        }
        itemUI.SubCount(count);
        if (itemUI.Count <= 0)
        {
            Destroy(itemUI.gameObject);
        }
    }

    private ItemUI FindItemUIById(int id)
    {
        foreach (var s in slots)
        {
            if (s.transform.childCount > 0)
            {
                return s.transform.GetChild(0).GetComponent<ItemUI>();
            }
        }
        return null;
    }

    public void AddItemByIndex(int index,int id,int count)
    {
        CreateItem(id, slots[index], count);
    }
}
