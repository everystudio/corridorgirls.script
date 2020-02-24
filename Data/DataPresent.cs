using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPresentParam : CsvDataParam
{
	public int serial { get; set; }
	public bool recieved { get; set; }
	public string get_date { get; set; }
	public string recieved_date { get; set; }

	public string outline { get; set; }
	public int item_id { get; set; }
	public int num { get; set; }


}

public class DataPresent : CsvData<DataPresentParam> {

	public void Add( int _item_id , int _num , string _outline)
	{
		int iSerial = 1;
		foreach( DataPresentParam d in list)
		{
			if(iSerial <= d.serial )
			{
				iSerial = d.serial + 1;
			}
		}

		DataPresentParam add = new DataPresentParam();

		add.recieved = false;
		add.serial = iSerial;
		add.item_id = _item_id;
		add.num = _num;
		add.outline = _outline;

		add.get_date = NTPTimer.Instance.now.ToString(Defines.FORMAT_STANDARD_DATE);

		list.Add(add);
	}

}
