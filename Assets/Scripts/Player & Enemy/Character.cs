using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class Character : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string displayName = "New Display Name";

    internal void Heal(int healthAmount)
    {
        throw new NotImplementedException();
    }

    [SerializeField] private Sprite icon;
    [SerializeField] private NetworkObject gameplayPrefab;
    public int Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public NetworkObject GameplayPrefab => gameplayPrefab;
}
