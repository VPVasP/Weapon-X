using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isgrounded;
    public Transform camTransform;
    public Transform playerModel;
    public AudioSource aud;
    [SerializeField] private LayerMask lyr;
    public bool isDead;
    public bool isDeadAnimation;
    public bool isPickedUp;
    public float health = 100;
    public float rage = 0;
    public float playerSpeed = 2.0f;
    public float jumpForce = 1.0f;
    public float _speed = 10;
    public float _rotationSpeed = 60;
    public float smallAttackDamage = 5;
    public float largeAttackDamage = 15;
    public float rageAttackDamage = 25;
    private Vector3 rotation;
    public Coroutine coroutine = null;
    public GameObject[] claws;
    [SerializeField] private Collider col;
    public AudioClip[] Jump;
    public AudioClip damageAudio;
    public AudioClip[] Attack;
    public AudioClip[] killedAudio;
    public GameObject kofx;
    public int attackSeq;
    public float dashForce;
    public string[] lAttack;
    public int maxAttack;
    public bool playingAnim;
    public Slider healthSlider;
    public Slider rageMeter;
    public AudioSource deadAudio, hurtSound,rageSound,whisperingRageSounds;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool isCurrentlyPickingUp = false;
    public bool isRaging = false;
    public string horizontalInputAxis;
    public string verticalInputAxis;
    public string jumpInputButton;
    public string attackInputButton;
    public string blockInputButton;
    public string dashInputButton;
    public string pickupButton;
    public string rageEffectButton;
    public string playerIdentifier;
    public GameObject[] rageClawEffects;
    public Light directionalLight;
    public Color directionalLightColor;
    private void Start()
    {
        directionalLight.color = directionalLightColor;
        Cursor.visible = false;
        healthSlider.value = health;
        rageMeter.value = rage;
        rageClawEffects[0].SetActive(false);
        rageClawEffects[1].SetActive(false);
        rageClawEffects[2].SetActive(false);
    }

    private void Update()
    {
        isgrounded = isGrounded();

        if (playingAnim)
        {
            foreach (GameObject theClaws in claws)
            {
                theClaws.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject i in claws)
            {
                i.SetActive(false);
            }
        }

        Vector3 forwardVector = (transform.position - new Vector3(camTransform.position.x, transform.position.y, camTransform.position.z)).normalized;
        Vector3 rightVector = Vector3.Cross(forwardVector, Vector3.up);
        float y = Input.GetAxis(verticalInputAxis);
        float x = Input.GetAxis(horizontalInputAxis);
        Vector3 between = (forwardVector * y) + (-x * rightVector);
        between = between.normalized;

        if (!playingAnim || anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "movement_idle")
        {
            playingAnim = false;
            transform.Translate(between * Time.deltaTime * playerSpeed);
        }

        if (between != Vector3.zero && !playingAnim)
        {
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(between), Time.deltaTime * 5f);
        }

        if ((Input.GetButtonDown(jumpInputButton) && isGrounded() && !playingAnim))
        {
            anim.SetTrigger("jump");
            aud.clip = Jump[Random.Range(0, Jump.Length)];
            aud.Play();
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Mathf.Abs(Input.GetAxis(verticalInputAxis)) > 0 || Mathf.Abs(Input.GetAxis(horizontalInputAxis)) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded());

        if (health <= 0)
        {
            isDead = true;
        }

        if (!isDeadAnimation && isDead)
        {
            isDeadAnimation = true;
            anim.SetTrigger("Dead");
            this.enabled = false;
        }

        if ( Input.GetButtonDown(attackInputButton))
        {
            isAttacking = true;
            if (!playingAnim && isGrounded())
            {
                playingAnim = true;

                Dictionary<float, Vector3> dictionary = new Dictionary<float, Vector3>();
                List<GameObject> enemy = new List<GameObject>();
                foreach (var enm in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (!enm.GetComponent<Enemy>().isDead)
                    {
                        enemy.Add(enm);
                    }
                }

                foreach (var enm in enemy)
                {
                    Vector3 v = (enm.transform.position - transform.position);
                    dictionary.Add(v.magnitude, v.normalized);
                }

                if (dictionary.Count > 0)
                {
                    Vector3 lookat = dictionary[dictionary.Keys.Min()];
                    if (lookat != Vector3.zero)
                    {
                        lookto(lookat);
                    }
                }

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
        }
        if ( Input.GetButton(blockInputButton))
        {
            isBlocking = true;
            anim.SetBool("isBlocking", isBlocking);
        }
        else if (isBlocking)
        {
            isBlocking = false;
            anim.SetBool("isBlocking", isBlocking);
        }
        if (Input.GetButton(pickupButton))
        {
            isPickedUp = true;
            anim.SetBool("isPickedUp", isPickedUp);
            Debug.Log("isPickedUp");
        }
        if(isPickedUp == true)
        {
            isCurrentlyPickingUp = true;
            anim.SetBool("isCurrentlyPickingUp", isCurrentlyPickingUp);
        }
       
      
        ActivateRageEffect();
    }
   


    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.1f);
        return Physics.Raycast(new Vector3(
            transform.position.x,
            transform.position.y + 0.1f,
            transform.position.z), Vector3.down, 0.2f, lyr);
    }

    public void lookto(Vector3 vector3)
    {
        vector3.y = 0;
        playerModel.rotation = Quaternion.LookRotation(vector3);
    }

    public void punch(int pushPower)
    {
        aud.clip = Attack[attackSeq];
        aud.Play();

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Vector3 vector3 = (enemy.transform.position - transform.position);
            vector3.y = 0;
            Vector3 localF = transform.GetChild(0).forward;

            if (vector3.magnitude <= 3)
            {
                float ang = Vector3.Angle(localF, vector3);
                if (ang < 90)
                {
                    if (!enemy.GetComponent<Enemy>().isDead)
                    {
                        if (pushPower > 0)
                        {
                            if (!enemy.GetComponent<Enemy>().isDead)
                            {
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
                                enemy.GetComponent<Enemy>().health -= smallAttackDamage;
                                enemy.GetComponent<Animator>().SetTrigger("DamageSmall");
                                Debug.Log(enemy.GetComponent<Enemy>().health);
                            }
                        }
                        var Rot = Quaternion.LookRotation(transform.position - enemy.transform.position);
                    }
                    else
                    {
                        if (!enemy.GetComponent<Enemy>().playedDead)
                        {
                            enemy.GetComponent<Enemy>().playedDead = true;
                            enemy.GetComponent<Enemy>().enabled = false;
                            enemy.GetComponent<Animator>().SetTrigger("Dead");
                            enemy.GetComponent<Rigidbody>().AddForce(vector3.normalized * 2f, ForceMode.Impulse);
                            aud.clip = killedAudio[Random.Range(0, killedAudio.Length)];
                            aud.Play();
                        }
                    }
                }
            }
        }
    }
 
    public void Dash(float force)
    {
        Vector3 localForce = transform.GetChild(0).forward * 5;
        localForce.y = 0;
        this.gameObject.GetComponent<Rigidbody>().AddForce(localForce * dashForce * force, ForceMode.Impulse);
    }

    public void nextSeq()
    {
        playingAnim = false;
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            healthSlider.value = health;
            hurtSound.Play();
            if (health <= 0)
            {
                isDead = true;
                deadAudio.Play();
                Invoke("RestartGame", 4f);
               
            }
        }
    }
    public void ActivateRageEffect()
    {
        if (Input.GetButton(rageEffectButton)&&rage>=100)
        {
            rageClawEffects[0].SetActive(true);
            rageClawEffects[1].SetActive(true);
            rageClawEffects[2].SetActive(true);
            isRaging =true;
            rageSound.Play();
            
        }
        if (isRaging == true)
        {
            directionalLight.color = Color.black;
            playerSpeed = 10; 
            smallAttackDamage = rageAttackDamage;
            largeAttackDamage = rageAttackDamage;
            rage -= 10 * Time.deltaTime;
            health += 2 * Time.deltaTime;
            rageMeter.value = rage;
            healthSlider.value = health;
          GameManager.instance.RageSlowMotion();
            GameManager.instance.aud.Pause();
          //  whisperingRageSounds.Play();
            Debug.Log(rage);
        }
        if (rage <= 0)
        {
            playerSpeed = 5;
            rageMeter.value = rage;
            isRaging = false;
            rageClawEffects[0].SetActive(false);
            rageClawEffects[1].SetActive(false);
            rageClawEffects[2].SetActive(false);
            smallAttackDamage = 5;
            largeAttackDamage = 15;
           GameManager.instance.RageEndMotion();
       //     whisperingRageSounds.Stop();
            directionalLight.color = directionalLightColor;
            GameManager.instance.aud.UnPause();
        }

    }
    public void AddRage()
    {
        rage += 20;
        rageMeter.value = rage;
    }
   
    void RestartGame()
    {
        GameManager.instance.Restart();
    }
}