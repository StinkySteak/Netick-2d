using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetworkPlayer = Netick.NetworkPlayer;

public class PlayerSetup : EnhancedNB
{
    public static PlayerSetup LocalPlayer;

    protected override void OnPlayerIdAssigned()
    {
        if (Object.IsInputSource)
            LocalPlayer = this;

        PlayerManager.Instance.AddSpawnedPlayerObj(PlayerId, this);
    }
}
