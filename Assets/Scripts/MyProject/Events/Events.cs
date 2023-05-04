using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class Events : MonoBehaviour
    {
        public static event Action<GameObject> PlayerCreated;
        public static void OnPlayerCreated(GameObject player)
        {
            PlayerCreated?.Invoke(player);
        }

        public static event Action PlayerDeath;
        public static void OnPlayerDie()
        {
            PlayerDeath?.Invoke();
        }

        public static event Action ProjectileIncrease;
        public static void OnProjectileIncrease()
        {
            ProjectileIncrease?.Invoke();
        }
    }
}
