using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomController : MonoBehaviour
{
	[SerializeField] Vector3 m_basePlayerOffset = Vector3.zero;
	Camera m_mainCam = null;
	bool m_isInAnimation = false;
	private void Start()
	{
		m_mainCam = Camera.main;
	}
	private void Update()
	{
		//Only update position if the player isn't in the middle of an attack
		if (!m_isInAnimation)
		{
			//Get the direction towards the mouse
			Vector3 mousePos = m_mainCam.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;
			Vector3 direction = (transform.parent.position - mousePos).normalized;
			//rotate the offset * direction to make it hover in front of the player
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.localPosition = rot * m_basePlayerOffset;
			transform.rotation = rot;
		}

		//attaack if input is given
		if (Input.GetMouseButtonDown(0))
		{
			//StartCoroutine();
		}
	}
	private void LateUpdate()
	{
		
	}
}
