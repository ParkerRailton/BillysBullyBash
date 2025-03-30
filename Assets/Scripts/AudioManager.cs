using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class AudioManager : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioSource battleMusic;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.sceneCount <= 1)
        {
            levelMusic.enabled = true;
            battleMusic.enabled = false;
        }
        else
        {
            levelMusic.enabled = false;
            battleMusic.enabled = true;
        }
    }
}
