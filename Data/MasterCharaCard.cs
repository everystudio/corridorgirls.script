using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCharaCardParam : CsvDataParam
{
	public int chara_id { get; set; }
	public int card_id { get; set; }
}

public class MasterCharaCard : CsvData<MasterCharaCardParam> {

}
