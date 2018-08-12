using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
	[SerializeField] Transform m_followTarget = null;
	[SerializeField] float m_rate = 1.0f;
	[SerializeField] float m_shakeRate = 1.0f;
	[SerializeField] float m_shakeAmp = 1.0f;
	Vector3 m_offset = Vector3.zero;
	Vector3 m_shakeVector = Vector3.zero;
	static float m_shakeAmount = 0.0f;
	void Start ()
	{
		m_offset = transform.position - m_followTarget.position;
	}
	public static void Shake(float amount)
	{
		m_shakeAmount += amount;
	}
	void LateUpdate ()
	{
		m_shakeAmount = Mathf.Clamp01(m_shakeAmount - Time.deltaTime);
		float time = Time.time * m_shakeRate;
		m_shakeVector.x = m_shakeAmount * m_shakeAmp * ((Mathf.PerlinNoise(time, 0) * 2) - 1);
		m_shakeVector.y = m_shakeAmount * m_shakeAmp * ((Mathf.PerlinNoise(0, time) * 2) - 1);

		transform.position = Vector3.Lerp(transform.position, m_followTarget.position + m_offset, Time.deltaTime*m_rate) + m_shakeVector;
	}
}
