using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCReference : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGameRef()
    {
        SceneController._instance.PlayGame();
    }
}
