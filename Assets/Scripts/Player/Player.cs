using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EnhancedNB
{
    public static Player LocalPlayer;

    [Networked] public int TeamId { get; set; }

    public override void NetworkStart()
    {
        print($"IsLocalPlayer: {Object.IsInputSource} TeamId: {TeamId}");

        if (Object.IsInputSource)
            LocalPlayer = this;
    }

    protected override void OnPlayerIdAssigned()
    {
        print($"Player Spawned: IsLocalPlayer: {Object.IsInputSource} PlayerId: {PlayerId.Id} Tick: {Sandbox.Tick}");

        PlayerManager.Instance.AddSpawnedPlayer(PlayerId, this);
    }
}
