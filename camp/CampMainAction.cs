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

		public void PartyReset()
		{
			MasterCharaParam left = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == (
			DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id
			));
			MasterCharaParam right = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == (
			DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id
			));
			MasterCharaParam back = DMCamp.Instance.masterChara.list.Find(p => p.chara_id == (
			DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "back").chara_id
			));

			Debug.Log(left);
			Debug.Log(right);
			Debug.Log(back);

			Debug.Log(DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id);
			Debug.Log(DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id);
			Debug.Log(DMCamp.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "back").chara_id);

			Debug.Log(campMain);
			Debug.Log(campMain.m_partyHolder);
			campMain.m_partyHolder.Initialize(left, right, back);
		}


		protected void CoverChara(int _iCharaId)
		{
			foreach (CharaIcon icon in campMain.m_panelChara.icon_list)
			{
				icon.Cover(_iCharaId);
			}
			campMain.m_partyHolder.Cover(_iCharaId);
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
				campMain.m_panelStatus.Initialize(DMCamp.Instance.dataUnit, DMCamp.Instance.masterChara);
				campMain.m_panelStatus.SetupSkill(DMCamp.Instance.dataSkill.list.FindAll(p => 0 < p.status), DMCamp.Instance.masterSkill.list);
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

			PartyReset();

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
			campMain.m_partyHolder.OnClickIcon.AddListener((CharaIcon _icon) =>
			{
				select_chara(_icon.m_masterChara.chara_id);
			});

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
				select_chara(_iCharaId);
			});
			// こっちでは閉じるボタンいらない
			campMain.m_panelChara.m_btnListClose.gameObject.SetActive(false);

		}

		private void select_chara(int _iCharaId)
		{
			chara_id.Value = _iCharaId;
			Fsm.Event("chara");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_partyHolder.OnClickIcon.RemoveAllListeners();
			campMain.m_panelChara.OnListCharaId.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.RemoveAllListeners();
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class select_exchange : CampMainActionBase
	{
		public FsmInt chara_id;
		public FsmInt exchange_chara_id;
		public override void OnEnter()
		{
			base.OnEnter();
			Debug.Log(chara_id.Value);
			CoverChara(chara_id.Value);

			campMain.m_panelDecideCheckBottom.m_txtMessage.text = "変更したいキャラを\n選択してください";
			campMain.m_panelDecideCheckBottom.m_goRoot.SetActive(true);

			campMain.m_panelDecideCheckBottom.m_btnDecide.interactable = false;

			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("cancel");
			});

			campMain.m_partyHolder.OnClickIcon.AddListener((CharaIcon _icon) =>
			{
				select_chara(_icon.m_masterChara.chara_id);
			});


			campMain.m_panelChara.OnListCharaId.AddListener((int _iCharaId) =>
			{
				select_chara(_iCharaId);
			});
			// こっちでは閉じるボタンいらない
			campMain.m_panelChara.m_btnListClose.gameObject.SetActive(false);
		}

		private void select_chara(int _iCharaId)
		{
			Debug.Log(chara_id.Value);
			Debug.Log(_iCharaId);
			if (chara_id.Value == _iCharaId)
			{
				Debug.Log("cancel");
				Fsm.Event("cancel");
			}
			else
			{
				bool chara_a = DMCamp.Instance.dataUnit.IsPartyChara(chara_id.Value);
				bool chara_b = DMCamp.Instance.dataUnit.IsPartyChara(_iCharaId);

				if (chara_a == false && chara_b == false)
				{
					Debug.Log("not_party");
					chara_id.Value = _iCharaId;

					CoverChara(chara_id.Value);
				}
				else
				{
					Debug.Log("exchange");
					exchange_chara_id.Value = _iCharaId;
					Fsm.Event("exchange");
				}
			}
		}


		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelDecideCheckBottom.m_btnDecide.interactable = true;

			campMain.m_partyHolder.OnClickIcon.RemoveAllListeners();
			campMain.m_panelChara.OnListCharaId.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnDecide.onClick.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class exchange_party : CampMainActionBase
	{
		public FsmInt chara_id;
		public FsmInt exchange_chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			Debug.Log(chara_id.Value);
			Debug.Log(exchange_chara_id.Value);
			bool chara_a = DMCamp.Instance.dataUnit.IsPartyChara(chara_id.Value);
			bool chara_b = DMCamp.Instance.dataUnit.IsPartyChara(exchange_chara_id.Value);

			DataUnitParam unit_a = DMCamp.Instance.dataUnit.list.Find(p => p.chara_id == chara_id.Value && p.unit == "chara");
			DataUnitParam unit_b = DMCamp.Instance.dataUnit.list.Find(p => p.chara_id == exchange_chara_id.Value && p.unit == "chara");

			string chara_a_position = unit_a.position;
			string chara_b_position = unit_b.position;
			unit_a.position = chara_b_position;
			unit_b.position = chara_a_position;

			foreach( DataUnitParam u in DMCamp.Instance.dataUnit.list.FindAll(p=>p.unit == "chara"))
			{
				Debug.Log(string.Format("chara_id={0} position={1}", u.chara_id, u.position));
			}

			PartyReset();
			CoverChara(0);
			Finish();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class unit_save : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			// データの上書き

			campMain.m_panelStatus.Initialize(DMCamp.Instance.dataUnit, DMCamp.Instance.masterChara);


			Finish();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class unit_reload : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			StartCoroutine(reload());

		}

		private IEnumerator reload()
		{
			yield return StartCoroutine(DMCamp.Instance.dataUnit.SpreadSheet(DMCamp.SS_TEST, "unit", () => { }));
			PartyReset();
			Finish();
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
