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

		string strOutline = "";
		if ( _master.situation == "field")
		{
			strOutline = string.Format("{0}\n\n<color=red>このアイテムはフィールドでのみ使用可能です</color>", _master.outline);
		}
		else if (_master.situation == "battle")
		{
			strOutline = string.Format("{0}\n\n<color=red>このアイテムは戦闘中でのみ使用可能です</color>", _master.outline);
		}
		else if (_master.situation == "none")
		{
			strOutline = string.Format("{0}\n\n<color=red>このアイテムはここでは使用できません</color>", _master.outline);
		}
		else
		{
			strOutline = _master.outline;
		}
		m_txtOutline.text = strOutline;

		if (m_master.range == 1)
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
