using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerController>().IncreaseXP();
            Destroy(gameObject);
        }
    }
}
