using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField] float m_moveSpeed = 5.0f;
	[SerializeField] float m_knockbackCutoffSqr = 0.01f;

	Transform m_playerTransform = null;
	Transform m_transform = null;
	Rigidbody2D m_rb = null;

	bool m_isInKnockback = false;

	void Start()
	{
		m_transform = transform;
		m_playerTransform = GameController.PlayerTransform;
		m_rb = GetComponent<Rigidbody2D>();
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
	public void ApplyKnockback(Vector2 force)
	{
		if (!m_isInKnockback)
		{
			m_rb.AddForce(force, ForceMode2D.Impulse);
			m_isInKnockback = true;
		}
	}
}