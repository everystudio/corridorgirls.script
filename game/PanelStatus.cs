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



	public void Initialize()
	{

		DataUnitParam left = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.status == "left");
		DataUnitParam right = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && p.status == "right");

		area_chara_left.Initialize(left);
		area_chara_right.Initialize(right);


	}


}
