using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMissionDetailParam : CsvDataParam {

	public int mission_id { get; set; }
	public string type { get; set; }
	public string param { get; set; }

}

public class MasterMissionDetail : CsvData<MasterMissionDetailParam>
{

}




