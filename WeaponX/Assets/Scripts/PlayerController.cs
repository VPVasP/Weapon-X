using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.XR;
//using Mono.Cecil;

public class PlayerController : MonoBehaviour
{
    public Animator anim; //refrence to our animator
    //bools to check if the player is  moving or is grounded
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isgrounded;
    public Transform camTransform; //our camera transform
    public Transform playerModel; //the player model transform
    public AudioSource aud; 
    [SerializeField] private LayerMask lyr; //the layermask refrence
    //bools to check if he is dead if he is playind dead animation and if he has picked up something
    public bool isDead;
    public bool isDeadAnimation;
    public bool isPickedUp;
    public float health = 100; //our health value
    public float rage = 0; //ou rage value
    public float playerSpeed = 2.0f; //our speed value
    public float jumpForce = 1.0f; //our jump force value
    public float smallAttackDamage = 5; //small attack damage value
    public float largeAttackDamage = 15;//large attack damage value
    public float rageAttackDamage = 25;//rage attack damage value
    private Vector3 rotation;  
    public Coroutine coroutine = null;
    [SerializeField] private Collider col; //refrence to the collider
    //audio clips for the player
    public AudioClip[] Jump;
    public AudioClip damageAudio;
    public AudioClip[] Attack;
    public AudioClip[] killedAudio;
    public int attackSeq;
    public float dashForce;
    public string[] lAttack;
    public int maxAttack; //max attack value
    public bool playingAnim; //bool to check if it playing animation
    public Slider healthSlider; //our healthsldier
    public Slider rageMeter;
    //bools for certain conditions
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isCurrentlyPickingUp = false;
    public bool isRaging = false;
    public bool isThrowing = false;
    public bool isRunningWithPickUp =false;
    public bool isHurt = false;
    public bool isJumping = false;
    public bool isEndCheering = false;
    //variables related to input and controls
    public string horizontalInputAxis;
    public string verticalInputAxis;
    public string jumpInputButton;
    public string attackInputButton;
    public string blockInputButton;
    public string pickupButton;
    public string rageEffectButton;
    public string playerIdentifier;
    //gameobject arrays for normal claw effects and rage claw effects
    public GameObject[] rageClawEffects;
    public GameObject[] normalClawEffects;
    public Light directionalLight; //directional light refrence
    public Color directionalLightColor; //directional light color
    public Color normalColor; //normal color of directional light
    //audio clips for dead,hurt,rage and block
    public AudioClip deadAudio;
    public AudioClip hurtSound;
    public AudioClip rageSound;
    public AudioClip blockSound;
    private void Start()
    {
        
        directionalLight.color = normalColor;
        Cursor.visible = false;
        healthSlider.value = health;
        rageMeter.value = rage;
        //the normal claws are set to false and the rage ones
        normalClawEffects[0].SetActive(false);
        normalClawEffects[1].SetActive(false);
        rageClawEffects[0].SetActive(false);
        rageClawEffects[1].SetActive(false);
        rageClawEffects[2].SetActive(false);
    }

