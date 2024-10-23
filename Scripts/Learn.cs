using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Learn : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // // print("Print");
        // // Debug.Log("Debug");
        // // StartCoroutine(EnterBoss());
        Player warrior = new(100, 30, "Warrior");
        warrior.Attack();
        // Player Enemy = new(50, 50, "Enemy");
        // warrior.Damage(Enemy);
        // // int val = Enemy.GetHealth();
        // //assuming Enemy used potion
        // int potionval=10;
        // // Enemy.SetHealth(val+potionval);
        // // Debug.Log("The Enemy used potion! Their health is: " + Enemy.GetHealth());
        // Debug.Log("The Enemy used potion! Their health is: " + (Enemy.Health+potionval));

        Warrior Tanjiro = new(100,100,"Tanjiro");
        Tanjiro.Attack();
    }

    // IEnumerator EnterBoss()
    // {
        // yield return new WaitForSeconds(5f);
        // Debug.Log("The boss has entered!");
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
