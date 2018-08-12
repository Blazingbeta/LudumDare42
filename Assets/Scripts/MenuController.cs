using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public void QuitGame()
	{
		Application.Quit();
	}
	public void PlayGame()
	{
		ObjectPool.ResetPools();
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}
}
