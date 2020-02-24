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
	public PanelSkillDetail m_panelSkillDetail;

	public PanelStatus m_panelStatus;
	public PanelPlayerDeck m_panelPlayerDeck;

	public PanelShop m_panelShop;

	public PanelDecideCheck m_panelDecideCheckBottom;

	public PartyHolder m_partyHolder;

	public PanelMenu m_panelMenu;

	public InfoHeaderCamp m_infoHeaderCamp;

	public PanelScout m_panelScout;
	public PanelPresent m_panelPresent;

	public Animator m_animSideButton;
	public BtnInvite m_btnInvite;
	public BtnPresent m_btnPresent;

	public override void Initialize()
	{
		base.Initialize();
	}


	public void SceneGame()
	{
		SceneManager.LoadScene("game");
	}



}
