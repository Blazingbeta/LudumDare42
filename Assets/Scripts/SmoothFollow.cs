using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
	[SerializeField] Transform m_followTarget = null;
	[SerializeField] float m_rate = 1.0f;
	Vector3 m_offset = Vector3.zero;
	void Start ()
	{
		m_offset = transform.position - m_followTarget.position;
	}
	void LateUpdate ()
	{
		transform.position = Vector3.Lerp(transform.position, m_followTarget.position + m_offset, Time.deltaTime*m_rate);
	}
}
