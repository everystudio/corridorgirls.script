using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DataUnitParam : CsvDataParam
{
	public int chara_id { get; set; }
	public string unit { get; set; }
	public string position { get; set; }
	public int hp { get; set; }
	public int hp_max { get; set; }
	public int str { get; set; }
	public int magic { get; set; }
	public int heal { get; set; }

	public string assist_type { get; set; }
	public int turn { get; set; }

	public void HpHeal(int _iHeal)
	{
		hp += _iHeal;
		if (hp_max < hp)
		{
			hp = hp_max;
		}
	}

	public void TrapDamage(int _iDamage)
	{
		hp -= _iDamage;
		if (hp <= 0)
		{
			hp = 1;
		}
	}

}

public class DataUnit : CsvData<DataUnitParam> {

	public bool IsPartyChara( int _iCharaId)
	{
		DataUnitParam unit = list.Find(p => p.chara_id == _iCharaId &&
		(p.position == "left" || p.position == "right" || p.position == "back"));
		Debug.Log(unit);
		return unit != null;
	}

	public bool IsAliveParty()
	{
		return 0 < list.FindAll(p => p.unit == "chara" && p.position != "none").Count;
	}

	public void AddAssist( string _strAssistType , int _iCharaId, string _strType, int _iParam , int _iTurn)
	{
		DataUnitParam add = new DataUnitParam();
		add.chara_id = _iCharaId;
		add.unit = "assist";
		add.turn = _iTurn;       // なんかいいの欲しいね
		add.assist_type = _strAssistType;

		switch(_strType)
		{
			case "str":
				add.str = _iParam;
				break;
			case "magic":
				add.magic = _iParam;
				break;
			case "heal":
				add.heal = _iParam;
				break;
			case "hp_max":
				add.hp_max = _iParam;
				break;
			case "luck":
				break;
			default:
				break;
		}
		list.Add(add);
	}

}
