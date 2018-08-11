using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomController : MonoBehaviour
{
	//Serialized Fields
	[SerializeField] Vector3 m_basePlayerOffset = Vector3.zero;

	//Component References
	Camera m_mainCam = null;
	Animator m_anim = null;

	//Local Variables
	bool m_isInAnimation = false;
	private void Start()
	{
		m_mainCam = Camera.main;
		m_anim = GetComponent<Animator>();
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
		if (Input.GetMouseButtonDown(0)&&!m_isInAnimation)
		{
			m_anim.SetTrigger("Strike");
			m_isInAnimation = true;
		}
		else if (Input.GetMouseButtonDown(1)&&!m_isInAnimation)
		{
			m_anim.SetTrigger("Sweep");
			m_isInAnimation = true;
		}
	}
	public void EndAnimation()
	{
		m_isInAnimation = false;
	}
	public void BeginStrikeHitbox()
	{

	}
	public void EndStrikeHitbox()
	{

	}
	public void BeginSweepHitbox()
	{

	}
	public void EndSweepHitbox()
	{

	}
}
