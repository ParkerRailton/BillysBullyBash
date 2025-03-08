using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CombatLoader : MonoBehaviour
{
    [SerializeField]
    Button triggerButton;

    [SerializeField]
    Collider2D triggerCollider;

    [SerializeField]
    List<Enemy> enemies;

    void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(loadCombat);
        }

        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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