using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T instance {
        get
        {
            if (m_applicationIsQuitting)
            {
                return null;
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    private static bool m_applicationIsQuitting = false;

    /* IMPORTANT!!! To use Awake in a derived class you need to do it this way
     * protected override void Awake()
     * {
     *     base.Awake();
     *     //Your code goes here
     * }
     * */

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this as T)
        {
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        m_applicationIsQuitting = true;
    }
}

// Written by EngiGames https://youtu.be/vmKxLibGrMo
