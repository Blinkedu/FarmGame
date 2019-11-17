using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Button newGameBtn;
    public Button loadGameBtn;
    public Button quitBtn;

    void Start()
    {
        newGameBtn.onClick.AddListener(OnNewGameBtn);
        loadGameBtn.onClick.AddListener(OnLoadGameBtn);
        quitBtn.onClick.AddListener(OnQuitBtn);
        if (!SaveAndLoadMgr.HaveKeep())
        {
            loadGameBtn.interactable = false;
        }
    }

    private void OnNewGameBtn()
    {
        SaveAndLoadMgr.isLoadData = false;
        SceneManager.LoadScene("Main");
    }

    private void OnLoadGameBtn()
    {
        SaveAndLoadMgr.isLoadData = true;
        SceneManager.LoadScene("Main");
    }

    private void OnQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DeleteKeep();
        }
    }

    /// <summary>
    /// 删除存档（测试用）
    /// </summary>
    private void DeleteKeep()
    {
        PlayerPrefs.DeleteAll();
        loadGameBtn.interactable = false;
        SaveAndLoadMgr.isLoadData = false;
    }
}
