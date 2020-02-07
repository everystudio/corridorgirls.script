using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageWaveParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int wave { get; set; }

	public int length { get; set; }

}

public class MasterStageWave : CsvData<MasterStageWaveParam> {

}
