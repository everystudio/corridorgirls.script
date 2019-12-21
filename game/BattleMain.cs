using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class BattleMain : MonoBehaviour {

	public bool IsBattleFinished;

	public GameMain gameMain;
	public Animator m_animator;

	public GameObject m_goPanelEnemyInfo;

	public Button m_btnShowEnemyDeck;

	public GameObject m_prefCard;
	public GameObject m_goCardRoot;

	public GameObject m_goPlayerCardRoot;
	public GameObject m_goEnemyCardRoot;
	public Card player_card;
	public Card enemy_card;

	public GameObject m_goBattleChara;
	public GameObject m_goBattleEnemy;

	public GameObject m_prefBattleIcon;
	public SpriteRenderer m_sprPlayer;
	public SpriteRenderer m_sprEnemy;

	public List<BattleIcon> player_icon_list = new List<BattleIcon>();
	public List<BattleIcon> enemy_icon_list = new List<BattleIcon>();

	public GameObject m_goPanelEnemyDeck;

	public UnityEvent OnOpeningEnd = new UnityEvent();

	public void Opening()
	{
		m_goPanelEnemyInfo.SetActive(true);
		m_animator.SetTrigger("opening");
	}

	public void OpeningEnd()
	{
		OnOpeningEnd.Invoke();
	}


}
