using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampMain : Singleton<CampMain> {


	public PanelStage m_panelStage;
	public PanelChara m_panelChara;

	public PanelStatus m_panelStatus;
	public PanelPlayerDeck m_panelPlayerDeck;

	public override void Initialize()
	{
		base.Initialize();

		m_panelStatus.m_btnDeck.onClick.AddListener(() =>
		{
			m_panelPlayerDeck.ShowCamp();
		});
	}


	public void SceneGame()
	{
		SceneManager.LoadScene("game");
	}



}
