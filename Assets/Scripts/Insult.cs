using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Insult
{
    public string name, description;

    public int cool, strength, wit; 

    public Insult(string _name, string _description, int _cool, int _strength, int _wit)
    {
        name = _name;
        description = (string.IsNullOrEmpty(_description)) ? "Please provide a description." : _description;
        cool = _cool;
        strength = _strength;
        wit = _wit;

    }

}

