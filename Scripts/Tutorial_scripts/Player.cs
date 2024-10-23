using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    private int health;
    public int power;
    public string name;

    protected string type;

    public Player(int health, int power, string name)
    {
        this.health=health;
        this.power=power;
        this.name=name;
        type="Default";

        Debug.Log("The health is: " + health);
        Debug.Log("The power is: " + power);
        Debug.Log("The name is: " + name);
    }

    public virtual void Attack()
    {
        Debug.Log("The player attacks!");
    }

    public void Damage(Player Enemy)
    {
        Debug.Log(name + "dealt some damage!");
        Enemy.health -= power;
        Debug.Log("Remaining Health: "+Enemy.health);
    }

    // public void SetHealth(int health)
    // {
    //     this.health = health;
    // }

    // public int GetHealth()
    // {
    //     return this.health;
    // }

    public int Health
    {
        get { return this.health; }

        set { this.health = value; }

        
    }
}
