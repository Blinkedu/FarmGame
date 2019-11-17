using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 消息管理
/// </summary>
public class MessageMgr : MonoBehaviour {
    private static MessageMgr _instance = null;
    public static MessageMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("EmptyGO").GetComponent<MessageMgr>();
            }
            return _instance;
        }
    }

    public Text msgText;        //显示消息的文本框
    
    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMsg(string msg)
    {
        msgText.color = Color.white;
        msgText.gameObject.SetActive(false);
        msgText.gameObject.SetActive(true);
        msgText.text = msg;
    }

    public void ShowMsg(string msg,Color color)
    {
        ShowMsg(msg);
        msgText.color = color;
    }
}
