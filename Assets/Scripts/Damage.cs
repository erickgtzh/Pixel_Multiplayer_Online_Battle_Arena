using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.AI;

[RequireComponent(typeof(Player))]
public class Damage : NetworkBehaviour
{
    //DisplayText display = new DisplayText();
    [SerializeField]private GameObject healthBarEnemy;
    private Animator anim;

    [SyncVar(hook = "OnHealthChanged")] private int health = 100;
    private int bulletDamage = 50;
    [SerializeField] private TextMesh healthText;
    private Vector3 startingPosition;
    public static int score = 0;
    private int deaths;

    public override void OnStartLocalPlayer()
    {
        //score = 0;
        //GameObject goal = GameObject.FindGameObjectWithTag("Player");
        //NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.transform.position;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            GameObject goal = GameObject.FindGameObjectWithTag("Player");
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.transform.position;
        }

        //agent.speed = agent.speed*.5f;
    }

    float totBar;
    void Start()
    {
        totBar = healthBarEnemy.transform.localScale.x;
        //Assert.IsNotNull(healthText);
        //score = 0;

        //name = "";
        startingPosition = transform.position;
        //deaths = 3;
    }

    IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            //print("Me disteeeeee!");
            //print(health);
            TakeDamage(gameObject);
            //WaitASecond();
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            // score++;
            Score.EnemyKilled();
            //GetScore();
            //print(Score.GetScore());
            //Scoreboard.Print(Score.GetScore());
        }
        //print(gameObject.tag);
    }


    void TakeDamage(GameObject gameObject)
    {
        //anim.SetBool("Death_b", true);
        health -= bulletDamage;

        if (health <= 0)
        {
            //health = 100;
            //score++;
            Score.EnemyKilled();
            //print(Score.GetScore());
            //RpcRespawn();
            //RpcDestroy(gameObject);
            Destroy(gameObject);
        }
        //RpcDestroy(gameObject);
        
        OnHealthChanged(health);
    }

    public int GetScore()
    {
        return score;
    }

    [ClientRpc]
    void RpcDestroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    void printScore()
    {
        print("Kills: " + score);
    }

    void OnHealthChanged(int updatedHealth)
    {
        healthText.text = updatedHealth.ToString();
        float normaliz = updatedHealth / 100f;
        float tot = normaliz * totBar;
        healthBarEnemy.transform.localScale = new Vector3(tot, healthBarEnemy.transform.localScale.y, healthBarEnemy.transform.localScale.z);
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
