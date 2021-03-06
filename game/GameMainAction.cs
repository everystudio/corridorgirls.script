﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using System;
using UnityEngine.Advertisements;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using GoogleMobileAds.Api;

namespace GameMainAction
{
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GameMainActionBase : FsmStateAction
	{
		protected GameMain gameMain;
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain = Owner.GetComponent<GameMain>();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GameMainActionBaseHelp : FsmStateAction
	{
		public int help_id;
		public override void OnEnter()
		{
			base.OnEnter();
			if (help_id != 0)
			{
				if (DataManagerGame.Instance.user_data.HasKey(Defines.GetHelpKey(help_id)) == false)
				{
					PanelHelp.Instance.Show(DataManagerGame.Instance.masterHelp.list.Find(p => p.help_id == help_id));
					DataManagerGame.Instance.user_data.AddInt(Defines.GetHelpKey(help_id), 1);
				}
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class DataWait : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_panelCameraScaler.gameObject.SetActive(false);

			PanelLogMessage.Instance.AddMessage("データ準備中");
		}
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (DataManagerGame.Instance.Initialized)
			{
				Finish();
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Startup : GameMainActionBase
	{
		public FsmBool show_ad_banner;
		public FsmInt stage_id;

		public FsmBool resume;
		public override void OnEnter()
		{
			base.OnEnter();
			if (show_ad_banner.Value)
			{
				gameMain.m_adsBannerBottom.Show();
				//gameMain.m_adsBannerTop.Show();
			}

			gameMain.gauge_mp.Setup();
			stage_id.Value = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_STAGE_ID);

			GameMain.Instance.total_turn = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_GAME_TURN);
			GameMain.Instance.m_iCountCardPlay = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_GAME_CARDPLAY);
			GameMain.Instance.m_iCountDeck = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_GAME_DECKRELOAD);

			MasterStageParam master_stage = DataManagerGame.Instance.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			//Debug.Log(master_stage.stage_name);
			//Debug.Log(master_stage.bgname);
			gameMain.m_backgroundControl.mat.mainTexture = TextureManager.Instance.Get(master_stage.bgname);


			// テンションのみだろうけどセット
			List<DataUnitParam> unit_param_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && (p.position == "left" || p.position == "right" || p.position == "back"));
			foreach (DataUnitParam unit in unit_param_list)
			{
				//Debug.Log(string.Format("chara_id={0} str={1}", unit.chara_id, unit.str));
				unit.BuildAssist(DataManagerGame.Instance.dataUnit.list.FindAll(p => p.chara_id == unit.chara_id && p.unit == "assist"));
				//Debug.Log(string.Format("chara_id={0} str={1}", unit.chara_id, unit.str));
			}

			if (DataManagerGame.Instance.gameData.HasKey(Defines.KEY_WAVE))
			{
				resume.Value = true;
			}
			else {
				resume.Value = false;
			}
			Finish();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardSetup : GameMainActionBase
	{
		public FsmBool resume;

		public override void OnEnter()
		{
			base.OnEnter();

			if( resume.Value == false ){

				// 初期設定
				DataManagerGame.Instance.dataCard.list.Clear();

				int serial = 1;

				List<DataUnitParam> unit_param_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && (p.position == "left" || p.position == "right" || p.position == "back"));

				foreach (DataUnitParam unit in unit_param_list)
				{
					List<MasterCharaCardParam> card_list = DataManagerGame.Instance.masterCharaCard.list.FindAll(p => p.chara_id == unit.chara_id);
					foreach (MasterCharaCardParam c in card_list)
					{
						DataCardParam dc = new DataCardParam();

						dc.Copy(DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == c.card_id), c.chara_id, serial);

						//dc.chara_id = c.chara_id;
						//dc.card_id = c.card_id;
						//dc.card_serial = serial;

						dc.status = (int)DataCard.STATUS.DECK;
						if (unit.position == "back")
						{
							dc.status = (int)DataCard.STATUS.NOTUSE;
						}
						serial += 1;

						DataManagerGame.Instance.dataCard.list.Add(dc);
					}
				}
				DataManagerGame.Instance.dataCard.CardFill(5);
			}
			gameMain.CardSetup(DataManagerGame.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND));

			gameMain.CardOrder();

			// スキル関係
			gameMain.ClearSkill();
			gameMain.m_panelStatus.SetupSkill(DataManagerGame.Instance.dataSkill.list.FindAll(p => 0 < p.status), DataManagerGame.Instance.masterSkill.list);


			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardSetup_tutorial : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			// 初期設定
			DataManagerGame.Instance.dataCard.list.Clear();

			// チュートリアル用のデッキデータをロード
			DataManagerGame.Instance.dataCard.Load(DataManagerGame.Instance.data_holder.Get("data_tutorial_card"));

