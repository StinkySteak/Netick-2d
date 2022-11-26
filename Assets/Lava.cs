using Netick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (!Sandbox.IsServer) return;

        LevelManager.Instance.DestroyPlayer(collision.gameObject.transform.parent.GetComponent<PlayerSetup>().PlayerId);
    }
}
