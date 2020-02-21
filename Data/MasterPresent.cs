using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterPresentParam : CsvDataParam
{
	public int present_id { get; set; }

}

public class MasterPresent : CsvData<MasterPresentParam> {

}
