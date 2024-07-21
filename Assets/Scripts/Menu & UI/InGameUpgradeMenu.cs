using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class InGameUpgradeMenu : MonoBehaviour
{
    [SerializeField] private OrbController orb;
    private PlayerMovement player;
    private int ringLevel = 0;
    private int magicLevel = 0;
    void Start()
    {
        player = FindObjectsOfType<PlayerMovement>().FirstOrDefault(pc => pc.IsLocal);
    }
    public void UpgradeRing()
    {
        if(ringLevel == 0)
        {
            player.gameObject.GetComponent<PlayerController>().EnableRing(true);
        } else {
            player.gameObject.GetComponent<PlayerController>().LevelUpRing();
        }
        Time.timeScale = 1f;
    }
    public void UpgradeMain()
    {
        player.gameObject.GetComponentInChildren<WeaponController>().LevelUp();
        Time.timeScale = 1f;
    }
    public void UpgradeMagic()
    {
        if(magicLevel == 0)
        {
            player.GetComponent<MagicCasting>().enabled = true;
        } else {
            player.GetComponent<MagicCasting>().LevelUp();
        }
        Time.timeScale = 1f;
    }
}
