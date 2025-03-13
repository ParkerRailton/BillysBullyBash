using System;
using UnityEngine;
using Object = UnityEngine.Object;
using static InsultArray;
using static CombatManager;

public class InsultUI : MonoBehaviour
{
     private CombatManager combatManager;
    public Object Locked_1;
    public Object Insult_1;
    public Object Locked_2;
    public Object Insult_2;
    public Object Locked_3;
    public Object Insult_3;
    public Object Locked_4;
    public Object Insult_4;
    public Object Locked_5;
    public Object Insult_5;
    public Object Locked_6;
    public Object Insult_6;

    private GameObject targetLocked_1, targetInsult_1;
    private GameObject targetLocked_2, targetInsult_2;
    private GameObject targetLocked_3, targetInsult_3;
    private GameObject targetLocked_4, targetInsult_4;
    private GameObject targetLocked_5, targetInsult_5;
    private GameObject targetLocked_6, targetInsult_6;

    private void Awake()
    {
        combatManager = FindObjectOfType<CombatManager>();
        if (combatManager == null)
        {
            Debug.LogError("CombatManager not found in the scene!");
        }

        targetLocked_1 = ((Locked_1 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_1 ?? gameObject) as GameObject;
        targetInsult_1 = ((Insult_1 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_1 ?? gameObject) as GameObject;

        targetLocked_2 = ((Locked_2 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_2 ?? gameObject) as GameObject;
        targetInsult_2 = ((Insult_2 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_2 ?? gameObject) as GameObject;

        targetLocked_3 = ((Locked_3 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_3 ?? gameObject) as GameObject;
        targetInsult_3 = ((Insult_3 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_3 ?? gameObject) as GameObject;

        targetLocked_4 = ((Locked_4 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_4 ?? gameObject) as GameObject;
        targetInsult_4 = ((Insult_4 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_4 ?? gameObject) as GameObject;

        targetLocked_5 = ((Locked_5 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_5 ?? gameObject) as GameObject;
        targetInsult_5 = ((Insult_5 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_5 ?? gameObject) as GameObject;

        targetLocked_6 = ((Locked_6 ?? gameObject) as Behaviour)?.gameObject ?? (Locked_6 ?? gameObject) as GameObject;
        targetInsult_6 = ((Insult_6 ?? gameObject) as Behaviour)?.gameObject ?? (Insult_6 ?? gameObject) as GameObject;
    }

    private void Update()
    {
        if (combatManager != null && combatManager.State == GameState.ATTACKING)
        {
            if (InsultArray.insults[0] == true)
            {
                if (targetLocked_1 != null)
                {
                    targetLocked_1.SetActive(false);
                }
                if (targetInsult_1 != null)
                {
                    targetInsult_1.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_1 != null)
                {
                    targetLocked_1.SetActive(true);
                }
                if (targetInsult_1 != null)
                {
                    targetInsult_1.SetActive(false);
                }
            }
            if (InsultArray.insults[1] == true)
            {
                if (targetLocked_2 != null)
                {
                    targetLocked_2.SetActive(false);
                }
                if (targetInsult_2 != null)
                {
                    targetInsult_2.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_2 != null)
                {
                    targetLocked_2.SetActive(true);
                }
                if (targetInsult_2 != null)
                {
                    targetInsult_2.SetActive(false);
                }
            }
            if (InsultArray.insults[2] == true)
            {
                if (targetLocked_3 != null)
                {
                    targetLocked_3.SetActive(false);
                }
                if (targetInsult_3 != null)
                {
                    targetInsult_3.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_3 != null)
                {
                    targetLocked_3.SetActive(true);
                }
                if (targetInsult_3 != null)
                {
                    targetInsult_3.SetActive(false);
                }
            }
            if (InsultArray.insults[3] == true)
            {
                if (targetLocked_4 != null)
                {
                    targetLocked_4.SetActive(false);
                }
                if (targetInsult_4 != null)
                {
                    targetInsult_4.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_4 != null)
                {
                    targetLocked_4.SetActive(true);
                }
                if (targetInsult_4 != null)
                {
                    targetInsult_4.SetActive(false);
                }
            }
            if (InsultArray.insults[4] == true)
            {
                if (targetLocked_5 != null)
                {
                    targetLocked_5.SetActive(false);
                }
                if (targetInsult_5 != null)
                {
                    targetInsult_5.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_5 != null)
                {
                    targetLocked_5.SetActive(true);
                }
                if (targetInsult_5 != null)
                {
                    targetInsult_5.SetActive(false);
                }
            }
            if (InsultArray.insults[5] == true)
            {
                if (targetLocked_6 != null)
                {
                    targetLocked_6.SetActive(false);
                }
                if (targetInsult_6 != null)
                {
                    targetInsult_6.SetActive(true);
                }
            }
            else
            {
                if (targetLocked_6 != null)
                {
                    targetLocked_6.SetActive(true);
                }
                if (targetInsult_6 != null)
                {
                    targetInsult_6.SetActive(false);
                }
            }
        }
        else
        {
            if (targetInsult_1 != null)
            {
                targetInsult_1.SetActive(true);
            }
            if (targetInsult_2 != null)
            {
                targetInsult_2.SetActive(true);
            }
            if (targetInsult_3 != null)
            {
                targetInsult_3.SetActive(true);
            }
            if (targetInsult_4 != null)
            {
                targetInsult_4.SetActive(true);
            }
            if (targetInsult_5 != null)
            {
                targetInsult_5.SetActive(true);
            }
            if (targetInsult_6 != null)
            {
                targetInsult_6.SetActive(true);
            }
        }
    }
}
