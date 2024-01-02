using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody), typeof (BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick movejoystick;
    [SerializeField] private FixedJoystick aimJoystick;

    [SerializeField] private Animator _animator;


    [SerializeField] private float _moveSpeed;


    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(movejoystick.Horizontal * _moveSpeed, 0f, movejoystick.Vertical * _moveSpeed);

        if (movejoystick.Horizontal != 0 || movejoystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool("runAim", true);
        }
        else
            _animator.SetBool("runAim", false);
    }


    Animator animator;
    public int speed = 4, bulletSpeed = 30;
    public Rigidbody bullet;
    public Transform shootingPoint;

    Vector3 aimVelocity;
    public GameObject enemy;
    public Slider healthBar;
    public int killCount = 0, powerUp = 0;
    public TextMeshProUGUI killCountText;

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "EnemyBullet") {
            healthBar.value-=5;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        healthBar.maxValue = 100;
        healthBar.value = 100;

        spawnEnemy();
    }

    void spawnEnemy() {
        // Create random x & z value for enemy spawn location
        Vector3 enemyLoc = new Vector3(Random.Range(-5.0f,5.0f),transform.position.y,Random.Range(-5.0f,5.0f));
        Instantiate(enemy,enemyLoc,transform.rotation);

        // Spawn enemy every 6 seconds
        Invoke("spawnEnemy",6.0f);
    }

    // Update is called once per frame
    void Update()
    {   
        // // Player rotate
        // transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        // // Play move forward/backward
        // if (Input.GetAxis("Vertical")!=0) {
        //     animator.SetBool("runAim", true);
        //     transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);

        //     // Player shoot
        //     if (Input.GetKeyDown("space")) {
        //         Rigidbody p = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
        //         p.velocity = transform.forward * bulletSpeed;
        //     }

        // } else {
        //     animator.SetBool("runAim", false);
        // }

        //Aim
        aimVelocity = new Vector3 (aimJoystick.Horizontal, 0f, aimJoystick.Vertical);
        Vector3 AimInput = new Vector3 (aimVelocity.x, 0f, aimVelocity.z);
        Vector3 lookAtPoint = transform.position + AimInput;
        transform.LookAt (lookAtPoint);

        if (aimJoystick.Horizontal != 0 || aimJoystick.Vertical != 0){
            Shoot ();
        }
        // if(aimJoystick.Horizontal >=0.6f || aimJoystick.Vertical >= 0.6) {
        //     Shoot ();
        // }
        // else if(aimJoystick.Horizontal <= -0.6f || aimJoystick.Vertical <= -0.6) {
        //     Shoot ();
        // }

        // Update kill count text
        killCountText.text = "Kill: " + killCount;

        // Power up effect
        if (powerUp <=5 && powerUp != 0) { // Health
            healthBar.value+=5;
            powerUp = 0;
        } else if (powerUp > 5) { // Speed
            speed = 10;
            animator.speed = 10;
            powerUp = 0;
            Invoke("speedToNormal",5.0f);
        }
    }

    void Shoot() {
        Rigidbody newBullet = Instantiate (bullet, shootingPoint.position, shootingPoint.rotation);
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody> ();
        bulletRb.velocity = transform.forward * bulletSpeed;
    }

    void speedToNormal() {
        speed = 4;
        animator.speed = 1;
    }
}
