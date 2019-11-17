using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    public Text countText;
    public Image iconImage;

    private int count = 0;
    public int Count { get { return count; } }
    private Crop crop = null;
    public Crop Crop { get { return crop; } }


    /// <summary>
    /// 创建物品
    /// </summary>
    /// <param name="crop"></param>
    /// <param name="c"></param>
    public void CreateItem(Crop crop,int c = 1)
    {
        this.crop = crop;
        count += c;
        if (count > 1)
        {
            countText.text = count.ToString();
        }
        else
        {
            countText.text = "";
        }
        iconImage.sprite = Resources.Load<Sprite>(crop.IconPath);
    }

    /// <summary>
    /// 添加物品个数
    /// </summary>
    /// <param name="c"></param>
    public void AddCount(int c = 1)
    {
        count += c;
        if (count > 1)
        {
            countText.text = count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    /// <summary>
    /// 减少物品个数
    /// </summary>
    /// <param name="c"></param>
    public void SubCount(int c = 1)
    {
        if (c > count)
        {
            //TODO 提示物品个数不足
            return;
        }
        count -= c;
        if (count > 1)
        {
            countText.text = count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

}
