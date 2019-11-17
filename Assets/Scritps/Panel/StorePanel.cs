using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : BasePanel {
    public Toggle seedToggle;       //种子
    public Toggle fruitToggle;      //果实
    public Button closeBtn;         //关闭
    public GameObject[] grids;      //格子

    protected override void Init()
    {
        closeBtn.onClick.AddListener(OnCloseBtn);
    }

    private void OnCloseBtn()
    {
        UIMgr.Instance.ClosePanel();
    }
}
