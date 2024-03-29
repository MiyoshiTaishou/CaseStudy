using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthanTheHero
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region FIELD
        private PlayerMovement playerMv;
        private Animator myAnim;
        private Rigidbody2D myBody;
        private PlayerAttackMethod playerAtt;

        private const string speed = "Speed";
        private const string runIdle = "RunIdlePlayying";
        private const string jump = "Grounded";
        private const string yvelocity = "Yvelocity";
        private const string dash = "Dashing";
        private const string wallSliding = "WallSliding";
        private const string wallJump = "WallJump";
        private const string hurt = "Hurt";
        private const string hurtEnded = "HurtEnded";
        private const string death = "Death";
        private const string deathEnded = "DeathEnded";



        private bool runIdleIsPlayying;

        #endregion

        void Awake()
        {
            playerMv = GetComponent<PlayerMovement>();
            myAnim = GetComponent<Animator>();
            myBody = GetComponent<Rigidbody2D>();
            playerAtt = GetComponent<PlayerAttackMethod>();
        }

        void Update()
        {
            #region IDLE & RUN

            myAnim.SetFloat(speed, Mathf.Abs(playerMv.move.x));

            //Set Run Animation
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("RunIdleTrans"))
            {
                runIdleIsPlayying = true;
                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    runIdleIsPlayying = false;
            }
            myAnim.SetBool(runIdle, runIdleIsPlayying);

            #endregion

            #region JUMP

            myAnim.SetBool(jump, playerMv.grounded);
            myAnim.SetFloat(yvelocity, myBody.velocity.y);

            #endregion

            #region DASH

            myAnim.SetBool(dash, playerMv.isDashing);

            #endregion

            #region WALL SLIDING & WALL JUMP

            myAnim.SetBool(wallSliding, playerMv.wallSliding);
            myAnim.SetBool(wallJump, playerMv.wallJump);

            #endregion

            #region HURT&DEATH

            //Set hurt animation 
            if (Input.GetKeyDown(KeyCode.H))
            {
                myAnim.SetTrigger(hurt);
                myBody.velocity = new Vector2(0f, 0f);
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    myAnim.SetTrigger(hurtEnded);
            }

            //Set death animation
            if (Input.GetKeyDown(KeyCode.X))
            {
                myAnim.SetTrigger(death);
                myBody.velocity = new Vector2(0f, 0f);
            }

            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    myAnim.SetTrigger(deathEnded);
            }

            #endregion
        }

    }
}


