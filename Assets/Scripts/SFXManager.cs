using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager i;
	private AudioSource m_source;
	[SerializeField] AudioClip m_enemyHit;
	[SerializeField] AudioClip m_enemyDie;
	[SerializeField] AudioClip m_playerHit;
	[SerializeField] AudioClip m_playerDie;
	[SerializeField] AudioClip m_burn;
	[SerializeField] AudioClip m_swing;

	private void Awake()
	{
		i = this;
		m_source = GetComponent<AudioSource>();
	}
	public void EnemyHit()
	{
		m_source.volume = 1.0f;
		m_source.PlayOneShot(m_enemyHit);
	}
	public void EnemyDie()
	{
		m_source.volume = 1.0f;
		m_source.PlayOneShot(m_enemyDie);
	}
	public void PlayerHit()
	{
		m_source.volume = 1.0f;
		m_source.PlayOneShot(m_playerHit);
	}
	public void PlayerDie()
	{
		m_source.volume = 1.0f;
		m_source.PlayOneShot(m_playerDie);
	}
	public void Burn()
	{
		m_source.volume = 1.0f;
		m_source.PlayOneShot(m_burn);
	}
	public void Swing()
	{
		m_source.volume = 0.6f;
		m_source.PlayOneShot(m_swing);
	}
}
