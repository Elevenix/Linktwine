using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Wave")]
public class Wave : ScriptableObject
{
    public float minRandom = 0.5f;
    public float maxRandom = 3f;
    public GameObject[] monsters;

}
