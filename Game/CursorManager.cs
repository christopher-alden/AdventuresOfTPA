using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region Singleton
    private static CursorManager instance;
    public static CursorManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public void EnableCursor()
    {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
    }
    public void DisableCursor()
    {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }
}
