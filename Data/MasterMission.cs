using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMissionParam : CsvDataParam
{
	public int mission_id { get; set; }
	public string title { get; set; }
	public string type { get; set; }

	public int prob_success { get; set; }
	public int prob_fail { get; set; }
	public int item_id { get; set; }

	public string btnlabel_yes { get; set; }
	public string btnlabel_no { get; set; }

	public bool IsSuccess()
	{
		bool bRet = false;

		int[] prob_arr = new int[]
		{
			prob_success,
			prob_fail
		};

		if( UtilRand.GetIndex(prob_arr) == 0)
		{
			bRet = true;
		}
		return bRet;
	}

}

public class MasterMission : CsvData<MasterMissionParam> {

}
