using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelStage : MonoBehaviour {

	public List<BannerStage> banner_list = new List<BannerStage>();
	public GameObject m_prefBanner;
	public GameObject m_goContents;

	public GameObject m_goPanelButtons;
	public Button m_btnClose;

	public class BannerStageHandler : UnityEvent<BannerStage>
	{
	}

	public BannerStageHandler OnBannerStage = new BannerStageHandler();

	public void ShowList()
	{
		BannerStage[] arr = m_goContents.GetComponentsInChildren<BannerStage>();
		foreach (BannerStage c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}
		banner_list.Clear();

		foreach( MasterStageParam p in DMCamp.Instance.masterStage.list.FindAll(p=>p.is_show == true))
		{
			BannerStage banner = PrefabManager.Instance.MakeScript<BannerStage>(m_prefBanner, m_goContents);
			banner.Initialize(p);

			banner.OnBanner.AddListener((BannerStage _banner) =>
			{
				OnBannerStage.Invoke(_banner);
			});
			banner_list.Add(banner);
		}

	}


}
