using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public int serial { get; set; }

	public MasterItemParam master;
}

public class DataItem : CsvData<DataItemParam> {

}
