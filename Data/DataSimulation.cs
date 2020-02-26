using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSimulationParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int level { get; set; }
	public int count { get; set; }
	public int clear { get; set; }
	public int arrive_boss { get; set; }
	public int clear_play_count { get; set; }
	public int clear_reload_count { get; set; }
	public int wave_1 { get; set; }
	public int wave_2 { get; set; }
	public int wave_3 { get; set; }
	public int wave_4 { get; set; }
	public int wave_5 { get; set; }

}

public class DataSimulation : CsvData< DataSimulationParam >{

}
