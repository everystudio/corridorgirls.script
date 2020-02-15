using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCharaParam : CsvDataParam
{
	public int chara_id { get; set; }
	public string unit { get; set; }
	public string name { get; set; }

	public int hp_max { get; set; }
	public int str { get; set; }
	public int magic { get; set; }
	public int heal { get; set; }
	public int luck { get; set; }

	public int food { get; set; }

	public string texture_name { get; set; }
	public string sprite_name { get; set; }

	private const float LEVELUP_PARAM_RATE = 0.05f;

	public DataUnitParam BuildLevel(DataUnitParam _unit , int _iLevel, int _iTension)
	{
		_unit.level = _iLevel;
		_unit.tension = _iTension;
		/*
		Debug.Log(string.Format("request level:{0}", _iLevel));
		for( int i = 1; i < 30; i++)
		{
			Debug.Log(string.Format("level:{0} hp_max_add={1} str={2} magic={3}", i,
				(int)(hp_max * LEVELUP_PARAM_RATE * (i - 1)),
				(int)(str * LEVELUP_PARAM_RATE * (i - 1)),
				(int)(magic * LEVELUP_PARAM_RATE * (i - 1))
				));
		}
		*/
		_unit.hp_max = hp_max + (int)(hp_max * LEVELUP_PARAM_RATE * (_iLevel));
		//Debug.Log(ret.hp_max);
		_unit.hp = _unit.hp_max;

		_unit.str = str + (int)(str * LEVELUP_PARAM_RATE * (_iLevel));
		_unit.magic = magic + (int)(magic * LEVELUP_PARAM_RATE * (_iLevel));
		_unit.heal = heal + (int)(heal * LEVELUP_PARAM_RATE * (_iLevel));
		_unit.luck = luck + (int)(luck * LEVELUP_PARAM_RATE * (_iLevel));

		_unit.food = food + (int)(food * LEVELUP_PARAM_RATE * (_iLevel));

		return _unit;
	}

	public DataUnitParam BuildLevel(int _iLevel , int _iTension)
	{
		DataUnitParam ret = new DataUnitParam();
		ret.chara_id = chara_id;
		ret.unit = "chara";

		BuildLevel(ret, _iLevel, _iTension);

		return ret;

	}





}

public class MasterChara : CsvData<MasterCharaParam> {

}
