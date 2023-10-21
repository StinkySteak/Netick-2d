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
    public float TickDuration => TargetTick - EstablishedTick;
    public float RemainingTick(NetworkSandbox sandbox)
    {
        return TargetTick - sandbox.Tick.TickValue;
    }
    public float RemainingSecond(NetworkSandbox sandbox)
    {
        return Mathf.Max(RemainingTick(sandbox) / (1 / sandbox.FixedDeltaTime), 0f);
    }

    /// <summary>
    /// Used to Create a Simple TickTimer by getting duration parameter
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <param name="duration">Duration in Realtime seconds</param>
    /// <returns></returns>
    public static TickTimer CreateFromSeconds(NetworkSandbox sandbox, float duration)
    {
        float tickRate = 1 / sandbox.FixedDeltaTime;

        return CreateFromTicks(sandbox, Mathf.RoundToInt(duration * tickRate));
    }

    /// <summary>
    /// Used to Pauseable TickTimer by getting tick duration parameter
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <param name="tickDuration">How many Ticks are required simulated</param>
    /// <returns></returns>
    public static TickTimer CreateFromTicks(NetworkSandbox sandbox, int tickDuration)
    {
        int currentTick = sandbox.Tick.TickValue;

        int targetTick = currentTick + tickDuration;

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
        return RemainingTick(sandbox) <= 0;
    }

    public static TickTimer None => default;
}


/// <summary>
/// A Networkable Lightweight Timer with Addition of Pause Functionality
/// </summary>
[Networked]
public struct PauseableTickTimer
{
    public int EstablishedTick { get; set; }
    public int TargetTick { get; set; }
    public float TickDuration => TargetTick - EstablishedTick;
    public bool IsPaused { get; set; }
    public int LastPausedTick { get; set; }

    public int RemainingTick(NetworkSandbox sandbox)
    {
        // Ex: 
        // TargetTick: 100
        // CurrentTick: 50                          // Remaining Tick Expected to be 50
        // 
        // Paused At Tick: 60                       // Remaining Tick Expected to be 40
        //
        // Step to Current Tick: 90                 // Remaining Tick Still Expected to be 40
        //
        // 
        //     
        //
        //
        //                  100      -   90        +      90      -     60 
        // RemainingTick: TargetTick - CurrentTick +  CurrentTick - (PausedTick)
        //          
        //                                  10     +     30
        //                                         40

        if(!IsPaused)
            return TargetTick - sandbox.Tick.TickValue;
        else
            return TargetTick - sandbox.Tick.TickValue + sandbox.Tick.TickValue - LastPausedTick;
    }
    public float RemainingSecond(NetworkSandbox sandbox)
    {
        return Mathf.Max(RemainingTick(sandbox) / (1 / sandbox.FixedDeltaTime), 0f);
    }

    /// <summary>
    /// Used to Pauseable TickTimer by getting tick duration parameter
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <param name="tickDuration">How many Ticks are required simulated</param>
    /// <returns></returns>
    public static PauseableTickTimer CreateFromTicks(NetworkSandbox sandbox, int tickDuration)
    {
        int currentTick = sandbox.Tick.TickValue;

        int targetTick = currentTick + tickDuration;

        return new PauseableTickTimer()
        {
            EstablishedTick = currentTick,
            TargetTick = targetTick,
        };
    }

    /// <summary>
    /// Used to Pauseable TickTimer by getting duration parameter
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <param name="duration">Duration in Realtime seconds</param>
    /// <returns></returns>
    public static PauseableTickTimer CreateFromSeconds(NetworkSandbox sandbox, float duration)
    {
        float tickRate = 1 / sandbox.FixedDeltaTime;

        return CreateFromTicks(sandbox, Mathf.RoundToInt(duration * tickRate));
    }

    /// <summary>
    /// This method will compare if running tick has pass the target tick & checks wheter its not paused
    /// </summary>
    public bool IsExpired(NetworkSandbox sandbox)
    {
        return RemainingTick(sandbox) <= 0;
    }



    public static PauseableTickTimer None => default;
}

public static class PauseableTickTimerExtension
{
    /// <summary>
    /// How to Use?
    /// <para>TickTimer ExistingTimer = PauseableTickTimer.Pause(Sandbox);
    /// </para>
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <returns></returns>
    public static PauseableTickTimer Pause(this PauseableTickTimer timer, NetworkSandbox sandbox)
    {
        if (timer.IsPaused)
            return timer;

        return new PauseableTickTimer()
        {
            EstablishedTick = timer.EstablishedTick,
            TargetTick = timer.TargetTick,
            IsPaused = true,
            LastPausedTick = sandbox.Tick.TickValue
        };
    }

    /// <summary>
    /// How to Use?
    /// <para>TickTimer ExistingTimer = PauseableTickTimer.Resume(Sandbox);
    /// </para>
    /// </summary>
    /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
    /// <returns></returns>
    public static PauseableTickTimer Resume(this PauseableTickTimer timer, NetworkSandbox sandbox)
    {
        if (!timer.IsPaused)
            return timer;

        // Ex: 
        // TargetTick: 200
        // CurrentTick: 50                          // Remaining Tick Expected to be 150
        // 
        // Paused At Tick: 100                     // Remaining Tick Expected to be 100
        //
        // We Paused for 50 Ticks
        //
        // We Resume Timer At Tick: 150             // Expiration is Expected at Tick 150 from (30 + 120)
        //     
        //
        //
        //
        //
        //

        //        50           =          120            -         70
        int pausedDurationTick =  sandbox.Tick.TickValue - timer.LastPausedTick;

        //         150      =         100      +       50     
        int finalTargetTick = timer.TargetTick + pausedDurationTick;

        Debug.Log($"Paused Duration Tick: {pausedDurationTick}");

        return new PauseableTickTimer()
        {
            EstablishedTick = timer.EstablishedTick,
            LastPausedTick = 0,
            TargetTick = finalTargetTick,
            IsPaused = false
        };
    }
}