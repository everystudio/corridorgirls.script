using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

abstract public class BtnCampMenuHandle : MonoBehaviour {
	public Button m_btn;
	public GameObject m_goBadge;
	public TextMeshProUGUI m_txtBadgeNum;

	public void SetBadgeNum(int _iNum)
	{
		m_goBadge.SetActive(0 < _iNum);
		m_txtBadgeNum.text = _iNum < 1000 ? _iNum.ToString() : "沢山";
	}

	public void Refresh()
	{
		_refresh();
	}

	protected abstract void _refresh();

}
