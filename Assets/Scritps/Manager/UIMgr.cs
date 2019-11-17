using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    #region 单例模式
    private static UIMgr _instance = null;
    public static UIMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("EmptyGO").GetComponent<UIMgr>();
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<PanelType, BasePanel> panelsDict = new Dictionary<PanelType, BasePanel>();       //UI缓存
    private Stack<BasePanel> panelsStack = new Stack<BasePanel>();                                      //UI栈
    private Transform panelNode = null;                                                                 //存放UI面板的节点

    private Image selectSeedIcon;
    private Text countText;
    private int selectCount = 0;

    void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        panelNode = GameObject.Find("Canvas/PanelNode").transform;
        selectSeedIcon = GameObject.Find("Canvas/SelectSeedIcon").GetComponent<Image>();
        countText = selectSeedIcon.transform.GetComponentInChildren<Text>();
        HideSelectSeedIcon();
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    /// <param name="type"></param>
    public void OpenPanel(PanelType type)
    {
        BasePanel panel = null;
        panelsDict.TryGetValue(type, out panel);
        if (panel != null)          //缓存区存在此面板
        {
            PushPanel(panel);
            panel.OnEnter();
        }
        else                        //缓存区不存在此面板
        {
            panel = LoadPanelToDict(type);
            PushPanel(panel);
            panel.OnEnter();
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public void ClosePanel()
    {
        PopPanel();
    }

    /// <summary>
    /// 加载面板到缓存区
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel LoadPanelToDict(PanelType panelType)
    {
        string path = "Panels/" + panelType.ToString();
        GameObject panelGo = Instantiate<GameObject>(Resources.Load<GameObject>(path), panelNode);
        BasePanel basePanel = panelGo.GetComponent<BasePanel>();
        panelsDict.Add(panelType, basePanel);
        return basePanel;
    }

    /// <summary>
    /// 面板入栈
    /// </summary>
    /// <param name="panel"></param>
    private void PushPanel(BasePanel panel)
    {
        if (panelsStack.Contains(panel))            //如果面板在显示UI栈中 直接返回
            return;

        if (panelsStack.Count > 0)
        {
            panelsStack.Peek().OnPause();
            panelsStack.Push(panel);
        }
        else
        {
            panelsStack.Push(panel);
        }
    }

    /// <summary>
    /// 面板出栈
    /// </summary>
    private void PopPanel()
    {
        if (panelsStack.Count <= 0)
            return;
        if (panelsStack.Count == 1)
        {
            panelsStack.Pop().OnExit();
        }
        else if (panelsStack.Count > 1)
        {
            panelsStack.Pop().OnExit();
            panelsStack.Peek().OnReenter();
        }
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelType"></param>
    /// <returns></returns>
    public T GetPanel<T>(PanelType panelType) where T : BasePanel
    {
        BasePanel panel = null;
        panelsDict.TryGetValue(panelType, out panel);
        if (panel != null)
        {
            return panel as T;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 显示选择的种子图标
    /// </summary>
    /// <param name="crop"></param>
    /// <param name="count"></param>
    public void ShowSelectSeedIcon(Crop crop, int count)
    {
        if (crop == null)
            return;
        if (GameManager.Instance.SelectSeed != null)
        {
            if (GameManager.Instance.SelectSeed.ID == crop.ID)
                return;
        }
        selectCount = count;
        selectSeedIcon.sprite = Resources.Load<Sprite>(crop.IconPath);
        countText.text = count.ToString();
        selectSeedIcon.gameObject.SetActive(true);
        GameManager.Instance.SelectSeed = crop;

    }

    /// <summary>
    /// 隐藏选择的种子图标
    /// </summary>
    public void HideSelectSeedIcon()
    {
        selectSeedIcon.gameObject.SetActive(false);
        GameManager.Instance.SelectSeed = null;
        selectCount = 0;
    }

    /// <summary>
    /// 减少数量
    /// </summary>
    /// <param name="count"></param>
    public void SubCount(int count = 1)
    {
        //TODO 种子库的数量减少
        selectCount -= 1;
        if (selectCount <= 0)
        {
            HideSelectSeedIcon();
        }
        countText.text = selectCount.ToString();
    }

    private void Update()
    {
        if (GameManager.Instance.SelectSeed != null)
        {
            selectSeedIcon.transform.position = Input.mousePosition;
        }
    }
}
