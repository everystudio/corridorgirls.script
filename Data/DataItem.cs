using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public int serial { get; set; }
	public int status { get; set; }

	public MasterItemParam master;
}

public class DataItem : CsvData<DataItemParam> {
	public enum STATUS
	{
		NONE	=0,
		STANDBY	,
		USING	,
		REMOVE	,
		MAX
	}

	public bool HasItem(int _iItemId)
	{
		DataItemParam item = list.Find(p => p.item_id == _iItemId && p.status == (int)STATUS.STANDBY);
		return item != null;
	}

	public void AddItem( MasterItemParam _master)
	{
		DataItemParam add = new DataItemParam();
		add.item_id = _master.item_id;

		int serial = 1;

		foreach( DataItemParam data in list)
		{
			if( serial <= data.serial)
			{
				serial = data.serial + 1;
			}
		}
		add.serial = serial;
		add.status = (int)STATUS.STANDBY;
		list.Add(add);
	}


}
