using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class BattleMain : MonoBehaviour {

	public GameObject m_goRoot;

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

	public OverrideSprite override_sprite_chara;

	public DataCard dataCardEnemy = new DataCard();

	public UnityEventInt RequestBattle = new UnityEventInt();
	public UnityEventBool OnBattleFinished = new UnityEventBool();

	public UnityEvent OnOpeningEnd = new UnityEvent();

	public void Opening()
	{
		m_goRoot.SetActive(true);
		m_goPanelEnemyInfo.SetActive(true);
		HpRefresh();
		m_animator.Play("standby");
		m_animator.SetTrigger("opening");
	}

	public void OpeningEnd()
	{
		Debug.LogWarning("OpeningEnd");
		OnOpeningEnd.Invoke();
	}

	public void BattleClose()
	{
		m_goRoot.SetActive(false);
		m_goPanelEnemyInfo.SetActive(false);
	}


	public void HpRefresh()
	{
		if(GameMain.Instance.SelectCharaId == 0)
		{
			return;
		}

		DataUnitParam select_chara = DataManagerGame.Instance.dataUnit.list.Find(p =>
		p.chara_id == GameMain.Instance.SelectCharaId &&
		p.unit == "chara");


		DataUnitParam enemy = DataManagerGame.Instance.dataUnit.list.Find(p =>
		p.unit == "enemy");

		if (select_chara != null && hp_bar_chara != null)
		{
			hp_bar_chara.SetValueMax(select_chara.hp_max);
			hp_bar_chara.SetValueCurrent(select_chara.hp);
		}
		if (enemy != null && hp_bar_enemy != null)
		{
			hp_bar_enemy.SetValueMax(enemy.hp_max);
			hp_bar_enemy.SetValueCurrent(enemy.hp);
		}
	}


	public void ChangeCharaId(int _iCharaId)
	{
		override_sprite_chara.overrideTexture = TextureManager.Instance.Get(string.Format(Defines.STR_FORMAT_CHARA_TEXTURE, _iCharaId));
	}

	public UnityEvent OnDamageFinished = new UnityEvent();

	public void Damage( bool _bIsPlayer , int _iDamage,Action _onFinished)
	{
		GameObject root = null;
		if (_bIsPlayer)
		{
			root = m_goBattleEnemy;
		}
		else
		{
			root = m_goBattleChara;
		}
		DamageNum script = PrefabManager.Instance.MakeScript<DamageNum>(m_prefDamageNum, root);
		//Debug.Log(script.gameObject.transform.localPosition);
		script.gameObject.transform.localPosition = new Vector3(0.0f, -1.5f, -1.5f);

		script.Action(_iDamage, () =>
		{
			_onFinished.Invoke();
		});
	}






}
