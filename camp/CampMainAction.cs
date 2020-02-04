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
	public class idle : CampMainActionBase
	{
		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelSkill.m_goControlRoot.SetActive(false);
			aging_timer = 0.0f;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if (DMCamp.Instance.IsAging) {
				aging_timer += Time.deltaTime;
				if (3.0f < aging_timer)
				{
					Fsm.Event("stage");
				}
			}
			#endregion
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class stage_top : CampMainActionBase
	{
		public FsmInt stage_id;
		private float aging_timer;

		public override void OnEnter()
		{
			base.OnEnter();
			aging_timer = 0.0f;

			campMain.m_panelStage.gameObject.SetActive(true);

			campMain.m_panelStage.m_goPanelButtons.SetActive(true);
			campMain.m_panelStage.OnBannerStage.AddListener(OnBannerStage);
			campMain.m_panelStage.ShowList();

			campMain.m_panelStage.m_btnClose.onClick.AddListener(OnClose);
		}

		private void OnBannerStage( int _iStageId)
		{
			stage_id.Value = _iStageId;

			DMCamp.Instance.config.WriteInt("stage_id", stage_id.Value);
			DMCamp.Instance.config.Save();

			Fsm.Event("select");
		}

		private void OnBannerStage(BannerStage arg0)
		{
			OnBannerStage(arg0.m_masterStageParam.stage_id);
		}

		private void OnClose()
		{
			campMain.m_panelStage.m_goPanelButtons.SetActive(false);
			campMain.m_panelStage.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			#region Aging
			if(DMCamp.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if( 3.0f < aging_timer)
				{
					int iStageIndex = UtilRand.GetRand(DMCamp.Instance.masterStage.list.Count);
					OnBannerStage(DMCamp.Instance.masterStage.list[iStageIndex].stage_id);
				}
			}
			#endregion
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

			campMain.m_panelChara.m_btnClose.gameObject.SetActive(true);
			campMain.m_panelChara.m_btnEdit.gameObject.SetActive(true);
			campMain.m_panelChara.m_btnList.gameObject.SetActive(true);

			campMain.m_panelChara.m_btnClose.onClick.AddListener(OnClose);
			campMain.m_panelChara.m_btnEdit.onClick.AddListener(OnEdit);
			campMain.m_panelChara.m_btnList.onClick.AddListener(OnList);


			campMain.m_panelChara.m_goCharaButtons.SetActive(true);
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
			campMain.m_panelChara.m_goCharaButtons.SetActive(false);
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

			campMain.m_panelChara.m_goCharaButtons.SetActive(true);
			campMain.m_panelChara.m_btnClose.gameObject.SetActive(true);
			campMain.m_panelChara.m_btnEdit.gameObject.SetActive(false);
			campMain.m_panelChara.m_btnList.gameObject.SetActive(false);


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

			campMain.m_panelChara.m_btnClose.onClick.AddListener(() =>
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
			campMain.m_panelChara.m_btnClose.onClick.RemoveAllListeners();
			campMain.m_panelChara.OnListCharaId.RemoveAllListeners();
			campMain.m_panelChara.m_btnListClose.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class chara_detail : CampMainActionBase
	{
		public FsmInt chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelChara.m_panelCharaDetail.gameObject.SetActive(true);

			campMain.m_panelChara.m_panelCharaDetail.Show(
				DMCamp.Instance.masterChara.list.Find(p=>p.chara_id == chara_id.Value ),
				DMCamp.Instance.masterCard.list,
				DMCamp.Instance.masterCharaCard.list,
				DMCamp.Instance.masterCardSymbol.list
				);

			campMain.m_panelChara.m_btnListClose.onClick.AddListener(() =>
			{
				Fsm.Event("close");
			});
			campMain.m_panelChara.m_btnClose.onClick.AddListener(() =>
			{
				Fsm.Event("close");
			});



		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelChara.m_btnListClose.onClick.RemoveAllListeners();
			campMain.m_panelChara.m_btnClose.onClick.RemoveAllListeners();
			campMain.m_panelChara.m_panelCharaDetail.gameObject.SetActive(false);
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
			campMain.m_panelChara.m_goCharaButtons.SetActive(false);

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

			//Debug.Log(chara_id.Value);
			//Debug.Log(exchange_chara_id.Value);
			//bool chara_a = DMCamp.Instance.dataUnit.IsPartyChara(chara_id.Value);
			//bool chara_b = DMCamp.Instance.dataUnit.IsPartyChara(exchange_chara_id.Value);

			DataUnitParam unit_a = DMCamp.Instance.dataUnit.list.Find(p => p.chara_id == chara_id.Value && p.unit == "chara");
			DataUnitParam unit_b = DMCamp.Instance.dataUnit.list.Find(p => p.chara_id == exchange_chara_id.Value && p.unit == "chara");

			string chara_a_position = unit_a.position;
			string chara_b_position = unit_b.position;
			unit_a.position = chara_b_position;
			unit_b.position = chara_a_position;

			foreach( DataUnitParam u in DMCamp.Instance.dataUnit.list.FindAll(p=>p.unit == "chara"))
			{
				//Debug.Log(string.Format("chara_id={0} position={1}", u.chara_id, u.position));
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
			DMCamp.Instance.dataUnit.Save();

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

			if (false == DMCamp.Instance.dataUnit.Load())
			{
				yield return StartCoroutine(DMCamp.Instance.dataUnit.SpreadSheet(DMCamp.SS_TEST, "unit", () => { }));
			}
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
			campMain.m_panelSkill.m_goControlRoot.SetActive(true);
			campMain.m_panelSkill.SetupSettingSkill(DMCamp.Instance.dataSkill.list.FindAll(p => 0 < p.status), DMCamp.Instance.masterSkill.list);

			campMain.m_panelSkill.m_textBtnList.text = "スキル一覧";
			campMain.m_panelSkill.m_textBtnEdit.text = "スキル編集";
			campMain.m_panelSkill.m_textBtnClose.text = "閉じる";

			campMain.m_panelSkill.m_btnList.interactable = true;
			campMain.m_panelSkill.m_btnEdit.interactable = true;
			campMain.m_panelSkill.m_btnClose.interactable = true;

			campMain.m_panelSkill.m_btnList.gameObject.SetActive(true);
			campMain.m_panelSkill.m_btnClose.gameObject.SetActive(true);
			campMain.m_panelSkill.m_btnEdit.gameObject.SetActive(true);

			campMain.m_panelSkill.ClearSkillList();

			campMain.m_panelSkill.m_btnList.onClick.AddListener(()=>
			{
				Fsm.Event("list");
			});
			campMain.m_panelSkill.m_btnClose.onClick.AddListener(OnClose);
			campMain.m_panelSkill.m_btnEdit.onClick.AddListener(() => {
				Fsm.Event("edit");
			});
		}

		private void OnClose()
		{
			campMain.m_panelSkill.gameObject.SetActive(false);
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnList.onClick.RemoveListener(OnClose);
			campMain.m_panelSkill.m_btnClose.onClick.RemoveListener(OnClose);
			campMain.m_panelSkill.m_btnEdit.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_list : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelSkill.SetupListSkill(DMCamp.Instance.masterSkill.list);

			campMain.m_panelSkill.m_textBtnList.text = "----";
			campMain.m_panelSkill.m_textBtnEdit.text = "----";
			campMain.m_panelSkill.m_textBtnClose.text = "閉じる";

			campMain.m_panelSkill.m_btnList.interactable = false;
			campMain.m_panelSkill.m_btnEdit.interactable = false;
			campMain.m_panelSkill.m_btnClose.interactable = true;


			campMain.m_panelSkill.m_btnClose.onClick.AddListener(()=>
			{
				Fsm.Event("close");
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnClose.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_edit : CampMainActionBase
	{
		public FsmInt skill_id;
		public FsmBool is_change;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_panelSkill.Select(0);
			campMain.m_panelSkill.SetupSettingSkill(DMCamp.Instance.dataSkill.list.FindAll(p => 0 < p.status), DMCamp.Instance.masterSkill.list);
			campMain.m_panelSkill.SetupListSkill(DMCamp.Instance.masterSkill.list);

			campMain.m_panelSkill.m_btnList.gameObject.SetActive(false);

			foreach ( DataSkillParam data_skill in DMCamp.Instance.dataSkill.list)
			{
				Debug.Log(string.Format("skill_id:{0} status:{1}", data_skill.skill_id, data_skill.status));
			}

			if (is_change.Value)
			{
				campMain.m_panelSkill.m_textBtnEdit.text = "決定";
				campMain.m_panelSkill.m_btnEdit.interactable = true;
			}
			else {
				campMain.m_panelSkill.m_textBtnEdit.text = "----";
				campMain.m_panelSkill.m_btnEdit.interactable = false;
			}

			campMain.m_panelSkill.m_textBtnClose.text = "キャンセル";
			campMain.m_panelSkill.m_btnEdit.onClick.AddListener(() => {
				Fsm.Event("decide");
			});
			campMain.m_panelSkill.m_btnClose.onClick.AddListener(() => {
				Fsm.Event("cancel");
			});

			campMain.m_panelSkill.OnSetSkillId.AddListener(_select);
			campMain.m_panelSkill.OnListSkillId.AddListener(_select);
		}

		private void _select( int _iSkillId)
		{
			skill_id.Value = _iSkillId;
			Fsm.Event("select");
		}
		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnEdit.onClick.RemoveAllListeners();
			campMain.m_panelSkill.m_btnClose.onClick.RemoveAllListeners();

			campMain.m_panelSkill.OnSetSkillId.RemoveAllListeners();
			campMain.m_panelSkill.OnListSkillId.RemoveAllListeners();

		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_select_exchange : CampMainActionBase
	{
		public FsmInt skill_id;
		public FsmInt skill_id_exchange;

		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelSkill.Select(skill_id.Value);

			campMain.m_panelSkill.m_textBtnEdit.text = "----";
			campMain.m_panelSkill.m_textBtnClose.text = "キャンセル";
			campMain.m_panelSkill.m_btnEdit.interactable = false;
			campMain.m_panelSkill.m_btnClose.onClick.AddListener(() => {
				cancel();
			});

			campMain.m_panelSkill.OnSetSkillId.AddListener(_select);
			campMain.m_panelSkill.OnListSkillId.AddListener(_select);
		}

		private void _select(int _iSkillId)
		{
			Debug.Log(skill_id.Value);
			Debug.Log(_iSkillId);
			if( _iSkillId == skill_id.Value)
			{
				cancel();
			}
			else
			{
				bool skill_a = campMain.m_panelSkill.IsSettingSkill(skill_id.Value);
				bool skill_b = campMain.m_panelSkill.IsSettingSkill(_iSkillId);
				if( skill_a == false && skill_b == false)
				{
					skill_id.Value = _iSkillId;
					campMain.m_panelSkill.Select(skill_id.Value);
				}
				else
				{
					skill_id_exchange.Value = _iSkillId;
					Fsm.Event("exchange");
				}
			}
		}

		private void cancel()
		{
			skill_id.Value = 0;
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelSkill.m_btnEdit.onClick.RemoveAllListeners();
			campMain.m_panelSkill.m_btnClose.onClick.RemoveAllListeners();

			campMain.m_panelSkill.OnSetSkillId.RemoveAllListeners();
			campMain.m_panelSkill.OnListSkillId.RemoveAllListeners();

		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class change_skill : CampMainActionBase
	{
		public FsmInt skill_id;
		public FsmInt skill_id_exchange;

		public override void OnEnter()
		{
			base.OnEnter();

			DataSkillParam skill_a = DMCamp.Instance.dataSkill.list.Find(p => p.skill_id == skill_id.Value);
			DataSkillParam skill_b = DMCamp.Instance.dataSkill.list.Find(p => p.skill_id == skill_id_exchange.Value);

			int temp_status = skill_a.status;
			skill_a.status = skill_b.status;
			skill_b.status = temp_status;

			Debug.Log(string.Format("skill_id={0} skill_id_exchange{1}", skill_id.Value, skill_id_exchange.Value));

			//skill_id.Value = 0;
			//skill_id_exchange.Value = 0;

			Finish();
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_save : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			// データの上書き
			DMCamp.Instance.dataSkill.Save();

			campMain.m_panelStatus.SetupSkill(DMCamp.Instance.dataSkill.list.FindAll(p => 0 < p.status), DMCamp.Instance.masterSkill.list);

			Finish();
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class skill_reload : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			StartCoroutine(reload());

		}

		private IEnumerator reload()
		{
			if (false == DMCamp.Instance.dataSkill.Load())
			{
				yield return StartCoroutine(DMCamp.Instance.dataSkill.SpreadSheet(DMCamp.SS_TEST, "skill", () => { }));
			}
			Finish();
		}
	}



	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class menu_top : CampMainActionBase
	{
		private string label_cancel;
		public override void OnEnter()
		{
			base.OnEnter();

			campMain.m_panelMenu.gameObject.SetActive(true);

			campMain.m_panelDecideCheckBottom.m_txtMessage.text = "メニュー/設定\n変更したい内容を選択してください";
			campMain.m_panelDecideCheckBottom.m_goRoot.SetActive(true);
			campMain.m_panelDecideCheckBottom.gameObject.SetActive(true);

			campMain.m_panelDecideCheckBottom.m_btnDecide.gameObject.SetActive(false);

			label_cancel = campMain.m_panelDecideCheckBottom.m_txtLabelCancel.text;
			campMain.m_panelDecideCheckBottom.m_txtLabelCancel.text = "Close";
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("close");
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_panelMenu.gameObject.SetActive(false);
			campMain.m_panelDecideCheckBottom.m_btnDecide.gameObject.SetActive(true);
			campMain.m_panelDecideCheckBottom.m_txtLabelCancel.text = label_cancel;
			campMain.m_panelDecideCheckBottom.m_btnCancel.onClick.RemoveAllListeners();
			campMain.m_panelDecideCheckBottom.m_goRoot.SetActive(false);
			campMain.m_panelDecideCheckBottom.gameObject.SetActive(false);
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class standby_goto_stage : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			List<DataUnitParam> party_members = DMCamp.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && p.position != "none");
			foreach( DataUnitParam unit in party_members)
			{
				unit.CopyParams(DMCamp.Instance.masterChara.list.Find(p => p.chara_id == unit.chara_id));
			}
			DMCamp.Instance.dataUnit.Save();

			Finish();
		}
	}




}
