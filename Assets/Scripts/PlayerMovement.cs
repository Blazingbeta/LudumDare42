using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] float m_movementForce = 5.0f;
	[SerializeField] float m_movementDragCoeffecient = 0.8f;
	[SerializeField] float m_movementMass = 3.0f;
	[SerializeField] float m_movementVelocitySleepSquared = 1.0f;
	[SerializeField] float m_movementStallDragMultiplier = 1.0f;
	Vector3 m_currentVelocity = Vector3.zero;
	private void FixedUpdate()
	{
		//Get the user's input and add it to the velocity
		Vector3 inputDir = Vector3.zero;
		inputDir.x = Input.GetAxis("Horizontal");
		inputDir.y = Input.GetAxis("Vertical");
		if (inputDir.sqrMagnitude > 1)
		{
			inputDir = inputDir.normalized;
		}
		//Input direction * force amount / mass
		m_currentVelocity += (inputDir * m_movementForce / m_movementMass) / 60.0f;
		//Apply the drag
		//force = v^2 * drag in negative v normalized dir
		Vector3 dragForce = -m_currentVelocity.normalized;
		dragForce *= m_currentVelocity.sqrMagnitude * m_movementDragCoeffecient;
		dragForce /= m_movementMass;
		m_currentVelocity += dragForce;
		//If there is no input and the velocity is below the cutoff
		if (inputDir.sqrMagnitude == 0 && m_currentVelocity.sqrMagnitude <= m_movementVelocitySleepSquared)
		{
			m_currentVelocity = Vector3.zero;
		}
		else if(inputDir.sqrMagnitude == 0)
		{
			m_currentVelocity += dragForce * m_movementStallDragMultiplier;
		}
		//Apply the velocity
		transform.position += m_currentVelocity;
	}
}