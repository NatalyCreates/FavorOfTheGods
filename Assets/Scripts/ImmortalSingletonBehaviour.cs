using UnityEngine;

public abstract class ImmortalSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(typeof(T).FullName + " class (Immortal Singleton) was found twice, removing excess object. If this message shows on the first time the scene loads, something is wrong!");
            Destroy(gameObject);
        }
    }
}

