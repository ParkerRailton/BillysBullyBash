using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueTr : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextAsset rawText;
    [SerializeField] Camera targetCamera;
    Canvas canvas;
    [SerializeField]
    TMP_Text textBox;
    [SerializeField]
    TMP_Text pressSpace;

    public float zoomSpeed = 0.1f;
    public float textDelay = 0.02f;

    private List<TextChunk> dialogue = new List<TextChunk>();
    private bool moveCam = false;
    private Vector3 targetPosition;
    private float targetZoom = 3f;
    Vector3 origionalCameraPos;
    float origionalSize;
   
    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        pressSpace.enabled = false;
        targetPosition = new Vector3(transform.position.x, transform.position.y, targetCamera.transform.position.z);

        origionalCameraPos = targetCamera.transform.position;
        origionalSize = targetCamera.orthographicSize;
        List<string> lines = new List<string>(rawText.text.Split("\n"));
        foreach (string line in lines)
        {
            string[] rawChunk = line.Split(";");
            dialogue.Add(new TextChunk(rawChunk[0], rawChunk[1]));

        }

        /* 
        foreach (TextChunk chunk in dialogue)
        {
            Debug.Log($"[{chunk.speaker}] {chunk.text}");
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<PlayerMove>().playerFrozen = true;
            moveCam = true;
        }
    }
    private void Update()
    {
        if (moveCam) MoveCamera();
        
    }

    void MoveCamera()
    {
        moveCam = false;


        //handle zoom
        if (targetCamera.orthographicSize != targetZoom)
        {
            moveCam = true;
            int zoomDirection = (int) Mathf.Sign(targetZoom - targetCamera.orthographicSize);
            targetCamera.orthographicSize += zoomDirection * zoomSpeed;

            if (MathF.Abs(targetCamera.orthographicSize - targetZoom) <= 0.1)
            {
                targetCamera.orthographicSize = targetZoom;
            }

        }
        

        //handle x
        if (targetCamera.transform.position.x != targetPosition.x)
        {
            moveCam = true;
            int xDirection = (int)Mathf.Sign(targetPosition.x - targetCamera.transform.position.x);
            targetCamera.transform.position = new Vector3(targetCamera.transform.position.x + xDirection * zoomSpeed, targetCamera.transform.position.y, targetCamera.transform.position.z);
            if (MathF.Abs(targetCamera.transform.position.x - targetPosition.x) <= 0.1)
            {
                targetCamera.transform.position = new Vector3(targetPosition.x, targetCamera.transform.position.y, targetCamera.transform.position.z);
            }

        }

        //handle y
        if (targetCamera.transform.position.y != targetPosition.y)
        {
            moveCam = true;
            int yDirection = (int)Mathf.Sign(targetPosition.y - targetCamera.transform.position.y);
            targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y + yDirection * zoomSpeed, targetCamera.transform.position.z);
            if (MathF.Abs(targetCamera.transform.position.y - targetPosition.y) <= 0.1)
            {
                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetPosition.y, targetCamera.transform.position.z);
            }

        }

        if (!moveCam)
        {
            Debug.Log("moveCam false");
            canvas.enabled = true;
            StartCoroutine(DisplayChunk(0));
        }
    }
    

    IEnumerator DisplayChunk(int i)
    {
        textBox.text = $"[{dialogue[i].speaker}] ";
        string text = dialogue[i].text;
        char[] chars = text.ToCharArray();
        int j = 0;
        while (j < chars.Length)
        {
            textBox.text += chars[j++];
            yield return new WaitForSeconds(textDelay);
        }

        pressSpace.enabled = true;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        pressSpace.enabled = false;

        if (i < dialogue.Count - 1) StartCoroutine(DisplayChunk(i + 1));
        else { canvas.enabled = false;
            targetCamera.transform.position = origionalCameraPos;
            targetCamera.orthographicSize = origionalSize;
            GameObject.Find("Player").GetComponent<PlayerMove>().playerFrozen = false;
        }
        yield break;

    }


}
