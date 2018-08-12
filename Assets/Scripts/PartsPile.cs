﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsPile : MonoBehaviour {
	int m_currentStackAmount = 1;
	bool m_isInKnockback = false;
	public void Initialize(Vector3 pos)
	{
		transform.position = pos;
		m_currentStackAmount = 1;
		m_isInKnockback = false;
	}
	public void HitPile(Vector2 force, float duration)
	{
		m_isInKnockback = true;
		StartCoroutine(KnockbackLoop(force, duration));
	}
	IEnumerator KnockbackLoop(Vector3 force, float duration)
	{
		float timer = duration;
		while (timer > 0)
		{
			transform.position += force * (timer / duration) * Time.deltaTime;
			yield return null;
			timer -= Time.deltaTime;
		}
		m_isInKnockback = false;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Check if collided with a pile
		if(collision.gameObject.layer == 10)
		{
			//If so, make sure the other pile didn't already absorb this pile
			if (gameObject.activeInHierarchy)
			{
				PartsPile other = collision.gameObject.GetComponent<PartsPile>();
				//If we are moving and the other pile isn't, then do nothing and let it absorb this pile
				if (!(m_isInKnockback && !other.m_isInKnockback))
				{
					collision.gameObject.SetActive(false);
					m_currentStackAmount += other.m_currentStackAmount;
					//debug
					transform.localScale = Vector3.one * m_currentStackAmount;
				}
			}
		}
	}
}