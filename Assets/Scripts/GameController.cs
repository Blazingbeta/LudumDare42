using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject m_player = null;
	[SerializeField] GameObject[] m_spawnPoints = null;

	public static Transform PlayerTransform = null;
	private void Awake()
	{
		PlayerTransform = m_player.transform;
		//Preload the pools
		ObjectPool.GetObjectFromPool("Enemy");
		ObjectPool.GetObjectFromPool("PartsPile");
	}
}
