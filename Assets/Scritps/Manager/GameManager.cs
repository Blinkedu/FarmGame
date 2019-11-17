using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("EmptyGO").GetComponent<GameManager>();
            }
            return _instance;
        }
    }

    public CursorType currentCursorType = CursorType.Normal;        //鼠标指针类型
    private GameObject selectGo = null;                             //选择的土地
    public GameObject SelectGO { get { return selectGo; } }
    private Light mainLight;                    //主灯光
    public Crop SelectSeed { get; set; }        //选择的需要播种的种子

    public int CoinCount { get; set; }          //金币数量
    private Field[] fields;                     //土地数组
    private float weatherChangeTimer;      //天气改变计时器
    public GameObject RainSysGo = null;

    private void Awake()
    {
        fields = GameObject.Find("Meshes/Fields").GetComponentsInChildren<Field>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateWeather();

        Operate();

        CancelPropSelect();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        weatherChangeTimer = SysDefine.WEATHER_CHANGE_TIME;
        UIMgr.Instance.OpenPanel(PanelType.MainPanel);

        mainLight = GameObject.Find("Directional light").GetComponent<Light>();
        string dateStr = DateTime.Now.ToString("yyyy-MM-dd");   //日期
        int h = int.Parse(DateTime.Now.Hour.ToString());
        if (h >= 0 && h <= 6 || h >= 18 && h <= 23)
        {
            mainLight.intensity = 0.36f;
            mainLight.shadowStrength = 0.3f;
            dateStr += "-晚上";
        }
        else
        {
            mainLight.intensity = 1.1f;
            mainLight.shadowStrength = 0.8f;
            dateStr += "-白天";
        }

        string w = RandomWeather() == 1 ? "晴天" : "雨天";

        UIMgr.Instance.GetPanel<MainPanel>(PanelType.MainPanel).SetDateAndWeather(dateStr, w);

        if (SaveAndLoadMgr.isLoadData)          //需要加载进度
        {
            SaveAndLoadMgr.LoadAll();
        }
        else                                    //不需要加载进度
        {
            CoinCount = 10000;
        }
        UIMgr.Instance.GetPanel<MainPanel>(PanelType.MainPanel).UpdateCoin(CoinCount);
    }

    /// <summary>
    /// 改变光标
    /// </summary>
    /// <param name="cursorType"></param>
    public void ChangeCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.Normal:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case CursorType.Hand:
                Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/手 1"), Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorType.Watering:
                Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/水壶 1"), Vector2.zero, CursorMode.ForceSoftware);
                break;
            case CursorType.Shovel:
                Cursor.SetCursor(Resources.Load<Texture2D>("Sprites/铲子 1"), Vector2.zero, CursorMode.ForceSoftware);
                break;
            default:
                return;
        }
        currentCursorType = cursorType;
    }

    /// <summary>
    /// 减少金币
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool SubCoin(int count)
    {
        if (count > CoinCount)
        {
            MessageMgr.Instance.ShowMsg("金币余额不足!", Color.red);
            return false;
        }
        CoinCount -= count;
        UIMgr.Instance.GetPanel<MainPanel>(PanelType.MainPanel).UpdateCoin(CoinCount);
        return true;
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    /// <param name="count"></param>
    public void AddCoin(int count)
    {
        CoinCount += count;
        UIMgr.Instance.GetPanel<MainPanel>(PanelType.MainPanel).UpdateCoin(CoinCount);
    }

    /// <summary>
    /// 播种
    /// </summary>
    public void SowSeeds(Field field)
    {
        SeedsMgr.Instance.SubSeed(SelectSeed.ID);
        field.SetCrop(SelectSeed, selectGo.transform);
        UIMgr.Instance.SubCount();
    }
    public void SowSeedsByIndex(int index, int isLock, int id, float timer)
    {
        Field field = fields[index];
        if (isLock == 0)
        {
            field.isLock = true;
            Instantiate(Resources.Load<GameObject>("Prefabs/File_Lock"), field.transform);
            return;
        }
        else
        {
            field.isLock = false;
        }
        if (id != -1)
        {
            field.SetCrop(InventoryMgr.Instacne.GetCropById(id), field.transform, false);
            field.Timer = timer;
            if (timer <= 0)
            {
                if (field.transform.childCount > 0)
                {
                    DestroyImmediate(field.transform.GetChild(0).gameObject);
                }
                Instantiate(Resources.Load<GameObject>(InventoryMgr.Instacne.GetCropById(id).BigPrefab), field.transform);
                field.isRipe = true;
            }
        }
    }

    /// <summary>
    /// 获取天气
    /// </summary>
    /// <returns></returns>
    private int RandomWeather()
    {
        //1-晴天，2-雨天
        int index = UnityEngine.Random.Range(1, 3);
        //int index = UnityEngine.Random.Range(2, 3);
        Color color;
        switch ((WeatherType)index)
        {
            case WeatherType.Fine:
                RainSysGo.SetActive(false);
                color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                mainLight.DOColor(color, 1);
                return 1;
            case WeatherType.Rain:
                RainSysGo.SetActive(true);
                color = new Color(174 / 255f, 174 / 255f, 174 / 255f);
                mainLight.DOColor(color, 1);
                return 0;
        }
        return -1;
    }

    /// <summary>
    /// 更新天气
    /// </summary>
    private void UpdateWeather()
    {
        weatherChangeTimer -= Time.deltaTime;
        if (weatherChangeTimer <= 0)
        {
            int temp = RandomWeather();
            string str = temp == 1 ? "晴天" : "雨天";
            UIMgr.Instance.GetPanel<MainPanel>(PanelType.MainPanel).SetWeather(str);
            weatherChangeTimer = SysDefine.WEATHER_CHANGE_TIME;

        }
    }

    /// <summary>
    /// 检测鼠标点击土地和实现各道具的操作
    /// </summary>
    private void Operate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject gameObject = hit.collider.gameObject;
            if (gameObject.CompareTag("Field"))
            {
                if (selectGo != null)
                {
                    selectGo.transform.localScale = Vector3.one;
                }
                selectGo = gameObject;
                gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                Field field = selectGo.GetComponent<Field>();
                if (field.isLock || field.isEmpty)
                {
                    TooltipMgr.Instance.Hide();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (field.isLock)       //土地未解锁
                    {
                        if (currentCursorType == CursorType.Shovel)
                        {
                            if (SubCoin(200))
                            {
                                field.Deblocking();
                                MessageMgr.Instance.ShowMsg("开垦土地成功!", Color.green);
                            }
                            else
                            {
                                MessageMgr.Instance.ShowMsg("开垦土地失败: 金币余额不足!", Color.red);
                            }
                        }
                        else
                        {
                            MessageMgr.Instance.ShowMsg("该土地未解锁!");
                        }
                    }
                    else                //土地已经解锁
                    {
                        if (currentCursorType == CursorType.Normal)        //普通
                        {
                            if (field.isEmpty)              //播种
                            {
                                if (SelectSeed != null)
                                {
                                    SowSeeds(field);
                                }
                            }
                            else
                            {

                            }
                        }
                        else if (currentCursorType == CursorType.Hand)          //手
                        {
                            if (field.isRipe)
                            {
                                MessageMgr.Instance.ShowMsg("成功收获 [" + field.crop.Name + "]", Color.green);
                                KnapsackMgr.Instance.AddItem(field.crop.ID, 1);
                                field.ResetField();
                            }
                            else if (!field.isRipe && !field.isEmpty)
                            {
                                MessageMgr.Instance.ShowMsg("该作物还未成熟，不能收获!", Color.yellow);
                            }
                            else if (field.isEmpty)
                            {
                                MessageMgr.Instance.ShowMsg("该土地没有任何作物，无法收获!", Color.yellow);
                            }
                        }
                        else if (currentCursorType == CursorType.Watering)      //水壶
                        {

                            if (field.isRipe)
                            {
                                MessageMgr.Instance.ShowMsg("该作物已经成熟，不需要浇水!", Color.yellow);
                            }
                            else if (!field.isEmpty && !field.isRipe)
                            {
                                if (SubCoin(50))
                                {
                                    field.SubTime(10);
                                    MessageMgr.Instance.ShowMsg("成功为 [" + field.crop.Name + "]浇水,减少10秒生长时间!", Color.green);
                                }
                                else
                                {
                                    MessageMgr.Instance.ShowMsg("浇水失败: 金币余额不足!", Color.red);
                                }
                            }
                            else if (field.isEmpty)
                            {
                                MessageMgr.Instance.ShowMsg("该土地没有任何作物，无法浇水!", Color.yellow);
                            }
                        }
                        else if (currentCursorType == CursorType.Shovel)        //铲子
                        {
                            MessageMgr.Instance.ShowMsg("该土地已被开垦!", Color.yellow);
                        }
                    }
                }
            }
            else
            {
                if (selectGo != null)
                {
                    selectGo.transform.localScale = Vector3.one;
                    selectGo = null;
                }
                else
                {
                    TooltipMgr.Instance.Hide();
                }
            }
        }
    }

    /// <summary>
    /// 取消道具选择
    /// </summary>
    private void CancelPropSelect()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentCursorType != CursorType.Normal)
            {
                ChangeCursor(CursorType.Normal);
            }
            else
            {
                if (SelectSeed != null)
                {
                    SelectSeed = null;
                    UIMgr.Instance.HideSelectSeedIcon();
                }
            }
        }
    }
}
