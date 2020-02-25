using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCorridorParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int index { get; set; }

	#region 自動生成プログラム用に追加
	public int type { get; set; }

	public int x { get; set; }
	public int y { get; set; }

	public int next_index { get; set; }
	public int next_index2 { get; set; }
	public int next_index3 { get; set; }
	#endregion

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


	public bool IsSingle()
	{
		// next_index != 0はいらないかも
		if( master != null)
		{
			return master.IsSingle();
		}

		return next_index != 0 && (next_index2 == 0 && next_index3 == 0);
	}



}

public class DataCorridor : CsvData<DataCorridorParam> {

	public void Create(List<MasterCorridorParam> _masterList)
	{
		list.Clear();
		foreach (MasterCorridorParam master in _masterList)
		{
			MasterCorridorEventParam event_param = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id==master.corridor_event_id);
			//Debug.Log(master.corridor_event_id);
			//Debug.Log(event_param);
			DataCorridorParam param = new DataCorridorParam(master , event_param);
			list.Add(param);
		}
	}

	public void BuildDungeon( MasterStageParam _master , int _iWave)
	{
		list.Clear();

		DataCorridorParam last = null;
		List<MasterStageEventParam> event_list = DataManagerGame.Instance.masterStageEvent.list.FindAll(p => p.stage_id == _master.stage_id && p.wave == _iWave);
		MasterStageWaveParam stage_wave = DataManagerGame.Instance.masterStageWave.list.Find(p => p.stage_id == _master.stage_id && p.wave == _iWave);

		// １からはじめたいため、ループがいつもと少し違う
		for ( int i = 1; i <= stage_wave.length; i++)
		{
			DataCorridorParam cor = new DataCorridorParam();

			cor.index = i;
			cor.x = i*2;
			cor.y = 0;
			cor.next_index = i + 1;

			cor.corridor_event = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id == 0); ;
			if( i == 1)
			{
				cor.corridor_event = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id == 1);
			}
			else
			{
				MasterStageEventParam stage_event = MasterStageEvent.GetRand(event_list);
				cor.corridor_event = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id == stage_event.corridor_event_id);
			}
			list.Add(cor);
			last = cor;
		}
		/*
		#region 分布チェック用
		int[] log_event = new int[10];
		for ( int i = 0; i < 10000; i++)
		{
			MasterStageEventParam stage_event = MasterStageEvent.GetRand(event_list);

			if (stage_event.corridor_event_id== 1001)
			{
				log_event[0] += 1;
			}
			else if (stage_event.corridor_event_id == 2001)
			{
				log_event[1] += 1;
			}
			else if (stage_event.corridor_event_id == 4101)
			{
				log_event[2] += 1;
			}
			else if (stage_event.corridor_event_id == 6101)
			{
				log_event[3] += 1;
			}
			else if (stage_event.corridor_event_id == 7001)
			{
				log_event[4] += 1;
			}
		}
		for( int i = 0; i < 5; i++)
		{
			Debug.Log(log_event[i]);
		}
		#endregion
		*/

		// ゴール


		last.corridor_event = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id == 2);
		if(_master.total_wave == _iWave)
		{
			last.corridor_event.label = "BOSS";
		}
		else
		{
			last.corridor_event.label = "NEXT";
		}
	}



	public void BuildDungeonSim(
		MasterStageParam _master,
		int _iWave ,
		MasterStageEvent _masterStageEvent ,
		MasterStageWave _masterStageWave ,
		MasterCorridorEvent _masterCorridorEvent)
	{
		list.Clear();

		DataCorridorParam last = null;
		List<MasterStageEventParam> event_list = _masterStageEvent.list.FindAll(p => p.stage_id == _master.stage_id && p.wave == _iWave);
		MasterStageWaveParam stage_wave = _masterStageWave.list.Find(p => p.stage_id == _master.stage_id && p.wave == _iWave);

		// １からはじめたいため、ループがいつもと少し違う
		for (int i = 1; i <= stage_wave.length; i++)
		{
			DataCorridorParam cor = new DataCorridorParam();

			cor.index = i;
			cor.x = i * 2;
			cor.y = 0;
			cor.next_index = i + 1;

			cor.corridor_event = _masterCorridorEvent.list.Find(p => p.corridor_event_id == 0); ;
			if (i == 1)
			{
				cor.corridor_event = _masterCorridorEvent.list.Find(p => p.corridor_event_id == 1);
			}
			else
			{
				MasterStageEventParam stage_event = MasterStageEvent.GetRand(event_list);
				cor.corridor_event = _masterCorridorEvent.list.Find(p => p.corridor_event_id == stage_event.corridor_event_id);
			}
			list.Add(cor);
			last = cor;
		}

		last.corridor_event = _masterCorridorEvent.list.Find(p => p.corridor_event_id == 2);
		if (_master.total_wave == _iWave)
		{
			last.corridor_event.label = "BOSS";
		}
		else
		{
			last.corridor_event.label = "NEXT";
		}
	}




}
