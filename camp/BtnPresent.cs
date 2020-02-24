using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnPresent : BtnCampMenuHandle
{

	protected override void _refresh()
	{
		int num = DMCamp.Instance.dataPresent.list.FindAll(p => p.recieved == false).Count;
		SetBadgeNum(num);
	}

}
