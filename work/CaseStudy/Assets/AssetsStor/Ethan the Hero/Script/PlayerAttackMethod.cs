using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthanTheHero
{
	public class PlayerAttackMethod : MonoBehaviour
	{
		#region FIELD

		private PlayerAnimation playerAnim;
		private PlayerMovement playerMv;
		private Animator myAnim;
		private Rigidbody2D myBody;

		[Header("Basic Attack")]
		public float basicAttack01Power = 0.5f;
		public float basicAttack02Power = 0.5f;
		public float basicAttack03Power = 0.9f;

		private bool atkButtonClickedOnAtk01;
		private bool atkButtonClickedOnAtk02;
		private bool atkButtonClickedOnAtk03;

		private const string attack01 = "Attack01";
		private const string attack02 = "Attack02";
		private const string attack03 = "Attack03";
		private const string notAttacking = "NotAttacking";

		#endregion

		void Awake()
		{
			myAnim = GetComponent<Animator>();
			playerAnim = GetComponent<PlayerAnimation>();
			myBody = GetComponent<Rigidbody2D>();
			playerMv = GetComponent<PlayerMovement>();
		}

		void Update()
		{
			if (playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding)
				return;


			BasicAttackCombo();


		}

		void FixedUpdate()
		{
			if (playerMv.isDashing || playerMv.wallJump || playerMv.wallSliding)
				return;

			BasicAttackMethod();

		}

		#region BASIC ATTACK

		private void BasicAttackCombo()
		{
			//Combo attack mechanic
			if (Input.GetMouseButtonDown(0) && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03") && playerMv.grounded)
				myAnim.SetTrigger(attack01);

			//Set combo attack 01 
			if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk01 = true;

				//Set if attack 01 animation is ended playying and attack button is clicked while attack 01 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8 && atkButtonClickedOnAtk01)
				{
					myAnim.SetTrigger(attack02);
					atkButtonClickedOnAtk01 = false;

				}
				//Set if attack 01 animation is ended playying and attack button is not clicked while attack 01 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk01)
					myAnim.SetTrigger(notAttacking);
			}

			//Set combo attack 02
			if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk02 = true;

				//Set if attack 02 animation is ended playying and attack button is clicked while attack 02 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8 && atkButtonClickedOnAtk02)
				{
					myAnim.SetTrigger(attack03);
					atkButtonClickedOnAtk02 = false;

				}
				//Set if attack 02 animation is ended playying and attack button is not clicked while attack 02 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk02)
					myAnim.SetTrigger(notAttacking);
			}

			//Set combo attack 03
			if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
			{
				//See if attak button is clicked
				if (Input.GetMouseButtonDown(0))
					atkButtonClickedOnAtk03 = true;

				//Set if attack 03 animation is ended playying and attack button is clicked while attack 03 animation is playing
				if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && atkButtonClickedOnAtk03)
				{
					myAnim.SetTrigger(attack01);
					atkButtonClickedOnAtk03 = false;

				}
				//Set if attack 03 animation is ended playying and attack button is not clicked while attack 03 animation is playing
				else if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !atkButtonClickedOnAtk03)
					myAnim.SetTrigger(notAttacking);
			}
		}

		private void BasicAttackMethod()
		{

			//Move player if player is in attacking state
			if (transform.localScale.x == 1)
			{
				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
					myBody.velocity = new Vector2(basicAttack01Power, myBody.velocity.y);

				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
					myBody.velocity = new Vector2(basicAttack02Power, myBody.velocity.y);

				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
					myBody.velocity = new Vector2(basicAttack03Power, myBody.velocity.y);
			}
			else
			{
				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
					myBody.velocity = new Vector2(-basicAttack01Power, myBody.velocity.y);

				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
					myBody.velocity = new Vector2(-basicAttack02Power, myBody.velocity.y);

				if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
					myBody.velocity = new Vector2(-basicAttack03Power, myBody.velocity.y);
			}

		}

		#endregion


	}
}
