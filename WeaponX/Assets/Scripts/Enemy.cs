using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed; //our enemy speed
    public List<Transform> target = new List<Transform>();  //lisft of the target players transform
    public float activateDis; //our activate distance to activate the enmy
    public Animator anim; //ou aniamtor refrence
    public bool isAsleep = true;
    public bool startMove = false;
    public float health;
    public bool DoesRange;
    public bool isDead = false;
    public bool isgrounded;
    public bool playedDead = false;
    public bool canAttack;
    public AudioSource aud;
    public AudioClip[] attackClip; // array of attack audio clips
    public AudioClip[] enemySounds;// array of enemySounds audio clips
    public string[] attackAnim; // array of strings for the attack animation
    [SerializeField] private LayerMask lyr; //layer mask to spot the ground
    private bool hasPlayedEnemySpottedAudio = false;
    private bool hasPlayedEnemyDeathAudio = false;
    private bool hasPlayedMainMusic = false;
    private bool hasAddedSecondPlayer = false;
    public GameObject deathEffect; //the death effect
    void Start()
    {
     
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //we find all the targets with the tag player
        foreach (var player in players)
        {
            target.Add(player.transform);
        }

        if (target.Count > 0) //we attack if the target players exist
        {
            StartCoroutine(doattack());
        }
       
    }

    void Update()
    {
        isgrounded = isGrounded(); //we check if the player is grounded and update his animation
        anim.SetBool("isGrounded", isGrounded());

        Transform closestPlayer = findBothPlayers(); //we find the closest player 
        //the check conditions to activat ehe enemy
        if (closestPlayer != null && (closestPlayer.position - transform.position).magnitude < activateDis && !isDead && !hasPlayedEnemySpottedAudio && !hasPlayedMainMusic)
        {
            //we activate the enemy and play the wakeup animation and audio
            anim.SetTrigger("Wakeup");
            isAsleep = false;
            aud.clip = enemySounds[0];
            aud.Play();
            hasPlayedEnemySpottedAudio = true;
            GameManager.instance.aud.volume = 0.3f;
            hasPlayedMainMusic = true;
        }

        if (!isAsleep && !isDead) //if the enemy is not asleep or dead perfom these 
        {
            if (closestPlayer != null) //if the closest player is valid
            {
                Vector3 direction = (closestPlayer.position - transform.position).normalized;
                Debug.DrawLine(transform.position, closestPlayer.position, Color.red);

                if (startMove && hasPlayedMainMusic) //if the enemy can move and has played the mainmusic
                {
                    GameManager.instance.aud.volume = 1.0f; //we adjust the game manager volume

                    if (DoesRange) //check if the enemy does a range attack
                    {
                        //move towards the closest player if the distance is bigger than 2 and update the animations
                        if ((closestPlayer.position - transform.position).magnitude > 2 && isGrounded() && !isDead)
                        {
                            transform.Translate(direction * speed * Time.deltaTime);
                            anim.SetBool("isWalking", true);
                        }
                        else
                        {
                            anim.SetBool("isWalking", false);
                        }
                    }
                    else
                    {
                        //the ability for the enemy to attack based on the distance 
                        if ((closestPlayer.position - transform.position).magnitude > 2 && isGrounded() && !isDead)
                        {
                            transform.Translate(direction * speed * Time.deltaTime);
                            anim.SetBool("isWalking", true);
                        }
                        else
                        {
                            anim.SetBool("isWalking", false);
                        }
                    }

                    if ((closestPlayer.position - transform.position).magnitude > 2)
                    {
                        canAttack = false;
                    }
                    else
                    {
                        canAttack = true;
                    }

                    if (isGrounded() && !isDead) //if the enemy is grounded and is not enemy we update his rotation
                    {
                        direction.y = 0;
                        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                    }
                }
            }
        }

        if (GameManager.instance.hasSpawnedSecondPlayer && !hasAddedSecondPlayer) //if the second enemy has spawned we add him to the list 
        {
            Transform secondPlayerTransform = GameManager.instance.secondPlayer.transform;
            target.Add(secondPlayerTransform);
            hasAddedSecondPlayer = true;
        }
        //if the enemy is dead and hasn't played the enemy audio death
        if (health <= 0 && !isDead && !hasPlayedEnemyDeathAudio)
        {
          
            isDead = true;
            anim.SetTrigger("Dead"); //we update the animation trigger to the death animation
            print("DEAD");
            
            GameManager.instance.enemiesKilled += 1;
            GameManager.instance.enemiesKilledText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;
            //we find both players and the player who killed the enemy we add rage 
            Transform targetPlayer = findBothPlayers();
            GameObject player = targetPlayer.gameObject;
            targetPlayer.GetComponent<PlayerController>().AddRage();
            //play the death audio and start the dead coroutine
            aud.clip = enemySounds[1];
            aud.Play();
        
            StartCoroutine(dead());
        }
    }
    //coroutine for the death of the enemy
    IEnumerator dead()
    {
       
        yield return new WaitForSeconds(5f);
        //we disable the gravity and colliders for the enemy
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        foreach (var col in this.gameObject.GetComponents<Collider>())
        {
            col.enabled = false;
        }
        //we hide the children of the model slowly to make the death animation
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        //we instatiate the death effect of the enemy
        GameObject theDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    //void to the start the enemy attack
    public void StartAttack()
    {
        startMove = true;
        anim.SetBool("isWalking", true);
    }
    //coroutine to do the enemy attacks
    IEnumerator doattack()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            //play a random attack animation 
            if (startMove && canAttack && !GameManager.instance.players[0].GetComponent<PlayerController>().isDead && !isDead)
            {
                anim.Play(attackAnim[Random.Range(0, 2)]);
            }
        }
    }
    //disable the animator 
    public void DisableAnim()
    {
        anim.enabled = false;
    }
    //method that handles the enemy attacks
    public void attack(float damage)
    {
        //we play the attack audio
        aud.clip = attackClip[Random.Range(0, attackClip.Length)];
        aud.Play();
        //we find the target player
        Transform targetPlayer = findBothPlayers();

        if (targetPlayer != null) //if the targetplayer is valid
        {
            Vector3 playerToEnemy = (targetPlayer.position - transform.position);
            playerToEnemy.y = 0;
            Vector3 localF = transform.GetChild(0).forward;

            GameObject player = targetPlayer.gameObject;
            //if the player is within the enemy rattack range
            if (playerToEnemy.magnitude <= 3)
            {
                float angle = Vector3.Angle(localF, playerToEnemy);
                print(angle);
                //we check the angle to do a valid attack
                if (angle < 90)
                {
                    aud.clip = attackClip[Random.Range(0, attackClip.Length)];
                    aud.Play();

                    Vector3 playerToEnemyNormalized = playerToEnemy.normalized;
                    playerToEnemyNormalized.y = 0;

                    Vector3 localForce = transform.GetChild(0).forward;

                    if (playerToEnemy.magnitude <= 3)
                    {
                        float angleToPlayer = Vector3.Angle(localForce, playerToEnemyNormalized);
                        print(angleToPlayer);

                        if (angleToPlayer < 90)
                        {
                            // apply damage if the player is not dead or blocking
                            if (!player.GetComponent<PlayerController>().isDead && !player.GetComponent<PlayerController>().isBlocking)
                            {
                                int randomDamage = Random.Range(15, 28);
                                player.GetComponent<PlayerController>().TakeDamage(randomDamage);

                                player.GetComponent<Animator>().SetTrigger("DamageSmall");

                                player.GetComponent<PlayerController>().playingAnim = false;
                                player.GetComponent<AudioSource>().clip = player.GetComponent<PlayerController>().damageAudio;
                                player.GetComponent<AudioSource>().Play();
                                //   player.GetComponent<PlayerController>().isHurt = true;
                                var rotation = Quaternion.LookRotation(transform.position - player.transform.position);
                            }
                            //apply the damage if the player is blocking
                            else if (!player.GetComponent<PlayerController>().isDead && player.GetComponent<PlayerController>().isBlocking)
                            {
                                Vector3 backwardForce = -transform.forward * 2.0f;
                                player.GetComponent<Rigidbody>().AddForce(backwardForce, ForceMode.Impulse);
                                int blockDamage = Random.Range(1, 4);
                                player.GetComponent<PlayerController>().TakeDamage(blockDamage);
                                player.GetComponent<PlayerController>().isHurt = false;
                            }
                            else
                            {
                                //we appply force to the dead player
                                if (!player.GetComponent<PlayerController>().isDeadAnimation)
                                {
                                    startMove = false;

                                    player.GetComponent<Rigidbody>().AddForce(playerToEnemyNormalized * 2f, ForceMode.Impulse);
                                }
                            }

                        }

                        }
                    }
                }
            }

        }
    //method to find the closest player of all the targets
    Transform findBothPlayers()
    {
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (Transform playerTransform in target)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = playerTransform;
            }
        }

        return closestPlayer;
    }//method to check if the enemy is grounded
    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.1f);
        return Physics.Raycast(new Vector3(
                transform.position.x,
                transform.position.y + 0.1f,
                transform.position.z), Vector3.down, 0.2f, lyr);
    }
}