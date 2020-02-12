using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageParam : CsvDataParam
{
	public int stage_id { get; set; }
	public string stage_name { get; set; }

	public string outline { get; set; }
	public int total_wave { get; set; }
	public int total_length { get; set; }

	public string thumb { get; set; }
	public string bgname { get; set; }

	public int boss_chara_id { get; set; }
	public int food_rate { get; set; }

	public int challenge_reload { get; set; }
	public int challenge_play { get; set; }

}

public class MasterStage : CsvData<MasterStageParam> {

}
