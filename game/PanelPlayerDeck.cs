using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelPlayerDeck : MonoBehaviour {

	public GameObject m_prefCard;
	public GameObject m_goContentRoot;

	public Button m_btnClose;

	public void show2(bool _bShowStatus , List<DataUnitParam> _unit_list, List<DataCardParam> _data_card_list, List<MasterCardParam> _master_card_list, List<MasterCardSymbolParam> _master_card_symbol_list)
	{
		Card[] arr = m_goContentRoot.GetComponentsInChildren<Card>();
		foreach (Card banner in arr)
		{
			Destroy(banner.gameObject);
		}
		List<DataCardParam> show_card_list = new List<DataCardParam>();
		foreach (DataUnitParam unit in _unit_list.FindAll(p=>p.unit == "chara"))
		{
			foreach (DataCardParam c in _data_card_list.FindAll(p => p.chara_id == unit.chara_id))
			{
				show_card_list.Add(c);
			}
		}
		foreach (DataCardParam card_param in show_card_list)
		{
			Card script = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goContentRoot);

			// 事前にセット
			card_param.master = _master_card_list.Find(p => p.card_id == card_param.card_id);
			//Debug.Log(card_param.master);
			script.Initialize(card_param, _master_card_symbol_list);
			if (_bShowStatus)
			{
				script.ShowStatus();
			}
		}
		gameObject.SetActive(true);

	}


	public void show2( List<DataUnitParam> _unit_list , List<DataCardParam> _data_card_list , List<MasterCardParam> _master_card_list, List<MasterCardSymbolParam> _master_card_symbol_list)
	{
		show2(true, _unit_list, _data_card_list, _master_card_list, _master_card_symbol_list);
	}

	public void Show()
	{
		show2(DataManagerGame.Instance.dataUnit.list, DataManagerGame.Instance.dataCard.list, DataManagerGame.Instance.masterCard.list, DataManagerGame.Instance.masterCardSymbol.list);
	}
	public void ShowCamp()
	{
		show2(false ,DMCamp.Instance.dataUnitCamp.list, DMCamp.Instance.dataCard.list, DMCamp.Instance.masterCard.list, DMCamp.Instance.masterCardSymbol.list);
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}
