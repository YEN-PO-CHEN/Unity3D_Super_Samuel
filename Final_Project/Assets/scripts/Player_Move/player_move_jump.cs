﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move_jump : MonoBehaviour {
    private float speed;
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField] float rotate_speed;
    private Vector3 jumpdirection = new Vector3(0, 1, 0);
    [SerializeField] float jumpforce;
    Rigidbody rb;
    Animator animator;
    private int a = 0,dead=0;
    private Vector3 maindirection;

    public int button_X;
    public int button_Y;
    public int button_Heigh;
    public int button_Wide;
    // Use this for initialization
    private void Awake()
    {
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveDirection = new Vector3(h, 0, v);
        if(Input.GetKey(KeyCode.LeftShift))
            speed = 4;
        else
            speed = 2.5f;
        if (Input.GetKey(KeyCode.W))
        {
            maindirection = speed * Time.deltaTime * transform.forward;
            rb.MovePosition(rb.position + maindirection * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, new Vector3(0, -1, 0), Time.deltaTime * rotate_speed);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 opDirection = new Vector3(0f, -180f, 0f);
            transform.Rotate(opDirection);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, new Vector3(0, 1, 0), Time.deltaTime * rotate_speed);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && a < 1)
        {
            rb.AddForce(jumpdirection * jumpforce);
            a += 2;
        }

        if (Input.GetKey("w") || Input.GetKeyDown("s") || Input.GetKey("a") || Input.GetKey("d"))
            animator.SetInteger("AnimationPar", 1);
        else
            animator.SetInteger("AnimationPar", 0);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
            a = 0;
        if (collision.gameObject.tag == "monster")
        {
            rb.AddForce(transform.forward * -40);
            Debug.Log("monster");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dead")
        {
            /*rb.constraints &= ~RigidbodyConstraints.FreezeRotationX;
            rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
            rb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;*/
            rb.constraints = RigidbodyConstraints.None;
            rb.AddTorque(new Vector3(1, 1, 1) * 5);
            dead = 1;
        }

    }
    private void OnGUI()
    {
        if (dead == 1)
        {
            Debug.Log("dead");

            if (GUI.Button(new Rect(400, 200, 100, 50), "Retry"))
            {
                Debug.Log("retry");
                //rb.constraints = RigidbodyConstraints.FreezeRotationX;
                //rb.constraints = RigidbodyConstraints.FreezeRotationY;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                transform.localPosition = new Vector3(0, 10, 0);
            }

        }
    }

}
