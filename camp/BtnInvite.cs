using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnInvite : BtnCampMenuHandle {

	protected override void _refresh()
	{
		int num = 0;
		foreach (MasterCharaParam master in DMCamp.Instance.masterChara.list.FindAll(p => p.unit == "chara" && 0 < p.scout))
		{
			if (null == DMCamp.Instance.dataUnitCamp.list.Find(p => p.chara_id == master.chara_id))
			{
				num += 1;
			}
		}
		SetBadgeNum(num);
	}

	public void Check()
	{

	}

}
