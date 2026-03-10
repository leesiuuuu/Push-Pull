using UnityEngine;

public class SingleMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = $"(Singleton) {typeof(T)}";
                    DontDestroyOnLoad(singleton);
                    Debug.Log($"[Singleton] '{typeof(T)}' 인스턴스가 생성되었습니다.");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] '{typeof(T)}'의 중복 인스턴스가 감지되어 파괴합니다.");
            Destroy(gameObject);
        }
    }
}
