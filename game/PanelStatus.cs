using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStatus : MonoBehaviour {

	public AreaChara area_chara_left;
	public AreaChara area_chara_right;

	public void Initialize()
	{

		DataUnitParam left = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.status == "left");
		DataUnitParam right = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.status == "right");

		area_chara_left.Initialize(left);
		area_chara_right.Initialize(right);


	}


}
