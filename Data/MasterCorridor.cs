using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCorridorParam :CsvDataParam
{
	public int stage_id { get; set; }
	public int index { get; set; }

	public int x { get; set; }
	public int y { get; set; }

	public int next_index { get; set; }
	public int next_index2 { get; set; }
	public int next_index3 { get; set; }

	public int corridor_event_id { get; set; }

	// ここはデータではなく、起動時に
	public List<int> target_index = new List<int>();

	public MasterCorridorParam() { }
	public MasterCorridorParam( MasterCorridorParam _param)
	{
		stage_id = _param.stage_id;
		index = _param.index;
		x = _param.x;
		y = _param.y;

		next_index = _param.next_index;
		next_index2 = _param.next_index2;
		next_index3 = _param.next_index3;
	}

	public bool IsSingle()
	{
		// next_index != 0はいらないかも
		return next_index != 0 && (next_index2 == 0 && next_index3 == 0);
	}

}

public class MasterCorridor : CsvData<MasterCorridorParam>{


}
