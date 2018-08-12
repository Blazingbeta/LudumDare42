using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerController : MonoBehaviour {
	[SerializeField] Sprite m_openSprite;
	[SerializeField] Sprite m_closedSprite;
	[SerializeField] float m_closedTime = 5.0f;
	[SerializeField] int[] m_scoreAmounts;

	SpriteRenderer m_sprite;
	ParticleSystem m_particle;

	bool m_isOpen = true;
	void Start ()
	{
		m_sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		m_particle = transform.GetChild(1).GetComponent<ParticleSystem>();
		m_sprite.sprite = m_openSprite;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 10&&m_isOpen)
		{
			m_particle.Play();
			SFXManager.i.Burn();
			PartsPile pile = collision.gameObject.GetComponent<PartsPile>();
			pile.gameObject.SetActive(false);
			GameController.i.AddScore(m_scoreAmounts[pile.m_currentStackAmount - 1]);
			StartCoroutine(CloseForTime());
		}
	}
	IEnumerator CloseForTime()
	{
		GetComponent<Collider2D>().enabled = false;
		m_isOpen = false;
		m_sprite.sprite = m_closedSprite;
		yield return new WaitForSeconds(m_closedTime);
		m_isOpen = true;
		m_sprite.sprite = m_openSprite;
		GetComponent<Collider2D>().enabled = true;
	}
}
