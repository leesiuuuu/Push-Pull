using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class EndPointSO : ScriptableObject
{
    [field: SerializeField]
    public string Login { get; set; } = "auth/login";
    [field: SerializeField]
    public string Logout { get; set; } = "auth/logout";
}
