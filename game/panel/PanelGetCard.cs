using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGetCard : MonoBehaviour {

	public int stage_id;

	public Button m_btnDecide;
	public List<Card> card_list = new List<Card>();

	public GameObject m_goCardRoot;
	public GameObject m_prefCard;

	public UnityEventDataCardParam OnSelectCardParam = new UnityEventDataCardParam();

	public void Initialize( int _iStageId , int _iWave , int[] _iCharaIdArr )
	{
		//Debug.Log(_iStageId);
		foreach( Card c in card_list)
		{
			Destroy(c.gameObject);
		}
		card_list.Clear();

		for ( int i = 0; i < _iCharaIdArr.Length; i++)
		{
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);
			card_list.Add(c);
		}

		List<MasterStageCardParam> appear_card = DataManagerGame.Instance.masterStageCard.list.FindAll(p => p.stage_id == _iStageId && p.wave == _iWave);
		if(appear_card.Count == 0)
		{
			appear_card = DataManagerGame.Instance.masterStageCard.list.FindAll(p => p.stage_id == _iStageId && p.wave == 0);
		}

		int[] item_prob = new int[appear_card.Count];
		//Debug.Log(appear_card.Count);
		for (int i = 0; i < appear_card.Count; i++)
		{
			item_prob[i] = appear_card[i].prob;
		}

		for (int i = 0; i < _iCharaIdArr.Length; i++)
		{
			int index = UtilRand.GetIndex(item_prob);

			DataCardParam add_card = new DataCardParam();
			// tempシリアルを配布

			MasterCardParam master = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == appear_card[index].card_id);

			add_card.Copy(master, _iCharaIdArr[i], i);
			/*
			add_card.card_id = appear_card[index].card_id;
			add_card.chara_id = _iCharaIdArr[i];
			//Debug.Log(add_card.chara_id);

			add_card.card_serial = i;
			*/


			//add_card.card_type = appear_card[index].card_type;
			//add_card.power = appear_card[index].power;

			card_list[i].Initialize(add_card, DataManagerGame.Instance.masterCardSymbol.list);
		}
	}



}
