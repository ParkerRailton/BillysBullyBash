using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    float start = 5.5f;
    float end = -5.5f;
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, start, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > end)
        {
             gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.0005f, gameObject.transform.position.z);
        }
        else
        {
            SceneManager.LoadScene("TutorialLevel");
        }
    }
}
