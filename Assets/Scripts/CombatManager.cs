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

    //billy combat stats
    int billyHP = 0;
    int billyHPThresh = 50;


    List<Enemy> enemies;
    List<Enemy> defaultEncounter = new List<Enemy> { new Enemy("Goon 1", 5, 10, 0, 25, 10), new Enemy("Goon 2", 5, 10, 0, 25, 10) };
    List<Insult> insults;
    private List<GameObject> enemyUI = new List<GameObject>();
    private List<GameObject> buttons = new List<GameObject>();

    private int selectedButton = -1;
    private int selectedEnemy;
    public IEnumerator MakeButtons(string[] buttonLabels, System.Action<int, int> callback)
    {
        int halfCount = buttonLabels.Count() / 2;
        float spawnShift = -halfCount;
        int selectedButton = -1;
        float evenFix = (buttonLabels.Count() % 2 == 0) ? 0.5f : 0f;

        for (int i = 0; i < buttonLabels.Count(); i++)
        {

            buttons.Add(Instantiate(buttonPrefab, buttonCenter));
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

        float halfCount = enemies.Count / 2;

        float spawnShift = -halfCount;
        for (int i = 0; i < enemies.Count; i++)
        {
            Debug.Log(spawnShift);
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
            s.value = enemies[i].hp / enemies[i].hPThresh;

            //  enemyUI[i].GetComponent<TMP_Text>().text = enemies[i].enemyName;
            TMP_Text text = enemyUI[i].transform.Find("Enemy Name").GetComponent<TMP_Text>();
            text.text = enemies[i].enemyName + ": (" + enemies[i].hp + "/" + enemies[i].hPThresh + ")";
        }

        billySlider.value = billyHP / billyHPThresh;
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

    void colorChange()
    {
        Color newColor = new Color(
        Mathf.PingPong(Time.time * colorChangeSpeed, 1),  // R
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.8f, 1),  // G
        Mathf.PingPong(Time.time * colorChangeSpeed * 0.6f, 1)   // B
        );
        panelImage.color = newColor;
    }

    private void Update()
    {
        if (gameState == GameState.ENEMYTURN) panelImage.color = Color.red;
        else colorChange();
        UpdateUI();
    }

    IEnumerator SetUpBattle()
    {
        enemies = CombatValues.enemies ?? defaultEncounter;
        insults = CombatValues.insults;
        SpawnUI();
        yield return StartCoroutine(Display("The battle begins!"));
        yield return new WaitForSeconds(5f);
        ClearDisplay();
        gameState = GameState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        StartCoroutine((Display("Choose an action")));
        List<string> buttons = new List<string>();
       
        foreach (Enemy e in enemies)
        {
            if (e.hp < e.hPThresh)
            {
                buttons.Add("Attack " + e.enemyName);
            }
        }
        buttons.Add("Flee");
        StartCoroutine(MakeButtons(buttons.ToArray(), PlayerTurnButtonHandler));
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
        foreach (Insult insult in insults)
        {
            buttons.Add(insult.name);
        }

        Debug.Log("Creating insult selection buttons...");
        yield return StartCoroutine(MakeButtons(buttons.ToArray(), InsultSelectionHandler));
    }

    void InsultSelectionHandler(int i, int numOfButtons)
    {
        Debug.Log("y no button");
    }
}
