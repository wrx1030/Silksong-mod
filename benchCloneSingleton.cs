using UnityEngine;

namespace PluginTutorial
{
    
    public class benchCloneSingleton : MonoBehaviour
    {
        public static benchCloneSingleton Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("[benchCloneSingleton] 已创建单例: " + gameObject.name);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}

