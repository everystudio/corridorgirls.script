using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : Singleton<GameMain> {

	public GameObject m_goStageRoot;
	public GameObject m_prefCorridor;

	public GameObject m_goCardRoot;
	public GameObject m_prefCard;

	public PanelStatus panelStatus;
	public CharaControl chara_control;

	public BattleMain battleMain;

	// 演出とかで使うもの
	public RouletteItem rouletteItem;
	public PanelGetCard panelGetCard;

	public GameObject m_goSkillButtonRoot;
	public GameObject m_prefBtnSkill;
	//public List<BtnSkill> m_btnSkillList = new List<BtnSkill>();
	public PanelSkillDetail m_panelSkillDetail;

	public PanelStatus m_panelStatus;
	public PanelPlayerDeck m_panelPlayerDeck;

	public PanelGameControlButtons m_panelGameControlButtons;

	public PanelGameStatus m_panelGameStatus;
	public PanelCameraScaler m_panelCameraScaler;

	public PanelGameMenu m_panelGameMenu;
	public PanelPartyEdit m_panelPartyEdit;

	public PanelBattlePrize m_panelBattlePrize;
	public PanelResult m_panelResult;

	public FadeScreen m_fadeScreen;

	public BackgroundControl m_backgroundControl;
	public GaugeMP gauge_mp;

	[HideInInspector] public bool m_bIsGoal;
	[HideInInspector] public bool m_bIsGameover;

	[HideInInspector] public int m_iCountCardPlay;
	[HideInInspector] public int m_iCountDeck;

	[HideInInspector] public int now_wave;
	[HideInInspector] public int total_wave;
	[HideInInspector] public int total_turn;

	public List<Corridor> corridor_list = new List<Corridor>();

	// これがゲーム中使うキャラ
	public List<DataUnitParam> party_chara_list = new List<DataUnitParam>();


	public UnityEventInt CharaIdHandler = new UnityEventInt();
	public int SelectCharaId
	{
		get
		{
			return m_iSelectCharaId;
		}
		set
		{
			if( m_iSelectCharaId != value)
			{
				m_iSelectCharaId = value;
				CharaIdHandler.Invoke(m_iSelectCharaId);
			}
		}

	}
	private int m_iSelectCharaId;

	public List<Card> card_list_hand = new List<Card>();

	void Start()
	{
		m_prefCard.SetActive(false);
		m_prefCorridor.SetActive(false);
		m_bIsGoal = false;
		m_bIsGameover = false;

		Card[] arr = m_goCardRoot.GetComponentsInChildren<Card>();
		foreach (Card c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}

		m_panelGameControlButtons.ShowButtonNum(0, null);
	}

	public void CardSetup(List<DataCardParam> _list)
	{
		card_list_hand.Clear();

		foreach( DataCardParam param in _list)
		{
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);

			c.Initialize(param, DataManagerGame.Instance.masterCardSymbol.list);
			card_list_hand.Add(c);
		}

		for( int i = 0; i < card_list_hand.Count; i++)
		{
			card_list_hand[i].Initialize(_list[i], DataManagerGame.Instance.masterCardSymbol.list);
		}
	}

	public void CardAdd(DataCardParam _data)
	{
		Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);

		c.Initialize(_data, DataManagerGame.Instance.masterCardSymbol.list);

		card_list_hand.Add(c);
	}


	public void CardOrder()
	{
		float x = -300.0f;
		float width = 600.0f;

		SEControl.Instance.Play(Defines.SE_CARDPLAY);

		float pitch = width / (card_list_hand.Count + 1);
		for ( int i = 0; i < card_list_hand.Count; i++) {

			iTween.MoveTo(card_list_hand[i].gameObject,
				iTween.Hash(
					"x", x + (pitch * (i + 1)),
					"y", 0.0f,
					"z", 0.0f,
					"time", 0.5f,
					"isLocal", true)
				);

			/*
			card_list_hand[i].transform.localPosition = new Vector3(
				,
				0.0f,
				0.0f);
				*/
		}
	}
	public void CardSelectUp( int _iSerial)
	{
		float x = -300.0f;
		float width = 600.0f;

		SEControl.Instance.Play(Defines.SE_CARDPLAY);

		float pitch = width / (card_list_hand.Count + 1);
		for (int i = 0; i < card_list_hand.Count; i++)
		{
			iTween.MoveTo(card_list_hand[i].gameObject,
				iTween.Hash(
					"x", x + (pitch * (i + 1)),
					"y", card_list_hand[i].data_card.card_serial == _iSerial ? 10.0f : 0.0f,
					"z", 0.0f,
					"time", 0.5f,
					"isLocal", true)
				);
		}
	}

	public int GetItem(int _iStageId , int _iCorridorIndex)
	{
		int iItemId = 0;
		return iItemId;
	}

	public void ClearSkill()
	{
		m_panelStatus.ClearSkill();
	}

	public void CharaRefresh()
	{
		panelStatus.area_chara_left.Refresh();
		panelStatus.area_chara_right.Refresh();
	}



}
