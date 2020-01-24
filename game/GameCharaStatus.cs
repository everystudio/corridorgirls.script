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

	public void Initialize(DataUnitParam _unit , List<DataUnitParam> _unit_list )
	{

		List<DataUnitParam> assist_list = _unit_list.FindAll(p => p.chara_id == _unit.chara_id && p.unit == "assist");



	}





}
