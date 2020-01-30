using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MasterItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public string name { get; set; }
	public string outline { get; set; }
	public string situation { get; set; }
	public string item_type { get; set; }
	public string item_type_sub { get; set; }
	public int range { get; set; }
	public int turn { get; set; }
	public int param { get; set; }
	public int param2 { get; set; }
	public int param3 { get; set; }
	public string sprite_name { get; set; }

	public bool CheckSituation(string _strSituation)
	{
		if (situation == "any")
		{
			return true;
		}
		else
		{
			return situation == _strSituation;
		}
	}

}


public class MasterItem : CsvData<MasterItemParam> {
}
