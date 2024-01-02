using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    UnityEngine.AI.NavMeshAgent agent;
    public Rigidbody bullet;
    public Transform shootingPoint;
    
    GameObject player;
    bool shootOn = true;
    int enemyLive = 3;
    PlayerController playerController;

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "PlayerBullet") {
            enemyLive--;
        }
    }

    // Start is called before the first frame update
    void Start()

    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy enemy-self when live = 0
        if (enemyLive == 0) {
            playerController.killCount++;
            Destroy(gameObject);
        }
        
        // Stop when 5 points from the player
        if (Vector3.Distance(player.transform.position,transform.position) < 5) {
            // Disable navmesh = stop enemy from moving
            agent.enabled = false;

            // Rotate enemy towards player
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up);
        
            // Shoot bullet every 5 seconds
            if (shootOn) {
                shootOn = false;
                Rigidbody p = Instantiate(bullet,shootingPoint.position,shootingPoint.rotation);
                p.velocity = transform.forward * 20;

                Invoke("shootAgain",5.0f);
            }

        } else {
            agent.enabled = true;
            agent.SetDestination(player.transform.position);
        }
    }

    void shootAgain() {
        shootOn = true;
    }
}
