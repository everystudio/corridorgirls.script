using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCorridorParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int index { get; set; }

	public MasterCorridorParam master;
	public MasterCorridorEventParam corridor_event;

	public DataCorridorParam() { }
	public DataCorridorParam(MasterCorridorParam _master , MasterCorridorEventParam _corridor_event)
	{
		stage_id = _master.stage_id;
		index = _master.index;

		master = _master;
		corridor_event = _corridor_event;
	}

}

public class DataCorridor : CsvData<DataCorridorParam> {

	public void Create(List<MasterCorridorParam> _masterList)
	{
		list.Clear();
		foreach (MasterCorridorParam master in _masterList)
		{
			MasterCorridorEventParam event_param = DataManager.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id==master.corridor_event_id);
			Debug.Log(master.corridor_event_id);
			Debug.Log(event_param);
			DataCorridorParam param = new DataCorridorParam(master , event_param);
			list.Add(param);
		}
	}

}
