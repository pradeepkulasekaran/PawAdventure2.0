using UnityEngine;
using System.Collections;
//using GooglePlayGames;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

      


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != instance)
                Destroy(this.gameObject);
        }
        
    }

    void OnEnable()
    {
        EventManager.OnSceneStart();

        
        //  #if UNITY_ANDROID 
		// 	//PlayGamesPlatform.Activate();
        //     Wenzil.Console.Console.Log("Authenticating Google Play Services");
		// #endif
         
       
        //GameCenterBinding.authenticateLocalPlayer();
       //GameCentreLoginManager.instance.AuthenticateGameCentreUser();
    }


    void OnDisable()
    {

    }


    // Use this for initialization
    void Start()
    {
        
         
    }



    public void GamePause()
    {
        //if(!gamePause)
        {
            GlobalVariables.GamePaused = true;
            Debug.Log("Game is paused");
            EventManager.OnGamePaused();
            Time.timeScale = 0.0f;

        }
    }

    public void GameResume()
    {
        //if(gamePause)
        {
            GlobalVariables.GamePaused = false;
            Debug.Log("Game resumed");
            EventManager.OnGameResumed();
            Time.timeScale = 1.0f;
        }
    }


    public void GameRestart()
    {
        GlobalVariables.GamePaused = false;
        Debug.Log("Game restarted");
        Time.timeScale = 1.0f;
        EventManager.OnGameRestart();

    }

    
    public void SceneStart()
    {
        EventManager.OnSceneStart();
    }

    public void SceneEnd()
    {
        Time.timeScale = 1.0f;
        EventManager.OnSceneEnd();
    }

    public void OnApplicationQuit()
    {
        
    }

}
