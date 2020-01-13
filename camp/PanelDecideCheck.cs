using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelDecideCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtMessage;

	public Button m_btnDecide;
	public Button m_btnCancel;

	public GameObject m_goRoot;

	public void SetMessage(string _strMessage)
	{
		m_txtMessage.text = _strMessage;
	}


}
