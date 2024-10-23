using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{

    public delegate void PlayerDeath();
    public static event PlayerDeath PlayerDeathInfo;

    // Start is called before the first frame update

    void LateUpdate()
    {
        // ExecuteEvent();
    }

    public void DeathEvent()
    {
        // if(Player.Health<=0)
        // {
        //     PlayerDeathInfo?.Invoke();
        //     Destroy(gameObject);
        //     // Debug.Log("1");
        // }
        PlayerDeathInfo?.Invoke();
        Destroy(this);
    }
}
