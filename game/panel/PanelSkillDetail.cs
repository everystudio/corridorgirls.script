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

	public string m_strSituation;
	public bool m_bUsed;
	public Button m_btnUse;
	public Button m_btnCancel;

	private MasterSkillParam m_masterSkillParam;

	public void Initialize( MasterSkillParam _master)
	{
		m_masterSkillParam = _master;
		common();

		string strOutline = "";
		string strForMp = "" ;

		if (m_masterSkillParam.situation == "any")
		{
			strOutline = m_masterSkillParam.outline + strForMp;
		}
		else if (m_masterSkillParam.situation == "field")
		{
			strOutline = string.Format("{0}\n\nこのスキルはフィールドでのみ使用可能です{1}", m_masterSkillParam.outline, strForMp);
		}
		else if (m_masterSkillParam.situation == "battle")
		{
			strOutline = string.Format("{0}\n\nこのスキルはバトル中のみ使用可能です{1}", m_masterSkillParam.outline, strForMp);
		}
		else
		{
			strOutline = "スキルの設定不備です";
		}
		m_txtOutline.text = strOutline;

	}

	private void common()
	{

		m_txtTitle.text = m_masterSkillParam.name;
		m_imgSkillIcon.sprite = SpriteManager.Instance.Get(m_masterSkillParam.sprite_name);
		m_txtMP.text = string.Format("MP:{0}", m_masterSkillParam.mp);



	}

	public void Initialize(int _iSkillId , string _strSituation , bool _bUsed)
	{
		m_masterSkillParam = DataManagerGame.Instance.masterSkill.list.Find(p => p.skill_id == _iSkillId);

		common();

		string strOutline = "";
		bool bEnableMp = m_masterSkillParam.mp <= DataManagerGame.Instance.GetMp();
		string strForMp = bEnableMp ? "" : "\n<color=red>MPが不足しています</color>";

		bool bMatchSituation = true;

		if( m_masterSkillParam.situation == "any")
		{
			strOutline = m_masterSkillParam.outline + strForMp;
		}
		else if(m_masterSkillParam.situation == "field")
		{
			strOutline = string.Format("{0}\n\nこのスキルはフィールドでのみ使用可能です{1}" , m_masterSkillParam.outline , strForMp);
			if( _strSituation != "field")
			{
				Debug.Log("not use");
				bMatchSituation = false;
			}
		}
		else if(m_masterSkillParam.situation == "battle")
		{
			strOutline = string.Format("{0}\n\nこのスキルはバトル中のみ使用可能です{1}", m_masterSkillParam.outline, strForMp);
			if (_strSituation != "battle")
			{
				Debug.Log("not use");
				bMatchSituation = false;
			}
		}
		else
		{
			strOutline = "スキルの設定不備です";
		}

		m_txtOutline.text = strOutline;

		gameObject.SetActive(true);

		m_strSituation = _strSituation;
		m_bUsed = _bUsed;

		m_btnUse.interactable = !m_bUsed;
		if( bMatchSituation == false)
		{
			m_btnUse.interactable = bMatchSituation;
		}
		else if(bEnableMp == false)
		{
			m_btnUse.interactable = bEnableMp;
		}

	}

}
