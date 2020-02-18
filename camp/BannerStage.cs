using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerStage : MonoBehaviour {

	public Button m_btn;
	public PanelStage.BannerStageHandler OnBanner = new PanelStage.BannerStageHandler();

	public Image m_imgThumb;
	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtOutline;
	public TextMeshProUGUI m_txtWave;

	public Image[] m_imgClearStarArr;

	public MasterStageParam m_masterStageParam;

	public void Initialize(MasterStageParam _master)
	{
		m_masterStageParam = _master;

		m_imgThumb.sprite = SpriteManager.Instance.Get(_master.thumb);

		m_txtName.text = _master.stage_name;
		m_txtOutline.text = _master.outline;

		m_txtWave.text = string.Format("Wave:{0}", _master.total_wave);

		DataStageParam data_stage = DMCamp.Instance.dataStage.list.Find(p => p.stage_id == _master.stage_id);
		if (data_stage != null)
		{
			m_imgClearStarArr[0].color = 0<data_stage.clear_count ? Color.yellow : Color.white;
			m_imgClearStarArr[1].color = data_stage.best_reload <= m_masterStageParam.challenge_reload ? Color.yellow : Color.white;
			m_imgClearStarArr[2].color = data_stage.best_play <= m_masterStageParam.challenge_play ? Color.yellow : Color.white;
		}
		else
		{
			for (int i = 0; i < m_imgClearStarArr.Length; i++)
			{
				m_imgClearStarArr[i].color = Color.white;
			}
		}



		m_btn.onClick.AddListener(() =>
		{
			SEControl.Instance.Play(Defines.KEY_SOUNDSE_DECIDE);// cursor_01
			OnBanner.Invoke(this);
		});
	}



}
