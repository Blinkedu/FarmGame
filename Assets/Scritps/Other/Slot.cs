using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isEnter = false;
    public Crop Crop { get; set; }
    public PanelType owenrPanel = PanelType.None;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            if (owenrPanel == PanelType.StorePanel)
            {
                UIMgr.Instance.OpenPanel(PanelType.BuyPanel);
                UIMgr.Instance.GetPanel<BuyPanel>(PanelType.BuyPanel).UpdateInfo(Crop);
            }
            else if (owenrPanel == PanelType.KnapsackPanel)
            {
                UIMgr.Instance.OpenPanel(PanelType.SellPanel);
                UIMgr.Instance.GetPanel<SellPanel>(PanelType.SellPanel).UpdateInfo(Crop,transform.GetChild(0).GetComponent<ItemUI>().Count);
            }
            else if (owenrPanel == PanelType.SeedsPanel)
            {
                ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
                UIMgr.Instance.ShowSelectSeedIcon(itemUI.Crop, itemUI.Count);
            }
        }
    }

    private void Update()
    {
        if (isEnter && Crop != null)
        {
            TooltipMgr.Instance.Show(Crop.GetInfoByStore());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 0)
            return;
        Crop = transform.GetChild(0).GetComponent<ItemUI>().Crop;
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        TooltipMgr.Instance.Hide();
    }

    public void SetOwner(PanelType panelType)
    {
        owenrPanel = panelType;
    }
}
