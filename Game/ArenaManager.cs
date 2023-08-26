using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;


public class ArenaManager : MonoBehaviour
{
    #region Singleton
    private static ArenaManager instance;
    public static ArenaManager Instance
    {
        get { return instance; }
    }

    private void Awake()
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
        playerList = new List<PlayerManager>();
        freeLookCameraList = new List<Cinemachine.CinemachineFreeLook>();
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager player in players)
        {
            playerList.Add(player);
            var cam = player.gameObject.GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
            if (cam != null)
            {
                freeLookCameraList.Add(cam);
            }
        }

        enemyList = new List<EnemyManager>();
    }
    #endregion

    private bool switchKey = false;
    private int playerIdx;
    private int playerLimit;
    private PlayerManager mainCharacter;

    private List<PlayerManager> playerList;
    private List<Cinemachine.CinemachineFreeLook> freeLookCameraList;
    private List<EnemyManager> enemyList;
    private CursorManager cursorManager;

    [SerializeField] private Cystal crystal;

    private int playerAttacker;
    private int towerAttacker;

    public List<PlayerManager> PlayerList
    {
        get { return playerList; }
    }
    public List<Cinemachine.CinemachineFreeLook> FreeLookCameraList
    {
        get { return freeLookCameraList; }
    }
    public PlayerManager MainCharacter
    {
        get { return mainCharacter; }
    }
    public List<EnemyManager> EnemyList
    {
        get { return enemyList; }
    }
    public int GetEnemyCount()
    {
        return enemyList.Count;
    }
    public int PlayerIdx
    {
        get { return playerIdx; }
    }
    public int PlayerAttacker
    {
        get { return playerAttacker; }
        set { playerAttacker = value; }
    }
    public int TowerAttacker
    {
        get { return towerAttacker; }
        set { towerAttacker = value; }
    }
    public Cystal Crystal
    {
        get { return crystal; }
    }


    //UTILITY===================================

    
    public void RemovePlayer(PlayerManager player)
    {
        if (player.IsMainCharacter)
        {
            playerIdx = (playerIdx + 1) % playerLimit;

            for (int i = 0; i < playerLimit; i++)
            {
                var p = playerList[i];
                if (i == playerIdx)
                {
                    p.SetIsMainCharacter(true);
                    mainCharacter = player;
                }
                else
                {
                    p.SetIsMainCharacter(false);
                    p.DisableAnimation();
                }
                p.ToggleAStar();

            }
        }
        
        playerList.RemoveAt(playerList.IndexOf(player));
        Destroy(player.gameObject);
        playerLimit--;
    }
    //add enemy to list
    public void AddEnemy(EnemyManager enemy)
    { 
        enemyList.Add(enemy);
    }
    public void AddPlayerAttacker()
    {
        playerAttacker++;
    }
    public void AddTowerAttacker()
    {
       towerAttacker++;
    }
    public void DecrementPlayerAttacker()
    {
        playerAttacker--;
    }
    public void DecrementTowerAttacker()
    {
        towerAttacker--;
    }

    //remove enemy from list
    public void RemoveEnemy(EnemyManager enemy)
    {
        enemyList.RemoveAt(enemyList.IndexOf(enemy));
        Destroy(enemy.gameObject);
        
    }

    //switch character, also initiate a*
    private void SwitchCharacter()
    {
        if (switchKey)
        {
            playerIdx = (playerIdx + 1) % playerLimit;

            for (int i = 0; i < playerLimit; i++)
            {
                var player = playerList[i];
                if (i == playerIdx)
                {
                    player.SetIsMainCharacter(true);
                    mainCharacter = player;
                }
                else
                {
                    player.SetIsMainCharacter(false);
                    player.DisableAnimation();
                }
                player.ToggleAStar();
                
            }
        }
    }


    void Start()
    {
        cursorManager = CursorManager.Instance;
        if (cursorManager != null) cursorManager.DisableCursor();
        playerLimit = playerList.Count;
        playerIdx = 0;
    }

    private void Update()
    {
        if(switchKey = Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter();
        }
        if(PlayerList.Count == 0)
        {
            SceneManager.LoadScene("Town");
        }

    }

    

    
}
