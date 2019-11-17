using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveAndLoadMgr
{
    public static bool isLoadData = false;

    #region Save
    public static void SaveSeedData()
    {
        //种子id-种子数量|
        Dictionary<int, ItemUI> seedDict = SeedsMgr.Instance.seedItemUIDict;
        string dataStr = "";
        foreach (var id in seedDict.Keys)
        {
            dataStr += id + "," + seedDict[id].Count + "|";
        }
        dataStr = dataStr.Trim('|');
        PlayerPrefs.SetString("SeedData", dataStr);
    }

    public static void SaveFieldData()
    {
        //土地索引-土地是否解锁(1为已经解锁，0为未解锁)-土地上的作物ID-作物剩余时间|
        Field[] fields = GameObject.Find("Meshes/Fields").GetComponentsInChildren<Field>();
        string dataStr = "";
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].isLock)
            {
                dataStr += i + "," + 0 + "," + -1 + "," + -1 + "|";
            }
            else
            {
                if (fields[i].isEmpty)
                {
                    dataStr += i + "," + 1 + "," + -1 + "," + -1 + "|";
                }
                else
                {
                    dataStr += i + "," + 1 + "," + fields[i].crop.ID + "," + fields[i].Timer + "|";
                }
            }
        }
        dataStr = dataStr.Trim('|');
        PlayerPrefs.SetString("FieldData", dataStr);
    }

    public static void SaveKnapsackData()
    {
        // 格子索引-物品id-物品数量|
        string dataStr = "";
        for (int i = 0; i < KnapsackMgr.Instance.slots.Length; i++)
        {
            Slot s = KnapsackMgr.Instance.slots[i];
            if (s.transform.childCount > 0)
            {
                ItemUI itemUI = s.transform.GetChild(0).GetComponent<ItemUI>();
                dataStr += i + "," + itemUI.Crop.ID + "," + itemUI.Count + "|";
            }
        }
        dataStr = dataStr.Trim('|');
        PlayerPrefs.SetString("KnapsackData", dataStr);
    }

    public static void SaveCoinData()
    {
        PlayerPrefs.SetInt("CoinCount", GameManager.Instance.CoinCount);
    }

    public static void SaveAll()
    {

        SaveCoinData();
        SaveFieldData();
        SaveKnapsackData();
        SaveSeedData();

    }
    #endregion

    #region Load

    public static void LoadAll()
    {
        LoadCoinData();
        LoadFieldData();
        LoadKanvsakeData();
        LoadSeedData();
    }

    public static void LoadSeedData()
    {
        string dataStr = PlayerPrefs.GetString("SeedData");
        if (string.IsNullOrEmpty(dataStr))
            return;
        string[] datas = dataStr.Split('|');
        foreach (string data in datas)
        {
            string[] temps = data.Split(',');
            int id = int.Parse(temps[0]);
            int count = int.Parse(temps[1]);
            SeedsMgr.Instance.CreateSeed(id, count);
        }
    }

    public static void LoadFieldData()
    {
        string dataStr = PlayerPrefs.GetString("FieldData");
        if (string.IsNullOrEmpty(dataStr))
            return;
        string[] datas = dataStr.Split('|');
        foreach (string data in datas)
        {
            string[] temps = data.Split(',');
            
            int index = int.Parse(temps[0]);
            int isLock = int.Parse(temps[1]);
            int id = int.Parse(temps[2]);
            float timer = float.Parse(temps[3]);
            GameManager.Instance.SowSeedsByIndex(index, isLock, id, timer);
        }
    }

    public static void LoadKanvsakeData()
    {
        string dataStr = PlayerPrefs.GetString("KnapsackData");
        if (string.IsNullOrEmpty(dataStr))
            return;
        string[] datas = dataStr.Split('|');
        foreach (string data in datas)
        {
            string[] temps = data.Split(',');
            int index = int.Parse(temps[0]);
            int id = int.Parse(temps[1]);
            int count = int.Parse(temps[2]);
            KnapsackMgr.Instance.AddItemByIndex(index, id, count);
        }
    }

    public static void LoadCoinData()
    {
        GameManager.Instance.CoinCount = PlayerPrefs.GetInt("CoinCount");
    }
    #endregion

    public static bool HaveKeep()
    {
        if (PlayerPrefs.HasKey("CoinCount"))
        {
            return true;
        }
        return false;
    }
}
