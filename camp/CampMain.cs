using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampMain : Singleton<CampMain> {


	public PanelStage m_panelStage;
	public PanelStageCheck m_panelStageCheck;
	public PanelCampItem m_panelCampItem;

	public PanelChara m_panelChara;
	public PanelSkill m_panelSkill;

	public PanelStatus m_panelStatus;
	public PanelPlayerDeck m_panelPlayerDeck;

	public PanelShop m_panelShop;

	public PanelDecideCheck m_panelDecideCheckBottom;

	public PartyHolder m_partyHolder;

	public PanelMenu m_panelMenu;

	public InfoHeaderCamp m_infoHeaderCamp;

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
