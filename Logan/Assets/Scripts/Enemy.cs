using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public List<Transform> target = new List<Transform>(); 
    public float activateDis;
    public Animator anim;
    public bool isAsleep = true;
    public bool startMove = false;
    public float health;
    public bool DoesRange;
    public bool isDead = false;
    public bool isgrounded;
    public bool playedDead = false;
    public bool canAttack;
    public AudioSource aud;
    public AudioClip[] attackClip;
    public AudioClip[] enemySounds;
    public AudioClip dmg;
    public string[] attackAnim;
    [SerializeField] private LayerMask lyr;
    private bool hasPlayedEnemySpottedAudio = false;
    private bool hasPlayedEnemyDeathAudio = false;
    private bool hasPlayedMainMusic = false;
    private bool hasAddedSecondPlayer = false;
    public bool isPickedUpEnemyAttack = false;
    public GameObject deathEffect;
    void Start()
    {
     
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            target.Add(player.transform);
        }

        if (target.Count > 0)
        {
            StartCoroutine(doattack());
        }
       
    }

    void Update()
    {
        isgrounded = isGrounded();
        anim.SetBool("isGrounded", isGrounded());

        Transform closestPlayer = findBothPlayers();

        if (closestPlayer != null && (closestPlayer.position - transform.position).magnitude < activateDis && !isDead && !hasPlayedEnemySpottedAudio && !hasPlayedMainMusic)
        {
            anim.SetTrigger("Wakeup");
            isAsleep = false;
            aud.clip = enemySounds[0];
            aud.Play();
            hasPlayedEnemySpottedAudio = true;
            GameManager.instance.aud.volume = 0.3f;
            hasPlayedMainMusic = true;
        }

        if (!isAsleep && !isDead)
        {
            if (closestPlayer != null)
            {
                Vector3 direction = (closestPlayer.position - transform.position).normalized;
                Debug.DrawLine(transform.position, closestPlayer.position, Color.red);

                if (startMove && hasPlayedMainMusic)
                {
                    GameManager.instance.aud.volume = 1.0f;

                    if (DoesRange)
                    {
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

                    if (isGrounded() && !isDead)
                    {
                        direction.y = 0;
                        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                    }
                }
            }
        }

        if (GameManager.instance.hasSpawnedSecondPlayer && !hasAddedSecondPlayer)
        {
            Transform secondPlayerTransform = GameManager.instance.secondPlayer.transform;
            target.Add(secondPlayerTransform);
            hasAddedSecondPlayer = true;
        }

        if (health <= 0 && !isDead && !hasPlayedEnemyDeathAudio)
        {
            int randomCoins = Random.Range(1, 3);
            isDead = true;
            anim.SetTrigger("Dead");
            print("DEAD");
            
            GameManager.instance.enemiesKilled += 1;
            GameManager.instance.enemiesKilledText.text = "Enemies Killed: " + GameManager.instance.enemiesKilled;
            GameManager.instance.Coins += randomCoins;
            GameManager.instance.AddRage();
            aud.clip = enemySounds[1];
            aud.Play();
        
            StartCoroutine(dead());
        }
    }
    IEnumerator dead()
    {
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        foreach (var col in this.gameObject.GetComponents<Collider>())
        {
            col.enabled = false;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject theDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void StartAttack()
    {
        startMove = true;
        anim.SetBool("isWalking", true);
    }

    IEnumerator doattack()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (startMove && canAttack && !GameManager.instance.players[0].GetComponent<PlayerController>().isDead && !isDead)
            {
                anim.Play(attackAnim[Random.Range(0, 2)]);
            }
        }
    }

    public void DisableAnim()
    {
        anim.enabled = false;
    }
    public void attack(float damage)
    {
        aud.clip = attackClip[Random.Range(0, attackClip.Length)];
        aud.Play();

        Transform targetPlayer = findBothPlayers();

        if (targetPlayer != null)
        {
            Vector3 playerToEnemy = (targetPlayer.position - transform.position);
            playerToEnemy.y = 0;
            Vector3 localF = transform.GetChild(0).forward;

            GameObject player = targetPlayer.gameObject;

            if (playerToEnemy.magnitude <= 3)
            {
                float angle = Vector3.Angle(localF, playerToEnemy);
                print(angle);

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
                                if (!player.GetComponent<PlayerController>().isDeadAnimation)
                                {
                                    startMove = false;

                                    player.GetComponent<Rigidbody>().AddForce(playerToEnemyNormalized * 2f, ForceMode.Impulse);
                                }
                            }
                                if (player.GetComponent<PlayerController>().isPickedUp == true)
                                {
                                    isPickedUpEnemyAttack = true;
}

                            }

                        }
                    }
                }
            }

        }

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
    }
    bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.1f);
        return Physics.Raycast(new Vector3(
                transform.position.x,
                transform.position.y + 0.1f,
                transform.position.z), Vector3.down, 0.2f, lyr);
    }
}