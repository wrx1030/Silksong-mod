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
                Instance = this;               
                DontDestroyOnLoad(gameObject);
                Debug.Log("[benchCloneSingleton] 检测到重复实例，替换旧的单例");
            }
        }
    }

}

