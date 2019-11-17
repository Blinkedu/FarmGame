using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店管理
/// </summary>
public class StoreMgr : MonoBehaviour
{
    private Slot[] slots;
    private void Start()
    {
        slots = transform.Find("Grids").GetComponentsInChildren<Slot>();
        Init();
    }

    public void Init()
    {
        foreach (Crop c in InventoryMgr.Instacne.Crops)
        {
            Slot slot = FindEmptySlot();
            if (slot != null)
            {
                AddItem(slot, c);
            }
            else
            {
                Debug.LogError("物品槽不足");
                return;
            }

        }
    }

    public Slot FindEmptySlot()
    {
        foreach (Slot s in slots)
        {
            if (s.transform.childCount == 0)
            {
                return s;
            }
        }
        return null;
    }

    private void AddItem(Slot slot, Crop crop)
    {
        GameObject pf = Resources.Load<GameObject>("Prefabs/Item");
        GameObject item = Instantiate(pf, slot.transform);
        ItemUI itemUI = item.GetComponent<ItemUI>();
        itemUI.CreateItem(crop);
        slot.SetOwner(PanelType.StorePanel);
    }
}
