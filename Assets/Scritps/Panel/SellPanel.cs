using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellPanel : BasePanel
{
    public Image iconImage;         //图标
    public Text nameText;           //名称
    public Text counText;           //总数
    public Text sellUnitPriceText;      //单价
    public InputField countInput;   //购买数量
    public Text countPriceText;     //总价
    public Button sellBtn;           //购买
    public Button closeBtn;         //关闭

    private Crop tempCrop = null;
    private int countPrice = 0;
    private int tempCount = 0;
    private int ItemCount = 0;

    protected override void Init()
    {
        closeBtn.onClick.AddListener(OnCloseBtn);
        countInput.onValueChanged.AddListener(OnInputEnd);
        sellBtn.onClick.AddListener(OnSellBtn);
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
    public void UpdateInfo(Crop crop, int count)
    {
        if (crop == null)
            return;
        tempCrop = crop;
        iconImage.sprite = Resources.Load<Sprite>(crop.IconPath);
        nameText.text = crop.Name;
        countPriceText.text = crop.SellPrice.ToString();
        ItemCount = count;
        counText.text = count.ToString();
        sellUnitPriceText.text = crop.SellPrice.ToString();
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
        ItemCount = 0;
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
            countPrice = tempCount * tempCrop.SellPrice;
            countPriceText.text = countPrice.ToString();
        }
    }

    /// <summary>
    /// 点击购买按钮
    /// </summary>
    private void OnSellBtn()
    {
        if (tempCount == 0)
        {
            MessageMgr.Instance.ShowMsg("数量为 0 无法出售!", Color.yellow);
            return;
        }
        if (ItemCount < tempCount)
        {
            MessageMgr.Instance.ShowMsg("出售失败: 数量不足!", Color.yellow);
        }
        else
        {
            MessageMgr.Instance.ShowMsg("出售成功!", Color.green);
            KnapsackMgr.Instance.SubItem(tempCrop.ID, tempCount);
            GameManager.Instance.AddCoin(countPrice);
            //
            UIMgr.Instance.ClosePanel();
        }

    }

}
