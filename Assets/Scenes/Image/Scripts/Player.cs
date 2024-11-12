using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player
{
    // Các thuộc tính của Player
    public int xDirection { get; set; }
    public int health { get; set; }
    public int mana { get; set; }
    public int armor { get; set; }
    public int attackPower { get; set; }
    public bool isInvincible { get; set; }
    public float moveSpeed { get; set; }
    public float jumpForce { get; set; }

    public float dashDistance { get; set; }
    public float dashDuration { get; set; }
    public float dashCooldown { get; set; }

    public float rollDistance { get; set; }
    public float rollDuration { get; set; }
    public float rollCooldown { get; set; }


    // Constructor
    public Player(int health, int mana, int armor, int attackPower, float moveSpeed, float jumpForce, float dashDistance, float dashDuration, float dashCooldown, float rollrollDistance, float rollDuration, float rollCooldown)
    {
        this.xDirection = 1;
        this.health = health;
        this.mana = mana;
        this.armor = armor;
        this.attackPower = attackPower;
        this.isInvincible = false;
        this.moveSpeed = moveSpeed;
        this.jumpForce = jumpForce;
        this.dashDistance = dashDistance;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
        this.rollDistance = rollrollDistance;
        this.rollDuration = rollDuration;
        this.rollCooldown = rollCooldown;
    }
}
