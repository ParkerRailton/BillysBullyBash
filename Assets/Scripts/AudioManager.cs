using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class AudioManager : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioSource battleMusic;

    //When adding audio into the audio manager in the inspector, create 2 audio source components - one with battle music and one with the level music. Then, drag them into the corresponding spots. 
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
