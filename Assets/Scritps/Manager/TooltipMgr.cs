using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示框管理类
/// </summary>
public class TooltipMgr : MonoBehaviour {

    private static TooltipMgr _instance = null;
    public static TooltipMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("EmptyGO").GetComponent<TooltipMgr>();
            }
            return _instance;
        }
    }

    public Text contextText;    //控制大小的文本框
    public Text showText;       //显示信息的文本框
	
    /// <summary>
    /// 显示
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        contextText.transform.position = Input.mousePosition;
        contextText.gameObject.SetActive(true);
        contextText.text = text;
        showText.text = text;
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public void Hide()
    {
        contextText.gameObject.SetActive(false);
    }
    
}
