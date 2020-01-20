using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCharaParam : CsvDataParam
{
	public int chara_id { get; set; }
	public string unit { get; set; }
	public string name { get; set; }

	public int hp_max { get; set; }
	public int str { get; set; }
	public int magic { get; set; }
	public int heal { get; set; }
	public int luck { get; set; }
}

public class MasterChara : CsvData<MasterCharaParam> {

}
