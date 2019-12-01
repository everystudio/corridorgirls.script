using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCardParam : CsvDataParam
{
	public string card_id { get; set; }

	public string card_type { get; set; }
	public int rarity { get; set; }

}

public class MasterCard : CsvData<MasterCardParam> {

}
