using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Transactions;
using UnityEngine;

public static class CombatValues 
{

    public static int buttonPressed = 0;
    public static List<Enemy> enemies = null;

    public static List<Insult> insults = new List<Insult> {new Insult("first insult", null, 10, 10, 10)};

    public static CombatLoader loadedFight = null;
   
}
