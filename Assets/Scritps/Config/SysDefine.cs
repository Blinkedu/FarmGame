using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysDefine
{
    public const float WEATHER_CHANGE_TIME = 600f;

}

/// <summary>
/// UI面板类型
/// </summary>
public enum PanelType
{
    None,
    StorePanel,
    SellPanel,
    KnapsackPanel,
    BuyPanel,
    MainPanel,
    SeedsPanel,
    MenuPanel
}

/// <summary>
/// 作物类型
/// </summary>
public enum CropType
{
    None,
    Seed,
    Fruit
}

public enum CursorType
{
    Normal,
    Hand,
    Watering,
    Shovel
}

public enum WeatherType
{
    Fine = 1,
    Rain = 2
}
