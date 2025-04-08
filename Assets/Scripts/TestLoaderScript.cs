using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TestLoaderScript : MonoBehaviour
{

    [SerializeField]
    Button button1, button2;

    

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(load1);
        button2.onClick.AddListener(load2);

    }

    void loadCombat() {
        SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Additive);
    }

    void load1()
    {
        CombatValues.buttonPressed = 1;
        loadCombat();

    }

    void load2()
    {
        CombatValues.buttonPressed = 2;
        loadCombat();
    }

    
}
