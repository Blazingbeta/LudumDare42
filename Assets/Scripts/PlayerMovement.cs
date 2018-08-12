using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] int m_maxHealth = 3;
	[SerializeField] float m_hitInvincibiltyTime = 1.0f;
	[SerializeField] float m_movementForce = 5.0f;
	[SerializeField] float m_movementDragCoeffecient = 0.8f;
	[SerializeField] float m_movementMass = 3.0f;
	[SerializeField] float m_movementVelocitySleepSquared = 1.0f;
	[SerializeField] float m_movementStallDragMultiplier = 1.0f;
	[SerializeField] float[] m_movementModifiers;   //MAKE SURE ARRAY INDEX 0 IS ALWAYS SET TO 1.0
	[SerializeField] UnityEngine.UI.Image m_healthMeter = null;
	Vector3 m_currentVelocity = Vector3.zero;
	int m_movementPartSlowdownIndex = 0;
	int m_currentHealth;
	bool m_isInvincible = false;
	private void Start()
	{
		m_currentHealth = m_maxHealth;
	}
	private void FixedUpdate()
	{
		//Get the user's input and add it to the velocity
		Vector3 inputDir = Vector3.zero;
		inputDir.x = Input.GetAxis("Horizontal");
		inputDir.y = Input.GetAxis("Vertical");
		if (inputDir.sqrMagnitude > 1)
		{
			inputDir = inputDir.normalized;
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
			if (m_currentHealth < 0)
			{
				//DIE
			}
			else
			{
				StartCoroutine(IFrames(m_hitInvincibiltyTime));
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
			Debug.Log("slowing to index " + m_movementPartSlowdownIndex);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 11)
		{
			Debug.Log("respeeding");
			m_movementPartSlowdownIndex = 0;
		}
	}
}