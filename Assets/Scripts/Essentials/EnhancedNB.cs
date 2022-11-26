using Netick;
using UnityEngine;

/// <summary>
/// Improved Network Behaviour with PlayerRef(PlayerId) functionallity
/// </summary>
public class EnhancedNB : NetworkBehaviour
{
    [Networked, HideInInspector] public PlayerRef PlayerId { get; set; }
#if UNITY_EDITOR
    [ReadOnly]
#endif
    [SerializeField]
    private int LatestPlayerId;

    [OnChanged(nameof(PlayerId))]
    void OnPlayerIdChanged(PlayerRef previous)
    {
      //  print($"PlayerId changed to {PlayerId.Id} at: {Sandbox.Tick}");
        LatestPlayerId = PlayerId;
        OnPlayerIdAssigned();
    }

    /// <summary>
    /// This Means PlayerRef Property is assigned/changed by Server
    /// </summary>
    protected virtual void OnPlayerIdAssigned()
    {

    }

}
