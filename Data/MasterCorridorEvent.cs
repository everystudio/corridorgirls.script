using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCorridorEventParam : CsvDataParam
{
	public int corridor_event_id { get; set; }
	public string corridor_type { get; set; }
	public int sub_type { get; set; }
	public string label { get; set; }
	public int param { get; set; }
	public string sprite_name { get; set; }
	public int color_r { get; set; }
	public int color_g { get; set; }
	public int color_b { get; set; }
	public int key_item_id { get; set; }
	public string outline { get; set; }
	public string comment { get; set; }

}

public class MasterCorridorEvent : CsvData<MasterCorridorEventParam> {

	public enum CORRIDOR_EVENT_TYPE
	{
		NONE		= 0,
		GOLD		,
		CARD		,
		ITEM		,
		MISSION		,
		HEAL		,
		SHOP		,
		BATTLE		,
		BOSS		,
		DOOR		,
		GOAL		,
		MAX			,
	}



}
