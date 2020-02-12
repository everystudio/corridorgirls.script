using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelStageCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtStageName;
	public Image m_imgStageThumb;
	public TextMeshProUGUI m_txtStageOutline;
	public TextMeshProUGUI m_txtTotalWave;
	public TextMeshProUGUI m_txtTotalCorridor;

	public List<Image> m_imgStageMissionStar;
	public List<TextMeshProUGUI> m_txtStageMissionText;

	public TextMeshProUGUI m_txtFoodParty;
	public TextMeshProUGUI m_txtFoodStage;
	public TextMeshProUGUI m_txtFoodTotal;

	public int Initialize( int _iStageId)
	{
		int iRetRequestFood = 0;
		MasterStageParam masterStage = DMCamp.Instance.masterStage.list.Find(p => p.stage_id == _iStageId);

		m_txtStageName.text = masterStage.stage_name;
		m_imgStageThumb.sprite = SpriteManager.Instance.Get(masterStage.thumb);
		m_txtStageOutline.text = masterStage.outline;

		m_txtTotalWave.text = string.Format("Wave:{0}", masterStage.total_wave);
		m_txtTotalCorridor.text = string.Format("約{0}回廊", masterStage.total_length);

		m_txtStageMissionText[0].text = "クリア済み";
		m_txtStageMissionText[1].text = string.Format("リロード{0}回以内" , masterStage.challenge_reload);
		m_txtStageMissionText[2].text = string.Format("カードプレイ{0}以内", masterStage.challenge_play);

		List<DataUnitParam> party_members = DMCamp.Instance.dataUnitCamp.list.FindAll(p => p.unit == "chara" && p.position != "none");
		int iPartyCost = 0;
		foreach( DataUnitParam unit in party_members)
		{
			MasterCharaParam master_chara = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == unit.chara_id);
			iPartyCost += master_chara.food;
		}
		m_txtFoodParty.text = iPartyCost.ToString();
		m_txtFoodStage.text = masterStage.food_rate.ToString();
		iRetRequestFood = iPartyCost * masterStage.food_rate;
		m_txtFoodTotal.text = iRetRequestFood.ToString();

		DataStageParam data_stage = DMCamp.Instance.dataStage.list.Find(p => p.stage_id == _iStageId);
		if( data_stage != null )
		{
			m_imgStageMissionStar[0].color = 0 < data_stage.clear_count ? Color.yellow : Color.white;
			m_imgStageMissionStar[1].color = data_stage.best_reload <= masterStage.challenge_reload ? Color.yellow : Color.white;
			m_imgStageMissionStar[2].color = data_stage.best_play <= masterStage.challenge_play ? Color.yellow : Color.white;
		}
		else
		{
			for( int i = 0; i < m_imgStageMissionStar.Count; i++)
			{
				m_imgStageMissionStar[i].color = Color.white;
			}
		}

		return iRetRequestFood;
	}

}
