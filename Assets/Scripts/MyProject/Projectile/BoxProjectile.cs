using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class BoxProjectile : MonoBehaviour
    {
        public enum ProjectileMovementType
        {
            Straight,
        }

        public ProjectileMovementType movementType = ProjectileMovementType.Straight;

        public float speed = 10f;
        public float lifetime = 5f;
        public int damage = 10;
        public float curveAmount = 1.0f;

        private PlayerActor owner;
        private ProjectileSpawner pool;

        private Coroutine returnPoolCoroutine;

        private bool isInit = false;

        private void OnEnable()
        {
            isInit = true;

            if (returnPoolCoroutine != null)
                returnPoolCoroutine = null;
            returnPoolCoroutine = StartCoroutine(ReturnPoolCO());
        }

        private void OnDisable()
        {
            returnPoolCoroutine = null;
            isInit = false;
        }

        public void Initalize(PlayerActor owner, ProjectileSpawner pool, Vector3 initPos)
        {
            this.owner  = owner;
            this.pool   = pool;

            transform.position = initPos;
        }

        IEnumerator ReturnPoolCO()
        {
            yield return new WaitForSeconds(lifetime);
            ReturnPool();

            returnPoolCoroutine = null;
        }
        
        void Update()
        {
            switch (movementType)
            {
                case ProjectileMovementType.Straight:
                    transform.position += new Vector3(transform.forward.x, 0f, transform.forward.z) * speed * Time.deltaTime;
                    break;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!IsValidCollision(other)) return;

            var enemy = other.GetComponent<EnemyActor>();
            if (enemy != null)
            {
                owner.Attack(enemy);
            }

            ReturnPool();
        }

        private void ReturnPool()
        {
            pool.DespawnObject(0, this.transform.parent.gameObject);
        }

        private bool IsValidCollision(Collider other)
        {
            if (isInit == false) return false;

            if (other.CompareTag("Player") || other.CompareTag("Projectile") || other.CompareTag("Buff")) return false;

            return true;
        }
    }
}
