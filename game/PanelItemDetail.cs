using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelItemDetail : MonoBehaviour {

	private MasterItemParam m_master;

	public Image m_imgBack;
	public PartyHolder m_partyHolder;

	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtOutline;

	public void Initialize(MasterItemParam _master)
	{
		m_master = _master;
		m_txtName.text = _master.name;
		m_txtOutline.text = _master.outline;

		if(m_master.range == 1)
		{
			m_partyHolder.gameObject.SetActive(true);
			MasterCharaParam left = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id
			));
			MasterCharaParam right = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id
			));
			MasterCharaParam back = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "back").chara_id
			));
			m_partyHolder.Initialize(left, right, back);
		}
		else
		{
			m_partyHolder.gameObject.SetActive(false);
		}







	}


}
