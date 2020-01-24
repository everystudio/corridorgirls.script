using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGameStatus : MonoBehaviour {


	public GameCharaStatus chara_left;
	public GameCharaStatus chara_right;
	public GameCharaStatus chara_back1;
	public GameCharaStatus chara_back2;

	public void Show(List<DataUnitParam> _unit_list)
	{
		// 一旦

		DataUnitParam left = _unit_list.Find(p => p.unit == "chara" && p.position == "left");
		DataUnitParam right = _unit_list.Find(p => p.unit == "chara" && p.position == "left");
		List<DataUnitParam> back_list = _unit_list.FindAll(p => p.unit == "chara" && p.position == "back");








	}


}
