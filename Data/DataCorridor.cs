using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCorridorParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int index { get; set; }

	public MasterCorridorParam master;

	public DataCorridorParam() { }
	public DataCorridorParam(MasterCorridorParam _master)
	{
		stage_id = _master.stage_id;
		index = _master.index;

		master = _master;
	}

}

public class DataCorridor : CsvData<DataCorridorParam> {

	public void Create(List<MasterCorridorParam> _masterList)
	{
		list.Clear();
		foreach (MasterCorridorParam master in _masterList)
		{
			DataCorridorParam param = new DataCorridorParam(master);
			list.Add(param);
		}
	}

}
