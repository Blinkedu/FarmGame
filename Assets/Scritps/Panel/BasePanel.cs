using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }

    protected virtual void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init() { }

    /// <summary>
    /// 进入
    /// </summary>
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        CanvasGroup.blocksRaycasts = true;
        transform.SetAsLastSibling();
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.3f);
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public virtual void OnPause()
    {
        CanvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 重新进入
    /// </summary>
    public virtual void OnReenter()
    {
        CanvasGroup.blocksRaycasts = true;
        transform.SetAsLastSibling();
    }

    /// <summary>
    /// 退出
    /// </summary>
    public virtual void OnExit()
    {
        CanvasGroup.blocksRaycasts = false;
        transform.DOScale(0, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
}
