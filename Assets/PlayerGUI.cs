using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    public PlayerSetup PlayerSetup;
    public TMP_Text Text_JumpCooldown;

    private void OnEnable()
    {
        LevelManager.OnRender += Render;
    }
    private void OnDisable()
    {
        LevelManager.OnRender -= Render;
    }

    private void Render()
    {
        if (PlayerSetup.LocalPlayer == null || PlayerSetup.PlayerController == null)
            return;

        Text_JumpCooldown.gameObject.SetActive(!PlayerSetup.PlayerController.JumpCooldownTimer.IsExpired(Player.LocalSandbox));

        Text_JumpCooldown.text = PlayerSetup.PlayerController.JumpCooldownTimer.RemainingSecond(Player.LocalSandbox) + "s";
    }
}
