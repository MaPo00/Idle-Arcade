using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Utils.Parents;

namespace Managers.SaveManager
{
    public class SaveManager : Utils.Parents.Singleton<SaveManager>
    {
        private void Start()
        {
            OnMoneyChanged.Invoke(Money);
        }

        #region Money
        [NonSerialized] public readonly UnityEvent<int> OnMoneyChanged = new();
        public static int Money
        {
            get => Instance.data.money;
            set
            {
                if (Money == value) return;
                Instance.data.money = value;
                Instance.OnMoneyChanged.Invoke(value);
            }
        }
        #endregion

        #region Initialisation Logic
#if UNITY_EDITOR
        [SerializeField] private bool dontUseSavings;
#endif
        private static readonly string PathToFile = Path.Combine(
#if UNITY_EDITOR
            Application.dataPath
#else
            Application.persistentDataPath
#endif
            , "Save.json"
        );

        [SerializeField]
        [Header("This data is only for debugging.")]
        private SaveManagerData data;

        protected override void OnAwakeAfterInit()
        {
            if (
#if UNITY_EDITOR
                dontUseSavings ||
#endif
                !File.Exists(PathToFile)
            ) data = new SaveManagerData();
            else Load();
        }

        private static void Save()
        {
            File.WriteAllText(PathToFile, JsonUtility.ToJson(Instance.data, true));
        }

        private static void Load()
        {
            try
            {
                Instance.data = JsonUtility.FromJson<SaveManagerData>(File.ReadAllText(PathToFile));
            }
            catch (UnityException e)
            {
                Debug.LogWarning(e);
            }
        }

        private void OnApplicationPause(bool _) => Save();
        private void OnApplicationQuit() => Save();
        #endregion
    }
}