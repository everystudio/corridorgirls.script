using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageControl : Singleton<MessageControl> {

	public Animator m_animator;
	public Button btn;
	public TextMeshProUGUI m_txtMessage;


	public void Show(string _strMessage)
	{
		m_animator.SetTrigger("show");
		m_txtMessage.text = _strMessage;
	}



}
