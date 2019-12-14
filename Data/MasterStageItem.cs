using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public int stage_id { get; set; }
	public int prob { get; set; }

}


public class MasterStageItem : CsvData<MasterStageItemParam> {

}
