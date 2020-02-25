using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCharaDetail : MonoBehaviour {

	public Image m_imgIcon;
	public TextMeshProUGUI m_txtCharaName;
	public TextMeshProUGUI m_txtHP;
	public TextMeshProUGUI m_txtSTR;
	public TextMeshProUGUI m_txtMAG;
	public TextMeshProUGUI m_txtHEAL;
	public TextMeshProUGUI m_txtFood;

	public EnergyBar m_barTension;

	public GameObject m_prefCard;
	public GameObject m_goCardRoot;

	public void ShowScout(MasterCharaParam _masterChara , List<MasterCardParam> _master_card_list, List<MasterCharaCardParam> _chara_card_list, List<MasterCardSymbolParam> _symbol_list)
	{
		m_imgIcon.sprite = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACEICON, _masterChara.chara_id));
		m_txtCharaName.text = _masterChara.name;
		m_txtHP.text = _masterChara.hp_max.ToString();
		m_txtSTR.text = _masterChara.str.ToString();
		m_txtMAG.text = _masterChara.magic.ToString();
		m_txtHEAL.text = _masterChara.heal.ToString();
		m_txtFood.text = _masterChara.food.ToString();

		m_barTension.SetValueCurrent(100);

		Card[] arr = m_goCardRoot.GetComponentsInChildren<Card>();
		foreach (Card c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}
		foreach (MasterCharaCardParam p in _chara_card_list.FindAll(p => p.chara_id == _masterChara.chara_id))
		{
			//Debug.Log(p.card_id);
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);

			DataCardParam data_card = new DataCardParam();
			//data_card.chara_id = _masterChara.chara_id;
			//data_card.card_id = p.card_id;

			// 表示用なのでシリアル不要
			data_card.Copy(_master_card_list.Find(a => a.card_id == p.card_id) , _masterChara.chara_id , 0 );

			c.Initialize(data_card, _symbol_list);
		}

	}

	public void Show(DataUnitParam _dataChara ,  MasterCharaParam _masterChara , List<MasterCardParam> _master_card_list , List<MasterCharaCardParam> _chara_card_list , List<MasterCardSymbolParam> _symbol_list)
	{
		m_imgIcon.sprite = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACEICON, _masterChara.chara_id));
		m_txtCharaName.text = _masterChara.name;
		m_txtHP.text = _dataChara.hp_max.ToString();
		m_txtSTR.text = _dataChara.str.ToString();
		m_txtMAG.text = _dataChara.magic.ToString();
		m_txtHEAL.text = _dataChara.heal.ToString();
		m_txtFood.text = _dataChara.food.ToString();

		m_barTension.SetValueCurrent(_dataChara.tension);

		Card[] arr = m_goCardRoot.GetComponentsInChildren<Card>();
		foreach (Card c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}

		foreach ( MasterCharaCardParam p in _chara_card_list.FindAll(p=>p.chara_id == _masterChara.chara_id))
		{
			//Debug.Log(p.card_id);
			Card c = PrefabManager.Instance.MakeScript<Card>(m_prefCard, m_goCardRoot);

			DataCardParam data_card = new DataCardParam();

			data_card.Copy(_master_card_list.Find(a => a.card_id == p.card_id), _masterChara.chara_id, 0);
			c.Initialize(data_card, _symbol_list);
		}
	}




}
