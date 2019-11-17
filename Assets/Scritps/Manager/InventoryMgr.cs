using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class InventoryMgr : MonoBehaviour {

    #region 单例模式
    private static InventoryMgr _instance = null;
    public static InventoryMgr Instacne
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("EmptyGO").GetComponent<InventoryMgr>();
            }
            return _instance;
        }
    }
    #endregion

    private List<Crop> crops = new List<Crop>();            //物品缓存
    public List<Crop> Crops
    {
        get
        {
            return crops;
        }
    }

    private void Awake()
    {
        ParseJson();
    }

    /// <summary>
    /// 解析json
    /// </summary>
    private void ParseJson()
    {
        crops.Clear();
        string jsonStr = Resources.Load<TextAsset>("Config/CropConfig").text;
        JsonData datas = JsonMapper.ToObject(jsonStr);
        foreach (JsonData data in datas)
        {
            int id = int.Parse(data["id"].ToString());
            string name = data["name"].ToString();
            int buyPrice = int.Parse(data["buyPrice"].ToString());
            int sellPrice = int.Parse(data["sellPrice"].ToString());
            float cycle = float.Parse(data["cycle"].ToString());
            string iconPath = data["iconPath"].ToString();
            string smallPrefab= data["smallPrefab"].ToString();
            string bigPrefab = data["bigPrefab"].ToString();

            Crop crop = new Crop(id, name, buyPrice, sellPrice, cycle, iconPath, smallPrefab, bigPrefab);
            crops.Add(crop);
        }
    }

    /// <summary>
    /// 通过id获取物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Crop GetCropById(int id)
    {
        foreach (var c in crops)
        {
            if (c.ID == id)
                return c;
        }
        return null;
    }

}
