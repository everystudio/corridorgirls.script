using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkillParam : CsvDataParam
{
	public int skill_id { get; set; }
	public string name { get; set; }
	public int mp { get; set; }
	public string sprite_name { get; set; }
	public string outline { get; set; }


}

public class MasterSkill : CsvData<MasterSkillParam> {

}
