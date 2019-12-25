﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MasterItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public string name { get; set; }
	public string outline { get; set; }
	public int type { get; set; }
	public int type_sub { get; set; }
	public int range { get; set; }
	public int turn { get; set; }
	public int param { get; set; }
	public int param2 { get; set; }
	public int param3 { get; set; }
	public string sprite_name { get; set; }

}


public class MasterItem : CsvData<MasterItemParam> {
}