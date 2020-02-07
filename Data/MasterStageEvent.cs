using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageEventParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int wave { get; set; }

	public int corridor_event_id { get; set; }
	public int prob { get; set; }
}

public class MasterStageEvent : CsvData<MasterStageEventParam> {

	public static MasterStageEventParam GetRand(List<MasterStageEventParam> _event_list)
	{
		int[] prob_list = new int[_event_list.Count];
		for( int i = 0; i < _event_list.Count; i++)
		{
			prob_list[i] = _event_list[i].prob;
		}

		int iIndex = UtilRand.GetIndex(prob_list);
		return _event_list[iIndex];
	}


}
