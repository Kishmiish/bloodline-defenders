using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagicCasting : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject magicOrb;

    void Start()
    {
        InvokeRepeating(nameof(CastMagic), 0, cooldown);
    }
    void CastMagic()
    {
        Instantiate(magicOrb, gameObject.transform.position, quaternion.identity);
    }
    public void LevelUp()
    {
        magicOrb.GetComponent<OrbController>().LevelUP();
    }
}
