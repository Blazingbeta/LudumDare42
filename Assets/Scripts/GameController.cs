using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject m_player = null;
	public static Transform PlayerTransform = null;
	private void Awake()
	{
		PlayerTransform = m_player.transform;
	}
}
