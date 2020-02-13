using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BannerCampShop : MonoBehaviour {

	public TextMeshProUGUI m_txtItemName;
	public TextMeshProUGUI m_txtMana;			// ジェム

	public Button m_btn;
	public Image m_imgBack;

	public MasterCampShopParam m_masterCampShop;
	public MasterCampItemParam m_masterCampItem;

	public class BannerCampShopEvent : UnityEvent<BannerCampShop>
	{
	}
	public BannerCampShopEvent OnClickBanner = new BannerCampShopEvent();

	public void Initialize( MasterCampShopParam _master_shop , MasterCampItemParam _master_campitem)
	{
		m_masterCampShop = _master_shop;
		m_masterCampItem = _master_campitem;

		m_txtMana.text = string.Format("購入：{0}ジェム", _master_shop.gem);
		m_txtItemName.text = _master_campitem.name;

		m_btn.onClick.RemoveAllListeners();
		m_btn.onClick.AddListener(() =>
		{
			OnClickBanner.Invoke(this);
		});
	}

	public void Select(int _iCampitemId)
	{
		m_imgBack.color = m_masterCampItem.campitem_id == _iCampitemId ? Color.red : Color.white;
	}



}
