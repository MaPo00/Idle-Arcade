using DG.DemiLib.Attributes;
using UnityEngine;

namespace Utils.Parents
{
    [DeScriptExecutionOrder(-5)]
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        private void Awake()
        {
            var self = this as T;


            if (Instance == null)
            {
                Instance = self;
                OnAwakeAfterInit();
            }
            else
            {
                if (self != null) Destroy(self.gameObject);
                return;
            }

            DontDestroyOnLoad(self);
        }

        protected virtual void OnAwakeAfterInit()
        {
        }
    }
}