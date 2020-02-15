using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCharaLevelup : MonoBehaviour {

	public BannerCharaStatus now;
	public BannerCharaStatus levelup;

	public TextMeshProUGUI m_txtNeedMana;

	public Button m_btnPlus;
	public Button m_btnMinus;

	public int m_iUpLevel;

	public void Initialize( DataUnitParam _now , MasterCharaParam _master)
	{
		gameObject.SetActive(true);
		now.Initialize(_now, _master);
	}
	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void SetTargetLevel( int _iLevelupNum , DataUnitParam _now, MasterCharaParam _master)
	{
		m_iUpLevel = _iLevelupNum;
		levelup.Initialize(_master.BuildLevel(_now.level + m_iUpLevel, _now.tension), _master);
	}

}
