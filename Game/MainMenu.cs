using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera menuCamera;
    [SerializeField] private CinemachineVirtualCamera settingsCamera;
    [SerializeField] private GameObject MenuCanvas;
    private CursorManager cursorManager;
    private bool isOnMenu;

    private static MainMenu instance;
    #region Singleton
    public static MainMenu Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    
    private void Start()
    {
        cursorManager = CursorManager.Instance;
        menuCamera.Priority = 61;
        isOnMenu = true;
        cursorManager.EnableCursor();
    }
    public void PlayGame()
    {
        cursorManager.DisableCursor();
        MenuCanvas.SetActive(false);
        menuCamera.Priority = 1;
        settingsCamera.Priority = 1;
        isOnMenu = false;

    }
    public void QuitGame()
    {
        Application.Quit();

    }
    public void Settings()
    {
        settingsCamera.Priority = 61;
        cursorManager.EnableCursor();
    }

    public bool GetIsOnMenu()
    {
        return isOnMenu;
    }
}
