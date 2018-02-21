using UnityEngine;

namespace Micropolis
{
    public class GameManager : MonoBehaviour
    {
        private MicropolisUnityEngine _engine;

        private void Start()
        {
            _engine = MicropolisUnityEngine.CreateUnityEngine();
            // TODO do init stuff with engine
        }

        private void Update()
        {
            _engine.tickEngine();
        }
    }
}