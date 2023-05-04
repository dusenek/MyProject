using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class ProjectileSpawner : BaseSpawner
    {
        public override GameObject SpawnObject(int prefabIndex, Vector3 position)
        {
            var spawnedObject = base.SpawnObject(prefabIndex, position);
            return spawnedObject;
        }
    }
}
