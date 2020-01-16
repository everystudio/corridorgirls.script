using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSkill : MonoBehaviour {

	public Button m_btnClose;

	public GameObject m_goSkillButtonRoot;
	public GameObject m_prefBtnSkill;
	public List<BtnSkill> m_btnSkillList = new List<BtnSkill>();

	public GameObject m_goSkillBannerRoot;
	public GameObject m_prefBannerSkill;
	public List<BannerSkill> m_bannerSkillList = new List<BannerSkill>();

	public void SetupSettingSkill(List<DataSkillParam> _skill_list, List<MasterSkillParam> _master_list)
	{
		BtnSkill[] arr = m_goSkillButtonRoot.GetComponentsInChildren<BtnSkill>();
		foreach (BtnSkill btn in arr)
		{
			Destroy(btn.gameObject);
		}
		m_btnSkillList.Clear();

		_skill_list.Sort((a, b) => a.status - b.status);

		foreach (DataSkillParam data in _skill_list)
		{
			BtnSkill btn = PrefabManager.Instance.MakeScript<BtnSkill>(m_prefBtnSkill, m_goSkillButtonRoot);
			MasterSkillParam master = _master_list.Find(p => p.skill_id == data.skill_id);
			btn.Initialize(master);
			m_btnSkillList.Add(btn);
		}
	}
	public void SetupListSkill(List<MasterSkillParam> _master_list)
	{
		BannerSkill[] arr = m_goSkillBannerRoot.GetComponentsInChildren<BannerSkill>();
		foreach (BannerSkill banner in arr)
		{
			Destroy(banner.gameObject);
		}
		m_bannerSkillList.Clear();

		foreach (MasterSkillParam master in _master_list)
		{
			BannerSkill banner = PrefabManager.Instance.MakeScript<BannerSkill>(m_prefBannerSkill, m_goSkillBannerRoot);
			banner.Initialize(master);
			m_bannerSkillList.Add(banner);
		}
	}




}
