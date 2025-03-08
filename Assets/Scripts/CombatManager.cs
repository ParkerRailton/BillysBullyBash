using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameState {START, ATTACKING, PLAYERTURN, ENEMYTURN, WIN, LOSE }

public class CombatManager : MonoBehaviour
{
    public GameState State;

    [SerializeField]
    TMP_Text bodyText, enemyName, billyStats, enemyStats;

    [SerializeField]
    Image panelImage;

    [SerializeField]
    Slider playerSlider, enemySlider;

    [SerializeField]
    Button button1, button2, button3;

    List<Enemy> enemies;

    private float colorChangeSpeed = 0.5f;

    private Enemy[] defaultEnemies = { new Enemy("goon", 0, 5, 10, 30, 10) };

    int billyAttack = 15;
    int billyHP = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (button1 != null)
        {
            button1.onClick.AddListener(onButton1Click);
        }
        if (button2 != null)
        {
            button2.onClick.AddListener(onButton2Click);
        }
        if (button3 != null)
        {
            button3.onClick.AddListener(onButton3Click);
        }
        enemies = (CombatValues.enemies != null) ? CombatValues.enemies : defaultEnemies.ToList();
        
        State = GameState.START;
        StartCoroutine(setUpBattle());
        
    }

    // Update is called once per frame
    void Update()
    {
        colorChange();
        updateStats();

        
    }

    void updateStats()
    {
        playerSlider.value = billyHP / 50f;
        enemySlider.value = enemies[0].hp / enemies[0].hPThresh;
        if (enemies != null)
            enemySlider.SetValueWithoutNotify(enemies[0].hp / (float) enemies[0].hPThresh);
        enemyName.text = enemies[0].enemyName;
        billyStats.text = "(" + billyHP + "/" + 50 + ")";
        enemyStats.text = "(" + enemies[0].hp + "/" + enemies[0].hPThresh + ")";
    }

    IEnumerator setUpBattle()
    {
        if (enemies != null)
        enemyName.text = enemies[0].enemyName;
       
        display("The battle begins");
        yield return new WaitForSeconds(5f);
        State = GameState.PLAYERTURN;
        StartCoroutine(playerTurn());
    }

    IEnumerator playerTurn()
    {
        display("choose an action");
        button1.GetComponentInChildren<TMP_Text>().text = "Attack";
        button2.GetComponentInChildren<TMP_Text>().text = "Run Away";
        yield break;
    }

    IEnumerator enemyTurn()
    {
        int damage = enemies[0].attackStrength;
        billyHP += damage;
        display(enemies[0].enemyName + " deals " + damage + " damage to you!");

        yield return new WaitForSeconds(2f);

        if (billyHP > 50)
        {
            State = GameState.LOSE;
            StartCoroutine(lose());

        }
        else
        {
            State = GameState.PLAYERTURN;
            StartCoroutine(playerTurn());
        }
    }

    void display(string text) { 
        bodyText.text = text;
    }
    void colorChange()
    {
        Color newColor = new Color(
        Mathf.PingPong(Time.time * colorChangeSpeed, 1),  // R
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.8f, 1),  // G
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.6f, 1)   // B
        );
        panelImage.color = newColor;
    }

    IEnumerator playerAttack(int damageType)
    {
        button1.GetComponentInChildren<TMP_Text>().text = "";
        button2.GetComponentInChildren<TMP_Text>().text = "";
        button3.GetComponentInChildren<TMP_Text>().text = "";
        int damage = 0;
        switch (damageType) {
            case 1:
               damage = enemies[0].takeDamage(billyAttack, 0, 0);
                break;
            case 2:
                damage = enemies[0].takeDamage(0, billyAttack, 0);
                break;
            case 3:
                damage = enemies[0].takeDamage(0, 0, billyAttack);
                break;
        }

       
        
        display("You deal " + damage + " damage!");

        yield return new WaitForSeconds(3f);

        if (enemies[0].hp > enemies[0].hPThresh)
        {
            State = GameState.WIN;
            StartCoroutine(win());
        }
        else
        {
            State = GameState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }


       

    }

    void end()
    {
        Scene currentScene = SceneManager.GetSceneByName("CombatScene");
        SceneManager.UnloadSceneAsync(currentScene);
    }
    IEnumerator win()
    {
        display("You won the battle");
        yield return new WaitForSeconds(3f);
        end();
    }

    IEnumerator attackChoice()
    {
        button1.GetComponentInChildren<TMP_Text>().text = "Insult their Cool";
        button2.GetComponentInChildren<TMP_Text>().text = "Insult their Strength";
        button3.GetComponentInChildren<TMP_Text>().text = "Insult their Wit";

        yield break;
    }

    void onButton1Click()
    {
        if (State == GameState.PLAYERTURN)
        {
            State = GameState.ATTACKING;
            StartCoroutine(attackChoice());
        }
        else if (State == GameState.ATTACKING)
        {
            StartCoroutine(playerAttack(1));
        }

    }
    IEnumerator runAway()
    {
        button1.GetComponentInChildren<TMP_Text>().text = "";
        button2.GetComponentInChildren<TMP_Text>().text = "";
        button3.GetComponentInChildren<TMP_Text>().text = "";
        display("You ran away like a coward!");
        yield return new WaitForSeconds(2f);
        State = GameState.LOSE;
        StartCoroutine(lose());
    }

    IEnumerator lose()
    {

        display("You lost the battle");
        yield return new WaitForSeconds(3f);
        end();
    }
    void onButton2Click()
    {
        if (State == GameState.PLAYERTURN)
        {
            StartCoroutine(runAway());
        }
        else if (State == GameState.ATTACKING)
        {
            StartCoroutine(playerAttack(2));
        }

    }

    void onButton3Click()
    {
        if (State == GameState.ATTACKING)
        {
            StartCoroutine(playerAttack(3));
        }
    }

}
