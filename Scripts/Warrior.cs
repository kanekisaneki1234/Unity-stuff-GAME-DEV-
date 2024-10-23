using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public Warrior(int health, int power, string name) : base(health, power, name)
    {
        Health = health;
        this.power = power;
        this.name = name;
        type = "Child";
    }

    public override void Attack()
    {
        Debug.Log("The warrior attacks bravely with his nichirin!");
        // base.Attack();
    }


}
