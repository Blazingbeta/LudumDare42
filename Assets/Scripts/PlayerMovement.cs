using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] int m_maxHealth = 3;
	[SerializeField] float m_hitInvincibiltyTime = 1.0f;
	[SerializeField] float m_movementForce = 5.0f;
	[SerializeField] float[] m_movementModifiers;   //MAKE SURE ARRAY INDEX 0 IS ALWAYS SET TO 1.0
	[SerializeField] UnityEngine.UI.Image m_healthMeter = null;

	PartsPile m_currentPile = null;

	int m_movementPartSlowdownIndex = 0;
	int m_currentHealth;
	bool m_isInvincible = false;
	private void Start()
	{
		m_currentHealth = m_maxHealth;
	}
	private void FixedUpdate()
	{
		//If the pile is still set, but is disabled
		//Basically a saftey mechanism for when OnTriggerExit doesn't work
		if (m_currentPile != null && (!m_currentPile.gameObject.activeInHierarchy||m_currentPile.m_currentStackAmount == 3))
		{
			m_movementPartSlowdownIndex = 0;
			m_currentPile = null;
		}
		//Get the user's input and add it to the velocity
		Vector3 inputDir = Vector3.zero;
		inputDir.x = Input.GetAxis("Horizontal");
		inputDir.y = Input.GetAxis("Vertical");
		if (inputDir.sqrMagnitude > 1)
		{
			inputDir = inputDir.normalized;
		}
		if(inputDir.sqrMagnitude != 0)
		{
			float angle = Mathf.Atan2(inputDir.y, inputDir.x) * Mathf.Rad2Deg;
			Quaternion rot = Quaternion.Euler(0, 0, angle); //Quaternion.AngleAxis(angle, Vector3.forward);
			transform.GetChild(0).rotation = rot;
		}
		transform.position += inputDir * m_movementForce * m_movementModifiers[m_movementPartSlowdownIndex] * Time.deltaTime;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		//If other is enemy
		if(collision.gameObject.layer == 9 && !m_isInvincible)
		{
			m_currentHealth--;
			//update health ui
			m_healthMeter.fillAmount = (float)m_currentHealth / m_maxHealth;
			//knockback/invincibility frames
			if (m_currentHealth <= 0)
			{
				//Do some fancy particles and sounds
				gameObject.SetActive(false);
				SmoothFollow.Shake(0.8f);
				SFXManager.i.PlayerDie();
				GameObject particle = ObjectPool.GetObjectFromPool("EnemyDieParticle");
				particle.transform.position = transform.position;
				particle.SetActive(true);
				//DIE
				GameController.i.GameOver();
			}
			else
			{
				StartCoroutine(IFrames(m_hitInvincibiltyTime));
				SFXManager.i.PlayerHit();
				SmoothFollow.Shake(0.4f);
				GameObject particle = ObjectPool.GetObjectFromPool("EnemyHitParticle");
				particle.transform.position = transform.position;
				particle.SetActive(true);
			}
		}
	}
	IEnumerator IFrames(float duration)
	{
		m_isInvincible = true;
		yield return new WaitForSeconds(duration);
		m_isInvincible = false;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 11)
		{
			PartsPile pile = collision.transform.parent.GetComponent<PartsPile>();
			m_movementPartSlowdownIndex = pile.m_currentStackAmount;
			m_currentPile = pile;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 11)
		{
			m_movementPartSlowdownIndex = 0;
		}
	}
}