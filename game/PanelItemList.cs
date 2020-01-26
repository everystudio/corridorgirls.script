using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelItemList : MonoBehaviour {
	public UnityEventInt OnSelectItem = new UnityEventInt();

	public GameObject m_prefBannerItem;
	public GameObject m_goContentRoot;

	public void Show( List<DataItemParam> _data_list , List<MasterItemParam> _master_list)
	{
		m_prefBannerItem.SetActive(false);
		BannerItem[] arr = m_goContentRoot.GetComponentsInChildren<BannerItem>();
		foreach (BannerItem c in arr)
		{
			if (m_prefBannerItem != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}

		foreach( DataItemParam data in _data_list)
		{
			BannerItem banner = PrefabManager.Instance.MakeScript<BannerItem>(m_prefBannerItem, m_goContentRoot);

			banner.Initialize(data, _master_list.Find(p => p.item_id == data.item_id));

			banner.OnClickBanner.AddListener((int _iSerial) =>
			{
				OnSelectItem.Invoke(_iSerial);
			});
		}


	}

}
