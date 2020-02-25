using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventDataCardParam : UnityEvent<DataCardParam>
{
}


public class DataCardParam : MasterCardParam
{
	//public int card_id { get; set; }
	public int card_serial { get; set; }
	public int status{get;set;}

	public int chara_id { get; set; }
	// 手札に加えた順番
	public int order { get; set; }
	/*
	// masterのデータと同じ（ただし拡張の可能性あり）
	public string label { get; set; }
	public int symbol_1 { get; set; }
	public int symbol_2 { get; set; }
	public int symbol_3 { get; set; }
	public int symbol_4 { get; set; }
	public int symbol_5 { get; set; }
	public int symbol_6 { get; set; }
	public int power { get; set; }
	*/

	/*
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
	*/

	public void Copy(MasterCardParam c , int _iCharaId , int _card_serial)
	{
		card_id = c.card_id;
		card_serial = _card_serial;
		status = (int)DataCard.STATUS.DECK;
		chara_id = _iCharaId;

		label = c.label;
		symbol_1 = c.symbol_1;
		symbol_2 = c.symbol_2;
		symbol_3 = c.symbol_3;
		symbol_4 = c.symbol_4;
		symbol_5 = c.symbol_5;
		symbol_6 = c.symbol_6;
		power = c.power;
	}

}

public class DataCard : CsvData< DataCardParam> {

	public enum STATUS{
		NONE		= 0,
		DECK,
		HAND,
		PLAY		,
		REMOVE		,
		NOTUSE		,
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
			// 捨札だけがシャッフル対象
			if( card.status == (int)STATUS.REMOVE )
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

	public void Reset(List<DataUnitParam> _unit_list , List<MasterCardParam> _master_card_list, List<MasterCharaCardParam> _master_chara_card_list )
	{
		int serial = 1;
		list.Clear();
		List<DataUnitParam> unit_param_list = _unit_list.FindAll(p => p.unit == "chara" && p.position != "none");
		foreach (DataUnitParam unit in unit_param_list)
		{
			List<MasterCharaCardParam> card_list = _master_chara_card_list.FindAll(p => p.chara_id == unit.chara_id);
			foreach (MasterCharaCardParam c in card_list)
			{
				DataCardParam dc = new DataCardParam();

				dc.Copy(_master_card_list.Find(p => p.card_id == c.card_id), c.chara_id, serial);
				//dc.chara_id = c.chara_id;
				//dc.card_id = c.card_id;
				//dc.card_serial = serial;

				dc.status = (int)DataCard.STATUS.DECK;
				serial += 1;

				list.Add(dc);
			}
		}
	}

	public int GetSerialContainSymbol( int _iContainSymbolId , List<DataCardParam> _data_list , List<MasterCardParam> _master_list )
	{
		foreach( DataCardParam data in _data_list)
		{
			MasterCardParam master = _master_list.Find(p => p.card_id == data.card_id);

			if(master.ContainSymbolId(_iContainSymbolId))
			{
				return data.card_serial;
			}
		}
		return 0;
	}

	private int get_symbol_battle( int _iTargetSymbolId)
	{
		int iRetSerial = 0;



		switch(_iTargetSymbolId)
		{
			case 1001:
				break;
			case 2001:
				break;
			case 3001:
				break;
			case 4001:
				break;
			default:
				break;
		}



		return iRetSerial;
	}


	public int SelectBattleCard_FromHand(MasterCardParam master_enemy_card , List<MasterCardParam> _master_card_list , List<MasterCardSymbolParam> _master_symbol_list)
	{
		int iRetSerial = 0;
		int enemy_main_symbol_id = master_enemy_card.GetMainSymbolId();

		List<DataCardParam> search_card_list = list.FindAll(p => p.status == (int)DataCard.STATUS.HAND);

		switch(enemy_main_symbol_id)
		{
			case 1001://物理攻撃
				iRetSerial = GetSerialContainSymbol(2001, search_card_list, _master_card_list);
				break;
			case 3001://魔法攻撃
				iRetSerial = GetSerialContainSymbol(4001, search_card_list, _master_card_list);
				break;
			case 2001://物理防御
				iRetSerial = GetSerialContainSymbol(3001, search_card_list, _master_card_list);
				break;
			case 4001://魔法防御
				iRetSerial = GetSerialContainSymbol(1001, search_card_list, _master_card_list);
				break;
			default:
				break;
		}
		return iRetSerial;
	}
}
