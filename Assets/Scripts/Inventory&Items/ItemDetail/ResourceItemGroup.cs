using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceItemGroup", menuName = "Project_2D/ResourceItemGroup", order = 4)]
public class ResourceItemGroup : ScriptableObject {
    //자원 아이템 그룹화
    public GameObject[] Resource => _resource;
    [SerializeField] private GameObject[] _resource;

    public GameObject GetResource(int i)
    {
        return Resource[i];
    }
}
