using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject m_player = null;
	[SerializeField] GameObject[] m_spawnPoints = null;
	[SerializeField] AnimationCurve m_nextSpawnTimeCurve;
	[SerializeField] int m_secondsTilDifficultyPeak = 60;
	[SerializeField] float m_spawnTimeAmplitude = 5.0f;
	[SerializeField] CanvasGroup m_menu = null;
	[SerializeField] Text m_scoreText = null;

	public static Transform PlayerTransform = null;
	public static GameController i;

	public int m_score = 0;
	float m_timePassed = 0.0f;
	bool m_gameRunning = true;
	int m_lastWaypointIndex = -1;
	private void Awake()
	{
		i = this;
		PlayerTransform = m_player.transform;
		//Preload the pools
		ObjectPool.GetObjectFromPool("Enemy");
		ObjectPool.GetObjectFromPool("PartsPile");
		ObjectPool.GetObjectFromPool("EnemyHitParticle");
		ObjectPool.GetObjectFromPool("EnemyDieParticle");
		StartCoroutine(SpawnEnemyLoop());
	}
	//Game loop needs to spawn enemies at intervals
	//Ramps up difficulty, doesn't repeat spawn points
	private void Update()
	{
		m_timePassed += Time.deltaTime;
	}
	public void AddScore(int amount)
	{
		m_score += amount;
		m_scoreText.text = "Score: " + m_score.ToString("D6");
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
	public void GameOver()
	{
		//Stop enemy spawning
		m_gameRunning = false;
		//Start a coroutine to fade in the menu
		m_menu.transform.GetChild(2).GetComponent<Text>().text = m_score.ToString("D6");
		StartCoroutine(FadeInMenu(1.0f, 2.0f));
	}
	IEnumerator FadeInMenu(float duration, float delay)
	{
		yield return new WaitForSeconds(delay);
		float timer = 0;
		while(timer < duration)
		{
			m_menu.alpha = timer / duration;
			yield return null;
			timer += Time.deltaTime;
		}
		m_menu.alpha = 1;
		m_menu.interactable = true;
		m_menu.blocksRaycasts = true;
	}
	public void RestartGame()
	{
		ObjectPool.ResetPools();
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}
	public void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
