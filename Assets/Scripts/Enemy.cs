using UnityEngine;
using System;
using UnityEngine.UI;
using System.Reflection;

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
    public Sprite sprite;

    public int takeDamage(Insult insult)
    {
        int damage = Math.Max(insult.cool - this.cool, 0) + Math.Max(insult.strength - this.strength, 0) + Math.Max(insult.wit - this.wit, 0);
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

    public Enemy(string _enemyName, int _cool, int _strength, int _wit, int _hpThresh, int _attackStrength, Sprite _sprite)
    {
        enemyName = _enemyName;
        cool = _cool;
        strength = _strength;
        wit = _wit;
        hPThresh = _hpThresh;
        attackStrength = _attackStrength;
        sprite = _sprite;
    }

}
