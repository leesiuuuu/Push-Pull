using System.Collections.Generic;
using UnityEngine;

public class KeyCounter : MonoBehaviour
{
    [SerializeField]
    private List<Key> _keys = new List<Key>();

    public int KeyCount { get; private set; } = 0;
    public int _maxCount = 0;

    public KeyCountUI countUI;

    private void Awake()
    {
        ResetCount();
        Key[] currentKeys = FindObjectsOfType<Key>();

        countUI = FindObjectOfType<KeyCountUI>();
        foreach (var key in currentKeys)
        {
            _keys.Add(key);
        }
        _maxCount = _keys.Count;
    }

    public void AddCount(int amount = 1)
    {
        if (KeyCount + amount <= _maxCount)
            KeyCount += amount;

        countUI.SetCountText();
        Debug.Log(KeyCount);
    }

    public void ResetCount()
    {
        KeyCount = 0;
        _maxCount = 0;
    }
}
