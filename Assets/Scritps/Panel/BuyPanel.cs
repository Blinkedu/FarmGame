using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : BasePanel {
    public Image iconImage;         //图标
    public Text nameText;           //名称
    public Text unitPriceText;      //单价
    public InputField countInput;   //购买数量
    public Text countPriceText;     //总价
    public Button buyBtn;           //购买
    public Button closeBtn;         //关闭

    private Crop tempCrop = null;
    private int countPrice = 0;
    private int tempCount = 0;

    protected override void Init()
    {
        closeBtn.onClick.AddListener(OnCloseBtn);
        countInput.onValueChanged.AddListener(OnInputEnd);
        buyBtn.onClick.AddListener(OnBuyBtn);
    }

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    private void OnCloseBtn()
    {
        UIMgr.Instance.ClosePanel();
        ResetShow();
    }

    /// <summary>
    /// 更新信息显示
    /// </summary>
    /// <param name="crop"></param>
    public void UpdateInfo(Crop crop)
    {
        if (crop == null)
            return;
        tempCrop = crop;
        iconImage.sprite = Resources.Load<Sprite>(crop.IconPath);
        nameText.text = crop.Name;
        unitPriceText.text = crop.BuyPrice.ToString();
    }

    /// <summary>
    /// 重置
    /// </summary>
    private void ResetShow()
    {
        tempCrop = null;
        countInput.text = "";
        countPriceText.text = "0";
        countPrice = 0;
        tempCount = 0;
    }

    /// <summary>
    /// 结束输入
    /// </summary>
    /// <param name="Str"></param>
    private void OnInputEnd(string str)
    {
        //if (string.IsNullOrEmpty(str))
        //{
        //    MessageMgr.Instance.ShowMsg("输入数量不能为空!", Color.red);
        //    return;
        //}
        tempCount = int.Parse(str);
        if (tempCount < 0)
        {
            MessageMgr.Instance.ShowMsg("输入数量必须 >= 1!", Color.red);
        }
        else
        {
            countPrice = tempCount * tempCrop.BuyPrice;
            countPriceText.text = countPrice.ToString();
        }
    }

    /// <summary>
    /// 点击购买按钮
    /// </summary>
    private void OnBuyBtn()
    {
        if (tempCount == 0)
        {
            MessageMgr.Instance.ShowMsg("数量为 0 无法购买!", Color.yellow);
            return;
        }
        if (countPrice>GameManager.Instance.CoinCount)
        {
            MessageMgr.Instance.ShowMsg("购买失败: 金币余额不足!", Color.red);
            return;
        }
        if (GameManager.Instance.SubCoin(countPrice))
        {
            MessageMgr.Instance.ShowMsg("购买成功!", Color.green);
            SeedsMgr.Instance.AddSeed(tempCrop.ID, tempCount);
            //
            UIMgr.Instance.ClosePanel();
        }
    }
}
