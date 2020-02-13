using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BannerCampItem : MonoBehaviour {
	public Button m_btn;
	public Image m_imgBg;

	public TextMeshProUGUI m_txtCampitemName;
	public TextMeshProUGUI m_txtItemName;
	public TextMeshProUGUI m_txtNeedMana;

	public DataCampItemParam m_data;
	public MasterCampItemParam m_master;
	public MasterItemParam m_masterItem;

	public class ClickBannerEvent : UnityEvent<BannerCampItem>
	{
	}
	public ClickBannerEvent OnClickBanner = new ClickBannerEvent();

	public void Initialize( DataCampItemParam _data , MasterCampItemParam _master , MasterItemParam _master_item)
	{
		m_data = _data;
		m_master = _master;
		m_masterItem = _master_item;

		m_txtCampitemName.text = m_master.name;
		m_txtItemName.text = m_masterItem.name;
		m_txtNeedMana.text = string.Format("必要マナ{0}", m_master.mana);

		m_btn.onClick.AddListener(() =>
		{
			_data.is_take = !_data.is_take;
			_select(_data.is_take);
			OnClickBanner.Invoke(this);
		});
		_select(_data.is_take);

	}

	private void _select(bool _bIsTake)
	{
		m_imgBg.color = _bIsTake ? Color.red : Color.white;
	}

}
