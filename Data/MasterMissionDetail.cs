using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMissionDetailParam : CsvDataParam {

	public int mission_id { get; set; }
	public string type { get; set; }
	public string message { get; set; }

	public string prize_type { get; set; }
	public string prize_type_sub { get; set; }
	public int param { get; set; }
}

public class MasterMissionDetail : CsvData<MasterMissionDetailParam>
{
	protected override void loadedAction()
	{
		base.loadedAction();


		foreach( MasterMissionDetailParam detail in list)
		{
			string[] divStringArr = detail.message.Split(new string[] { "\\n" }, System.StringSplitOptions.None);
			detail.message = "";
			for (int i = 0; i < divStringArr.Length; i++)
			{
				detail.message += divStringArr[i];
				if (i < (divStringArr.Length))
				{
					detail.message += "\n";
				}
			}
			Debug.Log(detail.message);
		}
	}

}




