using System.Collections.Generic;
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
        if (collision.CompareTag("Player") && SceneManager.sceneCount <= 1)
        {
            loadCombat();
        }
    }


    void loadCombat()
    {
        CombatValues.enemies = enemies;

        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
    }
}