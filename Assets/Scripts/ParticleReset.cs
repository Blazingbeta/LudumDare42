using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReset : MonoBehaviour
{
	[SerializeField] float m_time;
	private void OnEnable()
	{
		StartCoroutine(DisableAfterDelay());
	}
	IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(m_time);
		//Puts the object back into the pool
		gameObject.SetActive(false);
	}
}
