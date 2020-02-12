using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStageParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int clear_count { get; set; }
	public int challange_count { get; set; }
	public int best_reload { get; set; }
	public int best_play { get; set; }

}

public class DataStage : CsvData<DataStageParam> {

}
