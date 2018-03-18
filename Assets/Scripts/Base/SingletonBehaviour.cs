using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null) Instance = this as T;
        else throw new System.Exception(typeof(T).FullName + " class is Singleton, but has more than 1 instance!");
    }
}
