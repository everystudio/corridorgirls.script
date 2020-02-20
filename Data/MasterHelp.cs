using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterHelpParam : CsvDataParam
{
	public int help_id { get; set; }
	public string title { get; set; }
	public string message { get; set; }
	public string image_name { get; set; }

}

public class MasterHelp : CsvData<MasterHelpParam> {

}
