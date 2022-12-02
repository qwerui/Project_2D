using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCollider", menuName = "Project_2D/WeaponCollider", order = 9)]
public class WeaponCollider : ScriptableObject {
    //무기 충돌 판정 정보
    public Vector2 WeaponOffset => _weaponOffset;
    public Sprite WeaponEffect => _weaponEffect;
    public Vector2 WeaponHitSize => _weaponHitSize;

    [SerializeField] private Sprite _weaponEffect;
    [SerializeField] private Vector2 _weaponHitSize;
    [SerializeField] private Vector2 _weaponOffset;
}
