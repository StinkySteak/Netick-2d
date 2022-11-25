using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Netick;


/// <summary>
/// A Networkable Lightweight Timer
/// </summary>
[Networked]
public struct TickTimer
{
    public int EstablishedTick { get; private set; }
    public int TargetTick { get; private set; }
    public float RemainingTick => TargetTick - EstablishedTick;

    public float RemainingSecond(NetworkSandbox sandbox)
    {
        return RemainingTick * 1 / sandbox.FixedDeltaTime;
    }

    /// <summary>
    /// Used to Create a Simple TickTimer by getting duration parameter
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <param name="duration">Duration in Realtime seconds</param>
    /// <returns></returns>
    public static TickTimer CreateFromSeconds(NetworkSandbox sandbox, float duration)
    {
        int currentTick = sandbox.Tick.TickValue;
        float tickRate = 1 / sandbox.FixedDeltaTime;

        int targetTick = currentTick + Mathf.RoundToInt((duration * tickRate));

        return new TickTimer()
        {
            EstablishedTick = currentTick,
            TargetTick = targetTick,
        };
    }

    /// <summary>
    /// This method will compare if running tick has pass the target tick
    /// </summary>
    public bool IsExpired(NetworkSandbox sandbox)
    {
        return sandbox.Tick.TickValue >= TargetTick;
    }

    public static TickTimer None => default;
}
