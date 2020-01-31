﻿using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanelPartyEditAction
{
	[ActionCategory("PanelPartyEditAction")]
	[HutongGames.PlayMaker.Tooltip("PanelPartyEditAction")]
	public class PanelPartyEditActionBase : FsmStateAction
	{
		protected PanelPartyEdit panel;
		public override void OnEnter()
		{
			base.OnEnter();
			panel = Owner.GetComponent<PanelPartyEdit>();
		}

		protected void RefreshParty()
		{

			MasterCharaParam left = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id
			));
			MasterCharaParam right = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id
			));
			MasterCharaParam back = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == (
			DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "back").chara_id
			));

			panel.m_partyHolder.Initialize(left, right, back);

		}
	}

	[ActionCategory("PanelPartyEditAction")]
	[HutongGames.PlayMaker.Tooltip("PanelPartyEditAction")]
	public class initialize : PanelPartyEditActionBase
	{
		public FsmInt chara_id_selecting;

		public FsmInt chara_id_left;
		public FsmInt chara_id_right;
		public FsmInt chara_id_back;

		public override void OnEnter()
		{
			base.OnEnter();
			chara_id_selecting.Value = 0;

			RefreshParty();

			chara_id_left.Value = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id;
			chara_id_right.Value = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id;
			chara_id_back.Value = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "back").chara_id;

			Finish();
		}

	}

	[ActionCategory("PanelPartyEditAction")]
	[HutongGames.PlayMaker.Tooltip("PanelPartyEditAction")]
	public class Idle : PanelPartyEditActionBase
	{
		public FsmInt chara_id_selecting;

		public FsmInt chara_id_left;
		public FsmInt chara_id_right;

		public override void OnEnter()
		{
			base.OnEnter();

			panel.m_partyHolder.OnClickIcon.AddListener((CharaIcon _icon) =>
			{
				if(chara_id_selecting.Value == _icon.m_masterChara.chara_id)
				{
					chara_id_selecting.Value = 0;
				}
				else if(chara_id_selecting.Value == 0)
				{
					chara_id_selecting.Value = _icon.m_masterChara.chara_id;
				}
				else if(chara_id_selecting.Value != _icon.m_masterChara.chara_id) // elseでもいいのでは？
				{
					exchange(chara_id_selecting.Value , _icon.m_masterChara.chara_id);
					RefreshParty();

					chara_id_selecting.Value = 0;

					int temp_left_id = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id;
					int temp_right_id = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id;

					ResetButtonLabel(!(chara_id_left.Value == temp_left_id && chara_id_right.Value == temp_right_id));

				}
				panel.m_partyHolder.Cover(chara_id_selecting.Value);
			});

			int left_id = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "left").chara_id;
			int right_id = DataManagerGame.Instance.dataUnit.list.Find(a => a.unit == "chara" && a.position == "right").chara_id;
			ResetButtonLabel(!(chara_id_left.Value == left_id && chara_id_right.Value == right_id));


			panel.m_btnStatus.onClick.AddListener(() =>
			{
				Fsm.Event("status");
			});
			panel.m_btnDeck.onClick.AddListener(() =>
			{
				Fsm.Event("deck");
			});

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if( _iIndex == 0)
				{
					DataManagerGame.Instance.dataUnit.Save();
					Fsm.Event("change");
				}
				else if( _iIndex == 1)
				{
					StartCoroutine(cancel());
				}
			});

		}

		private void ResetButtonLabel(bool _bChange)
		{
			// あまり良くないけどこっちが呼ばれるってことはキャラ選択が必要だったってわけ
			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
				2,
				new string[2] {  "交代する", "キャンセル" },
				new bool[2] { _bChange, true });
		}

		private void exchange( int _iCharaIdA , int _iCharaIdB )
		{

			DataUnitParam unit_a = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == _iCharaIdA && p.unit == "chara");
			DataUnitParam unit_b = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == _iCharaIdB && p.unit == "chara");

			string chara_a_position = unit_a.position;
			string chara_b_position = unit_b.position;
			unit_a.position = chara_b_position;
			unit_b.position = chara_a_position;

		}

		private IEnumerator cancel()
		{
			if (false == DataManagerGame.Instance.dataUnit.Load())
			{
				yield return StartCoroutine(DataManagerGame.Instance.dataUnit.SpreadSheet(DataManagerGame.SS_TEST, "unit", () => { }));
			}
			Fsm.Event("cancel");
		}


		public override void OnExit()
		{
			base.OnExit();
			panel.m_partyHolder.OnClickIcon.RemoveAllListeners();
			panel.m_btnStatus.onClick.RemoveAllListeners();
			panel.m_btnDeck.onClick.RemoveAllListeners();

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}
	}




	[ActionCategory("PanelPartyEditAction")]
	[HutongGames.PlayMaker.Tooltip("PanelPartyEditAction")]
	public class Change : PanelPartyEditActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.panelStatus.Initialize(DataManagerGame.Instance.dataUnit, DataManagerGame.Instance.masterChara);
			Finish();
		}

	}


	[ActionCategory("PanelPartyEditAction")]
	[HutongGames.PlayMaker.Tooltip("PanelPartyEditAction")]
	public class End : PanelPartyEditActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel.OnFinished.Invoke();
			Finish();
		}

	}





}