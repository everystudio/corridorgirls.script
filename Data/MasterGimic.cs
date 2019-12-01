using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterGimicParam : CsvDataParam
{
	public int gimic_id { get; set; }
	public string type { get; set; }
	public string name { get; set; }
	public string text { get; set; }
	public string image_name { get; set; }
}

public class MasterGimic : CsvData<MasterGimicParam> {

	public MasterGimicParam Get(int _iGimicId)
	{
		foreach(MasterGimicParam param in list)
		{
			if( param.gimic_id == _iGimicId)
			{
				return param;
			}
		}
		return null;
	}
}
