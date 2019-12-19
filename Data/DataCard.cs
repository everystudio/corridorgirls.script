using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventDataCardParam : UnityEvent<DataCardParam>
{
}


public class DataCardParam : CsvDataParam
{
	public int card_id { get; set; }
	public int card_serial { get; set; }
	public string status{get;set;}

	public int chara_id { get; set; }

	public MasterCardParam master {
		get
		{
			if(master_card_param == null)
			{
				master_card_param = DataManager.Instance.masterCard.list.Find(p => p.card_id == card_id);
			}
			return master_card_param;
		}
		set
		{
			master_card_param = value;
		}
	}
	private MasterCardParam master_card_param;
}

public class DataCard : CsvData< DataCardParam> {

	public enum STATUS{
		NONE		= 0,
		HAND		,
		DECK		,
		MAX			,
	}


}
