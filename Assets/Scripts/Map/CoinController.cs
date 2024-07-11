using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private int value;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerController>().IncreaseCoin(value);
            Destroy(gameObject);
        }
    }
}
