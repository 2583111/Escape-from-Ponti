using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName = "WeaponData")]
public class GunData : ScriptableObject
{
    public new string name;

    public float damage;
    public float maxDistance;

    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public bool Reloading;


}
