using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillEffectParam : CsvDataParam
{
	public int skill_id { get; set; }
	public int order { get; set; }
	public int turn { get; set; }
	public string field { get; set; }
	public string calc { get; set; }
	public int param { get; set; }

}

public class MasterSkillEffect : CsvData<MasterSkillEffectParam> {
}
