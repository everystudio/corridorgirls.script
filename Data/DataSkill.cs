using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSkillParam : CsvDataParam
{
	public int skill_id { get; set; }
	public int status { get; set; }

	public DataSkillParam() { }
	public DataSkillParam(int _iSkillId , int _iStatus)
	{
		skill_id = _iSkillId;
		status = _iStatus;
	}
}

public class DataSkill : CsvData<DataSkillParam> {

	public void MakeInitialData()
	{
		list.Clear();
		list.Add(new DataSkillParam(1, 1));	
		list.Add(new DataSkillParam(2, 2));	
		list.Add(new DataSkillParam(3, 3));	
		list.Add(new DataSkillParam(4, 0));	
		list.Add(new DataSkillParam(5, -1));
	}

}
