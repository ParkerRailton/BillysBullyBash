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

public class CombatManager : MonoBehaviour {
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
    TMP_Text billyName;

    public float UISpacing = 100;
    public float buttonSpacing = 100;
    public float colorChangeSpeed = 1f;

    //billy combat stats
    int billyHP = 0;
    int billyHPThresh = 50;


    List<Enemy> enemies;
    List<Enemy> defaultEncounter = new List<Enemy> { new Enemy("Goon 1", 5, 10, 0, 25, 10), new Enemy("Goon 2", 5, 10, 0, 25, 10) };

    private List<GameObject> enemyUI = new List<GameObject>();
    private List<GameObject> buttons = new List<GameObject>();

    private int selectedButton = -1;
    public IEnumerator MakeButtons(string[] buttonLabels, System.Action<int> callback)
    {
        int halfCount = buttonLabels.Count() / 2;
        float spawnShift = -halfCount;
        int selectedButton = -1;
        float evenFix = (buttonLabels.Count() % 2 == 0) ? 0.5f : 0;

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
        
        callback?.Invoke(selectedButton);
        Debug.Log("This text should be after");
        foreach (GameObject button  in buttons)
        {
            Destroy(button);
        }
        buttons.Clear();
    }

    void test(int buttonIndex)
    {
        Debug.Log("success "+buttonIndex);
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
        for (int i = 0;i < enemies.Count;i++)
        {
            Slider s = enemyUI[i].transform.Find("Enemy Slider").GetComponent<Slider>();
            s.value = enemies[i].hp / enemies[i].hPThresh;

            //  enemyUI[i].GetComponent<TMP_Text>().text = enemies[i].enemyName;
            TMP_Text text = enemyUI[i].transform.Find("Enemy Name").GetComponent<TMP_Text>();
            text.text = enemies[i].enemyName+ ": (" + enemies[i].hp + "/" + enemies[i].hPThresh + ")";
        }

        billySlider.value = billyHP / billyHPThresh;
        billyName.text = "Billy: (" + billyHP + "/" + billyHPThresh + ")";

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
        enemies = CombatValues.enemies ?? defaultEncounter;
        SpawnUI();

        StartCoroutine(MakeButtons(new string[] {"hello", "world"}, test));
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
        colorChange();
        UpdateUI();
    }
}
