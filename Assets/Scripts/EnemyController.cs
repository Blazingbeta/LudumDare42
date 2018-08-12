using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] int m_healthMax = 10;
	[SerializeField] float m_moveSpeed = 5.0f;
	[SerializeField] float m_knockbackCutoffSqr = 0.01f;

	Transform m_playerTransform = null;
	Transform m_transform = null;
	Rigidbody2D m_rb = null;

	int m_health;
	//These two will probably always be the same, leaving seperated just in case I change it later
	bool m_healthIsInvincible = false;
	bool m_isInKnockback = false;

	void Start()
	{
		m_transform = transform;
		m_playerTransform = GameController.PlayerTransform;
		m_rb = GetComponent<Rigidbody2D>();

		SetupEnemy();
	}
	public void SetupEnemy()
	{
		//Get a spawnpoint and place self at it
		//Reset health and sprite values
		m_health = m_healthMax;
	}
	void Update()
	{
		if (m_isInKnockback)
		{
			if (m_rb.velocity.sqrMagnitude <= m_knockbackCutoffSqr)
			{
				m_rb.velocity = Vector2.zero;
				m_isInKnockback = false;
			}
		}
		else
		{
			m_transform.position = Vector3.MoveTowards(m_transform.position, m_playerTransform.position, m_moveSpeed * Time.deltaTime);
		}
	}
	public void DealDamage(int damage)
	{
		if (!m_healthIsInvincible)
		{
			m_health -= damage;
			if (m_health <= 0)
			{
				SmoothFollow.Shake(0.4f);
				GameObject particle = ObjectPool.GetObjectFromPool("EnemyDieParticle");
				particle.transform.position = transform.position;
				particle.SetActive(true);
				Die();
			}
			else
			{
				GameObject particle = ObjectPool.GetObjectFromPool("EnemyHitParticle");
				particle.transform.position = transform.position;
				particle.SetActive(true);
				SmoothFollow.Shake(0.2f);
				//Do damaged sprite logic (if I get that far)
			}
		}
	}
	void Die()
	{
		SFXManager.i.EnemyDie();
		GameController.i.AddScore(10);
		//Grab a set of spare parts from the pool and leave them at this location
		GameObject parts = ObjectPool.GetObjectFromPool("PartsPile");
		parts.GetComponent<PartsPile>().Initialize(transform.position);
		parts.SetActive(true);
		//Put self back into the enemy object pool
		gameObject.SetActive(false);
	}
	public void ApplyKnockback(Vector2 force, float duration)
	{
		if (!m_isInKnockback && m_health > 0)
		{
			SFXManager.i.EnemyHit();
			
			StartCoroutine(KnockbackLoop(force, duration));
			m_isInKnockback = true;
			m_healthIsInvincible = true;
		}
	}
	IEnumerator KnockbackLoop(Vector3 force, float duration)
	{
		float timer = duration;
		while(timer > 0)
		{
			transform.position += force * (timer / duration)*Time.deltaTime;
			yield return null;
			timer -= Time.deltaTime;
		}
		m_isInKnockback = false;
		m_healthIsInvincible = false;
	}
}