    private void Update()
    {
        //check if the player is grounded
        isgrounded = isGrounded();


        //calculate movement vectors based on player input and camera 
        Vector3 forwardVector = (transform.position - new Vector3(camTransform.position.x, transform.position.y, camTransform.position.z)).normalized;
        Vector3 rightVector = Vector3.Cross(forwardVector, Vector3.up);
        float vertical = Input.GetAxis(verticalInputAxis);
        float horizontal = Input.GetAxis(horizontalInputAxis);
        Vector3 between = (forwardVector * vertical) + (-horizontal * rightVector);
        between = between.normalized;
        //move the player based on input and animation state

        if (!playingAnim || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "movement_idle")
        {
            playingAnim = false;
            transform.Translate(between * Time.deltaTime * playerSpeed);
        }
        //rotate the player model to face the movement direction
        if (between != Vector3.zero && !playingAnim)
        {
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(between), Time.deltaTime * 5f);
        }
        //handle player jumping
        if ((Input.GetButtonDown(jumpInputButton) && isGrounded() && !playingAnim))
        {
            anim.SetTrigger("jump");
            aud.clip = Jump[Random.Range(0, Jump.Length)];
            aud.Play();
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
        //check if the player is moving
        if (Mathf.Abs(Input.GetAxis(verticalInputAxis)) > 0 || Mathf.Abs(Input.GetAxis(horizontalInputAxis)) > 0)
        {
            isMoving = true;

            Debug.Log("is  moving");
            isBlocking =false;
        }
        else
        {
            isMoving = false;
            Debug.Log("is not moving");
        }
        //update the  animation parameters based on the  player state
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded());
        //check if playyer is dead
        if (health <= 0)
        {
            isDead = true;
        }
        //play death animation and disable script when dead
        if (!isDeadAnimation && isDead)
        {
            isDeadAnimation = true;
            anim.SetTrigger("Dead");
            this.enabled = false;
        }
        //activate rage effect function
            ActivateRageEffect();
        //handle player blocking 
        if (Input.GetButtonDown(blockInputButton)&&isgrounded==true)
        {
            Debug.Log("Pressing Button");
            anim.SetBool("isBlocking", true);
            isBlocking = true;
        }
        else if(Input.GetButtonUp(blockInputButton)&&isgrounded==true)
        {
            Debug.Log("Pressing Button");
            anim.SetBool("isBlocking", false);
            isBlocking = false;
        }
        if (isMoving == true)
        {
            anim.SetBool("isBlocking", false);
        }
        if(isJumping == true)
        {
            anim.SetBool("isBlocking", false);
        }
        if (isCurrentlyPickingUp == true)
        {
            //if is currently being pickedup then we set the animator parameters
            anim.SetTrigger("PickingUpTrigger");
            isPickedUp = true;
            Debug.Log("Played Anim");
        }
       //if is pickedup is true then we set the animator parameters
        if(isPickedUp == true)
        {
            float moveInput = Mathf.Abs(Input.GetAxis(verticalInputAxis)) + Mathf.Abs(Input.GetAxis(horizontalInputAxis));
            anim.SetBool("isRunningWithPickUp", moveInput > 0);
            anim.SetBool("isPickedUp", moveInput <= 0);
            anim.SetBool("isPickedUp", true);
            isCurrentlyPickingUp = false;
            isAttacking = false;
            
        }
 // if is throwing is true handle the animation parameters
        if(isThrowing == true)
        {
            anim.SetBool("isRunningWithPickUp",false);
            anim.SetTrigger("Throwing");
            isPickedUp = false;
        }
       
     
        //handle player attacking
        if (Input.GetButtonDown(attackInputButton))
        {
            
            isAttacking = true;
            //we activate the normalclaweffects
            normalClawEffects[0].SetActive(true);
            normalClawEffects[1].SetActive(true);
            //check if the player is not currently playing  animation and is grounded
            if (!playingAnim && isGrounded())
            {
                playingAnim = true;
                //create a dictionary to put  enemy distances and directions
                Dictionary<float, Vector3> dictionary = new Dictionary<float, Vector3>();
                List<GameObject> enemy = new List<GameObject>();
                //we add the enemies that are not dead in the dictionary
                foreach (var enm in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (!enm.GetComponent<Enemy>().isDead)
                    {
                        enemy.Add(enm);
                    }
                }
                // we calculate distances and directions to enemies
                foreach (var enm in enemy)
                {
                    Vector3 v = (enm.transform.position - transform.position);
                    dictionary.Add(v.magnitude, v.normalized);
                }
                //iff there are enemies in the dictionary we  make the player look at the closest one
                if (dictionary.Count > 0)
                {
                    Vector3 lookat = dictionary[dictionary.Keys.Min()];
                    if (lookat != Vector3.zero)
                    {
                        lookto(lookat);
                    }
                }
                //we play the attack animation and update the attack sequence
                anim.Play(lAttack[attackSeq]);
                if (attackSeq == maxAttack)
                {
                    attackSeq = 0;
                }
                else
                {
                    attackSeq += 1;
                }
              
            }
           

            
        } //check if the player is grounded using a raycast
            bool isGrounded()
            {
                Debug.DrawRay(transform.position, Vector3.down * 0.1f);
                return Physics.Raycast(new Vector3(
                    transform.position.x,
                    transform.position.y + 0.1f,
                    transform.position.z), Vector3.down, 0.2f, lyr);
            }
        }
    //method to make the player model look at a specific direction
    public void lookto(Vector3 vector3)
    {
        vector3.y = 0;
        playerModel.rotation = Quaternion.LookRotation(vector3);
    }
    //method to handle player punch action
    public void punch(int pushPower)
    {
        //we set the audio clip to play when we punbch
        aud.clip = Attack[attackSeq];
        aud.Play();
        isHurt = false;
        //we create a foreach to go through all the objects with the tag enemy
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Vector3 vector3 = (enemy.transform.position - transform.position);
            vector3.y = 0;
            Vector3 localF = transform.GetChild(0).forward;
            //if the enemy is withing the punching range
            if (vector3.magnitude <= 3)
            {
                float ang = Vector3.Angle(localF, vector3);
                if (ang < 90)
                {
                    if (!enemy.GetComponent<Enemy>().isDead)
                    {
                        if (pushPower > 0)
                        {
                            //check if the enemy is not dead
                            if (!enemy.GetComponent<Enemy>().isDead)
                            {
                               //apply damage and forces
                                enemy.GetComponent<Enemy>().health -= largeAttackDamage;
                                enemy.GetComponent<Rigidbody>().AddForce(vector3.normalized * pushPower, ForceMode.Impulse);
                                enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * 8f, ForceMode.Impulse);
                                enemy.GetComponent<Enemy>().startMove = false;
                                enemy.GetComponent<Animator>().SetTrigger("DamageBig");
                             
                         
                                Debug.Log(enemy.GetComponent<Enemy>().health);
                            }
                        }
                        else
                        {
                            if (!enemy.GetComponent<Enemy>().isDead)
                            {
                                //apply damage for small attack
                                enemy.GetComponent<Enemy>().health -= smallAttackDamage;
                                enemy.GetComponent<Animator>().SetTrigger("DamageSmall");
                                Debug.Log(enemy.GetComponent<Enemy>().health);

                               
                            }
                        }
                        //we make the player look at the enemy 
                        var Rot = Quaternion.LookRotation(transform.position - enemy.transform.position);
                    }
                    else
                    {
                      //if the enemy is dead 
                        if (!enemy.GetComponent<Enemy>().playedDead)
                        {
                            enemy.GetComponent<Enemy>().playedDead = true;
                            enemy.GetComponent<Enemy>().enabled = false;
                            enemy.GetComponent<Animator>().SetTrigger("Dead");
                            enemy.GetComponent<Rigidbody>().AddForce(vector3.normalized * 2f, ForceMode.Impulse);
                            //play a random death audio
                            aud.clip = killedAudio[Random.Range(0, killedAudio.Length)];
                            aud.Play();
                        }
                    }
                }
            }
        }
    }
    //method called after completing an attack sequence
    public void nextSeq()
    {
        playingAnim = false;
        isAttacking = false;
    }
    //method to handle player taking damage
    public void TakeDamage(int damage)
    {
       //check if the player is not dead
        if (!isDead && damage > 0)
        {
            //we decrease our health,update the slider value and play the sound effect
            health -= damage;
           
            healthSlider.value = health;
            aud.clip = hurtSound;
            aud.Play();
            //if we are blocking we play the block sound
            if (isBlocking == true)
            {
               
                aud.clip = blockSound;
                aud.Play();
            }
            //if we are in phase 1 it respawns to the first respawn point and reset our health to max and play dead audio
            if (health <= 0 && GameManager.instance.isInPhase1 == true)
            {
                this.transform.localPosition = GameManager.instance.reSpawnPoints[0].position;
                aud.clip = deadAudio;
                aud.Play();
                health = 100;
                healthSlider.value = health;

            }
            //if we are in phase 2 it respawns to the first respawn point and reset our health to max and play dead audio
            if (health <= 0 && GameManager.instance.isInPhase2 == true)
            {
                this.transform.localPosition = GameManager.instance.reSpawnPoints[1].position;
                aud.clip = deadAudio;
                aud.Play();
                health = 100;
                healthSlider.value = health;

               

            }

            //if we are in phase 3 it respawns to the first respawn point and reset our health to max and play dead audio
            if (health <= 0 && GameManager.instance.isInPhase3 == true)
            {
                this.transform.localPosition = GameManager.instance.reSpawnPoints[2].position;
                aud.clip = deadAudio;
                aud.Play();
                health = 100;
                healthSlider.value = health;

            }

         
        }
       
    }
    
    //method to activate rage effect
    public void ActivateRageEffect()
    {
        if (Input.GetButton(rageEffectButton) && rage == 100)
        {
            //we set the normal claws to false and activate the rage oens
            rageClawEffects[0].SetActive(true);
            rageClawEffects[1].SetActive(true);
            rageClawEffects[2].SetActive(true);
            normalClawEffects[0].SetActive(false);
            normalClawEffects[1].SetActive(false);

            isRaging = true;
            //we pause the main music and play the rage sound 
            aud.clip = rageSound;
            aud.Play();
            GameManager.instance.aud.Pause();
        }

        if (isRaging == true)
        {
            //if we rage we update the rage damage change the light color and upgrade  speed etc
            smallAttackDamage = rageAttackDamage;
            largeAttackDamage = rageAttackDamage;
            directionalLight.color = directionalLightColor;
            playerSpeed = 10;
            rage -= 10 * Time.deltaTime;
            health += 1 * Time.deltaTime;
            healthSlider.value = health;
            rageMeter.value = rage;
            rage = Mathf.Clamp(rage, 0f, 100f);
            health = Mathf.Clamp(health, 0f, 100f);
            Debug.Log(rage);
            normalClawEffects[0].SetActive(false);
            normalClawEffects[1].SetActive(false);
        }
        //if we don't rage we update the rage damage back to the normal damage change the light color put back the normal values 
        if (rage == 0)
        {
            smallAttackDamage = 5;
            largeAttackDamage = 15;
            playerSpeed = 5;
            rageClawEffects[0].SetActive(false);
            rageClawEffects[1].SetActive(false);
            rageClawEffects[2].SetActive(false);
            rageMeter.value = rage;
            directionalLight.color = normalColor;
            isRaging = false;
            GameManager.instance.aud.UnPause();
        }
    }
    //method to add rage
    public void AddRage()
    {
        if (isRaging==false && rage < 100)
        {
            rage += 20;
            rageMeter.value = rage;
        }
        
        
    }
   
   
}