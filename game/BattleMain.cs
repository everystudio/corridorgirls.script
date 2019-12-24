using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class BattleMain : Singleton<BattleMain> {

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
	public GameObject m_prefDamageNum;

	public List<BattleIcon> player_icon_list = new List<BattleIcon>();
	public List<BattleIcon> enemy_icon_list = new List<BattleIcon>();

	public GameObject m_goPanelEnemyDeck;

	public EnergyBar hp_bar_chara;
	public EnergyBar hp_bar_enemy;

	public UnityEvent OnOpeningEnd = new UnityEvent();

	public void Opening()
	{
		m_goPanelEnemyInfo.SetActive(true);
		HpRefresh();
		m_animator.SetTrigger("opening");
	}

	public void OpeningEnd()
	{
		OnOpeningEnd.Invoke();
	}

	public void BattleClose()
	{
		m_goPanelEnemyInfo.SetActive(false);
		gameObject.SetActive(false);
	}


	public void HpRefresh()
	{
		if(GameMain.Instance.SelectCharaId == 0)
		{
			return;
		}

		DataUnitParam select_chara = DataManager.Instance.dataUnit.list.Find(p =>
		p.chara_id == GameMain.Instance.SelectCharaId &&
		p.unit == "chara");


		DataUnitParam enemy = DataManager.Instance.dataUnit.list.Find(p =>
		p.unit == "enemy");

		hp_bar_chara.SetValueMax(select_chara.hp_max);
		hp_bar_chara.SetValueCurrent(select_chara.hp);

		hp_bar_enemy.SetValueMax(enemy.hp_max);
		hp_bar_enemy.SetValueCurrent(enemy.hp);
	}




}
