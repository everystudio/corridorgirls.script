using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssistIcon : MonoBehaviour {

	public Image m_imgBack;

	public TextMeshProUGUI m_txtAssistName;
	public TextMeshProUGUI m_txtEffect;
	public TextMeshProUGUI m_txtTurn;

	public void Show( DataUnitParam _unit )
	{
		m_txtAssistName.text = _unit.assist_name;

		if (0 < _unit.str)
		{
			m_txtEffect.text = string.Format("物+{0}", _unit.str);
			m_imgBack.color = Color.red;
		}
		else if (0 < _unit.magic)
		{
			m_txtEffect.text = string.Format("魔+{0}", _unit.magic);
			m_imgBack.color = Color.blue;
		}
		else if (0 < _unit.heal)
		{
			m_txtEffect.text = string.Format("癒+{0}", _unit.heal);
			m_imgBack.color = Color.green;
		}
		else
		{
			m_imgBack.color = Color.white;
		}

		if( _unit.assist_type == "tension")
		{
			string message = "";
			if( 80 <= _unit.tension)
			{
				message = "絶好調";
			}
			else if( 60 <= _unit.tension)
			{
				message = "好調";
			}
			else if( 40 <= _unit.tension)
			{
				message = "普通";
			}
			else if( 20 <= _unit.tension)
			{
				message = "やや不調";
			}
			else
			{
				message = "不調";
			}
			m_txtEffect.text = message;
		}

		if( 100 <= _unit.turn)
		{
			m_txtTurn.text = "";
		}
		else if (0 < _unit.turn)
		{
			m_txtTurn.text = string.Format("あと{0}ターン", _unit.turn);
		}
		else
		{
			m_txtTurn.text = "";
		}



	}
}
