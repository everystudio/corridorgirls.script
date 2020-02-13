using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelEnemyDeck : MonoBehaviour {

	public GameObject m_prefCard;
	public GameObject m_goContentRoot;

	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtStr;
	public TextMeshProUGUI m_txtMagic;
	public TextMeshProUGUI m_txtHeal;

	public Button m_btnClose;

	public void Show()
	{
		Card[] arr = m_goContentRoot.GetComponentsInChildren<Card>();
		foreach (Card banner in arr)
		{
			Destroy(banner.gameObject);
		}

		List<DataCardParam> card_list = new List<DataCardParam>();
		List<DataUnitParam> unit_param_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "enemy");

		if( unit_param_list.Count != 1)
		{
			Debug.LogError("multi enemy");
		}

		foreach (DataUnitParam unit in unit_param_list)
		{
			MasterCharaParam master_chara = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == unit.chara_id);
			m_txtName.text = master_chara.name;

			m_txtStr.text = unit.str.ToString();
			m_txtMagic.text = unit.magic.ToString();
			m_txtHeal.text = unit.heal.ToString();

			foreach (DataCardParam c in GameMain.Instance.battleMain.dataCardEnemy.list.FindAll(p => p.chara_id == unit.chara_id))
			{
				card_list.Add(c);
			}
		}

		foreach (DataCardParam card_param in card_list)
		{
			Card script = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goContentRoot);
			script.Initialize(card_param, DataManagerGame.Instance.masterCardSymbol.list);
			script.ShowStatus();
		}
		gameObject.SetActive(true);
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
