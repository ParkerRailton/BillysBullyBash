using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class InsultArray
{
    public static bool[] insults = new bool[6];
}

public class PickupInsults : MonoBehaviour
{
    public int insultNum_0to5 = 0;
    public Object target;

    private void DoPickupInsults()
    {
        
        InsultArray.insults[insultNum_0to5] = true;
        Object currentTarget = target ?? gameObject;
        Behaviour targetBehaviour = currentTarget as Behaviour;
        GameObject targetGameObject = currentTarget as GameObject;
        if (targetBehaviour != null)
        {
            targetGameObject = targetBehaviour.gameObject;
        }
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(false);
        }
                
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DoPickupInsults();
    }
}
