using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStatus : MonoBehaviour {

	public AreaChara area_chara_left;
	public AreaChara area_chara_right;

	public Button m_btnStatus;
	public Button m_btnItem;
	public Button m_btnDeck;

	public GameObject m_goSkillButtonRoot;
	public GameObject m_prefBtnSkill;
	public List<BtnSkill> m_btnSkillList = new List<BtnSkill>();

	void Start()
	{
		m_prefBtnSkill.SetActive(false);
	}

	public void Initialize(DataUnit _dataUnit , MasterChara _masterChara)
	{

		DataUnitParam left = _dataUnit.list.Find(p=>p.unit == "chara" && p.position == "left");
		DataUnitParam right = _dataUnit.list.Find(p => p.unit == "chara" && p.position == "right");
		DataUnitParam back = _dataUnit.list.Find(p => p.unit == "chara" && p.position == "back");

		area_chara_left.Initialize(left, _masterChara.list.Find(p => p.chara_id == left.chara_id));
		area_chara_right.Initialize(right, _masterChara.list.Find(p => p.chara_id == right.chara_id));
	}

	public void ClearSkill()
	{
		BtnSkill[] arr = m_goSkillButtonRoot.GetComponentsInChildren<BtnSkill>();
		foreach (BtnSkill btn in arr)
		{
			Destroy(btn.gameObject);
		}
		m_btnSkillList.Clear();

	}
	public void SetupSkill( List<DataSkillParam> _skill_list , List<MasterSkillParam> _master_list )
	{
		_skill_list.Sort((a, b) => a.status - b.status);

		foreach(DataSkillParam data in _skill_list)
		{
			BtnSkill btn = PrefabManager.Instance.MakeScript<BtnSkill>(m_prefBtnSkill, m_goSkillButtonRoot);
			MasterSkillParam master = _master_list.Find(p => p.skill_id == data.skill_id);
			btn.Initialize(master);
			m_btnSkillList.Add(btn);
		}
	}


}
