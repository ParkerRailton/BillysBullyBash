using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameState
{
    START,
    WIN,
    LOSE,
    PLAYERTURN,
    ENEMYTURN
}
public class CombatManager : MonoBehaviour
{
    [SerializeField]
    GameObject eventSystemPrefab;

    [SerializeField]
    GameObject enemyUIPrefab;
    [SerializeField]
    Transform UICenter;

    [SerializeField]
    Image panelImage;

    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Transform buttonCenter;

    [SerializeField]
    Slider billySlider;
    [SerializeField]
    TMP_Text billyName, textBox;

    public GameState gameState = GameState.START;

    public float UISpacing = 100;
    public float buttonSpacing = 100;
    public float colorChangeSpeed = 1f;
    public float textDelay = 0.2f;
    public float textWait = 2f;
    //billy combat stats
    int billyHP = 0;
    int billyHPThresh = 50;


    List<Enemy> enemies;
    List<Enemy> defaultEncounter = new List<Enemy> { new Enemy("Goon 1", 5, 10, 0, 25, 10), new Enemy("Goon 2", 5, 10, 0, 25, 10) };
    List<Insult> insults;
    private List<GameObject> enemyUI = new List<GameObject>();
    private List<GameObject> buttons = new List<GameObject>();

    private int selectedEnemy;

    public IEnumerator MakeButtons(string[] buttonLabels, System.Action<int, int> callback)
    {

        int halfCount = buttonLabels.Count() / 2;
        float spawnShift = -halfCount;
        int selectedButton = -1;
        float evenFix = (buttonLabels.Count() % 2 == 0) ? 0.5f : 0f;
        Debug.Log("selectedButton = " + selectedButton);
        Debug.Log(buttonLabels.Length);
        for (int i = 0; i < buttonLabels.Count(); i++)
        {
            Debug.Log(buttonLabels[i]);
            GameObject newButton = Instantiate(buttonPrefab, buttonCenter);
            if (newButton == null)
            {
                Debug.Log("BAD");
            }
            else
            {
                Debug.Log("GOOD");
            }
            buttons.Add(newButton);
            buttons[i].transform.localPosition = new Vector2((spawnShift + evenFix) * buttonSpacing, 0);
            int buttonIndex = i;
            buttons[i].GetComponentInChildren<TMP_Text>().text = buttonLabels[i];
            buttons[i].GetComponent<Button>().onClick.AddListener(() => selectedButton = buttonIndex);
            spawnShift++;
        }

        while (selectedButton == -1)
            yield return null;

        callback?.Invoke(selectedButton, buttonLabels.Length);
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
    }



    void SpawnUI()
    {
        // GameObject newUI = Instantiate(enemyUIPrefab, UICenter);
        // enemyUI.Add(newUI);
        foreach(GameObject go in enemyUI)
        {
            Destroy(go);
        }
        enemyUI.Clear();
        float halfCount = enemies.Count / 2;

        float spawnShift = -halfCount;
        for (int i = 0; i < enemies.Count; i++)
        {
            //Debug.Log(spawnShift);
            GameObject newUI = Instantiate(enemyUIPrefab, UICenter);
            newUI.transform.localPosition = new Vector2(spawnShift * UISpacing, 0);
            enemyUI.Add(newUI);
            spawnShift++;
        }
    }
    void UpdateUI()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Slider s = enemyUI[i].transform.Find("Enemy Slider").GetComponent<Slider>();
            s.value = (float)enemies[i].hp / enemies[i].hPThresh;

