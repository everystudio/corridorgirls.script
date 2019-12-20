using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventDataCardParam : UnityEvent<DataCardParam>
{
}


public class DataCardParam : CsvDataParam
{
	public int card_id { get; set; }
	public int card_serial { get; set; }
	public int status{get;set;}

	public int chara_id { get; set; }
	// 手札に加えた順番
	public int order { get; set; }

	public MasterCardParam master {
		get
		{
			if(master_card_param == null)
			{
				master_card_param = DataManager.Instance.masterCard.list.Find(p => p.card_id == card_id);
			}
			return master_card_param;
		}
		set
		{
			master_card_param = value;
		}
	}
	private MasterCardParam master_card_param;

}

public class DataCard : CsvData< DataCardParam> {

	public enum STATUS{
		NONE		= 0,
		HAND		,
		DECK		,
		REMOVE		,
		MAX			,
	}

	public int hand_number;

	public bool CardFill(int _iFillNum)
	{
		List<DataCardParam> temp = new List<DataCardParam>();
		return CardFill(_iFillNum, ref temp);
	}

	public bool CardFill(int _iFillNum , ref List<DataCardParam> _add_list)
	{
		int _iExistNum = list.FindAll(p => p.status == (int)STATUS.HAND).Count;
		int iAdd = _iFillNum - _iExistNum;

		int[] create_prob_arr = new int[list.Count];

		int deck_card = 0;

		for( int i = 0; i < list.Count; i++)
		{
			int prob = 0;
			if(list[i].status == (int)STATUS.DECK)
			{
				prob = 100;
				deck_card += 1;
			}
			else {
			}
			create_prob_arr[i] = prob;
		}

		for ( int i = 0; i < iAdd; i++)
		{
			hand_number += 1;

			int select_index = UtilRand.GetIndex(create_prob_arr);

			list[select_index].status = (int)STATUS.HAND;
			list[select_index].order = hand_number;

			// 再選択させない
			create_prob_arr[select_index] = 0;

			_add_list.Add(list[select_index]);
		}
		Debug.Log(string.Format("fill={0} deck_card={1} add={2}", _iFillNum, deck_card, iAdd));
		return iAdd <= deck_card;
	}

	public void DeckShuffle()
	{
		foreach( DataCardParam card in list)
		{
			if( card.status != (int)STATUS.HAND)
			{
				card.status = (int)STATUS.DECK;
			}
		}
	}



}
