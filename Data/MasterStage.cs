﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageParam : CsvDataParam
{
	public int stage_id { get; set; }
	public string stage_name { get; set; }

	public string outline { get; set; }

	public int length { get; set; }
	public int total_wave { get; set; }


}

public class MasterStage : CsvData<MasterStageParam> {

}
