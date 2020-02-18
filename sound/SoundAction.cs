using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundAction {

	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class PlayBGM : FsmStateAction
	{
		public FsmString bgm_name;
		public override void OnEnter()
		{
			base.OnEnter();
			BGMControl.Instance.Play(bgm_name.Value);
			Finish();
		}
	}
	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class StopBGM : FsmStateAction
	{
		public override void OnEnter()
		{
			base.OnEnter();
			BGMControl.Instance.Stop();
			Finish();
		}
	}
	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class PauseBGM : FsmStateAction
	{
		public FsmString bgm_name;
		public override void OnEnter()
		{
			base.OnEnter();
			BGMControl.Instance.Pause();
			Finish();
		}
	}

	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class PlaySE : FsmStateAction
	{
		public FsmString se_name;
		public override void OnEnter()
		{
			base.OnEnter();
			SEControl.Instance.Play(se_name.Value);
			Finish();
		}
	}

	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class SetVolumeGame : FsmStateAction
	{
		public FsmGameObject panel_volume;

		public FsmGameObject sound_volume_slider_bgm;
		public FsmGameObject sound_volume_slider_se;

		private float fBGM;
		private float fSE;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_volume.Value.SetActive(true);


			fBGM = DataManagerGame.Instance.user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM);
			fSE = DataManagerGame.Instance.user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE);

			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "セットする" , "キャンセル" });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if( _iIndex == 0) {
					SEControl.Instance.Play("cursor_01");
				}
				else
				{
					DataManagerGame.Instance.user_data.Write(Defines.KEY_SOUNDVOLUME_BGM, fBGM.ToString());
					DataManagerGame.Instance.user_data.Write(Defines.KEY_SOUNDVOLUME_SE, fSE.ToString());
					SEControl.Instance.Play("cancel_01");

				}
				Finish();
			});

		}

		public override void OnExit()
		{
			base.OnExit();
			panel_volume.Value.SetActive(false);

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(0, null);
		}
	}



	[ActionCategory("SoundAction")]
	[HutongGames.PlayMaker.Tooltip("SoundAction")]
	public class SetVolumeCamp : FsmStateAction
	{
		public FsmGameObject panel_volume;

		public FsmGameObject sound_volume_slider_bgm;
		public FsmGameObject sound_volume_slider_se;

		private float fBGM;
		private float fSE;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_volume.Value.SetActive(true);
			fBGM = DMCamp.Instance.user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_BGM);
			fSE = DMCamp.Instance.user_data.ReadFloat(Defines.KEY_SOUNDVOLUME_SE);

			CampMain.Instance.m_panelDecideCheckBottom.m_goRoot.SetActive(true);
			CampMain.Instance.m_panelDecideCheckBottom.m_txtMessage.text = "音量調整を行えます";
			CampMain.Instance.m_panelDecideCheckBottom.m_txtLabelDecide.text = "セットする";
			CampMain.Instance.m_panelDecideCheckBottom.m_txtLabelCancel.text = "キャンセル";
			CampMain.Instance.m_panelDecideCheckBottom.m_btnDecide.onClick.RemoveAllListeners();
			CampMain.Instance.m_panelDecideCheckBottom.m_btnDecide.onClick.AddListener(() =>
			{
				SEControl.Instance.Play("cursor_01");
				Finish();
			});
			CampMain.Instance.m_panelDecideCheckBottom.m_btnCancel.gameObject.SetActive(true);
			CampMain.Instance.m_panelDecideCheckBottom.m_btnCancel.onClick.AddListener(() =>
			{
				DMCamp.Instance.user_data.Write(Defines.KEY_SOUNDVOLUME_BGM, fBGM.ToString());
				DMCamp.Instance.user_data.Write(Defines.KEY_SOUNDVOLUME_SE, fSE.ToString());
				SEControl.Instance.Play("cancel_01");
				Finish();
			});

		}

		public override void OnExit()
		{
			base.OnExit();
			panel_volume.Value.SetActive(false);

		}
	}



}
