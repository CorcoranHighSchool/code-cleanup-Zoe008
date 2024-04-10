﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //The player's Rigidbody
    [serializedFeild] public Rigidbody playerRb;
    //Jump force
    [serializedFeild] public float jumpForce = 15.0f;
    //Gravity Modifier
    [serializedFeild] private float gravityModifier;
    //Are we on the ground?
    [serializedFeild] public bool isOnGround = true;
    //Is the Game Over
    public bool gameOver = false;

    //Player Animator
    private Animator playerAnim;

    //ParticleSystem explosion
    [serializedFeild]public ParticleSystem explositionParticle;
    //ParticleSystem dirt
    [serializedFeild] private ParticleSystem dirtParticle;

    //Jump sound
    [serializedFeild] private AudioClip jumpSound;
    //Crash sound
    [serializedFeild] private AudioClip crashSound;
    //Player Audio
    [serializedFeild]private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody
        playerRb = GetComponent<Rigidbody>();
        //Connect the Animator
        playerAnim = GetComponent<Animator>();
        //Player Audio
        //playerAudio.GetComponent<AudioSource>();
        //update the gravity
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        //If we press space, jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //trigger the jump animation
            playerAnim.SetTrigger("Jump_trig");
            isOnGround = false;
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtParticle.Play();
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            explositionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);

            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
        }
    }
}