			foreach(DataCardParam card in DataManagerGame.Instance.dataCard.list)
			{
				MasterCardParam master = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == card.card_id);
				card.Copy(master, card.chara_id, card.card_serial);
			}


			//DataManagerGame.Instance.dataSkill.Load(DataManagerGame.Instance.data_holder.Get("data_tutorial_skill"));

			DataManagerGame.Instance.dataCard.CardFill(5);

			gameMain.CardSetup(DataManagerGame.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND));

			gameMain.CardOrder();

			// スキル関係
			gameMain.ClearSkill();
			gameMain.m_panelStatus.SetupSkill(DataManagerGame.Instance.dataSkill.list.FindAll(p => 0 < p.status), DataManagerGame.Instance.masterSkill.list);

			Finish();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class UnitSetup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.panelStatus.Initialize(DataManagerGame.Instance.dataUnit, DataManagerGame.Instance.masterChara);
			Finish();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class AutomodeSetup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (gameMain.m_btnAutoMove != null)
			{
				gameMain.m_btnAutoMove.ShowUpdate();
			}
			Finish();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CreateDungeon : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;

		public FsmBool resume;

		public override void OnEnter()
		{
			base.OnEnter();
			//DataManagerGame.Instance.dataCorridor.Create(DataManagerGame.Instance.masterCorridor.list.FindAll(p => p.stage_id == 1));
			// ここでフラグを落とすのもなんか不自然な気がするけど
			gameMain.m_bIsGoal = false;
			gameMain.m_fadeScreen.Open();

			MasterStageParam master_stage = DataManagerGame.Instance.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			gameMain.total_wave = master_stage.total_wave;
			gameMain.now_wave = wave.Value;


			foreach (Corridor c in gameMain.corridor_list)
			{
				GameObject.Destroy(c.gameObject);
			}
			gameMain.corridor_list.Clear();

			int set_index = 1;
			if (resume.Value == false)
			{
				DataManagerGame.Instance.dataCorridor.BuildDungeon(master_stage, stage_id.Value, wave.Value);
			}
			else
			{
				set_index = DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_CURRENT_INDEX);
			}
			foreach (DataCorridorParam param in DataManagerGame.Instance.dataCorridor.list)
			{
				//Debug.Log(string.Format("x={0} y={1}", param.master.x, param.master.y));

				Corridor corr = PrefabManager.Instance.MakeScript<Corridor>(gameMain.m_prefCorridor, gameMain.m_goStageRoot);
				corr.Initialize(param);
				gameMain.corridor_list.Add(corr);
				//obj.transform.localPosition = new Vector3(param.master.x, param.master.y, 0.0f);
			}

			gameMain.chara_control.SetCorridor(DataManagerGame.Instance.dataCorridor.list.Find(p => p.index == set_index));

			if (gameMain.m_fadeScreen.is_open)
			{
				end();
			}
			else
			{
				gameMain.m_fadeScreen.OnOpen.AddListener(() =>
				{
					gameMain.m_fadeScreen.OnOpen.RemoveAllListeners();
					end();
				});
			}
		}
		private void end()
		{
			if( 0 < DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_GAME_IS_BATTLE))
			{
				GameMain.Instance.SelectCharaId = DataManagerGame.Instance.dataUnit.list.Find(p => 0 < p.hp && (p.position == "left" || p.position == "right")).chara_id;
				Fsm.Event("battle");
			}
			else if( 0 < DataManagerGame.Instance.gameData.ReadInt(Defines.KEY_GAME_IS_BOSS))
			{
				GameMain.Instance.SelectCharaId = DataManagerGame.Instance.dataUnit.list.Find(p => 0 < p.hp && (p.position == "left" || p.position == "right")).chara_id;
				Fsm.Event("boss");
			}
			else
			{
				Finish();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CreateDungeonCorridor : GameMainActionBase
	{
		public FsmInt stage_id;

		public override void OnEnter()
		{
			base.OnEnter();
			DataManagerGame.Instance.dataCorridor.Create(DataManagerGame.Instance.masterCorridor.list.FindAll(p => p.stage_id == stage_id.Value));
			// ここでフラグを落とすのもなんか不自然な気がするけど
			gameMain.m_bIsGoal = false;

			gameMain.m_fadeScreen.Open();

			foreach (Corridor c in gameMain.corridor_list)
			{
				GameObject.Destroy(c.gameObject);
			}
			gameMain.corridor_list.Clear();

			foreach (DataCorridorParam param in DataManagerGame.Instance.dataCorridor.list)
			{
				Corridor corr = PrefabManager.Instance.MakeScript<Corridor>(gameMain.m_prefCorridor, gameMain.m_goStageRoot);
				corr.Initialize(param);

				gameMain.corridor_list.Add(corr);
			}
			gameMain.chara_control.SetCorridor(DataManagerGame.Instance.dataCorridor.list.Find(p => p.index == 1));

			if (gameMain.m_fadeScreen.is_open)
			{
				Finish();
			}
			else
			{
				gameMain.m_fadeScreen.OnOpen.AddListener(() =>
				{
					gameMain.m_fadeScreen.OnOpen.RemoveAllListeners();
					Finish();
				});
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SaveData : GameMainActionBase
	{
		public FsmInt wave;
		public FsmBool is_battle;
		public FsmBool is_boss;

		public override void OnEnter()
		{
			base.OnEnter();

			bool bRemove = false;
			// ターン毎のデータ保存
			foreach (DataUnitParam assist in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "assist" && 0 < p.turn ))
			{
				//Debug.Log(assist.assist_name);
				assist.turn -= 1;
				if(assist.turn == 0)
				{
					bRemove = true;
					DataUnitParam chara = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == assist.chara_id && p.unit == "chara");
					chara.RemoveAssist(assist);
				}
			}
			if(bRemove)
			{
				GameMain.Instance.battleMain.HpRefresh();
				GameMain.Instance.CharaRefresh();
			}

			if (wave.Value != 0)
			{
				DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_WAVE, wave.Value);
			}
			//Debug.Log(GameMain.Instance.chara_control.target_corridor.index);
			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_CURRENT_INDEX, GameMain.Instance.chara_control.target_corridor.index);

			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_GAME_TURN, GameMain.Instance.total_turn);
			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_GAME_CARDPLAY, GameMain.Instance.m_iCountCardPlay);
			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_GAME_DECKRELOAD, GameMain.Instance.m_iCountDeck);

			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_GAME_IS_BATTLE, is_battle.Value ? 1 : 0);
			DataManagerGame.Instance.gameData.WriteInt(Defines.KEY_GAME_IS_BOSS, is_boss.Value ? 1 : 0);

			DataManagerGame.Instance.SaveTurn();

			Finish();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class TurnStart : GameMainActionBase
	{
		public FsmInt card_fill_num;
		public override void OnEnter()
		{
			base.OnEnter();

			GameMain.Instance.total_turn += 1;

			// ターン消費アイテムの削除

			Debug.Log(DataManagerGame.Instance.user_data.ReadInt(Defines.KeyGem));



			int hand_card_num = DataManagerGame.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND).Count;
			//Debug.Log(hand_card_num);
			PanelLogMessage.Instance.AddMessage("行動を開始してください");

			if (hand_card_num <= card_fill_num.Value)
			{
				Fsm.Event("card_fill");
			}
			else
			{
				Finish();
			}
		}
	}

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class GetCoin : FsmStateAction
	{
		public FsmInt coin;
		public override void OnEnter()
		{
			base.OnEnter();
			coin.Value = DataManagerGame.Instance.gameData.ReadInt(Defines.KeyGold);
			Debug.Log(coin.Value);
			Finish();
		}
	}
	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class AddCoin : FsmStateAction
	{
		public FsmInt coin;
		public FsmInt add;
		public override void OnEnter()
		{
			base.OnEnter();
			PanelLogMessage.Instance.AddMessage(string.Format("ゴールドを<color=#0FF>{0}</color>獲得しました" , add.Value));
			coin.Value = DataManagerGame.Instance.gameData.AddInt(Defines.KeyGold, add.Value);
			PrizeList.Instance.m_iGold = coin.Value;
			Finish();
		}
	}
	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class AddGem : FsmStateAction
	{
		public FsmInt gem;
		public FsmInt add;
		public override void OnEnter()
		{
			base.OnEnter();
			gem.Value = DataManagerGame.Instance.gameData.AddInt(Defines.KeyGem, add.Value);
			PanelLogMessage.Instance.AddMessage(string.Format("ジェムを<color=#0FF>{0}</color>獲得しました", add.Value));
			PrizeList.Instance.m_iGem = gem.Value;
			Finish();
		}
	}
	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class AddMana : FsmStateAction
	{
		public FsmInt mana;
		public FsmInt add;
		public override void OnEnter()
		{
			base.OnEnter();
			mana.Value = DataManagerGame.Instance.gameData.AddInt(Defines.KeyMana, add.Value);
			PanelLogMessage.Instance.AddMessage(string.Format("マナを<color=#0FF>{0}</color>獲得しました", add.Value));
			PrizeList.Instance.m_iMana = mana.Value;
			Finish();
		}
	}
	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class AddFood : FsmStateAction
	{
		public FsmInt food;
		public FsmInt add;
		public override void OnEnter()
		{
			base.OnEnter();
			food.Value = DataManagerGame.Instance.gameData.AddInt(Defines.KeyFood, add.Value);
			PanelLogMessage.Instance.AddMessage(string.Format("フードを<color=#0FF>{0}</color>獲得しました", add.Value));
			PrizeList.Instance.m_iFood = food.Value;
			Finish();
		}
	}

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class CardFill : GameMainActionBase
	{
		public FsmInt fill_num;
		public override void OnEnter()
		{
			base.OnEnter();

			PanelLogMessage.Instance.AddMessage("手札にカードを補充します");

			List<DataCardParam> add_list = new List<DataCardParam>();
			bool bResult = DataManagerGame.Instance.dataCard.CardFill(fill_num.Value, ref add_list);

			foreach (DataCardParam card in add_list)
			{
				GameMain.Instance.CardAdd(card);
			}
			GameMain.Instance.CardOrder();

			if (bResult)
			{
				Finish();
			}
			else
			{
				Fsm.Event("shuffle");
			}
		}
	}

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class DeckShuffle : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			PanelLogMessage.Instance.AddMessage("山札がなくなりました");
			PanelLogMessage.Instance.AddMessage("デッキリロードを行います");

			GameMain.Instance.m_iCountDeck += 1;

			DataManagerGame.Instance.dataCard.DeckShuffle();

			Finish();
		}

	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CardSelect : GameMainActionBase
	{
		public FsmInt select_card_serial;

		private int temp_select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			select_card_serial.Value = 0;
			temp_select_card_serial = 0;
			gameMain.CardOrder();

			foreach (Card card in gameMain.card_list_hand)
			{
				card.OnClickCard.AddListener(OnClickCard);
			}
		}
		private void OnClickCard(int arg0)
		{
			if (temp_select_card_serial == arg0)
			{
				select_card_serial.Value = arg0;
				Fsm.Event("select");
			}
			else {
				temp_select_card_serial = arg0;
				gameMain.CardSelectUp(temp_select_card_serial);

				// 強引な気もするけど
				gameMain.chara_control.m_animator.Play("idle");

				DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == temp_select_card_serial);

				GameMain.Instance.SelectCharaId = card.chara_id;
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			foreach (Card card in gameMain.card_list_hand)
			{
				card.OnClickCard.RemoveListener(OnClickCard);
			}
		}
	}

	[ActionCategory("AgincAction")]
	[HutongGames.PlayMaker.Tooltip("AgincAction")]
	public class AgingCardSelect : GameMainActionBase
	{
		public bool auto_mode;
		public FsmInt select_card_serial;


		private int temp_select_card_serial;
		private float time;

		public override void OnEnter()
		{
			base.OnEnter();

			int iIndex = UtilRand.GetRand(gameMain.card_list_hand.Count);
			temp_select_card_serial = gameMain.card_list_hand[iIndex].data_card.card_serial;

			time = 0.0f;
		}

		public override void OnUpdate()
		{
			if (DataManagerGame.Instance.IsAging || (auto_mode && DataManagerGame.Instance.IsAutoMove()))
			{
				base.OnUpdate();
				time += Time.deltaTime;

				if (2.5f < time)
				{
					time -= 2.5f;
					OnClickCard(temp_select_card_serial);
				}
			}
		}

		// これは普通のselectと同じ関数内容。
		// OnEnterがオーバーライドしたくなかったので継承してない
		private void OnClickCard(int arg0)
		{
			if (select_card_serial.Value == arg0)
			{
				Fsm.Event("select");
			}
			else {
				select_card_serial.Value = arg0;
				gameMain.CardSelectUp(select_card_serial.Value);

				DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);
				GameMain.Instance.SelectCharaId = card.chara_id;
			}
		}
	}


	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class SkillSelect : GameMainActionBase
	{
		public FsmInt skill_id;
		public override void OnEnter()
		{
			base.OnEnter();
			//Debug.Log("SkillSelect");
			foreach (BtnSkill btn in GameMain.Instance.m_panelStatus.m_btnSkillList)
			{
				//Debug.Log(btn.gameObject.name);
				btn.OnSkillButton.AddListener(OnSkill);
			}
		}

		private void OnSkill(BtnSkill arg0)
		{
			skill_id.Value = arg0.m_masterSkillParam.skill_id;
			Fsm.Event("skill");
		}

		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				foreach (BtnSkill btn in GameMain.Instance.m_panelStatus.m_btnSkillList)
				{
					btn.OnSkillButton.RemoveListener(OnSkill);
				}
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class idle_menu : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			GameCamera.Instance.RequestMoveStart.Invoke();

			if (gameMain.m_btnAutoMove != null)
			{
				gameMain.m_btnAutoMove.gameObject.SetActive(true);
			}

			gameMain.m_animCardRoot.SetBool("is_active", true);
			gameMain.m_panelGameControlButtons.ShowButtonNum(0, null);

			gameMain.m_panelStatus.m_btnStatus.onClick.AddListener(() =>
			{
				Fsm.Event("status");
			});
			gameMain.m_panelStatus.m_btnItem.onClick.AddListener(() =>
			{
				Fsm.Event("item");
			});
			gameMain.m_panelStatus.m_btnDeck.onClick.AddListener(() =>
			{
				Fsm.Event("deck");
				//m_panelPlayerDeck.Show();
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			if (gameMain.m_btnAutoMove != null)
			{
				gameMain.m_btnAutoMove.gameObject.SetActive(false);
			}

			gameMain.m_panelStatus.m_btnStatus.onClick.RemoveAllListeners();
			gameMain.m_panelStatus.m_btnItem.onClick.RemoveAllListeners();
			gameMain.m_panelStatus.m_btnDeck.onClick.RemoveAllListeners();
			if (gameMain.m_animCardRoot != null)
			{
				gameMain.m_animCardRoot.SetBool("is_active", false);
			}

			if (GameCamera.Instance != null)
			{
				GameCamera.Instance.RequestMoveStop.Invoke();
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowDeck : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.CardSelectUp(0);
			PanelLogMessage.Instance.AddMessage("現在のデッキを表示します");
			SEControl.Instance.Play("cursor_01");

			GameMain.Instance.m_panelPlayerDeck.m_btnClose.gameObject.SetActive(false);
			GameMain.Instance.m_panelPlayerDeck.Show();

			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				SEControl.Instance.Play("cancel_01");
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				GameMain.Instance.m_panelPlayerDeck.Close();
				GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowStatus : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.CardSelectUp(0);
			PanelLogMessage.Instance.AddMessage("ステータスを表示します");

			SEControl.Instance.Play("cursor_01");

			GameMain.Instance.m_panelGameStatus.gameObject.SetActive(true);
			GameMain.Instance.m_panelGameStatus.Show(DataManagerGame.Instance.dataUnit.list, DataManagerGame.Instance.masterChara.list);

			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				SEControl.Instance.Play("cancel_01");
				Finish();
			});

		}
		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				GameMain.Instance.m_panelGameStatus.gameObject.SetActive(false);
				GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowItem : GameMainActionBase
	{
		public FsmInt move;
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.CardSelectUp(0);

			PanelLogMessage.Instance.AddMessage("所持アイテムを表示します");
			SEControl.Instance.Play("cursor_01");

			ItemMain.Instance.move = 0;

			ItemMain.Instance.RequestShow.Invoke("field");

			ItemMain.Instance.OnClose.AddListener(() =>
			{
				if (0 < ItemMain.Instance.move)
				{
					move.Value = ItemMain.Instance.move;
					Fsm.Event("move");
				}
				else {
					SEControl.Instance.Play("cancel_01");
					Finish();
				}
			});

		}
		public override void OnExit()
		{
			base.OnExit();
			if (ItemMain.Instance != null)
			{
				ItemMain.Instance.OnClose.RemoveAllListeners();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SkillShow : GameMainActionBase
	{
		public FsmGameObject panel_skill_detail;
		public FsmInt skill_id;
		public FsmString situation;
		public FsmBool skill_used;

		private PanelSkillDetail m_panelSkillDetail;

		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.CardSelectUp(0);
			SEControl.Instance.Play("cursor_01");

			m_panelSkillDetail = panel_skill_detail.Value.GetComponent<PanelSkillDetail>();
			m_panelSkillDetail.Initialize(skill_id.Value, situation.Value, skill_used.Value);

			m_panelSkillDetail.m_btnUse.onClick.AddListener(OnSkill);
			m_panelSkillDetail.m_btnCancel.onClick.AddListener(OnCancel);
		}

		private void OnSkill()
		{
			Fsm.Event("use");
		}

		private void OnCancel()
		{
			SEControl.Instance.Play("cancel_01");
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			m_panelSkillDetail.m_btnCancel.onClick.RemoveListener(OnCancel);
			m_panelSkillDetail.m_btnUse.onClick.RemoveListener(OnSkill);
			if (m_panelSkillDetail != null)
			{
				m_panelSkillDetail.gameObject.SetActive(false);
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SkillUse : GameMainActionBase
	{
		public FsmInt skill_id;
		public FsmGameObject panel_skill_detail;

		public FsmInt move_num;
		public FsmInt damage;

		private MasterSkillParam masterSkillParam;
		public override void OnEnter()
		{
			base.OnEnter();
			if (panel_skill_detail != null)
			{
				panel_skill_detail.Value.SetActive(false);
			}

			SkillMain.Instance.SkillFinishHandler.AddListener(OnSkillFinished);
			SkillMain.Instance.SkillRequest.Invoke(skill_id.Value);

			masterSkillParam = DataManagerGame.Instance.masterSkill.list.Find(p => p.skill_id == skill_id.Value);
			DataManagerGame.Instance.gameData.AddInt(Defines.KEY_MP, -1 * masterSkillParam.mp);

			PanelLogMessage.Instance.AddMessage(string.Format("スキル<color=#0FF>{0}</color>を使います",masterSkillParam.name));


		}

		private void OnSkillFinished(bool arg0)
		{
			if (0 < SkillMain.Instance.move_num)
			{
				move_num.Value = SkillMain.Instance.move_num;
				Fsm.Event("move");
			}
			else if (0 < SkillMain.Instance.damage)
			{
				damage.Value = SkillMain.Instance.damage;
				Fsm.Event("damage");
			}
			else {
				Finish();
			}


		}
		public override void OnExit()
		{
			base.OnExit();
			SkillMain.Instance.SkillFinishHandler.RemoveListener(OnSkillFinished);
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CharaMove : GameMainActionBase
	{
		public FsmInt select_card_serial;
		public FsmInt move_num;

		public override void OnEnter()
		{
			base.OnEnter();
			GameCamera.Instance.ResetPosition();

			if (select_card_serial.Value != 0)
			{
				Card selected_card = null;
				selected_card = gameMain.card_list_hand.Find(p => p.data_card.card_serial == select_card_serial.Value);

				move_num.Value = selected_card.data_card.power;

				selected_card.data_card.status = (int)DataCard.STATUS.REMOVE;
				selected_card.m_animator.SetBool("delete", true);

				gameMain.card_list_hand.Remove(selected_card);
				gameMain.CardOrder();
			}

			//DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

			StartCoroutine(gameMain.chara_control.RequestMove(move_num.Value, () =>
			{
				Finish();
			}));
		}

		public override void OnExit()
		{
			base.OnExit();
			if (gameMain.m_panelCameraScaler != null)
			{
				gameMain.m_panelCameraScaler.gameObject.SetActive(false);
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class AddMp : GameMainActionBase
	{
		public FsmInt add_mp;
		public override void OnEnter()
		{
			base.OnEnter();
			DataManagerGame.Instance.gameData.AddInt(Defines.KEY_MP, add_mp.Value);
			Finish();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class AddMp_CardSerial : GameMainActionBase
	{
		public FsmInt card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			// プレイ回数はここで良いはず？
			GameMain.Instance.m_iCountCardPlay += 1;

			// ここのシリアルは手札じゃなくてもOK
			//Card selected_card = GameMain.Instance.card_list_hand.Find(p => p.data_card.card_serial == card_serial.Value);
			DataCardParam selected_card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == card_serial.Value);
			/*
			if (selected_card == null)
			{
				foreach (Card dc in GameMain.Instance.card_list_hand)
				{
					//Debug.Log(dc.data_card.card_serial);
				}
				//Debug.Log(card_serial.Value);
			}
			else if (selected_card.master == null)
			{
				//Debug.Log(selected_card.card_id);
			}
			*/
			DataManagerGame.Instance.MpHeal(selected_card.power);

			Finish();
		}
	}



	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CheckCorridorEvent : GameMainActionBase
	{
		public FsmBool battle;
		public override void OnEnter()
		{
			base.OnEnter();
			//string event_name = "none";
			/*
			switch (gameMain.chara_control.target_corridor.corridor_event.corridor_type)
			{
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.GOLD:
					event_name = "gold";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.CARD:
					event_name = "card";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.ITEM:
					event_name = "item";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.MISSION:
					event_name = "mission";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.HEAL:
					event_name = "heal";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.SHOP:
					event_name = "shop";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.BATTLE:
					event_name = "battle";
					break;
				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.BOSS:
					event_name = "boss";
					break;

				case MasterCorridorEvent.CORRIDOR_EVENT_TYPE.NONE:
					// ゴール
					if (gameMain.chara_control.target_corridor.corridor_event.sub_type == 2)
					{
						event_name = "goal";
					}
					break;

				default:
					event_name = "none";
					break;
			}
			*/
			//Debug.Log(gameMain.chara_control.target_corridor.corridor_event.corridor_type);
			if (battle.Value)
			{
				Fsm.Event("battle");
			}
			else {
				Fsm.Event(gameMain.chara_control.target_corridor.corridor_event.corridor_type);
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GetPrize : GameMainActionBase
	{
		public FsmString show_message;
		public FsmInt prize_add_num;
		public override void OnEnter()
		{
			base.OnEnter();
			MasterCorridorEventParam corridor_event = gameMain.chara_control.target_corridor.corridor_event;
			switch (corridor_event.sub_type)
			{
				case "gold":
					show_message.Value = string.Format("ゴールドを{0}手に入れた", corridor_event.param);
					break;
				case "gem":
					show_message.Value = string.Format("ジェムを{0}手に入れた", corridor_event.param);
					break;
				case "food":
					show_message.Value = string.Format("食料を{0}手に入れた", corridor_event.param);
					break;
				case "mana":
					show_message.Value = string.Format("マナを{0}手に入れた", corridor_event.param);
					break;

				default:
					break;
			}

			prize_add_num.Value = corridor_event.param;
			Fsm.Event(gameMain.chara_control.target_corridor.corridor_event.sub_type);

		}



	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CorridorGetGold : GameMainActionBase
	{


	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CorridorHeal : GameMainActionBase
	{


	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GetItem : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;

		public FsmInt item_id;
		public override void OnEnter()
		{
			base.OnEnter();

			gameMain.rouletteItem.Initialize(stage_id.Value, wave.Value);
			gameMain.rouletteItem.gameObject.SetActive(true);
			gameMain.rouletteItem.OnSelectedItemId.AddListener(OnSelectedItemId);
		}

		private void OnSelectedItemId(int arg0)
		{
			item_id.Value = arg0;
			MasterItemParam master_item = DataManagerGame.Instance.masterItem.list.Find(p => p.item_id == arg0);
			Debug.Log(master_item.name);

			DataManagerGame.Instance.dataItem.AddItem(master_item);
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
			gameMain.rouletteItem.OnSelectedItemId.RemoveListener(OnSelectedItemId);
			gameMain.rouletteItem.gameObject.SetActive(false);
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GetCard : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;

		public override void OnEnter()
		{
			base.OnEnter();
			PanelLogMessage.Instance.AddMessage(string.Format("カードを獲得することが出来ます"));

			gameMain.panelGetCard.gameObject.SetActive(true);


			gameMain.panelGetCard.stage_id = stage_id.Value;

			// ここ順番をうまく入れ替えしたい気がする
			List<DataUnitParam> unit_chara_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && p.position != "none");
			int[] chara_id_arr = new int[unit_chara_list.Count];
			for (int i = 0; i < chara_id_arr.Length; i++)
			{
				chara_id_arr[i] = unit_chara_list[i].chara_id;
			}

			gameMain.panelGetCard.Initialize(stage_id.Value, wave.Value, chara_id_arr);

			gameMain.panelGetCard.OnSelectCardParam.AddListener(OnSelectCardParam);
		}

		private void OnSelectCardParam(DataCardParam arg0)
		{
			//Debug.LogWarning(string.Format("card:label={0} power={1} chara_id={2}", arg0.master.label, arg0.master.power, arg0.chara_id));

			DataUnitParam add_unit = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == arg0.chara_id);

			DataCard.STATUS add_status = DataCard.STATUS.NONE;

			if (add_unit.position == "left" || add_unit.position == "right")
			{
				add_status = DataCard.STATUS.DECK;
			}
			else
			{
				add_status = DataCard.STATUS.NOTUSE;
			}

			DataManagerGame.Instance.dataCard.AddNewCard(arg0, add_status);

			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
			if (gameMain.panelGetCard != null)
			{
				gameMain.panelGetCard.OnSelectCardParam.RemoveListener(OnSelectCardParam);
				gameMain.panelGetCard.gameObject.SetActive(false);
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class StageShop : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		private int total_gold;
		private List<IconStageShopItem> icon_list = new List<IconStageShopItem>();

		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();
			PanelLogMessage.Instance.AddMessage(string.Format("ショップで買い物ができます"));
			aging_timer = 0.0f;
			PrizeList.Instance.m_bMenu = true;
			total_gold = 0;
			icon_list.Clear();
			GameMain.Instance.panelStageShop.gameObject.SetActive(true);
			GameMain.Instance.panelStageShop.Initialize(stage_id.Value, wave.Value);
			GameMain.Instance.panelStageShop.m_txtTotalGold.text = "0";
			GameMain.Instance.panelStageShop.OnClickIcon.RemoveAllListeners();
			GameMain.Instance.panelStageShop.OnClickIcon.AddListener((IconStageShopItem _icon) =>
			{
				//Debug.Log(_icon.icon_index);
				if (icon_list.Contains(_icon))
				{
					icon_list.Remove(_icon);
					_icon.Select(false);
					SEControl.Instance.Play(false ? Defines.KEY_SOUNDSE_PLUS : Defines.KEY_SOUNDSE_MINUS);
				}
				else
				{
					icon_list.Add(_icon);
					_icon.Select(true);
					SEControl.Instance.Play(true ? Defines.KEY_SOUNDSE_PLUS : Defines.KEY_SOUNDSE_MINUS);
				}
				total_gold = 0;
				foreach( IconStageShopItem icon in icon_list)
				{
					total_gold += icon.m_masterItemParam.gold;
				}
				GameMain.Instance.panelStageShop.m_txtTotalGold.text = total_gold.ToString();

				if( 0 < icon_list.Count)
				{
					int gold = DataManagerGame.Instance.gameData.ReadInt(Defines.KeyGold);
					//Debug.Log(gold);
					if( total_gold <= gold)
					{
						GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "購入", "退店" }, new bool[2] { true, true });
					}
					else
					{
						GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "<color=red>ゴールド不足</color>", "退店" }, new bool[2] { false, true });
					}
				}
				else
				{
					// ボタンの初期状態
					GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "購入", "退店" }, new bool[2] { false, true });
				}
			});

			// ボタンの初期状態
			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "購入", "退店" } , new bool[2] { false, true });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if (_iIndex == 0)
				{
					// 購入手続き
					GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "購入", "退店" }, new bool[2] { false, true });
					PrizeList.Instance.m_iGold = DataManagerGame.Instance.gameData.AddInt(Defines.KeyGold, -1 * total_gold);
					foreach (IconStageShopItem icon in icon_list)
					{
						DataManagerGame.Instance.dataItem.AddItem(icon.m_masterItemParam);
						icon.m_animator.Play("soldout");
						icon.m_btn.onClick.RemoveAllListeners();
					}

					total_gold = 0;
					icon_list.Clear();
					GameMain.Instance.panelStageShop.m_txtTotalGold.text = total_gold.ToString();

					SEControl.Instance.Play(Defines.KEY_SOUNDSE_CASH);
				}
				else
				{
					Finish();
				}
			});
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if( 3.0f < aging_timer)
				{
					// キャンセルボタン
					GameMain.Instance.m_panelGameControlButtons.button_list[1].onClick.Invoke();
				}
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			GameMain.Instance.panelStageShop.gameObject.SetActive(false);
			PrizeList.Instance.m_bMenu = false;

			GameMain.Instance.panelStageShop.OnClickIcon.RemoveAllListeners();
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}

	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Battle : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;

		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.battleMain.gameObject.SetActive(true);
			GameMain.Instance.battleMain.OnBattleFinished.AddListener((bool _bFlag) =>
			{
				if(_bFlag)
				{
					Finish();
				}
				else
				{
					GameMain.Instance.m_bIsGameover = true;
					Fsm.Event("gameover");
				}
			});

			List<MasterStageEnemyParam> appear_enemy_list = DataManagerGame.Instance.masterStageEnemy.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == wave.Value);
			if(appear_enemy_list.Count == 0)
			{
				appear_enemy_list = DataManagerGame.Instance.masterStageEnemy.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == 0);
			}

			int[] enemy_prob = new int[appear_enemy_list.Count];
			for (int i = 0; i < appear_enemy_list.Count; i++)
			{
				enemy_prob[i] = appear_enemy_list[i].prob;
			}

			int index = UtilRand.GetIndex(enemy_prob);

			// chara_id = enemy_idです
			GameMain.Instance.battleMain.RequestBattle.Invoke(false ,appear_enemy_list[index].enemy_id);
		}

		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				if (GameMain.Instance.battleMain != null)
				{
					GameMain.Instance.battleMain.OnBattleFinished.RemoveAllListeners();
					GameMain.Instance.battleMain.BattleClose();
				}
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class BossBattle : GameMainActionBase
	{
		public FsmInt stage_id;

		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.battleMain.gameObject.SetActive(true);
			GameMain.Instance.battleMain.OnBattleFinished.AddListener((bool _bFlag) =>
			{
				if (_bFlag)
				{
					Finish();
				}
				else
				{
					GameMain.Instance.m_bIsGameover = true;
					Fsm.Event("gameover");
				}
			});

			// ステージデータ
			MasterStageParam master_stage = DataManagerGame.Instance.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			// chara_id = enemy_idです
			GameMain.Instance.battleMain.RequestBattle.Invoke(true, master_stage.boss_chara_id);
		}

		public override void OnExit()
		{
			base.OnExit();
			if (GameMain.Instance != null)
			{
				if (GameMain.Instance.battleMain != null)
				{
					GameMain.Instance.battleMain.OnBattleFinished.RemoveAllListeners();
					GameMain.Instance.battleMain.BattleClose();
				}
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class BattlePrizeCreate : GameMainActionBase
	{
		public FsmInt prize_num;
		public FsmInt stage_id;
		public FsmInt wave;

		public FsmInt select_prize_index;

		public override void OnEnter()
		{
			base.OnEnter();

			select_prize_index.Value = 0;

			GameMain.Instance.m_panelBattlePrize.gameObject.SetActive(true);

			List<DataUnitParam> unit_chara_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && p.position != "none");
			int[] chara_id_arr = new int[unit_chara_list.Count];
			for (int i = 0; i < chara_id_arr.Length; i++)
			{
				chara_id_arr[i] = unit_chara_list[i].chara_id;
			}
			GameMain.Instance.m_panelBattlePrize.Initialize(prize_num.Value, stage_id.Value, wave.Value, chara_id_arr);


			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
				3,
				new string[3] { "決定","ステータス", "受け取らず" },
				new bool[3] { false,true, true });
			Finish();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class BattlePrizeIdle : GameMainActionBase
	{
		public FsmInt select_prize_index;

		private float aging_timer;
		public override void OnEnter()
		{
			base.OnEnter();
			aging_timer = 0.0f;

			GameMain.Instance.m_panelBattlePrize.Select(select_prize_index.Value );
			GameMain.Instance.m_panelBattlePrize.OnClickHolder.AddListener((int _iHolderIndex) =>
			{
				GameMain.Instance.m_panelBattlePrize.Select(_iHolderIndex);
				select_prize_index.Value = _iHolderIndex;

				GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
					3,
					new string[3] { "決定","ステータス", "受け取らず" },
					new bool[3] { true,true, true });
			});


			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(
				3,
				new string[3] { "決定", "ステータス", "受け取らず" },
				new bool[3] { select_prize_index.Value != 0 ? true : false, true, true });


			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if (_iIndex == 0)
				{
					BattleBonusHolder bbholder = GameMain.Instance.m_panelBattlePrize.GetBBHolder(select_prize_index.Value);

					for (int i = 0; i < bbholder.chara_id_list.Count; i++)
					{
						DataUnitParam unit_chara = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == bbholder.chara_id_list[i] && p.unit == "chara");

						DataManagerGame.Instance.dataUnit.AddAssist(unit_chara , "bb","バトルボーナス",
							bbholder.chara_id_list[i],
							bbholder.battle_bonus_list[i].field,
							bbholder.battle_bonus_list[i].param,
							-1);
					}
					GameMain.Instance.battleMain.HpRefresh();
					GameMain.Instance.CharaRefresh();

					//Debug.Log(DataManagerGame.Instance.dataUnit.list.Count);
				}

				if (_iIndex == 1)
				{
					Fsm.Event("status");
				}
				else {
					GameMain.Instance.m_panelBattlePrize.gameObject.SetActive(false);
					GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(0, null);

					Finish();
				}
			});
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if( 2.0f < aging_timer)
				{
					if( select_prize_index.Value == 0)
					{
						GameMain.Instance.m_panelBattlePrize.OnClickHolder.Invoke(1);
					}
					else
					{
						GameMain.Instance.m_panelGameControlButtons.OnClickButton.Invoke(0);
					}
					aging_timer = 0.0f;
				}
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			GameMain.Instance.m_panelBattlePrize.OnClickHolder.RemoveAllListeners();
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}

	}



	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Mission : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();
			PanelMission.Instance.OnFinished.AddListener(OnFinished);

			MasterStageMissionParam m = DataManagerGame.Instance.masterStageMission.Select(stage_id.Value, wave.Value);

			PanelMission.Instance.RequestMission(m.mission_id);
		}

		private void OnFinished()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			if (PanelMission.Instance != null)
			{
				PanelMission.Instance.OnFinished.RemoveListener(OnFinished);
			}
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class PartyEdit : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			PanelLogMessage.Instance.AddMessage(string.Format("パーティーの入れ替えが可能です"));

			gameMain.m_panelPartyEdit.gameObject.SetActive(true);

			gameMain.m_panelPartyEdit.OnFinished.AddListener(() =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			gameMain.m_panelPartyEdit.gameObject.SetActive(false);
			gameMain.m_panelPartyEdit.OnFinished.RemoveAllListeners();
		}
	}




	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class EnterGoal : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_bIsGoal = true;
			Finish();
		}

	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class MoveEnt : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (gameMain.m_bIsGoal)
			{
				Fsm.Event("goal");
			}
			else if (gameMain.m_bIsGameover)
			{
				Fsm.Event("gameover");
			}
			else {
				Finish();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class CheckNextWave : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;

		public override void OnEnter()
		{
			base.OnEnter();

			MasterStageParam master_stage = DataManagerGame.Instance.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			if( master_stage.total_wave <= wave.Value)
			{
				Finish();
			}
			else
			{
				Fsm.Event("next");
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class NextWave : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.m_fadeScreen.Close();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if(gameMain.m_fadeScreen.is_open == false)
			{
				Finish();
			}
		}
	}



	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class ShowResult : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			gameMain.m_panelResult.gameObject.SetActive(true);
			StartCoroutine(gameMain.m_panelResult.show(
				PrizeList.Instance.m_iFood,
				PrizeList.Instance.m_iMana,
				PrizeList.Instance.m_iGem));

			gameMain.m_panelResult.m_btn.onClick.AddListener(() =>
			{
				gameMain.m_panelResult.gameObject.SetActive(false);
				gameMain.m_panelResult.m_btn.onClick.RemoveAllListeners();
				Finish();
			});
		}

	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SaveResult : GameMainActionBase
	{
		public FsmInt stage_id;
		public FsmBool is_win;

		public override void OnEnter()
		{
			base.OnEnter();
			bool request_review = false;
			gameMain.m_adsBannerBottom.Hide();
			//gameMain.m_adsBannerTop.Hide();

			if ( gameMain.m_bIsGoal)
			{
				if (is_win.Value)
				{
					request_review = 0 == DataManagerGame.Instance.dataStage.list.FindAll(p => 0 < p.clear_count).Count;

					DataStageParam data_stage = DataManagerGame.Instance.dataStage.list.Find(p => p.stage_id == stage_id.Value);
					data_stage.clear_count += 1;
					if (GameMain.Instance.m_iCountCardPlay < data_stage.best_play)
					{
						data_stage.best_play = GameMain.Instance.m_iCountCardPlay;
					}
					if (GameMain.Instance.m_iCountDeck < data_stage.best_reload)
					{
						data_stage.best_reload = GameMain.Instance.m_iCountDeck;
					}

					// ゲームデータから取得しないあたりちょっとゆがんだプログラムな気がする
					DataManagerGame.Instance.user_data.AddInt(Defines.KeyFood, PrizeList.Instance.m_iFood);
					DataManagerGame.Instance.user_data.AddInt(Defines.KeyMana, PrizeList.Instance.m_iMana);
					DataManagerGame.Instance.user_data.AddInt(Defines.KeyGem, PrizeList.Instance.m_iGem);

					DataManagerGame.Instance.dataStage.Save();
				}
			}

			// ユーザーデータはここで保存
			DataManagerGame.Instance.user_data.Write(Defines.KEY_GAMEMODE, "camp");
			DataManagerGame.Instance.user_data.Save();

			DataUnit unit_camp = new DataUnit();
			unit_camp.SetSaveFilename(Defines.FILENAME_UNIT_CAMP);
			if (unit_camp.Load())
			{
				foreach (DataUnitParam party_unit in DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara"))
				{
					DataUnitParam unit = unit_camp.list.Find(p => p.chara_id == party_unit.chara_id);
					if (unit != null)
					{
						int set_tension = Mathf.Max(party_unit.tension - 5, 0);
						unit.tension = set_tension;
					}
				}
				// テンションのみ修正
				unit_camp.Save();
			}


			if (request_review)
			{
				Fsm.Event("review");
			}
			else
			{
				Finish();
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Review : GameMainActionBase
	{
		public FsmGameObject panel;
		public FsmGameObject button;
		public override void OnEnter()
		{
			base.OnEnter();
			panel.Value.SetActive(true);

			button.Value.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
			{
				DataPresent dataPresent = new DataPresent();
				dataPresent.SetSaveFilename(Defines.FILENAME_DATA_PRESENT);
				dataPresent.LoadMulti();
				dataPresent.Add(1, 30, "ステージ初クリアのお祝い");
				DataManagerGame.Instance.user_data.AddInt(Defines.KeyGem, 30);
				dataPresent.Save();
#if UNITY_IOS


			if (Device.RequestStoreReview())
			{
				Finish();
			}
			else
			{
				Finish();
			}
#else
				Finish();
#endif


			});
		}

		public override void OnExit()
		{
			base.OnExit();
			panel.Value.SetActive(false);
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GameMenu : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			PrizeList.Instance.m_bMenu = true;
			//SEControl.Instance.Play("cursor_01");

			GameMain.Instance.m_panelGameMenu.gameObject.SetActive(true);
			GameMain.Instance.m_panelGameMenu.Show();

			GameMain.Instance.m_panelGameMenu.m_bannerRetire.m_btn.onClick.AddListener(() =>
			{
				Fsm.Event("retire");
			});


			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				Fsm.Event("close");
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(0, null);


			// 各ボタンのイベント削除
			GameMain.Instance.m_panelGameMenu.m_bannerRetire.m_btn.onClick.RemoveAllListeners();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class GameMenuClose : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.m_panelGameMenu.gameObject.SetActive(false);
			PrizeList.Instance.m_bMenu = false;
			//SEControl.Instance.Play("cancel_01");

			Finish();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class RetireCheck : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.m_panelGameMenu.m_goRetireAttention.SetActive(true);

			GameMain.Instance.m_panelGameControlButtons.ShowButtonNum(2, new string[2] { "キャンセル","あきらめる" });

			GameMain.Instance.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				if (_iIndex == 0)
				{
					GameMain.Instance.m_panelGameMenu.m_goRetireAttention.SetActive(false);
					Fsm.Event("cancel");
				}
				else
				{
					GameMain.Instance.m_panelGameMenu.gameObject.SetActive(false);
					Fsm.Event("retire");
				}
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			GameMain.Instance.m_panelGameMenu.m_goRetireAttention.SetActive(false);
			GameMain.Instance.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}

	}


	#region tutorial

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class TutorialMessage : FsmStateAction
	{
		public FsmString message;
		public FsmString message2;
		public override void OnEnter()
		{
			base.OnEnter();

			PanelLogMessage.Instance.AddMessage(message.Value);
			if( message2.Value != "")
			{
				PanelLogMessage.Instance.AddMessage(message2.Value);
			}
			PanelTutorial.Instance.m_goRoot.SetActive(true);
			PanelTutorial.Instance.m_txtMessage.text = message.Value + message2.Value;
			PanelTutorial.Instance.m_img.gameObject.SetActive(false);
			PanelTutorial.Instance.m_btn.onClick.AddListener(() =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			if (PanelTutorial.Instance != null)
			{
				PanelTutorial.Instance.m_goRoot.SetActive(false);
				PanelTutorial.Instance.m_btn.onClick.RemoveAllListeners();
			}
		}
	}

	[ActionCategory("Common")]
	[HutongGames.PlayMaker.Tooltip("Common")]
	public class TutorialImage : FsmStateAction
	{
		public FsmString image_name;
		public override void OnEnter()
		{
			base.OnEnter();

			PanelTutorial.Instance.m_goRoot.SetActive(true);
			PanelTutorial.Instance.m_txtMessage.text = "";
			PanelTutorial.Instance.m_img.gameObject.SetActive(true);
			PanelTutorial.Instance.m_img.sprite = SpriteManager.Instance.Get(image_name.Value);
			PanelTutorial.Instance.m_btn.onClick.AddListener(() =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			if (PanelTutorial.Instance != null)
			{
				PanelTutorial.Instance.m_goRoot.SetActive(false);
				PanelTutorial.Instance.m_btn.onClick.RemoveAllListeners();
			}
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class TutorialEnd : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			// これだけでチュートリアル完了扱い？
			DataManagerGame.Instance.user_data.Write(Defines.KEY_GAMEMODE, "camp");
			// 一応入れておく
			DataManagerGame.Instance.user_data.WriteInt("tutorial_status", 1);

			DataManagerGame.Instance.user_data.Save();

			Finish();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class PlayAdsMovie : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if (DataManagerGame.Instance.gameData.HasKey(Defines.KeyMoviePlay_GameEnd))
			{
				if (Advertisement.IsReady())
				{
					var options = new ShowOptions { resultCallback = HandleShowResult };
					Advertisement.Show(options);
				}
				else
				{
					RewardAd.Instance.rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
					RewardAd.Instance.RequestRewardBasedVideo();
				}
			}
			else
			{
				Finish();
			}


		}

		private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
		{
			RewardAd.Instance.rewardBasedVideo.OnAdLoaded -= HandleRewardBasedVideoLoaded;

			if (RewardAd.Instance.rewardBasedVideo.IsLoaded())
			{
				RewardAd.Instance.rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
				RewardAd.Instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
				RewardAd.Instance.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
				RewardAd.Instance.rewardBasedVideo.Show();
			}
			else
			{
				Finish();
			}

		}
		private void HandleRewardBasedVideoRewarded(object sender, Reward e)
		{
			//rewarded = true;
		}
		private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
		{
			Finish();
		}
		private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
		{

		}

		private void HandleShowResult(UnityEngine.Advertisements.ShowResult obj)
		{
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();

			RewardAd.Instance.rewardBasedVideo.OnAdStarted -= HandleRewardBasedVideoStarted;
			RewardAd.Instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
			RewardAd.Instance.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;

		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class SelectRequest : GameMainActionBase
	{
		public FsmInt request_serial;
		public FsmInt select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			select_card_serial.Value = 0;

			foreach (Card card in gameMain.card_list_hand)
			{
				if( card.data_card.card_serial == request_serial.Value)
				{
					card.OnClickCard.AddListener(OnClickCard);
				}
			}
		}
		private void OnClickCard(int arg0)
		{
			if (select_card_serial.Value == arg0)
			{
				Fsm.Event("select");
			}
			else {
				select_card_serial.Value = arg0;
				gameMain.CardSelectUp(select_card_serial.Value);

				DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

				GameMain.Instance.SelectCharaId = card.chara_id;
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			foreach (Card card in gameMain.card_list_hand)
			{
				card.OnClickCard.RemoveListener(OnClickCard);
			}
		}
	}

	#endregion
}