using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDungeonStepParam : CsvDataParam
{
	public int pos { get; set; }
	public int dungeon_step_id { get; set; }
}

public class DataDungeonStep : CsvData<DataDungeonStepParam> {

}
