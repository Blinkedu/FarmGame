using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isLock = true;
    public Crop crop = null;
    public bool isEmpty = true;
    public bool isRipe = false;         //是否成熟

    public float Timer { get; set; }
   

    void Start()
    {
        if (isLock && SaveAndLoadMgr.isLoadData==false)
        {
             Instantiate(Resources.Load<GameObject>("Prefabs/File_Lock"),transform);
        }
    }

   
    void Update()
    {
        if (GameManager.Instance.SelectGO == this.gameObject && crop != null)
        {
            if (GameManager.Instance.currentCursorType != CursorType.Hand && GameManager.Instance.currentCursorType != CursorType.Shovel)
            {
                TooltipMgr.Instance.Show(crop.GetInfoByField() + "剩余时间: " + (int)Timer + "秒\n");
            }
        }
        if (crop != null && Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                Instantiate(Resources.Load<GameObject>(crop.BigPrefab), transform);
                isRipe = true;
            }
        }
    }

    public void SetCrop(Crop crop,Transform field,bool updateTimer=true)
    {
        string path = crop.SmallPrefab;
        GameObject fruit = Instantiate(Resources.Load<GameObject>(path), field);
        fruit.transform.localPosition = new Vector3(0, -0.03f, 0);

        this.crop = crop;
        if (updateTimer)
        {
            this.Timer = crop.GetCycleToSec();
        }
        isEmpty = false;
        isRipe = false;
    }

    public void ResetField()
    {
        if (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        this.crop = null;
        this.Timer = 0;
        this.isEmpty = true;
        isRipe = false;
    }

    public void SubTime(int t)
    {
        Timer -= t;
        if(Timer<=0)
        {
            Timer = 0;
            DestroyImmediate(transform.GetChild(0).gameObject);
            Instantiate(Resources.Load<GameObject>(crop.BigPrefab), transform);
            isRipe = true;
        }
    }

    public void Deblocking()
    {
        if (isLock)
        {
            isLock = false;
            ResetField();
        }
    }
}
