using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemData", menuName = "Project_2D/WeaponItemData", order = 8)]
public class WeaponItemData : EquipItemData //무기 정보
{
    public WeaponCollider HitBox => _hitBox;
    [SerializeField] private WeaponCollider _hitBox;
}
