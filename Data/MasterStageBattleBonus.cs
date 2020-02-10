using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageBattleBonusParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int wave { get; set; }

	public int battle_bonus_id { get; set; }
	public int prob { get; set; }
}

public class MasterStageBattleBonus : CsvData<MasterStageBattleBonusParam> {
	
}
