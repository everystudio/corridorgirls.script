using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSkillParam : CsvDataParam
{
	public int skill_id { get; set; }
	public int status { get; set; }
}

public class DataSkill : CsvData<DataSkillParam> {

}
