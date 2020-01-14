using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CampMainAction {
	public class CampMainActionBase : FsmStateAction
	{
		protected CampMain campMain;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain = Owner.GetComponent<CampMain>();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class startup : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(DMCamp.Instance.Initialized)
			{
				Finish();
			}
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class stage_top : CampMainActionBase
	{
		public FsmInt stage_id;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelStage.gameObject.SetActive(true);

			campMain.m_panelStage.OnBannerStage.AddListener(OnBannerStage);
			campMain.m_panelStage.ShowList();

			campMain.m_panelStage.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnBannerStage(BannerStage arg0)
		{
			stage_id.Value = arg0.m_masterStageParam.stage_id;

			DMCamp.Instance.config.WriteInt("stage_id", stage_id.Value);
			DMCamp.Instance.config.Save();

			Fsm.Event("select");
		}

		private void OnClose()
		{
			campMain.m_panelStage.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelStage.OnBannerStage.RemoveListener(OnBannerStage);
			campMain.m_panelStage.m_btnClose.onClick.RemoveListener(OnClose);
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class chara_top : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelChara.gameObject.SetActive(true);
			//campMain.m_panelChara.ShowList();

			campMain.m_partyHolder.Initialize(
				DMCamp.Instance.masterChara.list.Find(p=>p.chara_id == 1 ),
				DMCamp.Instance.masterChara.list.Find(p=>p.chara_id == 3 ),
				DMCamp.Instance.masterChara.list.Find(p=>p.chara_id == 2 )
				);

			// 非表示にする
			campMain.m_panelDecideCheckBottom.gameObject.SetActive(false);
			campMain.m_panelChara.CloseList();


			campMain.m_panelChara.m_btnClose.onClick.AddListener(OnClose);
			campMain.m_panelChara.m_btnEdit.onClick.AddListener(OnEdit);
			campMain.m_panelChara.m_btnList.onClick.AddListener(OnList);
		}

		private void OnEdit()
		{
			Fsm.Event("edit");
		}

		private void OnList()
		{
			Fsm.Event("chara");
		}

		private void OnClose()
		{
			campMain.m_panelChara.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelChara.m_btnClose.onClick.RemoveListener(OnClose);
			campMain.m_panelChara.m_btnEdit.onClick.RemoveListener(OnEdit);
			campMain.m_panelChara.m_btnList.onClick.RemoveListener(OnList);
		}
	}
	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class chara_list : CampMainActionBase
	{
		public FsmInt chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelChara.ShowList();

			campMain.m_panelChara.OnListCharaId.AddListener((int _iCharaId) =>
			{
				chara_id.Value = _iCharaId;
				Fsm.Event("chara");
			});
			campMain.m_panelChara.m_btnListClose.gameObject.SetActive(true);
			campMain.m_panelChara.m_btnListClose.onClick.AddListener(() =>
			{
				Fsm.Event("close");
				campMain.m_panelChara.CloseList();

			});
		}

		public override void OnExit()
		{
			base.OnExit();

			// これあんまり使いたくないけど、ボタンのイベントは唯一でいく
			// 逆にRemoveAllListenersを使わない場合はその必要性がある前提とする
			campMain.m_panelChara.OnListCharaId.RemoveAllListeners();
			campMain.m_panelChara.m_btnListClose.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class party_edit_top : CampMainActionBase
	{
		public FsmInt chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_partyHolder.Cover(0);

			campMain.m_panelDecideCheckBottom.m_txtMessage.text = "変更したいキャラを\n選択してください";
			campMain.m_panelDecideCheckBottom.m_goRoot.SetActive(true);

			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.AddListener(() =>
			{
				Fsm.Event("decide");
			});

			campMain.m_panelDecideCheckBottom.gameObject.SetActive(true);
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("cancel");
			});

			campMain.m_panelChara.ShowList();
			campMain.m_panelChara.OnListCharaId.AddListener((int _iCharaId) =>
			{
				chara_id.Value = _iCharaId;
				Fsm.Event("chara");
			});
			// こっちでは閉じるボタンいらない
			campMain.m_panelChara.m_btnListClose.gameObject.SetActive(false);

		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.RemoveAllListeners();
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class party_exchange : CampMainActionBase
	{
		public FsmInt chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			foreach( CharaIcon icon in campMain.m_panelChara.icon_list)
			{
				icon.Cover(chara_id.Value);
			}
			campMain.m_partyHolder.Cover(chara_id.Value);


			campMain.m_panelDecideCheckBottom.m_txtMessage.text = "変更したいキャラを\n選択してください";
			campMain.m_panelDecideCheckBottom.m_goRoot.SetActive(true);

			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.AddListener(() =>
			{
				Fsm.Event("decide");
			});
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("cancel");
			});

			campMain.m_panelChara.OnListCharaId.AddListener((int _iCharaId) =>
			{
				chara_id.Value = _iCharaId;
				Fsm.Event("chara");
			});
			// こっちでは閉じるボタンいらない
			campMain.m_panelChara.m_btnListClose.gameObject.SetActive(false);

		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.RemoveAllListeners();
		}
	}



	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_top : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelSkill.gameObject.SetActive(true);
			//campMain.m_panelStage.ShowList();

			campMain.m_panelSkill.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnClose()
		{
			campMain.m_panelSkill.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnClose.onClick.RemoveListener(OnClose);
		}

	}



}
