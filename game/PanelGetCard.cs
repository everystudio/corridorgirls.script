using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGetCard : MonoBehaviour {

	public Button m_btnDecide;
	public List<Card> card_list = new List<Card>();

	public GameObject m_goCardRoot;
	public GameObject m_prefCard;

	public UnityEventDataCardParam OnSelectCardParam = new UnityEventDataCardParam();

	public void Initialize( int _iNum , int _iStageId)
	{

		foreach( Card c in card_list)
		{
			Destroy(c.gameObject);
		}
		card_list.Clear();

		for ( int i = 0; i < _iNum; i++)
		{
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);
			card_list.Add(c);
		}

		int iCount = card_list.Count;

		List<MasterStageCardParam> appear_card = DataManagerGame.Instance.masterStageCard.list.FindAll(p => p.stage_id == _iStageId);

		int[] item_prob = new int[appear_card.Count];
		for (int i = 0; i < appear_card.Count; i++)
		{
			item_prob[i] = appear_card[i].prob;
		}

		for (int i = 0; i < iCount; i++)
		{
			//int index = UtilRand.GetIndex(item_prob);

			DataCardParam add_card = new DataCardParam();

			// tempシリアルを配布
			add_card.card_serial = i;

			MasterCardParam master = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == appear_card[i].card_id);
			add_card.master = master;

			//add_card.card_type = appear_card[index].card_type;
			//add_card.power = appear_card[index].power;

			card_list[i].Initialize(add_card, DataManagerGame.Instance.masterCardSymbol.list);
		}
	}



}
