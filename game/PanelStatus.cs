using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStatus : MonoBehaviour {

	public AreaChara area_chara_left;
	public AreaChara area_chara_right;

	public Button m_btnStatus;
	public Button m_btnItem;
	public Button m_btnDeck;



	/*
	public void Initialize()
	{
		DataUnitParam left = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.position == "left");
		DataUnitParam right = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.position == "right");
		area_chara_left.Initialize(left);
		area_chara_right.Initialize(right);
	}
	*/

	public void Initialize(DataUnit _dataUnit , MasterChara _masterChara)
	{

		DataUnitParam left = _dataUnit.list.Find(p=>p.unit == "chara" && p.position == "left");
		DataUnitParam right = _dataUnit.list.Find(p => p.unit == "chara" && p.position == "right");
		DataUnitParam back = _dataUnit.list.Find(p => p.unit == "chara" && p.position == "back");

		area_chara_left.Initialize(left, _masterChara.list.Find(p => p.chara_id == left.chara_id));
		area_chara_right.Initialize(right, _masterChara.list.Find(p => p.chara_id == right.chara_id));

	}


}
