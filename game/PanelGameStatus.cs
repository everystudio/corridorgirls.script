﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGameStatus : MonoBehaviour {


	public GameCharaStatus chara_left;
	public GameCharaStatus chara_right;
	public GameCharaStatus chara_back1;
	public GameCharaStatus chara_back2;

	public void Show(List<DataUnitParam> _unit_list , List<MasterCharaParam> _master_chara_list )
	{
		// 一旦

		DataUnitParam left = _unit_list.Find(p => p.unit == "chara" && p.position == "left");
		DataUnitParam right = _unit_list.Find(p => p.unit == "chara" && p.position == "right");
		List<DataUnitParam> back_list = _unit_list.FindAll(p => p.unit == "chara" && p.position == "back");

		chara_left.Initialize(left, _master_chara_list.Find(p=>p.chara_id == left.chara_id) ,  _unit_list);
		chara_right.Initialize(right, _master_chara_list.Find(p => p.chara_id == right.chara_id), _unit_list);


		chara_back1.Initialize(back_list[0], _master_chara_list.Find(p => p.chara_id == back_list[0].chara_id), _unit_list);

		// ここプログラムとしてはおかしい
		if( 2 <= back_list.Count)
		{
			chara_back2.Initialize(back_list[1], _master_chara_list.Find(p => p.chara_id == back_list[1].chara_id), _unit_list);
		}
		else
		{
			chara_back2.gameObject.SetActive(false);
		}






	}


}
