using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCampItemParam : CsvDataParam
{
	public int campitem_id { get; set; }
	public string name { get; set; }
	public int mana { get; set; }
	public int item_id { get; set; }
}

public class MasterCampItem : CsvData<MasterCampItemParam> {

}
