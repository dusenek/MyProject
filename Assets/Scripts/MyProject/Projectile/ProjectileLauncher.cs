using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public interface ILaunchStrategy
    {
        float GetAngleStep(int numberOfProjectiles);
    }

    public class FullCircleLaunch : ILaunchStrategy
    {
        public float GetAngleStep(int numberOfProjectiles)
        {
            return 360.0f / numberOfProjectiles;
        }
    }

    public class HalfCircleLaunch : ILaunchStrategy
    {
        public float GetAngleStep(int numberOfProjectiles)
        {
            return 180.0f / numberOfProjectiles;
        }
    }

    public class SpreadLaunch : ILaunchStrategy
    {
        public float GetAngleStep(int numberOfProjectiles)
        {
            return 45.0f / numberOfProjectiles;
        }
    }

    public class ProjectileLauncher : MonoBehaviour
    {
        public GameObject projectilePrefab;

        public int numberOfProjectiles = 1;
        public float shootInterval = 0.5f;

        private float lastShootTime;

        private PlayerActor owner;

        private ProjectileSpawner projectileSpawner;
        private Coroutine shootProjectilesRoutine;

        public ILaunchStrategy launchStrategy;

        private void Awake()
        {
            Events.ProjectileIncrease += IncreaseProjectiles;
        }

        private void OnDestroy()
        {
            Events.ProjectileIncrease -= IncreaseProjectiles;
        }

        private void Start()
        {
            launchStrategy = new FullCircleLaunch();

            owner = GetComponent<PlayerActor>();

            projectileSpawner = ObjectPoolManager.Instance.GetSpawner<ProjectileSpawner>();
        }

        void Update()
        {
            if (Time.time - lastShootTime > shootInterval)
            {
                ShootProjectiles();
                lastShootTime = Time.time;
            }
        }

        void ShootProjectiles()
        {
            float angleStep = launchStrategy.GetAngleStep(numberOfProjectiles);

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var projectile = projectileSpawner.SpawnObject(0, this.transform.position + Vector3.up);
                projectile.transform.rotation = Quaternion.Euler(new Vector3(0, angleStep * i, 0));

                var boxProjectile = projectile.GetComponentInChildren<BoxProjectile>();
                boxProjectile.Initalize(owner, projectileSpawner, projectile.transform.position);
            }
        }

        void ShootProjectilesCoroutine()
        {
            if (shootProjectilesRoutine != null)
                shootProjectilesRoutine = null;

            shootProjectilesRoutine = StartCoroutine(ShootProjectilesCO());
        }

        IEnumerator ShootProjectilesCO()
        {
            float angleStep = launchStrategy.GetAngleStep(numberOfProjectiles);

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                var projectile = projectileSpawner.SpawnObject(0, this.transform.position + Vector3.up);
                projectile.transform.rotation = Quaternion.Euler(new Vector3(0, angleStep * i, 0));

                var boxProjectile = projectile.GetComponentInChildren<BoxProjectile>();
                boxProjectile.Initalize(owner, projectileSpawner, projectile.transform.position);

                yield return null;
            }

            shootProjectilesRoutine = null;
        }

        public void IncreaseProjectiles()
        {
            numberOfProjectiles++;
        }
    }
}
