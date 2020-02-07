using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageMissionParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int wave { get; set; }

	public int mission_id { get; set; }
	public int prob { get; set; }
	public int level { get; set; }
}

public class MasterStageMission : CsvData<MasterStageMissionParam> {

	public MasterStageMissionParam Select(int _iStageId , int _iWave)
	{
		List<MasterStageMissionParam> stage_mission_list = list.FindAll(p => p.stage_id == _iStageId && p.wave == _iWave);

		if(stage_mission_list.Count == 0)
		{
			Debug.Log("stage_mission_list.Count == 0");
			stage_mission_list = list.FindAll(p => p.stage_id == _iStageId && p.wave == 0);
		}

		Debug.Log(_iStageId);
		Debug.Log(_iWave);
		Debug.Log(stage_mission_list.Count);

		int[] prob_arr = new int[stage_mission_list.Count];
		for( int i = 0; i < prob_arr.Length; i++)
		{
			prob_arr[i] = stage_mission_list[i].prob;
		}

		int iSelectIndex = UtilRand.GetIndex(prob_arr);

		return stage_mission_list[iSelectIndex];
	}

}
