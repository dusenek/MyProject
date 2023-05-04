using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public interface IBuff
    {
        void ApplyBuff(GameObject target);
    }

    public class ProjectileIncreaseBuff : IBuff
    {
        public void ApplyBuff(GameObject target)
        {
            Events.OnProjectileIncrease();
        }
    }

    public class BuffItem : MonoBehaviour
    {
        public float lifetime = 5f;

        private IBuff buff;
        private BuffSpawner pool;

        private Coroutine returnPoolCoroutine;

        private bool isInit = false;

        public void Initialize(BuffSpawner buffSpawner)
        {
            this.pool = buffSpawner;
            buff = new ProjectileIncreaseBuff();
        }

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

        IEnumerator ReturnPoolCO()
        {
            yield return new WaitForSeconds(lifetime);
            ReturnPool();

            returnPoolCoroutine = null;
        }

        private void ReturnPool()
        {
            pool.DespawnObject(0, this.transform.parent.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && isInit == true)
            {
                if (buff != null)
                {
                    buff.ApplyBuff(other.gameObject);
                }

                ReturnPool();
            }
        }
    }
}
