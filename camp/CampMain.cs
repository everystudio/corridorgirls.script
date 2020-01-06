using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampMain : Singleton<CampMain> {

	public void SceneGame()
	{
		SceneManager.LoadScene("game");
	}



}
