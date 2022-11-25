﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Netick;

namespace Netick.Samples.Bomberman
{
    public class BombermanController : NetworkBehaviour
    {
        public List<Bomb>            SpawnedBombs                  = new List<Bomb>();
        [HideInInspector]
        public Vector3               SpawnPos;
        [SerializeField]
        private float                _speed                        = 6.0f;
        [SerializeField]
        private float                _speedBoostMultiplayer        = 2f;

      
        private GameObject           _bombPrefab;
        private CharacterController  _CC;

        // Networked properties
        [NetworkedGroup]
        public int                   Score            { get; set; } = 0;
        public int                   PlayerNumber     { get; set; }
        public bool                  Alive            { get; set; } = true;
        [NetworkedGroup(relevancy: Relevancy.InputSource)]
        public int                   MaxBombs         { get; set; } = 1;
        public float                 SpeedPowerUpTimer{ get; set; } = 0;
        public float                 BombPowerUpTimer { get; set; } = 0;

        void Awake()
        {
            // We store the spawn pos so that we use it later during respawn
            SpawnPos    = transform.position;
            _CC         = GetComponent<CharacterController>();
        } 

        public override void NetworkStart()
        {
            _bombPrefab = Sandbox.GetPrefab("Bomb");
        }

        public override void OnInputSourceLeft()
        {
            Sandbox.GetComponent<BombermanEventsHandler>().KillPlayer(this);
            // destroy the player object when its input source (controller player) leaves the game
            Sandbox.Destroy(Object);
        }

        public override void NetworkFixedUpdate()
        {
            if (!Alive)
                return;

            if (FetchInput(out BombermanInput input))
            {
                if (BombPowerUpTimer > 0)
                    BombPowerUpTimer -= Sandbox.FixedDeltaTime;
                else
                    MaxBombs = 1;

                if (SpeedPowerUpTimer > 0)
                    SpeedPowerUpTimer -= Sandbox.FixedDeltaTime;

                var hasSpeedBoost  = SpeedPowerUpTimer > 0;
                var speed          = hasSpeedBoost ? _speed * _speedBoostMultiplayer : _speed;

                _CC.Move(input.Movement * speed * Sandbox.FixedDeltaTime);

                // we make sure the z coord of the pos of the player is always zero
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

                // * only instantiate the bomb on the server.
                // * round the bomb pos so that it snaps to the nearest square.
                if (IsServer && input.PlantBomb && SpawnedBombs.Count < MaxBombs)
                {
                    var bomb = Sandbox.NetworkInstantiate(_bombPrefab, Round(transform.position), Quaternion.identity).GetComponent<Bomb>();
                    bomb.Bomber = this;
                    SpawnedBombs.Add(bomb);
                }
            }
        }

        public void ReceivePowerUp(PowerUpType type, float boostTime)
        {
            if (type == PowerUpType.IncreaseBombs)
            {
                SpeedPowerUpTimer += boostTime;
            }
            else if (type == PowerUpType.Speed)
            {
                BombPowerUpTimer  += boostTime;
                MaxBombs          += 1;
            }
        }

        public void Die()
        {
            Alive = false;
            Sandbox.GetComponent<BombermanEventsHandler>().KillPlayer(this);
        }

        public void Respawn()
        {
            Alive = true;
            Sandbox.GetComponent<BombermanEventsHandler>().RespawnPlayer(this);

            transform.position = SpawnPos;

            SpeedPowerUpTimer  = 0;
            BombPowerUpTimer   = 0;
            MaxBombs           = 1;
        }

        [OnChanged(nameof(Alive))]
        private void OnAliveChanged(bool previous)
        {
            // Based on state of Alive:

            // * Hide/show player object
            GetComponentInChildren<Renderer>().SetEnabled(Sandbox,Alive);

            // * Enable/disable the CharacterController
            _CC.enabled                                = Alive;
        }

        public Vector3 Round(Vector3 vec)
        {
            return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
        }

    }
}