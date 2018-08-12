using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject m_player = null;
	[SerializeField] GameObject[] m_spawnPoints = null;
	[SerializeField] AnimationCurve m_nextSpawnTimeCurve;
	[SerializeField] int m_secondsTilDifficultyPeak = 60;
	[SerializeField] float m_spawnTimeAmplitude = 5.0f;

	public static Transform PlayerTransform = null;

	float m_timePassed = 0.0f;
	bool m_gameRunning = true;
	int m_lastWaypointIndex = -1;
	private void Awake()
	{
		PlayerTransform = m_player.transform;
		//Preload the pools
		ObjectPool.GetObjectFromPool("Enemy");
		ObjectPool.GetObjectFromPool("PartsPile");
		StartCoroutine(SpawnEnemyLoop());
	}
	//Game loop needs to spawn enemies at intervals
	//Ramps up difficulty, doesn't repeat spawn points
	private void Update()
	{
		m_timePassed += Time.deltaTime;
	}
	IEnumerator SpawnEnemyLoop()
	{
		while (m_gameRunning)
		{
			//waits for spawn amplitude * animation curve, evaluated at timepassed/difficultypeaktime
			float waitTime = m_spawnTimeAmplitude *
				m_nextSpawnTimeCurve.Evaluate(Mathf.Clamp01(m_timePassed / m_secondsTilDifficultyPeak));
			Debug.Log("Waiting for " + waitTime);
			yield return new WaitForSeconds(waitTime);
			SpawnEnemy();
		}
	}
	void SpawnEnemy()
	{
		GameObject enemy = ObjectPool.GetObjectFromPool("Enemy");
		enemy.SetActive(true);
		enemy.GetComponent<EnemyController>().SetupEnemy();
		int waypointIndex = m_lastWaypointIndex;
		while(waypointIndex == m_lastWaypointIndex)
		{
			waypointIndex = Random.Range(0, m_spawnPoints.Length);
		}
		m_lastWaypointIndex = waypointIndex;
		enemy.transform.position = m_spawnPoints[waypointIndex].transform.position;
	}
}
