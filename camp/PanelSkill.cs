using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelSkill : MonoBehaviour {

	public Button m_btnList;
	public Button m_btnClose;
	public Button m_btnEdit;

	public TextMeshProUGUI m_textBtnList;
	public TextMeshProUGUI m_textBtnEdit;
	public TextMeshProUGUI m_textBtnClose;

	public GameObject m_goSkillButtonRoot;
	public GameObject m_prefBtnSkill;
	public List<BtnSkill> m_btnSkillList = new List<BtnSkill>();

	public GameObject m_goSkillBannerRoot;
	public GameObject m_prefBannerSkill;
	public List<BannerSkill> m_bannerSkillList = new List<BannerSkill>();

	public UnityEventInt OnSetSkillId = new UnityEventInt();
	public UnityEventInt OnListSkillId = new UnityEventInt();

	public GameObject m_goControlRoot;

	public void Select( int _iSkillId)
	{
		foreach( BtnSkill btn in m_btnSkillList)
		{
			btn.Cover(btn.m_masterSkillParam.skill_id == _iSkillId);
		}

		foreach( BannerSkill banner in m_bannerSkillList)
		{
			banner.m_imgSelectCover.gameObject.SetActive(banner.m_master.skill_id == _iSkillId);
		}
	}

	public bool IsSettingSkill( int _iSkillId)
	{
		Debug.Log(m_btnSkillList.Count);
		foreach( BtnSkill btn in m_btnSkillList)
		{
			Debug.Log(btn.m_masterSkillParam.skill_id);
			if( btn.m_masterSkillParam.skill_id == _iSkillId)
			{
				return true;
			}
		}
		return false;
	}

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
			btn.OnSkillButton.AddListener((BtnSkill _btn) =>
			{
				OnSetSkillId.Invoke(_btn.m_masterSkillParam.skill_id);
			});
			m_btnSkillList.Add(btn);
		}
	}

	public void ClearSkillList()
	{
		BannerSkill[] arr = m_goSkillBannerRoot.GetComponentsInChildren<BannerSkill>();
		foreach (BannerSkill banner in arr)
		{
			Destroy(banner.gameObject);
		}
		m_bannerSkillList.Clear();

	}
	public void SetupListSkill(List<MasterSkillParam> _master_list)
	{
		ClearSkillList();
		foreach (MasterSkillParam master in _master_list)
		{
			BannerSkill banner = PrefabManager.Instance.MakeScript<BannerSkill>(m_prefBannerSkill, m_goSkillBannerRoot);
			banner.Initialize(master);
			banner.OnSkillBanner.AddListener((BannerSkill _banner) =>
			{
				OnSetSkillId.Invoke(_banner.m_master.skill_id);
			});
			m_bannerSkillList.Add(banner);
		}
	}




}
