using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMover : MonoBehaviour {
	[SerializeField] float m_amp = 50.0f;
	[SerializeField] float m_rate = 1.0f;
	Vector3 m_startPos;
	private void Start()
	{
		m_startPos = transform.position;
	}
	void Update ()
	{
		transform.position = m_startPos + Vector3.up * Mathf.Sin(Time.time * m_rate) * m_amp;
	}
}
