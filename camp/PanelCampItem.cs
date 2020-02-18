using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCampItem : MonoBehaviour {

	public GameObject m_goContentRoot;
	public GameObject m_prefCampItem;

	public TextMeshProUGUI m_txtNeedMana;

	public int m_iTotalMana;
	public UnityEventInt OnChangeTotalMana = new UnityEventInt();

	public void Initialize(List<DataCampItemParam> _campitem_list )
	{
		m_iTotalMana = 0;
		m_prefCampItem.SetActive(false);

		BannerCampItem[] arr = m_goContentRoot.GetComponentsInChildren<BannerCampItem>();
		foreach (BannerCampItem c in arr)
		{
			if (m_prefCampItem != c.gameObject)
			{
				GameObject.Destroy(c.gameObject);
			}
		}

		foreach ( DataCampItemParam param in _campitem_list)
		{
			MasterCampItemParam master_campitem = DMCamp.Instance.masterCampItem.list.Find(p => p.campitem_id == param.campitem_id);
			MasterItemParam master_item = DMCamp.Instance.masterItem.list.Find(p => p.item_id == master_campitem.item_id);
			BannerCampItem script = PrefabManager.Instance.MakeScript<BannerCampItem>(m_prefCampItem, m_goContentRoot);

			if( param.is_take)
			{
				m_iTotalMana += master_campitem.mana;
			}

			script.Initialize(param, master_campitem, master_item);
			script.OnClickBanner.AddListener((BannerCampItem _banner) =>
			{
				if(_banner.m_data.is_take)
				{
					SEControl.Instance.Play(Defines.KEY_SOUNDSE_PLUS);
					m_iTotalMana += _banner.m_master.mana;
				}
				else
				{
					SEControl.Instance.Play(Defines.KEY_SOUNDSE_MINUS);
					m_iTotalMana -= _banner.m_master.mana;
				}
				OnChangeTotalMana.Invoke(m_iTotalMana);

				m_txtNeedMana.text = string.Format("必要マナ:{0}", m_iTotalMana);

			});
		}
		m_txtNeedMana.text = string.Format("必要マナ:{0}", m_iTotalMana);

	}


}
