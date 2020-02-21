using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageShopItemParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int wave { get; set; }
	public int item_id { get; set; }
	public int prob { get; set; }

}

public class MasterStageShopItem : CsvData<MasterStageShopItemParam> {

}
