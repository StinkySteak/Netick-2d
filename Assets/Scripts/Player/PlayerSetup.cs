using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetworkPlayer = Netick.NetworkPlayer;

public class PlayerSetup : EnhancedNB
{
    public static PlayerSetup LocalPlayer;

    public Player PlayerData => PlayerManager.Instance.SpawnedPlayers[PlayerId];

    protected override void OnPlayerIdAssigned()
    {
        if (Object.IsInputSource)
            LocalPlayer = this;

        if (Object.IsOwner)
            PlayerData.PlayerState = Player.State.Spawned;

        PlayerManager.Instance.AddSpawnedPlayerObj(PlayerId, this);
    }
}
