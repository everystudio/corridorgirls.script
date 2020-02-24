using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPresent : MonoBehaviour {

	public GameObject m_prefBanner;
	public GameObject m_goContents;

	public BannerPresent.ClickBannerEvent OnClickBanner = new BannerPresent.ClickBannerEvent();

	public IEnumerator Initialize( List<DataPresentParam> _list , Action _onFinished )
	{
		OnClickBanner.RemoveAllListeners();
		m_prefBanner.SetActive(false);
		BannerPresent[] arr = m_goContents.GetComponentsInChildren<BannerPresent>();
		foreach (BannerPresent c in arr)
		{
			if (m_prefBanner != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}


		foreach( DataPresentParam p in _list)
		{
			BannerPresent script = PrefabManager.Instance.MakeScript<BannerPresent>(m_prefBanner, m_goContents);
			script.Initialize(p);

			script.OnClickBanner.AddListener((_data)=>
			{
				OnClickBanner.Invoke(_data);
			});

			yield return null;
		}

	}

}
