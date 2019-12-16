using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageCardParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int card_type { get; set; }
	public int power { get; set; }
	public int prob { get; set; }
	public int level { get; set; }
}

public class MasterStageCard : CsvData<MasterStageCardParam> {

}
