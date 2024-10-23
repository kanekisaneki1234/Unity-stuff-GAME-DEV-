using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeshiftSC : MonoBehaviour
{
    // // SceneController makeShift;
    // public delegate void ButtonEventDelegate();
    // public static event ButtonEventDelegate PlayAgainPressed;
    // public static event ButtonEventDelegate QuitPressed;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void PlayAgain()
    {
        // PlayAgainPressed?.Invoke();
        SceneController._instance.PlayAgain();
    }

    public void Quit()
    {
        // QuitPressed?.Invoke();
        SceneController._instance.MainMenu();
        // SceneManager.LoadScene("MainMenu");
    }
}
