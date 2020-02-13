using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCampItemParam : CsvDataParam
{
	public int campitem_id { get; set; }
	public bool is_take { get; set; }
}

public class DataCampItem : CsvData<DataCampItemParam> {

}
