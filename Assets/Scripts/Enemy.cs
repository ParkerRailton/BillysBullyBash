using UnityEngine;
using System;

[System.Serializable]
public class Enemy
{
    public string enemyName;
    public int cool;
    public int strength;
    public int wit;
    public int hp = 0;
    public int hPThresh;
    public int attackStrength;

    public int takeDamage(int cool, int strength, int wit)
    {  
        int damage = Math.Max(cool - this.cool, 0) + Math.Max(strength - this.strength, 0) + Math.Max(wit - this.wit, 0);
        hp += damage;
        return damage;
    }

    public int attack()
    {
        return attackStrength;
    }

    public Enemy(string _enemyName, int _cool, int _strength, int _wit, int _hpThresh, int _attackStrength)
    {
        enemyName = _enemyName;
        cool = _cool;
        strength = _strength;
        wit = _wit;
        hPThresh = _hpThresh;
        attackStrength = _attackStrength;
    }

}
