using System;
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
				master_card_param = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == card_id);
			}
			return master_card_param;
		}
		set
		{
			master_card_param = value;
		}
	}
	private MasterCardParam master_card_param;

	public void Copy(MasterCardParam c , int _iCharaId , int _card_serial)
	{
		card_id = c.card_id;
		card_serial = _card_serial;
		status = (int)DataCard.STATUS.DECK;
		chara_id = _iCharaId;
		master = c;
	}
}

public class DataCard : CsvData< DataCardParam> {

	public enum STATUS{
		NONE		= 0,
		DECK,
		HAND,
		PLAY		,
		REMOVE		,
		MAX			,
	}

	public int hand_number;

	public DataCardParam RandomSelectFromHand()
	{
		List<DataCardParam> temp = list.FindAll(p => p.status == (int)STATUS.HAND);

		int[] select_prob = new int[temp.Count];
		for( int i = 0; i < temp.Count; i++)
		{
			select_prob[i] = 100;
		}

		int index = UtilRand.GetIndex(select_prob);

		return temp[index];
	}

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

		int select_count = iAdd <= deck_card ? iAdd : deck_card;
		Debug.Log(string.Format("fill={0} deck_card={1} add={2} select_count={3}", _iFillNum, deck_card, iAdd, select_count));

		for ( int i = 0; i < select_count; i++)
		{
			hand_number += 1;

			int select_index = UtilRand.GetIndex(create_prob_arr);

			list[select_index].status = (int)STATUS.HAND;
			list[select_index].order = hand_number;

			// 再選択させない
			create_prob_arr[select_index] = 0;

			_add_list.Add(list[select_index]);
		}
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

	public int GetNewSerial()
	{
		int iSerial = 1;
		foreach( DataCardParam data in list)
		{
			if(iSerial <= data.card_serial)
			{
				iSerial = data.card_serial + 1;
			}
		}
		return iSerial;
	}

	public void AddNewCard(DataCardParam arg0 , STATUS _eStatus)
	{
		int iSerial = GetNewSerial();
		arg0.card_serial = iSerial;
		arg0.status = (int)_eStatus;
		list.Add(arg0);
	}
}
