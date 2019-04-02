using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public enum EntityType
{
    Enemy,
    Player,
    Obstacle
}

[RequireComponent(typeof(AudioSource))]

public class Entity : MonoBehaviour
{
    public EntityType type;

    public Vector2Int _gridPos = new Vector2Int();
    public Health _health = new Health();
    public scr_EntityAI _ai;
    public Territory entityTerritory;
    public SpriteRenderer spr;
    Color baseColor;
    public float lerpSpeed;

    public bool has_iframes;
    public bool invincible = false;
    public float invulnTime;
    public bool isStunned = false;
    float invulnCounter = 0f;
    public bool hasShield = false;
    float shieldCounter = 0f;
    int shieldProtection = 0; //the amount of damage the shield is reducing damage by
    int shieldProtectionIncrement = 1; //the rate the damage reduction of the shield increasesby when you move
    int shieldProtectionMax = 50;
    bool isBeingDamagedOverTime = false;

    AudioSource Hurt_SFX;
    public AudioClip[] hurts_SFX;
    private AudioClip hurt_SFX;
    public AudioClip die_SFX;

    public Animator anim;

    public GameObject deathManager;

    public void Start()
    {
        deathManager = GameObject.Find("DeathSFXManager");
        baseColor = spr.color;
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Hurt_SFX = SFX_Sources[1];
    }
    public void Update()
    {
        if (gameObject.activeSelf)
        {
            if (isStunned == false)
            {
                _ai.UpdateAI();
            }
        }
        if (_health.hp <= 0)
        {
            _ai.Die();
        }
        
        transform.position = Vector3.Lerp(transform.position, scr_Grid.GridController.GetWorldLocation(_gridPos.x, _gridPos.y), (lerpSpeed*Time.deltaTime));
        //Counts down iframes
        if (invulnCounter > 0)
        {
            invulnCounter -= Time.deltaTime;
            if(invulnCounter <= 0)
            {
                setInvincible(false, 0f);
            }
        }

        if (shieldCounter > 0)
        {
            shieldCounter -= Time.deltaTime;
            if (shieldCounter <= 0)
            {
                SetShield(false, 0f, 0, 0, 0);
            }
        }
      
    }


    public void InitPosition(int x, int y)
    {
        _gridPos = new Vector2Int(x, y);
        transform.position = scr_Grid.GridController.GetWorldLocation(_gridPos.x, _gridPos.y); 
        scr_Grid.GridController.SetTileOccupied(true, x, y, this);
        spr.sortingOrder = -_gridPos.y;
    }

    /// <summary>
    /// Tells entity to move to new coordinates. This only checks if an attack is in the space. It does not check the validity of the arguments otherwise.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetTransform(int x, int y)
    {
        //Check if you are already on this tile
        if (_gridPos == new Vector2Int(x, y))
        {                                                                                                         //if we set transform, and we havent moved
            return;                                                                                                                                    //return
        }

        //Animate movement
        if (anim != null)
        {
            anim.SetInteger("Movement", 1);
        }

        
        if(scr_Grid.GridController.CheckIfOccupied(x,y) == false && scr_Grid.GridController.CheckIfFlooded(_gridPos.x,_gridPos.y) == false)
        {
            scr_Grid.GridController.SetTileOccupied(false, _gridPos.x, _gridPos.y, this);
            _gridPos = new Vector2Int(x, y);
            scr_Grid.GridController.SetTileOccupied(true, _gridPos.x, _gridPos.y, this);
        }
        else
        {
            return;
        }

        spr.sortingOrder = -_gridPos.y;
        AttackData atk = AttackController.Instance.MoveIntoAttackCheck(_gridPos, this);

        
        if (hasShield)
        {
            Debug.Log(shieldProtection);
            if (shieldProtection < shieldProtectionMax)
            {
                shieldProtection += shieldProtectionIncrement;
            }
        }

        if(atk != null)
        {
            if (!invincible)
            {
                HitByAttack(atk);
                if (has_iframes)
                {
                    setInvincible(true, invulnTime);
                }
            }
        }
        
    }

    /// <summary>
    /// Takes an attack object and damages the entity if the attack's type is different from the entity's type.
    /// </summary>
    /// <param name="attack"></param>
    public void HitByAttack(AttackData attack)
    {
        if (attack.type != type)
        {
            int index = Random.Range(0, hurts_SFX.Length);
            hurt_SFX = hurts_SFX[index];
            Hurt_SFX.clip = hurt_SFX;
            Hurt_SFX.Play();

            float tempDamage = attack.damage * attack.modifier;
            if (tempDamage - shieldProtection >= 0)
            {
                _health.TakeDamage((int)tempDamage - shieldProtection);
            }
            else
            {
                _health.TakeDamage(0);
            }
            StartCoroutine(HitClock(.3f));
            if (type == EntityType.Player)
            {
                //camera shake
                CameraShaker.Instance.ShakeOnce(2f, 2f, 0.2f, 0.2f);
            }
        }
    }

