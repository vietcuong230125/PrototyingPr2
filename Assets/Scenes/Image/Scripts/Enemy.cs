using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy 
{
    public int xDirection { get; set; }
    private int health { get; set; }
    public int attackPower { get; set; }
    public float moveSpeed { get; set; }
    public Enemy (int health, int attackPower, float moveSpeed)
    {
        this.health = health;
        this.attackPower = attackPower;
        this.moveSpeed = moveSpeed;
        xDirection = -1;
    }
}
