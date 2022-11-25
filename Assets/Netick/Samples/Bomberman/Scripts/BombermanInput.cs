using UnityEngine;
using Netick;

namespace Netick.Samples.Bomberman
{
    public class BombermanInput : NetworkInput
    {
        public Vector2 Movement;
        public bool    PlantBomb;
    }
}