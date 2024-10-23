using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Animator fadeControl;
    // private int sceneIndex;
    // string sceneName = SceneManager.GetActiveScene().name;
    // public static int none;
    // public static int done;
    // private static SceneController _instance;
    public static SceneController _instance;

    // // // Public property to access the singleton instance
    // public static SceneController Instance
    // {
    //     get
    //     {
    //         // If the instance doesn't exist, find it in the scene or create it
    //         if (_instance == null)
    //         {
    //             _instance = FindObjectOfType<SceneController>();

    //             // If it's still null, create a new GameObject and attach the singleton script
    //             if (_instance == null)
    //             {
    //                 GameObject singletonObject = new GameObject("MySingleton");
    //                 _instance = singletonObject.AddComponent<SceneController>();
    //             }
    //         }

    //         return _instance;
    //     }

    // }

    // // Ensure the singleton instance isn't destroyed when loading new scenes
    void Awake()
    {
        MySingleton();
    }

    void Start()
    {
        // DontDestroyOnLoad(gameObject);
    }

    void MySingleton()
    {
        // if (_instance != null)
        // {
        //     // _instance = this;
        //     // DontDestroyOnLoad(this);
        //     if(_instance!=this)
        //         Destroy(FindFirstObjectByType<SceneController>());
        // }

        // // if(_instance != null)
        // else
        // {
        //     // Destroy(FindFirstObjectByType<SceneController>());
        //     _instance=this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // _instance = this;
        if(_instance==null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else //if(_instance!=null && _instance!=this)
        {
            // var foundObjects = FindObjectsOfType<SceneController>();
            // if(foundObjects.Length>1)
            //     Destroy(foundObjects[1].gameObject);
            Destroy(gameObject);
        }
        // _instance = this;
        // DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        Listener.PlayerDeathInfo += Gameover;
        // MakeshiftSC.QuitPressed += MainMenu;
        // MakeshiftSC.PlayAgainPressed += PlayGame;
        SceneManager.sceneUnloaded += FadeInTrigger;
        // SceneManager.sceneUnloaded += FadeOutTrigger;
        // Listener.PlayerDeathInfo += FadeIn;
        Debug.Log("Enabled");
    }

    void OnDisable()
    {
        Listener.PlayerDeathInfo -= Gameover;
        // MakeshiftSC.QuitPressed += MainMenu;
        // MakeshiftSC.PlayAgainPressed += PlayGame;
        SceneManager.sceneUnloaded -= FadeInTrigger;
        // SceneManager.sceneLoaded -= FadeInTrigger;
        // Listener.PlayerDeathInfo -= FadeIn;
        Debug.Log("Disabled");
    }

    // public void OnFadeComplete()
    // {
    //     SceneManager.LoadScene(sceneIndex);
    // }
    // public void FadeOutTrigger(Scene scene)
    // {
    //     fadeControl.SetTrigger("Fadeout");
    //     // SceneManager.UnloadSceneAsync(scene);
    // }

    public void FadeInTrigger(Scene scene)
    {
        // if(SceneManager.GetActiveScene().name!="MainMenu")
        fadeControl.SetTrigger("Fadein");
        // if(SceneManager.GetActiveScene().name=="MainMenu")
        // {
            
        // }
        // if(SceneManager.LoadSceneAsync(name))
        // if(SceneManager.GetActiveScene().name!="MainMenu")
    }

    IEnumerator SceneLoadInvokation(string name)
    {
        // if(SceneManager.GetActiveScene().name!="Gameplay")
        fadeControl.SetTrigger("Fadeout");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(name);
        // AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(name);
        // while(!asyncSceneLoad.isDone)
        //     yield return null;

        // fadeControl.SetTrigger("Fadein");
    }

    public void PlayGame()
    {
        Player.Health = 100;
        StartCoroutine(SceneLoadInvokation("Gameplay"));
        // SceneManager.LoadScene("Gameplay");
        // FadeInTrigger();
    }

    public void Gameover()
    {
        // fadeControl.SetTrigger("Fadeout");
        // SceneManager.LoadScene("GameOver");
        // Invoke("gameovertest", 2f);
        StartCoroutine(SceneLoadInvokation("GameOver"));
        // Debug.Log("1");
    }

    public void MainMenu()
    {
        StartCoroutine(SceneLoadInvokation("MainMenu"));
    }

    public void PlayAgain()
    {
        Player.Health = 100;
        StartCoroutine(SceneLoadInvokation("Gameplay"));
    }

    // void gameovertest()
    // {
    //     SceneManager.LoadSceneAsync("GameOver");
    //     IEnumerator PlayTransitionAndLoadSceneCoro(string sceneName) {
    //     transition.SetTrigger("Start");
    //     yield return new WaitForSeconds(transitionTime);

    //     AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(sceneName);
    //     while (!asyncSceneLoad.isDone)
    //         yield return null; // Block execution until the scene is loaded

    //     transition.SetTrigger("End");
    // }
    // }

    // void Awake()
    // {
    //     if(instance==null)
    //     {
    //         instance=this;
    //     }
    // }

    
}
