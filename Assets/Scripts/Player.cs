using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SerializeField] private AudioClip MusicClip;
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private GameObject healthBar;
    int layerMask = 1 << 8;
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;
    public int kills;
    public int deaths;
    Color originalColor;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDistance = 150f;
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private TextMesh healthText;
    //[SerializeField] private TextMesh TestText;
    [SerializeField] private GameObject enemy;
    //[SerializeField] private TextMesh PlayerNameText;
    //[SerializeField] private TextMesh KillsText;
    //[SerializeField] private TextMesh DeathsText;
    GameObject auxEnemy;
    bool isBullet = false;
    bool shooting = false;
    float speedCap = 7f;
    private Animator anim;
    private GameObject scoreboard;
    public float spawnTime = 10f;
    [Space(20)]
    [Header("SpawnEnemy Points:")]
    [SerializeField] private GameObject InstructionsText;
    [SerializeField] private GameObject WelcomeText;
    public Transform[] spawnPoints;
    private int playerCount;
    private Transform targetedEnemy;
    private bool enemyClicked;
    private UnityEngine.AI.NavMeshAgent navAgent;
    private float nextFire;
    private float timeBetweenShots = 1f;
    private Vector3 startingPosition;
    public static int score = 0;
    public static int iterations;
    bool blinker = true;
    [SyncVar(hook = "OnHealthChanged")] private int playerhealth = 100;
    private int lives = 3;
    private int bulletDamage = 50;

    public override void OnStartLocalPlayer()
    {
        //if (GameObject.Find("SF_Character_Elf")!=null)
        //{
        //    print(GameObject.Find("SF_Character_Elf").GetComponentsInChildren<Transform>());
        //}
        //navAgent.speed = 5f;
        //originalColor = gameObject.GetComponent<Material>().color;
        Camera.main.GetComponent<CameraMov>().setTarget(gameObject.transform);
        //Camera.main.GetComponent<CameraMov>().detectKeyboard();

        currentPlayer.SetActive(true);
        //gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        //print(anim.name);
        //animation
        anim.SetFloat("Speed_f", 0.0f);
        anim.SetInteger("Animation_int",0);
        anim.SetInteger("WeaponType_int",0);
        anim.SetBool("Grounded",true);
        anim.SetBool("IsWalking", false);
        //GetComponent<MeshRenderer>().material.color = Color.blue;
         tag = "Player";
    }

    public int GetScore()
    {
        return score;
    }

    float totBar;
    void Start()
    {
        //WelcomeText.SetActive(true);
        // InstructionsText.SetActive(false);
        MusicSource.clip = MusicClip;
        totBar = healthBar.transform.localScale.x;
        layerMask = ~layerMask;
        WelcomeText = GameObject.Find("CanvasInstructions").transform.Find("WelcomeText").gameObject;
        InstructionsText = GameObject.Find("CanvasInstructions").transform.Find("InstructionsText").gameObject;

        StartCoroutine(LateCall());

        iterations = 0;
        InvokeRepeating("CmdSpawn", spawnTime, spawnTime);
        scoreboard = GameObject.Find("CanvasScoreboard").transform.Find("PanelScoreboard").gameObject;
       // InstructionsText = GameObject.Find("CanvasInstructions").transform.Find("InstructionsText").gameObject;
        //PlayerNameText = GameObject.FindGameObjectWithTag("TextPlayerName").GetComponent<TextMesh>();
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(bulletSpawnPoint);
        Assert.IsNotNull(currentPlayer);
        Assert.IsNotNull(healthText);

        anim = GetComponent<Animator>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        startingPosition = transform.position;
        //scoreboard.SetActive(false);
    }

    IEnumerator IncreaseSpeedPerSecond(float waitTime)
    {
        //while agent's speed is less than the speedCap
        while (navAgent.speed < speedCap)
        {
            //wait "waitTime"
            yield return new WaitForSeconds(waitTime);
            //add 0.5f to currentSpeed every loop 
            navAgent.speed = navAgent.speed + 0.5f;
        }
    }

    void Win()
    {
        if (Score.GetKills() == 5)
        {
            SceneManager.LoadScene("Win");
        }
    }

    IEnumerator WaitALittle()
    {
        yield return new WaitForSeconds(2f);
        //gameObject.GetComponent<Material>().color = originalColor;
    }

    [Command]
    void CmdSpawn()
    {
        if (iterations < 20)
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating("CmdSpawn", spawnTime, spawnTime);
        }
        else
        {
            CancelInvoke("CmdSpawn");
        }
        // If the player has no health left...
        //if (playerHealth.currentHealth <= 0f)
        //{
        //    // ... exit the function.
        //    return;
        //}

        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        //print("i: "+spawnPointIndex);
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        iterations++;

    }
    // Update is called once per frame
    void Update()
    {
        //print("shooting:"+shooting);
        if (!shooting)
        {
            if (!pathComplete())
            {
                anim.SetFloat("Speed_f", 0.4f);
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsAtacking", false);
                anim.SetInteger("WeaponType_int", 0);
            }
            else
            {
                anim.SetFloat("Speed_f", 0f);
                anim.SetInteger("WeaponType_int", 0);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsAtacking", false);
            }
        }
        else if(shooting)
        {
            anim.SetInteger("WeaponType_int", 10);
            anim.SetBool("IsAtacking", true);
            anim.SetBool("Reload_b", true);
        }

        navAgent.speed = 3;
        //anim.SetTrigger("Idle_SittingOnGround");
       // PlayerNameText.text = "Shushugua";
        if (!isLocalPlayer)
        {
            return;
        }
        Win();
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }


        //Scoreboard
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    //print("Tab");
        //    //PlayerNameText.text = PlayerName;
        //    PlayerNameText.text = "ErickGtz";
        //    KillsText.text = "Kills: " + score;
        //    DeathsText.text = "Deaths: 0";
        //    TestText.text = "YEAH";
        //    scoreboard.SetActive(true);
        //}
        //else if (Input.GetKeyUp(KeyCode.Tab))
        //{
        //    //print("UNTab");
        //    scoreboard.SetActive(false);
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.CompareTag("Enemy"))
                { 
                    auxEnemy = hit.collider.gameObject;
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                }
                else
                {
                    enemyClicked = false;
                    navAgent.SetDestination(hit.point);
                    navAgent.Resume();
                }
            }

        }

        if (enemyClicked)
        {
            MoveAndShoot();
        }
    }

    IEnumerator LateCall()
    {
        WelcomeText.GetComponent<Text>().text = "Welcome to my game, "+Score._userName;
        WelcomeText.SetActive(true);
        yield return new WaitForSeconds(5f);
        WelcomeText.SetActive(false);
        yield return new WaitForSeconds(.5f);
        InstructionsText.SetActive(true);
        yield return new WaitForSeconds(5f);
        InstructionsText.SetActive(false);
    }

    IEnumerator LateCall2()
    {
        //InstructionsText.SetActive(true);
        //WelcomeText.SetActive(true);
        yield return new WaitForSeconds(5f);
        WelcomeText.SetActive(false);
    }

    void MoveAndShoot()
    {
        if (targetedEnemy == null)
        {
            shooting = false;
            return;
        }
        else
        {
            shooting = true;
            if (navAgent.remainingDistance <= shootDistance)
            {
                transform.LookAt(targetedEnemy);

                if (Time.time > nextFire)
                {
                    nextFire = Time.time + timeBetweenShots;
                    //isBullet = false;
                    CmdFire(navAgent.transform.position);
                    MoveAndShoot();
                }
            }

            else if (navAgent.remainingDistance >= shootDistance)
            {
                navAgent.Resume();
            }
            else if (navAgent.remainingDistance <= 10)
            {
                navAgent.isStopped = true;
            }


            navAgent.destination = targetedEnemy.position;
        }
    }


    protected bool pathComplete()
    {
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    [Command]
    void CmdFire(Vector3 navPos)
    {
        FindObjectOfType<ShootSound>().MusicSource.Play();
        //MusicSource.Play();
        shooting = true;
        //anim.SetTrigger("Attack");
        GameObject fireball = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
        fireball.GetComponent<Rigidbody>().velocity = fireball.transform.forward * 6;
        NetworkServer.Spawn(fireball);
        //float dist = Vector3.Distance(fireball.transform.position, navPos);
        //print("dist: " + dist);
        Destroy(fireball,8f);
        //if(dist <= 10)   Destroy(fireball);
    }

    [ClientRpc]
    void RpcDestroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //bulletDamage = 30;
            TakePlayerDamage();
            RpcDestroy(collision.gameObject);
            Destroy(collision.gameObject);
            //gameObject.GetComponent<Material>().color = Color.red;
            //WaitALittle();
            //print("Player atacked!!!!");
        }
        //bulletDamage = 30;
        //print(gameObject.tag);
    }

    void TakePlayerDamage()
    {
        playerhealth -= bulletDamage;

        if (playerhealth <= 0)
        {
            playerhealth = 100;
            //print("GAME OVER!!!!!");
            RpcRespawn();
            Score.PlayerDie();
            //RpcDestroy(gameObject);
            //Destroy(gameObject);
            //Application.Quit();
            //SceneManager.LoadScene("Lose");
            //RpcRespawn();
        }
        //RpcDestroy();
        OnHealthChanged(playerhealth);
        //getHealth();
    }

    void OnHealthChanged(int updatedHealth)
    {
        healthText.text = updatedHealth.ToString();
        float normaliz = updatedHealth / 100f;
        float tot = normaliz * totBar;
        healthBar.transform.localScale = new Vector3(tot, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    void getHealth()
    {
        print("Health: " + playerhealth);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = startingPosition;
        }
    }

}
