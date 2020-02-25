using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DataUnitParam : CsvDataParam
{
	public int chara_id { get; set; }
	public int level { get; set; }
	public string unit { get; set; }
	public string position { get; set; }
	public int hp { get; set; }
	public int hp_max { get; set; }
	public int tension { get; set; }
	public int str { get; set; }
	public int magic { get; set; }
	public int heal { get; set; }
	public int luck { get; set; }
	public int food { get; set; }

	public string assist_type { get; set; }
	public string assist_name { get; set; }
	public int turn { get; set; }
	public bool assist_set { get; set; }

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

	public void CopyParams(MasterCharaParam _master)
	{
		hp_max = _master.hp_max;
		hp = hp_max;

		str = _master.str;
		magic = _master.magic;
		heal = _master.heal;
		luck = _master.luck;
	}


	public void BuildTension(MasterCharaParam _base, int _iTension)
	{
		chara_id = _base.chara_id;
		unit = "assist";
		assist_type = "tension";
		assist_name = "テンション";

		hp = 0;
		hp_max = 0;

		tension = _iTension;

		// テンションは60が基準
		float fSwing = (_iTension - 60) * 0.005f;

		str = (int)(_base.str * fSwing);
		magic = (int)(_base.magic * fSwing);
		heal = (int)(_base.heal * fSwing);

		turn = -1;	// 永続
		return;
	}

	public void BuildAssist(DataUnitParam _assist)
	{
		if (_assist.chara_id != chara_id)
		{
			Debug.LogError("not equal chara_id");
		}
		if (_assist.unit != "assist")
		{
			Debug.LogError("not unit type is assist");
		}

		if (_assist.assist_set == false)
		{
			// 最大値が増える場合はhpも増やす
			hp_max += _assist.hp_max;
			if (0 < _assist.hp_max)
			{
				// HPの最大値につられて増えるのは生きてる時だけ
				if (0 < hp)
				{
					hp += _assist.hp_max;
				}
			}
			else if (_assist.hp_max < 0)
			{
				if (hp_max < hp)
				{
					hp = hp_max;
				}
			}

			str += _assist.str;
			magic += _assist.magic;
			heal += _assist.heal;
			luck += _assist.luck;
			_assist.assist_set = true;
		}
	}

	public void BuildAssist(List<DataUnitParam> _list)
	{
		foreach (DataUnitParam unit in _list)
		{
			BuildAssist(unit);
		}
	}
	public void RemoveAssist(DataUnitParam _assist)
	{
		if (_assist.chara_id != chara_id)
		{
			Debug.LogError("not equal chara_id");
			return;
		}
		if (_assist.unit != "assist")
		{
			Debug.LogError("not unit type is assist");
			return;
		}
		if( _assist.assist_set == false)
		{
			Debug.LogError("not set");
		}

		hp_max -= _assist.hp_max;
		hp = Math.Min(hp, hp_max);

		str -= _assist.str;
		magic -= _assist.magic;
		heal -= _assist.heal;
		luck -= _assist.luck;
		_assist.assist_set = false;
	}

	public void Attack_Sim( int _iAttack , int _iSymbolId , DataCardParam _card , DataUnitParam _target)
	{
		for( int i = 0; i< 6; i++)
		{
			if(_card.GetSymbolId(i)== _iSymbolId)
			{
				_target.hp -= _iAttack;
			}
		}
		return;
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
		return 0 < list.FindAll(p => p.unit == "chara" && p.position != "none" && 0 < p.hp).Count;
	}

	public void AddTension( int _iCharaId , int _iAdd , List<MasterCharaParam> _master_chara_list )
	{
		DataUnitParam unit_chara = list.Find(p => p.chara_id == _iCharaId && p.unit == "chara");
		DataUnitParam unit_tension = list.Find(p => p.chara_id == _iCharaId && p.unit == "assist" && p.assist_type == "tension");
		MasterCharaParam master_chara = _master_chara_list.Find(p => p.chara_id == _iCharaId);

		if( unit_chara == null )
		{
			Debug.LogError("unit_chara");
		}
		else if(unit_tension == null)
		{
			Debug.LogError("unit_tension");
		}
		else if(master_chara == null)
		{
			Debug.LogError("master_chara");
		}

		unit_chara.tension += _iAdd;
		if(100 < unit_chara.tension)
		{
			unit_chara.tension = 100;
		}
		else if(unit_chara.tension < 0)
		{
			unit_chara.tension = 0;
		}
		if (unit_tension != null)
		{
			unit_chara.RemoveAssist(unit_tension);
		}
		unit_tension.BuildTension(master_chara, unit_chara.tension);
		unit_chara.BuildAssist(unit_tension);


	}

	public static DataUnitParam MakeUnit( MasterCharaParam _base, string _strPosition, int _iTension )
	{
		DataUnitParam ret = new DataUnitParam();
		ret.chara_id = _base.chara_id;
		ret.unit = _base.unit;

		ret.hp = _base.hp_max;
		ret.hp_max = _base.hp_max;

		ret.tension = _iTension;
		ret.position = _strPosition;

		ret.str = _base.str;
		ret.magic = _base.magic;
		ret.heal = _base.heal;

		return ret;
	}
	public void AddAssist(DataUnitParam _unit , string _strAssistType, string _strAssistName, int _iCharaId, string _strType, int _iParam, int _iTurn)
	{
		DataUnitParam assist = AddAssist(_strAssistType, _strAssistName, _iCharaId, _strType, _iParam, _iTurn);
		_unit.BuildAssist(assist);
	}

	private DataUnitParam AddAssist( string _strAssistType,string _strAssistName , int _iCharaId, string _strType, int _iParam , int _iTurn)
	{
		DataUnitParam add = new DataUnitParam();
		add.chara_id = _iCharaId;
		add.unit = "assist";
		add.assist_name = _strAssistName;
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

		return add;
	}

	public void MakeInitialData(List<MasterCharaParam> _masterList)
	{
		MasterCharaParam left = _masterList.Find(p => p.chara_id == 1);
		MasterCharaParam right = _masterList.Find(p => p.chara_id == 3);
		MasterCharaParam back = _masterList.Find(p => p.chara_id == 2);

		DataUnitParam data_left = left.BuildLevel(1, 60);
		data_left.position = "left";
		DataUnitParam data_right = right.BuildLevel(1, 60);
		data_right.position = "right";
		DataUnitParam data_back = back.BuildLevel(1, 60);
		data_back.position = "back";

		list.Add(data_left);
		list.Add(data_right);
		list.Add(data_back);

	}

}
