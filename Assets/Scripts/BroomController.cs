using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomController : MonoBehaviour
{
	private enum CollisionType { Strike, Sweep }
	//Serialized Fields
	[SerializeField] Vector3 m_basePlayerOffset = Vector3.zero;

	[SerializeField]  int  m_damageStrike = 5;
	[SerializeField]  int  m_damageSweep  = 3;

	[SerializeField] float m_knockbackStrikeForce = 3.0f;
	[SerializeField] float m_knockbackSweepForce  = 3.0f;
	[SerializeField] float m_knockbackStrikeDuration = 0.8f;
	[SerializeField] float m_knockbackSweepDuration  = 1.2f;

	[SerializeField] float m_knockbackPileForce = 3.0f;
	[SerializeField] float m_knockbackPileDuration = 1.0f;

	//Component References
	Camera m_mainCam = null;
	Animator m_anim = null;

	//Local Variables
	bool m_isInAnimation = false;
	CollisionType m_currentAttackType = CollisionType.Strike;
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
			Quaternion rot = Quaternion.Euler(0, 0, angle); //Quaternion.AngleAxis(angle, Vector3.forward);
			transform.localPosition = rot * m_basePlayerOffset;
			transform.rotation = rot;
		}

		//attaack if input is given
		if (Input.GetMouseButtonDown(0)&&!m_isInAnimation)
		{
			m_anim.SetTrigger("Strike");
			m_currentAttackType = CollisionType.Strike;
			m_isInAnimation = true;
		}
		else if (Input.GetMouseButtonDown(1)&&!m_isInAnimation)
		{
			m_anim.SetTrigger("Sweep");
			m_currentAttackType = CollisionType.Sweep;
			m_isInAnimation = true;
		}
	}
	public void CollideWithEnemy(GameObject other)
	{
		//Debug.Log(other.name + " hit in " + m_currentAttackType);
		//Check if its an enemy
		if(other.layer == 9)
		{
			EnemyController enem = other.GetComponent<EnemyController>();
			if (m_currentAttackType == CollisionType.Strike)
			{
				enem.DealDamage(m_damageStrike);
				//strike knocks the enemy back away from you
				Vector2 direction = (other.transform.position - transform.position).normalized;
				enem.ApplyKnockback(direction * m_knockbackStrikeForce, m_knockbackStrikeDuration);
			}
			else
			{
				enem.DealDamage(m_damageSweep);
				//Sweep pushes the enemy forward
				Vector2 direction = -transform.right;
				enem.ApplyKnockback(direction * m_knockbackSweepForce, m_knockbackSweepDuration);
			}
		}
		//If its a parts pile
		else if(other.layer == 10)
		{
			PartsPile pile = other.GetComponent<PartsPile>();
			//Parts piles are only affected by sweeps
			if(m_currentAttackType == CollisionType.Sweep)
			{
				Vector2 direction = -transform.right;
				pile.HitPile(direction * m_knockbackPileForce, m_knockbackPileDuration);
			}
		}
	}
	public void EndAnimation()
	{
		m_isInAnimation = false;
	}
	public void BeginStrikeHitbox()
	{
		SFXManager.i.Swing();
	}
	public void EndStrikeHitbox()
	{

	}
	public void BeginSweepHitbox()
	{
		SFXManager.i.Swing();
	}
	public void EndSweepHitbox()
	{

	}
}
