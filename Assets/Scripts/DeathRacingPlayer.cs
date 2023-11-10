using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DeathRacingPlayer : ScriptableObject
{
    public string name;

    public float laserDamage;
    public float projectileDamage;
    public float laserFireRate;
    public float projectileFireRate;
    public float projectileSpeed;
}
