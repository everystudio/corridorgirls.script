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

	public Image[] m_imgClearStarArr;

	public MasterStageParam m_masterStageParam;

	public void Initialize(MasterStageParam _master)
	{
		m_masterStageParam = _master;

		m_imgThumb.sprite = SpriteManager.Instance.Get(_master.thumb);

		m_txtName.text = _master.stage_name;
		m_txtOutline.text = _master.outline;

		m_btn.onClick.AddListener(() =>
		{
			OnBanner.Invoke(this);
		});
	}



}
