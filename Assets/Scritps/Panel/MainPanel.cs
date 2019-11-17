using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainPanel : BasePanel {
    public Text coinText;       //金币
    public Text dateText;       //日期
    public Text weatherText;    //天气
    public Button storeBtn;     //商店
    public Button knapsackBtn;  //背包
    public Button handBtn;      //手
    public Button wateringBtn;  //水壶
    public Button shovelBtn;    //铁锹
    public Button menuBtn;

    public Transform seedsPanel;    //种子面板
    public Button seedBtn;          //种子按钮


    private bool isShowingSeedPanel = false;   //是否

    protected override void Init()
    {
        storeBtn.onClick.AddListener(OnStoreBtn);
        knapsackBtn.onClick.AddListener(OnKnapsackBtn);
        handBtn.onClick.AddListener(OnHandBtn);
        wateringBtn.onClick.AddListener(OnWateringBtn);
        shovelBtn.onClick.AddListener(OnShovelBtn);
        seedBtn.onClick.AddListener(OnSeedBtn);
        menuBtn.onClick.AddListener(OnMenuBtn);
    }

    /// <summary>
    /// 点击商店按钮
    /// </summary>
    private void OnStoreBtn()
    {
        UIMgr.Instance.OpenPanel(PanelType.StorePanel);
    }

    /// <summary>
    /// 点击背包按钮
    /// </summary>
    private void OnKnapsackBtn()
    {
        UIMgr.Instance.OpenPanel(PanelType.KnapsackPanel);
    }

    /// <summary>
    /// 点击手按钮
    /// </summary>
    private void OnHandBtn()
    {
        GameManager.Instance.ChangeCursor(CursorType.Hand);
    }

    /// <summary>
    /// 点击水壶按钮
    /// </summary>
    private void OnWateringBtn()
    {
        GameManager.Instance.ChangeCursor(CursorType.Watering);
    }

    /// <summary>
    /// 点击铲子按钮
    /// </summary>
    private void OnShovelBtn()
    {
        GameManager.Instance.ChangeCursor(CursorType.Shovel);
    }
    
    /// <summary>
    /// 点击种子按钮
    /// </summary>
    private void OnSeedBtn()
    {
        if (isShowingSeedPanel)
        {
            //seedsPanel.localPosition = new Vector2(707.3f, 0);
            seedsPanel.transform.DOLocalMoveX(707.3f, 0.3f);
        }
        else
        {
            //seedsPanel.localPosition = new Vector2(556.8f, 0);
            seedsPanel.transform.DOLocalMoveX(556.8f, 0.3f);
        }
        isShowingSeedPanel = !isShowingSeedPanel;
    }
    
    /// <summary>
    /// 点击菜单按钮
    /// </summary>
    public void OnMenuBtn()
    {
        UIMgr.Instance.OpenPanel(PanelType.MenuPanel);
    }

    /// <summary>
    /// 设置时间和天气显示
    /// </summary>
    /// <param name="dateStr"></param>
    /// <param name="weatherStr"></param>
    public void SetDateAndWeather(string dateStr,string weatherStr)
    {
        dateText.text = dateStr;
        SetWeather(weatherStr);
    }
 
    /// <summary>
    /// 设置天气显示
    /// </summary>
    /// <param name="weatherStr"></param>
    public void SetWeather(string weatherStr)
    {
        weatherText.text = weatherStr;
    }

    /// <summary>
    /// 更新金币显示
    /// </summary>
    /// <param name="count"></param>
    public void UpdateCoin(int count)
    {
        coinText.text = count.ToString();
    }


    public override void OnEnter() { }
    public override void OnExit() { }
}
