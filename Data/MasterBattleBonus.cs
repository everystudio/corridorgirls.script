using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBattleBonusParam : CsvDataParam
{
	public int battle_bonus_id { get; set;}
	public string field { get; set; }
	public int param { get; set; }

}

public class MasterBattleBonus : CsvData<MasterBattleBonusParam> {

}
