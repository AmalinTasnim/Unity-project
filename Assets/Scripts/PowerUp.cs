using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject particleHealth, particleSpeed, powerCube;
    GameObject player;
    PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player") {
            int powerUp = Random.Range(1,10);
            playerController.powerUp = powerUp;

            if (powerUp <= 5) {
                // health
                Instantiate(particleHealth,transform.position,transform.rotation);
            } else {
                // speed
                Instantiate(particleSpeed,transform.position,transform.rotation);
            }

            Vector3 cubeLocation = new Vector3(Random.Range(-10.0f,10.0f),transform.position.y,Random.Range(-10.0f,10.0f));
            Instantiate(powerCube,cubeLocation,transform.rotation);
            Destroy(gameObject);
        }
    }    
}
