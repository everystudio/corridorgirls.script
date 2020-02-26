using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Corridor : MonoBehaviour {

	public DataCorridorParam data_param;
	public TextMeshPro m_txtLabel;
	public SpriteRenderer m_sprIcon;

	public void Initialize(DataCorridorParam _data)
	{
		data_param = _data;

		if (data_param.master != null)
		{
			transform.localPosition = new Vector3(data_param.master.x, data_param.master.y, 0.0f);
		}
		else
		{
			transform.localPosition = new Vector3(data_param.x, data_param.y, 0.0f);

		}
		//Debug.Log(_data);
		//Debug.Log(_data.corridor_event);

		if(_data.corridor_event == null)
		{
			_data.corridor_event = DataManagerGame.Instance.masterCorridorEvent.list.Find(p => p.corridor_event_id == _data.corridor_event_id);
		}

		m_txtLabel.text = _data.corridor_label;
		//Debug.Log(_data.corridor_event.sprite_name);
		if (_data.corridor_event.sprite_name != "")
		{
			m_sprIcon.sprite = SpriteManager.Instance.Get(_data.corridor_event.sprite_name);
			m_sprIcon.color = new Color(
				_data.corridor_event.color_r / 255.0f, 
				_data.corridor_event.color_g / 255.0f, 
				_data.corridor_event.color_b / 255.0f);
		}
		else
		{
			m_sprIcon.enabled = false;
		}



	}

	



}
