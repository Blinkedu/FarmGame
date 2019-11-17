using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作物类
/// </summary>
public class Crop
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    public float Cycle { get; private set; }             //生长周期(分钟)
    public string IconPath { get; private set; }         //图标路径
    public string SmallPrefab { get; set; }
    public string BigPrefab { get; set; }

    public Crop() { }

    public Crop(int id,string name,int buyprice,int sellprice ,float cycle,string iconPath,string smallPrefab,string bigPrefab)
    {
        this.ID = id;
        this.Name = name;
        this.BuyPrice = buyprice;
        this.SellPrice = sellprice;
        this.Cycle = cycle;
        this.IconPath = iconPath;
        this.BigPrefab = bigPrefab;
        this.SmallPrefab = smallPrefab;
    }

    public string GetInfoByStore()
    {
        return string.Format("\n{0}(种子)\n生长周期: {1}分钟\n种子单价: {2}\n熟果单价: {3}\n", Name, Cycle, BuyPrice, SellPrice);
    }

    public string GetInfoByField()
    {
        return string.Format("\n{0}\n生长周期: {1}分钟\n", Name, Cycle);
    }

    public float GetCycleToSec()
    {
        return Cycle * 60;
    }
}
