﻿using UnityEngine;
using System.Collections;

public enum EnemyState 
{
	Idle, DiscoverPlayer, RunToPlayer, Attack
}

public class Enemy : MonoBehaviour {

	GameObject player;
	string enemyName = "evil cube";
	float guardDistance;
	float attackDistance;
	float moveSpeed;
	float rotateSpeed;
	float smooth = 5.0f;
	float atk = 10.0f;
	float atkCD = 2.0f;
	float atkTime;

	int directionParameter;

	Vector3 startPoint;
	EnemyState state = EnemyState.Idle;

	// Use this for initialization
	void Awake () {
		guardDistance = 20.0f;
		attackDistance = 1.0f;
		moveSpeed = PlayerConfiguration.SPEED;
		rotateSpeed = PlayerConfiguration.ROTATE_SPEED;

		startPoint = transform.position;
		atkTime = 0;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.FindWithTag("Player");

		float distance = Vector3.Distance(transform.position , player.transform.position);
		if(distance < guardDistance)
		{
			if(distance < attackDistance && state != EnemyState.Attack)
			{
				state = EnemyState.Attack;
			}

			if(distance >= attackDistance && state == EnemyState.Attack)
			{
				state = EnemyState.RunToPlayer;
			}

			if(state == EnemyState.Idle)
			{
				state = EnemyState.DiscoverPlayer;

				Vector3 targetVector = player.transform.position - transform.position;
				if(Vector3.Dot(targetVector, transform.forward) > 0)
				{
					directionParameter = -1;
				}
				else
				{
					directionParameter = 1;
				}
			}

			if(state == EnemyState.DiscoverPlayer)
			{
				//stage 1:rotate to player
				if(RotateToPlayerWithFrames(1.0f)) {state = EnemyState.RunToPlayer;}
				transform.Translate(0, 0, moveSpeed * Time.deltaTime * 0.1f);
			}

			if(state == EnemyState.RunToPlayer)
			{
				transform.LookAt(player.transform.position);
				transform.Translate(0, 0, moveSpeed * Time.deltaTime);
			}

			if(state == EnemyState.Attack)
			{
				//attack
				if(atkTime > 0)
				{
					atkTime -= Time.deltaTime;
				}
				else
				{
					atkTime = atkCD;
					Attack();
					Debug.Log("attack");
				}
			}
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, startPoint, Time.deltaTime * smooth);

			state = EnemyState.Idle;
		}
	}

	private bool RotateToPlayerWithFrames(float a)
	{

		float angle = Vector3.Angle((player.transform.position - transform.position), transform.forward);

		if(angle > a)
		{
			transform.Rotate(0,directionParameter * rotateSpeed * Time.deltaTime,0,Space.Self);

			return false;
		}
		else
		{
			return true;
		}
	}
	private void Attack() {
		int damageValue = (int)atk;
		GlobalObject.player.Damage(damageValue);
	}
	public string GetEnemyName() {
		return this.enemyName;
	}
}