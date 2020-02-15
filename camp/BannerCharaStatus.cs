using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class BannerCharaStatus : MonoBehaviour {

	public DataUnitParam dataUnit;
	public MasterCharaParam masterChara;

	public Image m_imgFaceicon;
	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtLevel;
	public TextMeshProUGUI m_txtHP;
	public TextMeshProUGUI m_txtStr;
	public TextMeshProUGUI m_txtMagic;
	public TextMeshProUGUI m_txtHeal;
	public TextMeshProUGUI m_txtFood;
	public EnergyBar m_barTension;

	public void Initialize(DataUnitParam _unit , MasterCharaParam _master)
	{
		dataUnit = _unit;
		masterChara = _master;

		m_imgFaceicon.sprite = SpriteManager.Instance.Get(string.Format(Defines.STR_FORMAT_FACEICON, masterChara.chara_id));
		m_txtName.text = masterChara.name;
		m_txtLevel.text = string.Format("Lv{0}", _unit.level);
		m_txtHP.text = dataUnit.hp_max.ToString();
		m_txtStr.text = dataUnit.str.ToString();
		m_txtMagic.text = dataUnit.magic.ToString();
		m_txtHeal.text = dataUnit.heal.ToString();
		m_txtFood.text = dataUnit.food.ToString();
	}

}
