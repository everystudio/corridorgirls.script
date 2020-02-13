using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelShop : MonoBehaviour {

	[SerializeField]
	private LogMessageLine message;
	public GameObject m_goContentRoot;
	public GameObject m_prefBanner;

	public GameObject m_goPanelBuyCheck;
	public TextMeshProUGUI m_txtBuyCheckMessage;
	public TextMeshProUGUI m_txtBuyCheckGem;

	public BannerCampShop.BannerCampShopEvent OnClickBanner = new BannerCampShop.BannerCampShopEvent();

	public enum LIST_TYPE{
		SHOP_LIST	= 0,
		PURCHASED_LIST,
	}

	public void Select(int _iCampitemId)
	{
		foreach(BannerCampShop banner in banner_list)
		{
			banner.Select(_iCampitemId);
		}
	}
	private List<BannerCampShop> banner_list = new List<BannerCampShop>();
	public void Initialize(LIST_TYPE _eListType)
	{
		List<MasterCampShopParam> disp_list = new List<MasterCampShopParam>();

		if (_eListType == LIST_TYPE.SHOP_LIST)
		{
			foreach (MasterCampShopParam param in DMCamp.Instance.masterCampShop.list)
			{
				if (null == DMCamp.Instance.dataCampItem.list.Find(p => p.campitem_id == param.campitem_id))
				{
					disp_list.Add(param);
				}
			}
		}
		else
		{
			foreach (MasterCampShopParam param in DMCamp.Instance.masterCampShop.list)
			{
				if (null != DMCamp.Instance.dataCampItem.list.Find(p => p.campitem_id == param.campitem_id))
				{
					disp_list.Add(param);
				}
			}
		}

		m_prefBanner.SetActive(false);
		banner_list.Clear();
		BannerCampShop[] arr = m_goContentRoot.GetComponentsInChildren<BannerCampShop>();
		foreach (BannerCampShop c in arr)
		{
			if (m_prefBanner != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}
		foreach (MasterCampShopParam param in disp_list)
		{
			BannerCampShop script = PrefabManager.Instance.MakeScript<BannerCampShop>(m_prefBanner, m_goContentRoot);

			script.Initialize(param, DMCamp.Instance.masterCampItem.list.Find(p => p.campitem_id == param.campitem_id));
			script.OnClickBanner.AddListener((BannerCampShop _script) =>
			{
				OnClickBanner.Invoke(_script);
			});
			banner_list.Add(script);
		}
	}

	public void Message(string _strMessage)
	{
		LogMessageLine.MessageData data = new LogMessageLine.MessageData(_strMessage);
		message.SetMessage(data);
	}

	/*
	void OnEnable()
	{
	}
	*/

}





