using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChunk
{
  public  string speaker;
    public string text;
    public TextChunk(string speaker, string text)
    {
        this.speaker = speaker;
        this.text = text;
    }
}
