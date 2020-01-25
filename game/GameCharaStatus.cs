using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCharaStatus : MonoBehaviour {

	public Image m_imgFaceIcon;
	public EnergyBar m_barHp;

	public TextMeshProUGUI m_txtAttack;
	public TextMeshProUGUI m_txtMagic;
	public TextMeshProUGUI m_txtHeal;

	public List<AssistIcon> m_AssistIconList;

	public void Initialize(DataUnitParam _unit , MasterCharaParam _master_chara , List<DataUnitParam> _unit_list )
	{
		List<DataUnitParam> assist_list = _unit_list.FindAll(p => p.chara_id == _unit.chara_id && p.unit == "assist");

		m_imgFaceIcon.sprite = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACEICON, _master_chara.chara_id));

		m_barHp.SetValueMax(_unit.hp_max);
		m_barHp.SetValueCurrent(_unit.hp);


		int iAttack = _unit.str;
		int iMagic = _unit.magic;
		int iHeal = _unit.heal;
		foreach( DataUnitParam assist in assist_list)
		{
			iAttack += assist.str;
			iMagic += assist.magic;
			iHeal += assist.heal;
		}

		m_txtAttack.text = iAttack.ToString();
		m_txtMagic.text = iMagic.ToString();
		m_txtHeal.text = iHeal.ToString();


		if(assist_list.Count <= 3)
		{
			m_AssistIconList[3].gameObject.SetActive(false);
		}

		foreach( AssistIcon icon in m_AssistIconList)
		{
			icon.Show("なし", Color.white);
		}

		for( int i = 0; i< assist_list.Count; i++)
		{
			if( 0 < assist_list[i].str)
			{
				m_AssistIconList[i].Show(string.Format("物+{0}", assist_list[i].str), Color.red);
			}
			else if( 0 < assist_list[i].magic)
			{
				m_AssistIconList[i].Show(string.Format("魔+{0}", assist_list[i].magic), Color.blue);
			}
			else if( 0 < assist_list[i].heal)
			{
				m_AssistIconList[i].Show(string.Format("癒+{0}", assist_list[i].heal), Color.green);
			}
		}

	}





}
