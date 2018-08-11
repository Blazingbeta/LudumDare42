using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomHitbox : MonoBehaviour
{
	BroomController m_controller = null;
	private void Start()
	{
		m_controller = transform.parent.GetComponent<BroomController>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
	m_controller.CollideWithEnemy(collision.gameObject);
	}
}
