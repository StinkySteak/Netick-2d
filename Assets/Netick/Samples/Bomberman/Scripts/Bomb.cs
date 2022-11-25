using UnityEngine;
using Netick;

namespace Netick.Samples.Bomberman
{
	public class Bomb : NetworkBehaviour
	{
		public GameObject		   ExplosionPrefab;

		public BombermanController Bomber;
		public float			   ExplosionDelay = 3.0f;

		private float			   _timer = 0;
		private readonly Vector3[] _directionsAroundBomb = new Vector3[4] { Vector3.right, Vector3.left, Vector3.up, Vector3.down };

		public override void NetworkDestroy()
		{
			Bomber?.SpawnedBombs.Remove(this);

			// Spawn Explosion
			if (ExplosionPrefab != null)
				Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
		}

		public override void NetworkReset()
		{
			_timer = 0;
			GetComponent<Renderer>().enabled = true;
		}

		public override void NetworkFixedUpdate()
		{
			// Destroying the bomb and dealing damage is done on the server only

			if (!IsOwner)
				return;

			_timer += Sandbox.FixedDeltaTime;

			if (_timer >= ExplosionDelay)
				Explode();
		}


		private void Explode()
		{
			// hide bomb after delay
			GetComponent<Renderer>().enabled = false;

			DamageTargetsAroundBomb(transform.position);
			Sandbox.Destroy(Object);
		}

		
		private void DamageTargetsAroundBomb(Vector3 pos)
		{
			// Find all objects around the bomb position
			// Note: Causes GC
            foreach (var dir in _directionsAroundBomb)
            {
				var hits = Physics.RaycastAll(pos, dir, 1f);

				foreach (var hit in hits)
					Damage(hit.collider.gameObject);
			}
		}

		private void Damage(GameObject target)
		{
			var obj    = target.GetComponent<NetworkObject>();
			var block  = target.GetComponent<Block>();
			var bomber = target.GetComponent<BombermanController>();

			// make sure the object is not null and in the same sandbox as the bomb
			if (obj == null || obj.Sandbox != Sandbox)
				return;

			if (block != null)
				block.Visible = false;

			if (bomber != null)
				bomber.Die();
		}
	}
}