using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPanel : BasePanel {

    public Button backMenuBtn;
    public Button quitBtn;
    public Button closeBtn;

    protected override void Init()
    {
        backMenuBtn.onClick.AddListener(OnBackMenuBtn);
        quitBtn.onClick.AddListener(OnQuitBtn);
        closeBtn.onClick.AddListener(OnCloseBtn);
    }

    private void OnBackMenuBtn()
    {
        SaveAndLoadMgr.SaveAll();
        SceneManager.LoadScene("Menu");
    }

    private void OnQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SaveAndLoadMgr.SaveAll();
        Application.Quit();
#endif
    }

    private void OnCloseBtn()
    {
        UIMgr.Instance.ClosePanel();
    }
}
