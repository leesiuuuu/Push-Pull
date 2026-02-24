using UnityEngine;

public interface IGetable
{
    public void Get();
}

public class Key : MonoBehaviour, IGetable
{
    KeyCounter counter;
    [SerializeField]
    AudioClip clip;
    void Start()
    {
        counter = FindObjectOfType<KeyCounter>();
    }
    public void Get()
    {
        SoundManager.Instance.SFXPlay("GetKey",clip);
        counter.AddCount();
        Destroy(gameObject);
    }
}
