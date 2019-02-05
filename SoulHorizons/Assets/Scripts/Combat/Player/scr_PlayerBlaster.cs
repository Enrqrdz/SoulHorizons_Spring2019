//Colin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
[RequireComponent(typeof(AudioSource))]

/// <summary>
/// Manages the player blaster functionality. Receives user input and passes information to the projectile when firing.
/// </summary>
public class scr_PlayerBlaster : MonoBehaviour {

	public float baseDamage = 5f;
	public float baseSpeed = 10f; //the speed at which the projectile moves
	public float chargeTime1 = 1.2f; //time in seconds for the blast to reach charge level 1
	public float damageIncrease1 = 8f; //the amount that damage dealt increases when charge level 1 is increased
	public float damageIncreaseRate = 3f; //how much damage increases per second. This increase is independent of the charge level damage increase. Set this to 0 to have only the charge level increase.
    public float damageMultiplier = 0f; //How much to multiply the damage, for cards like inner strength

	private float timePressed = 0f; //the time the fire button has been held
	private bool pressed;
	public float fireRate = 0.2f; //how often the player can fire the blaster
	public float chargeCooldown = 0.4f; //we can use this instead of fire rate after a charged shot if we want a longer cooldown for charged shots
	private bool readyToFire = true; //used to indicate if the blaster is ready to fire again
	public AttackData attack; //the attack that will be launched
	private scr_Entity playerEntity;

	public SpriteRenderer baseProjectile;
	public SpriteRenderer projectile1;
    public SpriteRenderer playerSprite;

    AudioSource Blaster_SFX;
    AudioSource BlasterCharge_SFX;
    public AudioClip[] blasters_SFX;
    private AudioClip blaster_SFX;
    public AudioClip charging_SFX;
    public AudioClip chargingLoop_SFX;


    void Awake()
	{
		//objectPool_scr = GetComponent<scr_ObjectPool>();
		playerEntity = GetComponent<scr_Entity>();
	}
	
	void Start () {
		pressed = false;
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Blaster_SFX = SFX_Sources[4];
        BlasterCharge_SFX = SFX_Sources[5];
    }

    private bool blastLastFrame = false; //this is used to get detect "buttonUp" since we can only determine if the button is down or not
	void Update () {
		bool blastUp = blastLastFrame && !scr_InputManager.Blast(); //blastUp is true if the button was down(true) last frame and is not pressed this frame
		blastLastFrame = scr_InputManager.Blast(); //update the blast last frame

		if (pressed)
		{
			timePressed += Time.deltaTime;
			//TODO:need to calculate charge level here for visual indicators that you have increased the charge level
		}

		if (scr_InputManager.Blast() && readyToFire)
		{
            if (BlasterCharge_SFX.isPlaying != true && timePressed < chargeTime1)
            {
                BlasterCharge_SFX.clip = charging_SFX;
                BlasterCharge_SFX.Play();
            }
            pressed = true;
        }

        if (scr_InputManager.Blast() && readyToFire)
        {
            if (timePressed > chargeTime1)
            {
                playerSprite.color = new Color(0.2f, 0.6f, .86f);
                if (BlasterCharge_SFX.isPlaying != true)
                {
                    BlasterCharge_SFX.clip = chargingLoop_SFX;
                    BlasterCharge_SFX.Play();
                }
            }
        }

		if(blastUp && readyToFire)
		{
            int index = Random.Range(0, blasters_SFX.Length);
            BlasterCharge_SFX.Stop();
            blaster_SFX = blasters_SFX[index];
            Blaster_SFX.clip = blaster_SFX;
            Blaster_SFX.Play();
            //scr_PlayerProjectile proj = objectPool_scr.CreateObject(transform.position, transform.rotation).GetComponent<scr_PlayerProjectile>();
            float damage = baseDamage + damageIncreaseRate * timePressed;
			//check if charged
			if (timePressed < chargeTime1)
			{
				//fire a normal shot

				//set the damage for the attack
				attack.damage = (int) Mathf.Round(damage*damageMultiplier);
				//set the projectile sprite
				attack.particles = baseProjectile;
				scr_AttackController.attackController.AddNewAttack(attack, playerEntity._gridPos.x, playerEntity._gridPos.y, playerEntity);
				StartCoroutine(AttackCooldown(fireRate));
			}
			else
			{
                //fire a shot at charge level 1
                CameraShaker.Instance.ShakeOnce(4f, 4f, 0.2f, 0.2f);
				damage += damageIncrease1;
				attack.damage = (int) Mathf.Round(damage*damageMultiplier);
				//set the projectile sprite
				attack.particles = projectile1;
				//proj.Fire(damage, 1, baseSpeed);
				scr_AttackController.attackController.AddNewAttack(attack, playerEntity._gridPos.x, playerEntity._gridPos.y, playerEntity);
				StartCoroutine(AttackCooldown(chargeCooldown));
                
			}

			//reset variables
			timePressed = 0f;
			pressed = false;
            playerSprite.color = Color.white;
		}
	}//end Update

	/// <summary>
	/// Called after an attack to disable the blaster for the cooldown time
	/// </summary>
	/// <returns></returns>
	private IEnumerator AttackCooldown(float cooldown)
	{
		readyToFire = false;
		yield return new WaitForSeconds(cooldown);
		readyToFire = true;
	}

    private IEnumerator MultiplierReset(float resetTime)
    {
        yield return new WaitForSecondsRealtime(resetTime);
    }

    //Set the damage multiplier to a certain number for an amount of time
    public void setMultiplier(float num, float time)
    {
        damageMultiplier = num;
        StartCoroutine(MultiplierReset(time));
    }
}
