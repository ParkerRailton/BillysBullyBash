using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CombatLoader : MonoBehaviour
{
    [SerializeField]
    Button triggerButton;


    Collider2D tc;

    [SerializeField]
    List<Enemy> enemies;

    public bool battleWon = false;

    void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(loadCombat);
        }

        tc = GetComponent<Collider2D>();

        if (tc != null)
        {
            tc.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") && SceneManager.sceneCount <= 1) && !battleWon)
        {
            CombatValues.loadedFight = this;
            loadCombat();
        }
    }

   

    void loadCombat()
    {
        Debug.Log("combat should load");
        CombatValues.enemies = enemies;

        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
    }
}