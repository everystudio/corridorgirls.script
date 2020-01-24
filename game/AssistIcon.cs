using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssistIcon : MonoBehaviour {

	public Image m_imgBack;
	public TextMeshProUGUI m_txtMessage;

	public void Show( string _strMessage , Color _color )
	{
		m_txtMessage.text = _strMessage;
		m_imgBack.color = _color;
	}
}
