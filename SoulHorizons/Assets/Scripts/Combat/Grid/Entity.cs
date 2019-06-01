using System.Collections;
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

    public int height = 1;
    public int width = 1;
    public Vector2Int _gridPos = new Vector2Int();
    public Vector2Int[] gridPositions;
    public Health _health = new Health();
    public scr_EntityAI _ai;
    public Territory entityTerritory;
    public SpriteRenderer spr;
    public Shader hitShader;
    private Shader baseShader;
    public GameObject staticShield;
    public GameObject blur;
    public GameObject stun;
    public GameObject dampen;
    Color baseColor;
    public float lerpSpeed;
    private float hitFlashTimer = .01f;

    public bool has_iframes;
    public bool invincible = false;
    public float invulnTime;
    public float damageVulnerability = 1f;
    public bool isStunned = false;
    public bool isImmobile = false;
    public bool isDampened = false;
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
    public bool hasDeathAnim;

    public GameObject deathManager;
    private Material defaultMaterial;
    [SerializeField]
    private Material highlightMaterial;

    public void Start()
    {
        deathManager = GameObject.Find("DeathSFXManager");
        baseColor = spr.color;
        baseShader = spr.material.shader;
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Hurt_SFX = SFX_Sources[1];
    }
    public void Update()
    {
        if (isDampened)
        {
            dampen.SetActive(true);
        }
        else
        {
            dampen.SetActive(false);
        }

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
                SetInvincible(false, 0f);
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

        if(height * width > 1)
        {
            SetLargeTransform(new Vector2Int(x, y));
        }

        //Animate movement
        if (anim != null)
        {
            anim.SetInteger("Movement", 1);
        }


        if (scr_Grid.GridController.CheckIfOccupied(x, y) == false  && isImmobile == false)
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
                    SetInvincible(true, invulnTime);
                }
            }
        }

    }

    public void Highlight()
    {
        defaultMaterial = spr.material;
        spr.material = highlightMaterial;
    }

    public void DeHighlight()
    {
        spr.material = defaultMaterial;
    }

    //NOTE: GridPosition is the origin of the large transform, or the bottom leftmost tile.
    public void SetLargeTransform(Vector2Int gridPosition)
    {
        //Check if you are already on this tile
        if (_gridPos == gridPosition || width <= 0 || height <= 0)
        {
            return;
        }

        //Animate movement
        if (anim != null)
        {
            anim.SetInteger("Movement", 1);
        }

        gridPositions = new Vector2Int[width*height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                scr_Grid.GridController.SetTileOccupied(false, _gridPos.x + i, _gridPos.y + j, this);
            }
        }
        _gridPos = gridPosition;
        spr.sortingOrder = -_gridPos.y;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridPositions[i * height + j] = new Vector2Int(gridPosition.x + i, gridPosition.y + j);
                scr_Grid.GridController.SetTileOccupied(true, gridPosition.x, gridPosition.y, this);
            }
        }

        AttackData atk = AttackController.Instance.MoveIntoAttackCheck(gridPositions[0], this);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                atk = AttackController.Instance.MoveIntoAttackCheck(gridPositions[i * height + j], this);
            }
        }

        if (atk != null)
        {
            if (!invincible)
            {
                HitByAttack(atk);
                if (has_iframes)
                {
                    //Activate invincibility frames
                    SetInvincible(true, invulnTime);
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
            if (anim)
            {
                anim.SetBool("Hit", true);
            }

            float tempDamage = attack.damage * attack.damageModifier * damageVulnerability;
            Debug.Log("Damage Vuln: " + damageVulnerability);
            if (scr_Grid.GridController.CheckIfHelpful(_gridPos.x, _gridPos.x) == true)
            {
                tempDamage = attack.damage * scr_Grid.GridController.grid[_gridPos.x, _gridPos.y].GetTileProtection();
            }
            if (tempDamage - shieldProtection >= 0)
            {
                _health.TakeDamage((int)tempDamage - shieldProtection, this);
            }
            else
            {
                _health.TakeDamage(0, this);
            }
            StartCoroutine(HitClock(hitFlashTimer));
            if (type == EntityType.Player)
            {
                //camera shake
                CameraShaker.Instance.ShakeOnce(2f, 2f, 0.2f, 0.2f);
            }
        }
    }

    public void Weaken(float vulnerability, float duration)
    {
        StartCoroutine(WeakenTimer(vulnerability, duration));
    }

    public IEnumerator WeakenTimer(float vulnerability, float duration)
    {
        damageVulnerability = vulnerability;
        yield return new WaitForSeconds(duration);
        damageVulnerability = 1f;
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
                _health.TakeDamage(damage - shieldProtection, this);
            }
            else
            {
                _health.TakeDamage(0, this);
            }
            StartCoroutine(HitClock(hitFlashTimer));
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
    public void SetInvincible(bool inv, float time)
    {
        invincible = inv;
        if (inv)
        {
            invulnCounter = time;
            spr.color = Color.gray;
            if(type == EntityType.Player)
            {
                blur.SetActive(true);
            }
        }
        else
        {
            invulnCounter = 0f;
            invincible = false;
            spr.color = baseColor;
            if (type == EntityType.Player)
            {
                blur.SetActive(false);
            }
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
            staticShield.SetActive(true);
        }
        else
        {
            shieldCounter = 0f;
            shieldProtection = 0;
            shieldProtectionIncrement = 0;
            hasShield = false;
            staticShield.SetActive(false);
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
        _health.TakeDamage(_health.hp, this);
        if (hasDeathAnim)
        {
            gameObject.GetComponent<Entity>().enabled = false;
        }else
        {
            gameObject.SetActive(false);
        }
        //scr_Grid.GridController.RemoveEntity(this);
    }

    public void TakeDamageOverTime (float duration, float damageRate, int damage)
    {
        while (duration >= 0)
        {
            _health.TakeDamage(damage, this);
            StartCoroutine(GenericClock(damageRate));
            duration -= damageRate;
            Debug.Log("shit");
        }
    }


    public IEnumerator Teleport (float waitTime, int damage, int playerX, int playerY, Entity enemy)
    {
        enemy._health.TakeDamage(damage, this);
        enemy.isStunned = true;
        //isImmobile = true;
        yield return new WaitForSeconds(waitTime);
        SetTransform(playerX, playerY);
        enemy.isStunned = false;
        //isImmobile = false;
    }

     public IEnumerator GenericClock (float waitTime)
     {
        yield return new WaitForSeconds(waitTime);
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
        stun.SetActive(true);
        yield return new WaitForSeconds(stunTime);
        stun.SetActive(false);
        isStunned = false;
    }

    IEnumerator HitClock(float hitTime)
    {
        spr.material.shader = hitShader;
        yield return new WaitForSeconds(hitTime);
        spr.material.shader = baseShader;
    }

    IEnumerator DamageOverTime (float rate, int damage)
    {
        yield return new WaitForSeconds(rate);
        _health.TakeDamage(damage, this);
    }
}
[System.Serializable]
public class Health{

    public int hp = 10; //NOTE: These would be better as private variables to make mistakes less likely and to enforce the max_hp - Colin
    public int shield = 0;
    public int max_hp;


    public void TakeDamage(int damage, Entity entity)
    {

        if (shield > 0)
        {
            DamageNumbersController.Instance.SpawnNumbers(damage, entity.transform.position);
            shield -= damage;
            if(shield < 0)
            {
                //Carry over extra damage to normal hp
                DamageNumbersController.Instance.SpawnNumbers(-shield, entity.transform.position);
                hp += shield;
                shield = 0;
            }
        }
        else
        {
            DamageNumbersController.Instance.SpawnNumbers(damage, entity.transform.position);
            hp -= damage;
        }
        if (hp <= 0)
        {
            hp = 0;
        }


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
