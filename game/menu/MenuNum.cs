using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuNum : MonoBehaviour {

	public TextMeshProUGUI m_txtTitle;
	public TextMeshProUGUI m_txtNum;
	
	public void Initialize(string _strTitle, string _strParam )
	{
		m_txtTitle.text = _strTitle;
		m_txtNum.text = _strParam;
	}

}