            //  enemyUI[i].GetComponent<TMP_Text>().text = enemies[i].enemyName;
            TMP_Text text = enemyUI[i].transform.Find("Enemy Name").GetComponent<TMP_Text>();
            text.text = enemies[i].enemyName + ": (" + enemies[i].hp + "/" + enemies[i].hPThresh + ")";
        }

        billySlider.value = (float)billyHP / billyHPThresh;
        billyName.text = "Billy: (" + billyHP + "/" + billyHPThresh + ")";

    }
    IEnumerator Display(string text)
    {
        textBox.text = string.Empty;
        if (string.IsNullOrEmpty(text)) yield break;
        char[] chars = text.ToCharArray();
        int i = 0;
        while (i < chars.Length)
        {
            textBox.text += chars[i++];
            yield return new WaitForSeconds(textDelay);
        }
    }

    void ClearDisplay()
    {
        textBox.text = string.Empty;
    }

    private void Awake()
    {
        if (SceneManager.sceneCount == 1)
        {
            Instantiate(eventSystemPrefab);
        }
    }
    private void Start()
    {
        StartCoroutine(SetUpBattle());

    }

    void ColorChange()
    {
        Color newColor = new Color(
        Mathf.PingPong(Time.time * colorChangeSpeed, 1),  // R
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.8f, 1),  // G
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.6f, 1)   // B
        );
        panelImage.color = newColor;
    }

    void EnemyColorChange()
    {
        Color newColor = new Color(Mathf.PingPong(Time.time * colorChangeSpeed, 1), 0, 0);
        panelImage.color = newColor;
    }

    void ColorManager()
    {
        if (gameState == GameState.ENEMYTURN) EnemyColorChange();
        else ColorChange();
    }

    private void Update()
    {
        ColorManager();
        UpdateUI();
    }

    IEnumerator SetUpBattle()
    {
        enemies = CombatValues.enemies ?? defaultEncounter;
        insults = CombatValues.insults;
        SpawnUI();
        yield return StartCoroutine(Display("The battle begins!"));
        yield return new WaitForSeconds(textWait);
        ClearDisplay();
        gameState = GameState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        yield return StartCoroutine((Display("Choose an action:")));
        List<string> buttons = new List<string>();

        foreach (Enemy e in enemies)
        {
            if (e.hp < e.hPThresh)
            {
                buttons.Add("Attack " + e.enemyName);
            }
        }
        buttons.Add("Flee");

        yield return StartCoroutine(MakeButtons(buttons.ToArray(), PlayerTurnButtonHandler));
        yield break;
    }

    void PlayerTurnButtonHandler(int i, int numOfButtons)
    {
        selectedEnemy = i;

        if (i == numOfButtons - 1)
        {
            // Run away logic here
            return;
        }

        StartCoroutine(InsultSelection());
    }

    IEnumerator InsultSelection()
    {
        List<string> buttons = new List<string>();
        foreach (Insult i in insults)
        {
            buttons.Add(i.name);
        }
        yield return StartCoroutine(Display("Select an insult:"));
        //Debug.Log(i.Count);
        yield return StartCoroutine(MakeButtons(buttons.ToArray(), InsultSelectionHandler));
    }

    void InsultSelectionHandler(int i, int numOfButtons)
    {
        StartCoroutine(DamageDisplay(enemies[selectedEnemy].takeDamage(insults[i]), enemies[selectedEnemy].enemyName));
    }

    IEnumerator DamageDisplay(int damage, string enemyName)
    {
        yield return StartCoroutine(Display($"You dealt {damage} damage to {enemyName}!"));
        yield return new WaitForSeconds(textWait);
        bool allEnemiesDefeated = true;
        List<int> deadEnemies = new List<int>();
        for (int i = 0; i < enemies.Count; i++) 
        {
            if (enemies[i].hp < enemies[i].hPThresh)
            {
                allEnemiesDefeated = false;
            }
            else
            {
                yield return StartCoroutine(Display($"{enemies[i].enemyName} died!"));
                yield return new WaitForSeconds(textWait);
                deadEnemies.Add(i);
                SpawnUI();
            }
        }
        foreach (int deadIndex in deadEnemies)
        {
            enemies.Remove(enemies[deadIndex]);
        }

        if (allEnemiesDefeated)
        {
            gameState = GameState.WIN;
            //win
        }
        else
        {


            gameState = GameState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        foreach (Enemy e in enemies)
        {
            if (e.hp < e.hPThresh)
            {
                billyHP += e.strength;
                yield return StartCoroutine(Display($"{e.enemyName} dealt {e.strength} damage to Billy!"));
                yield return new WaitForSeconds(textWait);
            }

            if (billyHP >= billyHPThresh)
            {
                gameState = GameState.LOSE;
                break;
            }
        }

        if (gameState == GameState.LOSE)
        {
            gameState = GameState.LOSE;
            //lose
        }
        else
        {
            gameState = GameState.PLAYERTURN;
            yield return StartCoroutine(PlayerTurn());
        }
    }
}
