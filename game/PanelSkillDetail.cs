using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelSkillDetail : MonoBehaviour {

	public Image m_imgSkillIcon;
	public TextMeshProUGUI m_txtTitle;
	public TextMeshProUGUI m_txtOutline;
	public TextMeshProUGUI m_txtMP;

	public Button m_btnUse;
	public Button m_btnCancel;

	private MasterSkillParam m_masterSkillParam;

	public void Initialize(int _iSkillId)
	{
		m_masterSkillParam = DataManager.Instance.masterSkill.list.Find(p => p.skill_id == _iSkillId);

		m_txtTitle.text = m_masterSkillParam.name;
		m_imgSkillIcon.sprite = SpriteManager.Instance.Get(m_masterSkillParam.sprite_name);
		m_txtMP.text = string.Format("MP:{0}", m_masterSkillParam.mp);

		m_txtOutline.text = m_masterSkillParam.outline;
		gameObject.SetActive(true);
	}

}
