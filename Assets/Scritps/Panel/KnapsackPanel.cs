using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnapsackPanel : BasePanel {
    public Toggle seedToggle;       //种子
    public Toggle fruitToggle;      //果实
    public Button closeBtn;         //关闭
    public Button allSellBtn;       //全部出售
    public GameObject[] grids;      //格子

    protected override void Init()
    {
        closeBtn.onClick.AddListener(OnCloseBtn);
        seedToggle.onValueChanged.AddListener(OnSeedToggle);
        fruitToggle.onValueChanged.AddListener(OnFruitToggle);
    }

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnCloseBtn()
    {
        UIMgr.Instance.ClosePanel();
    }

    private void OnSeedToggle(bool isOn)
    {
        
    }

    private void OnFruitToggle(bool isOn)
    {
        
    }
}
