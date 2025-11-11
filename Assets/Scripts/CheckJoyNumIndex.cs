using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckJoyNumIndex : MonoBehaviour
{
    private void Start()
    {
        string[] currentJoyNumNames = Input.GetJoystickNames();
        for (int i = 0; i < currentJoyNumNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(currentJoyNumNames[i]))
            {
                Debug.Log($"[{i}] 연결된 조이스틱: {currentJoyNumNames[i]}");
            }
            else
            {
                Debug.Log($"[{i}] 비어있는 슬롯");
            }
        }
    }
}
