using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStageEnemyParam : CsvDataParam
{
	public int stage_id { get; set; }
	public int enemy_id { get; set; }//chara_id
	public int prob { get; set; }
	public int level { get; set; }
}

public class MasterStageEnemy : CsvData<MasterStageEnemyParam> {

}
