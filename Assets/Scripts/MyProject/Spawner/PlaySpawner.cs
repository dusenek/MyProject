using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class PlaySpawner : BaseSpawner
    {
        private void Start()
        {
            SpawnObject(0, Vector3.left);
        }

        public override GameObject SpawnObject(int prefabIndex, Vector3 position)
        {
            var spawnedObject = base.SpawnObject(prefabIndex, position);
            spawnedObject.name = "Player";
            var actor = spawnedObject.AddComponent<PlayerActor>();

            actor.Initialize("플레이어",100,10,
                new DefaultSkill(),
                new DamageCalculator() );

            ActorManager.Instance.AddPlayer(actor);
            Events.OnPlayerCreated(spawnedObject);

            return spawnedObject;
        }
    }
}
