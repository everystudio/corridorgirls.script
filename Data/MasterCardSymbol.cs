using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCardSymbolParam : CsvDataParam
{
	public int card_symbol_id { get; set; }
	public string name { get; set; }
	public string sprite_name { get; set; }
	public int line { get; set; }
}

public class MasterCardSymbol : CsvData<MasterCardSymbolParam> {

}
