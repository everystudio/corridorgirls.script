using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelDecideCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtMessage;

	public TextMeshProUGUI m_txtLabelDecide;
	public Button m_btnDecide;
	public TextMeshProUGUI m_txtLabelCancel;
	public Button m_btnCancel;

	public TextMeshProUGUI m_txtLabelOther;
	public Button m_btnOther;

	public GameObject m_goRoot;

	public void Set( string[] _message)
	{
		m_btnDecide.gameObject.SetActive(false);
		m_btnCancel.gameObject.SetActive(false);
		m_btnOther.gameObject.SetActive(false);
		for ( int i = 0; i < _message.Length; i++)
		{
			if( i == 0)
			{
				m_btnDecide.gameObject.SetActive(true);
				m_txtLabelDecide.text = _message[i];
			}
			else if( i == 1)
			{
				m_btnOther.gameObject.SetActive(true);
				m_txtLabelOther.text = _message[i];
			}
			else if( i == 2)
			{
				m_btnCancel.gameObject.SetActive(true);
				m_txtLabelCancel.text = _message[i];
			}
		}
	}

	public void SetMessage(string _strMessage)
	{
		m_txtMessage.text = _strMessage;
	}


}
