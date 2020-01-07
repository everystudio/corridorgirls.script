using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStage : MonoBehaviour {

	public List<BannerStage> banner_list = new List<BannerStage>();
	public GameObject m_prefBanner;
	public GameObject m_goContents;

	public Button m_btnClose;

	public void ShowList()
	{

		BannerStage[] arr = m_goContents.GetComponentsInChildren<BannerStage>();
		foreach (BannerStage c in arr)
		{
			GameObject.Destroy(c.gameObject);
		}
		
		banner_list.Clear();

		foreach( MasterStageParam p in DMCamp.Instance.masterStage.list)
		{
			BannerStage banner = PrefabManager.Instance.MakeScript<BannerStage>(m_prefBanner, m_goContents);
			banner_list.Add(banner);
		}

	}


}
