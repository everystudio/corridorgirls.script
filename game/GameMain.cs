using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : Singleton<GameMain> {

	public GameObject m_goStageRoot;
	public GameObject m_prefCorridor;

	public GameObject m_goCardRoot;
	public GameObject m_prefCard;

	public CharaControl chara_control;

	public List<Card> card_list_hand = new List<Card>();

	void Start()
	{
		m_prefCard.SetActive(false);


		Card[] arr = m_goCardRoot.GetComponentsInChildren<Card>();
		foreach (Card c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}
	}

	public void CardSetup(List<DataCardParam> _list)
	{
		card_list_hand.Clear();

		foreach( DataCardParam param in _list)
		{
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);

			c.Initialize(param);
			card_list_hand.Add(c);
		}

		for( int i = 0; i < card_list_hand.Count; i++)
		{
			card_list_hand[i].Initialize(_list[i]);
		}
	}


	public void CardOrder()
	{
		float x = -300.0f;
		float width = 600.0f;

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



}
