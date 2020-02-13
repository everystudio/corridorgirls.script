using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCampShopParam : CsvDataParam
{
	public int campitem_id { get; set; }
	public int gem { get; set; }

}

public class MasterCampShop : CsvData<MasterCampShopParam> {

}
