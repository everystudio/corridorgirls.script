using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCardParam : CsvDataParam
{
	public int card_id { get; set; }
	public string label { get; set; }

	public int symbol_1 { get; set; }
	public int symbol_2 { get; set; }
	public int symbol_3 { get; set; }
	public int symbol_4 { get; set; }
	public int symbol_5 { get; set; }
	public int symbol_6 { get; set; }
	public int power { get; set; }

	public int GetSymbolId(int _iIndex)
	{
		switch(_iIndex)
		{
			case 0:
				return symbol_1;
			case 1:
				return symbol_2;
			case 2:
				return symbol_3;
			case 3:
				return symbol_4;
			case 4:
				return symbol_5;
			case 5:
				return symbol_6;
			default:
				break;
		}
		return 0;
	}
	public bool ContainSymbolId(int _iSymbolId , ref int _iIndex)
	{
		for( int i = 0; i < 6; i++)
		{
			if( GetSymbolId(i) == _iSymbolId)
			{
				_iIndex = i;
				return true;
			}
		}
		return false;
	}
	public int GetMainSymbolId()
	{
		int iSymbolId = 0;
		int iNum = 0;

		Dictionary<int, int> dict = new Dictionary<int, int>();

		int[] symbol_arr = new int[6]
		{
			symbol_1,
			symbol_2,
			symbol_3,
			symbol_4,
			symbol_5,
			symbol_6
		};

		foreach( int symbol_id in symbol_arr)
		{
			if( dict.ContainsKey(symbol_id) == false)
			{
				if (0 < symbol_id)
				{
					dict.Add(symbol_id, 1);
				}
			}
			else
			{
				dict[symbol_id] += 1;
			}
		}

		foreach( KeyValuePair<int,int>kvp in dict)
		{
			if(iNum <kvp.Value)
			{
				iNum = kvp.Value;
				iSymbolId = kvp.Key;
			}
		}
		return iSymbolId;
	}

	public bool ContainSymbolId( int _iSymbolId)
	{
		bool bRet = false;

		int[] symbol_arr = new int[6]
		{
			symbol_1,
			symbol_2,
			symbol_3,
			symbol_4,
			symbol_5,
			symbol_6
		};

		foreach( int symbol_id in symbol_arr)
		{
			if( symbol_id == _iSymbolId)
			{
				return true;
			}
		}



		return bRet;
	}


}

public class MasterCard : CsvData<MasterCardParam> {

	public enum CARD_TYPE
	{
		NONE = 0,
		ATTACK,
		GUARD,
		MAGIC,
		MIND,
		HEAL,
		BUFF,
		DEBUFF,
		MAX,
	}

	static public string GetIconSpriteName(int _iCardType)
	{
		_iCardType %= (int)CARD_TYPE.MAX;
		string[] name_arr =
		{
			"",
			"icon_game_149",
			"shield_attack",
			"icon_game_159",
			"shield_magic",
			"icon_game_8",
			"icon_game_168",
			"icon_game_168",
		};
		return name_arr[_iCardType];

	}
	
	static public Color GetCardColor(int _iCardType)
	{

		Color ret = Color.white;
		switch((CARD_TYPE)_iCardType)
		{
			case CARD_TYPE.ATTACK:
			case CARD_TYPE.GUARD:
				ret = Color.red;
				break;
			case CARD_TYPE.MAGIC:
			case CARD_TYPE.MIND:
				ret = Color.cyan;
				break;
			case CARD_TYPE.HEAL:
				ret = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
				break;

			case CARD_TYPE.BUFF:
				ret = new Color(255.0f / 255.0f, 165.0f / 255.0f, 0.0f / 255.0f);
				break;
			case CARD_TYPE.DEBUFF:
				ret = new Color(75.0f / 255.0f, 0.0f / 255.0f, 130.0f / 255.0f);
				break;
		}


		return ret;
	}

}