    /// <summary>
    /// Used when an attack does not go through the attack controller system.
    /// </summary>
    /// <param name="damage">Damage dealt</param>
    /// <param name="attackType">The type of the attacking entity</param>
    public void HitByAttack(int damage, EntityType attackType)
    {
        if (attackType != type)
        {
            int index = Random.Range(0, hurts_SFX.Length);
            hurt_SFX = hurts_SFX[index];
            Hurt_SFX.clip = hurt_SFX;
            Hurt_SFX.Play();

            if (damage - shieldProtection >= 0)
            {
                _health.TakeDamage(damage - shieldProtection);
            }
            else
            {
                _health.TakeDamage(0);
            }
            StartCoroutine(HitClock(.3f));
            if (type == EntityType.Player)
            {
                //camera shake
                CameraShaker.Instance.ShakeOnce(2f, 2f, 0.2f, 0.2f);
            }
        }
    }

    public bool isInvincible()
    {
        return invincible;
    }

    //makes the entity invincible for a time
    public void setInvincible(bool inv, float time)
    {
        invincible = inv;
        if (inv)
        {
            invulnCounter = time;
            spr.color = Color.gray;
        }
        else
        {
            invulnCounter = 0f;
            invincible = false;
            spr.color = baseColor;
        }
    }

    public void SetShield (bool shield, float time, int protect, int increment, int incrementMax)
    {
        hasShield = shield;
        shieldProtection = protect; //the amount of damage the shield is reducing damage by
        shieldProtectionIncrement = increment; //the rate the damage reduction of the shield increasesby when you move
        shieldProtectionMax = incrementMax;
        if (shield == true)
        {
            shieldCounter = time;
            spr.color = Color.gray;
        }
        else
        {
            shieldCounter = 0f;
            shieldProtection = 0;
            shieldProtectionIncrement = 0;
            hasShield = false;
            spr.color = baseColor;
        }

    }

    public void Death()
    {
        deathManager.GetComponent<AudioSource>().clip = die_SFX;
        deathManager.GetComponent<AudioSource>().Play();
        if (anim != null)
        {
            anim.SetBool("Dead", true);
        }
        //Debug.Log("I AM DEAD");
        scr_Grid.GridController.SetTileOccupied(false, _gridPos.x, _gridPos.y, this);
        gameObject.SetActive(false); 
        //scr_Grid.GridController.RemoveEntity(this);  
    }

    public void TakeDamageOverTime (float duration, float damageRate, int damage)
    {
        while (duration >= 0)
        {
            _health.TakeDamage(damage);
            StartCoroutine(GenericClock(damageRate));
            duration -= damageRate;
            Debug.Log("shit");
        }
    }

     IEnumerator GenericClock (float waitTime)
     {
        yield return new WaitForSecondsRealtime(waitTime);
     }

    public void HealOverTime(float duration, float healRate, int healAmount)
    {
        while (duration >= 0)
        {
            _health.Heal(healAmount);
            StartCoroutine(GenericClock(healRate));
            duration -= healRate;
            Debug.Log("Nice");
        }
    }


    public IEnumerator gotStunned(float stunTime)
    {
        Debug.Log("Got Stunned");
        isStunned = true;
        yield return new WaitForSecondsRealtime(stunTime);
        isStunned = false;
    }

    IEnumerator HitClock(float hitTime)
    {
        spr.color = Color.red;
        //Debug.Log("I'M RED");
        yield return new WaitForSecondsRealtime(hitTime);
        spr.color = baseColor;
        //Debug.Log("NOT RED");
    }

    IEnumerator DamageOverTime (float rate, int damage)
    {       
        yield return new WaitForSecondsRealtime(rate);
        _health.TakeDamage(damage);
    }

}
[System.Serializable]
public class Health{

    public int hp = 10; //NOTE: These would be better as private variables to make mistakes less likely and to enforce the max_hp - Colin
    public int shield = 0;
    public int max_hp;

    public void TakeDamage(int damage)
    {
        if (shield > 0)
        {
            shield -= damage;
            if(shield < 0)
            {
                //Carry over extra damage to normal hp
                hp += shield;
                shield = 0;
            }
        }
        else
        {
            hp -= damage;
        }
        if (hp <= 0)
        {
            hp = 0;
        }
        //Debug.Log("MY HP: " + hp);  This was bothering me, uncomment if you desire 

    }

    public void Heal(int healAmount)
    {
        if(hp + healAmount < max_hp)
        {
            hp += healAmount;
        }
        else
        {
            hp = max_hp;
        }
    }

}





    


