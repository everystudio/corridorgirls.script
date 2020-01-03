using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DataUnitParam : CsvDataParam
{
	public int chara_id { get; set; }
	public string unit { get; set; }
	public string status { get; set; }
	public int hp { get; set; }
	public int hp_max { get; set; }
	public int str { get; set; }
	public int wis { get; set; }
	public int heal { get; set; }

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

}
