using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.MyProject
{
    public class Launcher : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene((int)EScene.Playground);
        }
    }
}
