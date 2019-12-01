using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCardParam : CsvDataParam
{
	public string card_id { get; set; }
	public int card_serial { get; set; }
	public int card_type { get; set; }
	public int rarity { get; set; }

	public int speed { get; set; }
	public int power { get; set; }

	public string status{get;set;}

}

public class DataCard : CsvData< DataCardParam> {

	public enum STATUS{
		NONE		= 0,
		HAND		,
		DECK		,
		MAX			,
	}

}
