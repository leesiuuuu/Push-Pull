using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCountUI : MonoBehaviour
{
    [SerializeField]
    Text countText;

    KeyCounter counter;
    private void Start()
    {
        counter = FindObjectOfType<KeyCounter>();
        SetCountText();
    }

    public void SetCountText()
    {
        countText.text = $"{counter.KeyCount}"+" / "+$"{counter._maxCount}";
        if(counter.KeyCount == counter._maxCount)
            countText.color = Color.yellow;
    }
}
