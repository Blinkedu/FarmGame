using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedsMgr : MonoBehaviour
{
    private static SeedsMgr _instance = null;
    public static SeedsMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas/PanelNode/MainPanel(Clone)/SeedsPanel").GetComponent<SeedsMgr>();
            }
            return _instance;
        }
    }

    public Transform seedNode;
    public Dictionary<int, ItemUI> seedItemUIDict = new Dictionary<int, ItemUI>();  //id

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 添加种子
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void AddSeed(int id, int count = 1)
    {
        Slot slot = FindSlot(id);
        if (slot == null)
        {
            CreateSeed(id, count);
        }
        else
        {
            AddSeedCount(id, count);
        }
    }

    /// <summary>
    /// 减少种子
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    public void SubSeed(int id, int count = 1)
    {
        ItemUI itemUI = null;
        seedItemUIDict.TryGetValue(id, out itemUI);
        if (itemUI != null)
        {
            itemUI.SubCount(count);
            if (itemUI.Count <= 0)
            {
                Destroy(itemUI.transform.parent.gameObject);
                seedItemUIDict.Remove(id);
            }
        }
    }

    private Slot FindSlot(int id)
    {
        Slot[] slots = seedNode.GetComponentsInChildren<Slot>();
        foreach (var s in slots)
        {
            if (s.Crop.ID == id)
            {
                return s;
            }
        }
        return null;
    }

    private void AddSeedCount(int id, int count)
    {
        if (seedItemUIDict.ContainsKey(id))
        {
            ItemUI itemUI = null;
            seedItemUIDict.TryGetValue(id, out itemUI);
            if (itemUI != null)
            {
                itemUI.AddCount(count);
            }
        }
    }

    public void CreateSeed(int id, int count)
    {
        GameObject slotGO = Instantiate(Resources.Load<GameObject>("Prefabs/Grid"), seedNode);
        GameObject ItemGO = Instantiate(Resources.Load<GameObject>("Prefabs/Item"), slotGO.transform);
        ItemGO.transform.localEulerAngles = Vector3.zero;
        ItemUI itemUI = ItemGO.GetComponent<ItemUI>();
        itemUI.CreateItem(InventoryMgr.Instacne.GetCropById(id), count);
        Slot slot = slotGO.GetComponent<Slot>();
        slot.Crop = itemUI.Crop;
        slot.SetOwner(PanelType.SeedsPanel);
        seedItemUIDict.Add(id, itemUI);
    }
}
