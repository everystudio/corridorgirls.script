using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Corridor : MonoBehaviour {

	public DataCorridorParam data_param;
	public TextMeshPro m_txtLabel;

	public void Initialize(DataCorridorParam _data)
	{
		data_param = _data;

		transform.localPosition = new Vector3(data_param.master.x, data_param.master.y, 0.0f);

		m_txtLabel.text = "";



	}

	



}
