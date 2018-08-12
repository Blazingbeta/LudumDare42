using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerController : MonoBehaviour {
	[SerializeField] Sprite m_openSprite;
	[SerializeField] Sprite m_closedSprite;
	[SerializeField] float m_closedTime = 5.0f;

	SpriteRenderer m_sprite;

	bool m_isOpen = true;
	void Start ()
	{
		m_sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
		m_sprite.sprite = m_openSprite;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.layer == 10&&m_isOpen)
		{
			PartsPile pile = collision.gameObject.GetComponent<PartsPile>();
			pile.gameObject.SetActive(false);
			Debug.Log("Consuming pile with " + pile.m_currentStackAmount);
			StartCoroutine(CloseForTime());
		}
	}
	IEnumerator CloseForTime()
	{
		m_isOpen = false;
		m_sprite.sprite = m_closedSprite;
		yield return new WaitForSeconds(m_closedTime);
		m_isOpen = true;
		m_sprite.sprite = m_openSprite;
	}
}
