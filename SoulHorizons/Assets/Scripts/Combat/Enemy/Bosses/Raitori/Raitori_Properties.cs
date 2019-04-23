using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raitori_Properties : MonoBehaviour
{
    //Attacks
    //Constant ability that is on throughout the entirety of the encounter.
    public AttackData StormStrikes;
    public float stormStrikeWindUpTime;
    public float stormStrikeCooldown;
    [HideInInspector]
    public bool stormStrikesIsActive = true;
    [HideInInspector]
    public Phase stormStrikePhase;

    //Melee attack that hits the tiles directly in front of the boss, depending on birdBashRange.
    public AttackData BirdBash;
    [Tooltip("Range in number of tiles")]
    public float birdBashWindUpTime;
    public float birdBashCooldownTime;
    [HideInInspector]
    public bool birdBashIsActive;

    //Ranged attack where the boss creates two tornados projectiles.
    public AttackData TwinTornado;
    public float twinTornadoWindUpTime;
    public float twinTornadoCooldown;
    [HideInInspector]
    public bool twinTornadoIsActive;

    //Ranged attack that throws the player into the air.
    public AttackData GustGale;
    public float gustGaleWindUpTime;
    public float gustGaleKnockUpTime;
    public float gustGaleCooldownTime;
    public int gustGaleFallDamage;
    [HideInInspector]
    public bool gustGaleIsActive;

    //Melee attack that down a crossing attack
    public AttackData FeatherRend;
    public float featherRendWindUpTime;
    public float featherRendCooldownTime;
    [HideInInspector]
    public bool featherRendIsActive;

    public AttackData ConductiveSpark;
    public float conductiveSparkWindUpTime;
    public float conductiveSparkCooldownTime;
    [HideInInspector]
    public bool conductiveSparkIsActive;

    public AttackData ThunderDefense;
    public float thunderDefenseWindUpTime;
    public float thunderDefenseCooldownTime;
    [HideInInspector]
    public bool thunderDefenseIsActive;
}
