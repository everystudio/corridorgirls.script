using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLevelupParam : CsvDataParam
{
	public int level { get; set; }
	public int mana { get; set; }
}

public class MasterLevelup : CsvData<MasterLevelupParam> {

}
