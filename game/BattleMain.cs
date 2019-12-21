using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMain : MonoBehaviour {

	public bool IsBattleFinished;

	public GameMain gameMain;
	public Animator m_animator;

	public GameObject m_goPanelEnemyInfo;

	public Button m_btnShowEnemyDeck;

	public GameObject m_prefCard;
	public GameObject m_goCardRoot;

	public GameObject m_goPanelEnemyDeck;

	public void Opening()
	{
		m_goPanelEnemyInfo.SetActive(true);
		m_animator.SetTrigger("opening");
	}


}
