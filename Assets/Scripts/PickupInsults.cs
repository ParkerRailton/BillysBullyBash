using System;
using UnityEngine;


public class PickupInsults : MonoBehaviour
{
    [SerializeField]
    Insult insult;

    private void DoPickupInsults()
    {
        
      gameObject.SetActive(false);
                
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DoPickupInsults();
    }
}
