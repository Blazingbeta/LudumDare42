using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject m_player = null;
	[SerializeField] GameObject[] m_spawnPoints = null;
	[SerializeField] AnimationCurve m_nextSpawnTimeCurve;
	[SerializeField] int m_secondsTilDifficultyPeak = 60;

	public static Transform PlayerTransform = null;
	private void Awake()
	{
		PlayerTransform = m_player.transform;
		//Preload the pools
		ObjectPool.GetObjectFromPool("Enemy");
		ObjectPool.GetObjectFromPool("PartsPile");
	}
	//Game loop needs to spawn enemies at intervals
	//Ramps up difficulty, doesn't repeat spawn points
	private void Update()
	{
		
	}
}
