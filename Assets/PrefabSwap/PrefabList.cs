using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "ScriptableObjects/PrefabList", order = 1)]
public class PrefabList : ScriptableObject
{
    [System.Serializable]
    public struct PrefabInfo
    {
        public string name;
        public GameObject prefab;
    }

    public List<PrefabInfo> prefabList;
}
