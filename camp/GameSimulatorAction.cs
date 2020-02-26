using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameSimulatorAction{

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class GameSimulatorActionBase : FsmStateAction
	{
		protected GameSimulator simulator;
		public override void OnEnter()
		{
			base.OnEnter();
			simulator = Owner.GetComponent<GameSimulator>();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class standby : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			simulator.start_simulation = false;

			simulator.load_masterdata();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if(simulator.start_simulation)
			{
				simulator.stage_id_index =-1;
				Finish();
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			simulator.start_simulation = false;
		}
	}


	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class set_stage_id : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt level;
		public FsmInt simulation_count;
		public override void OnEnter()
		{
			base.OnEnter();

			simulator.stage_id_index += 1;
			level.Value = simulator.chara_level;
			simulation_count.Value = simulator.simulation_count;

			if (simulator.stage_id_index < simulator.stage_id_list.Count)
			{
				stage_id.Value = simulator.stage_id_list[simulator.stage_id_index];
				Finish();
			}
			else
			{
				Fsm.Event("end");
			}

		}
	}





	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class party_initialize : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			simulator.party_initialize();
			Finish();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class card_initialize : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			simulator.card_initialize();

			foreach( DataCardParam card in simulator.dataCard.list)
			{
				if( card.status == (int)DataCard.STATUS.HAND)
				{
					//Debug.Log("hand:" + card.card_serial.ToString());
				}
			}
			Finish();
		}
	}


	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class create_stage : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();
			simulator.create_stage(stage_id.Value, wave.Value);
			Finish();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class move : GameSimulatorActionBase
	{
		public FsmInt card_play_count;
		public FsmInt mp;
		public FsmInt current_index;
		public FsmString corridor_type;
		public override void OnEnter()
		{
			base.OnEnter();

			StartCoroutine(move_wait());

		}

		private IEnumerator move_wait()
		{
			simulator.move_delay += 1;
			if (200 < simulator.move_delay)
			{
				simulator.move_delay = 0;
				yield return null;
			}


			DataCardParam select_card = simulator.dataCard.RandomSelectFromHand();

			DataCorridorParam current_corridor = simulator.dataCorridor.list.Find(p => p.index == current_index.Value);
			card_play_count.Value += 1;
			mp.Value += select_card.power;
			if(30 < mp.Value)
			{
				mp.Value = 30;
			}
			for (int i = 0; i < select_card.power; i++)
			{
				current_index.Value = current_corridor.next_index;
				current_corridor = simulator.dataCorridor.list.Find(p => p.index == current_index.Value);

				if (current_corridor.corridor_event.corridor_event_id == 2)
				{
					// ここダサすぎる
					break;
				}
			}

			select_card.status = (int)DataCard.STATUS.REMOVE;
			corridor_type.Value = current_corridor.corridor_event.corridor_type;
			Fsm.Event(current_corridor.corridor_event.corridor_type);


		}
	}
	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class prize : GameSimulatorActionBase
	{
		public FsmInt current_index;
		public override void OnEnter()
		{
			base.OnEnter();
			DataCorridorParam current_corridor = simulator.dataCorridor.list.Find(p => p.index == current_index.Value);
			Fsm.Event(current_corridor.corridor_event.sub_type);
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class get_card : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();

			List<MasterStageCardParam> appear_card = simulator.masterStageCard.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == wave.Value);
			if (appear_card.Count == 0)
			{
				appear_card = simulator.masterStageCard.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == 0);
			}

			int[] item_prob = new int[appear_card.Count];
			//Debug.Log(appear_card.Count);
			for (int i = 0; i < appear_card.Count; i++)
			{
				item_prob[i] = appear_card[i].prob;
			}

			int index = UtilRand.GetIndex(item_prob);

			DataUnitParam left_chara = simulator.dataUnit.list.Find(p => p.unit == "chara" && p.position == "left");

			DataCardParam add_card = new DataCardParam();
			// tempシリアルを配布
			//add_card.card_id = appear_card[index].card_id;
			//add_card.chara_id = left_chara.chara_id;
			//Debug.Log(add_card.chara_id);


			MasterCardParam master = simulator.masterCard.list.Find(p => p.card_id == appear_card[index].card_id);

			if( master == null)
			{

			}


			// シリアルはAddNewCardで設定し直し
			add_card.Copy(master, left_chara.chara_id, 0);

			simulator.dataCard.AddNewCard(add_card, DataCard.STATUS.DECK);
			Finish();
		}
	}



	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class card_fill : GameSimulatorActionBase
	{
		public FsmInt deck_reload_count;
		public override void OnEnter()
		{
			base.OnEnter();
			if(simulator.dataCard.CardFill(5)==false)
			{
				deck_reload_count.Value += 1;
				simulator.dataCard.DeckShuffle();
				if( simulator.dataCard.CardFill(5) == false)
				{
					Debug.LogError("warning player_card_fill");

					foreach ( DataCardParam card in simulator.dataCard.list)
					{
						Debug.Log(string.Format("card_id{0} serial:{1} status:{2}",
							card.card_id,
							card.card_serial,
							(DataCard.STATUS)card.status));
					}
				}
			}
			Finish();
		}
	}


	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class battle : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class battle_turn_start : GameSimulatorActionBase
	{

		public FsmInt mp;
		public override void OnEnter()
		{
			base.OnEnter();

			//Debug.Log(string.Format("mp:{0}", mp.Value));

			List<DataUnitParam> damage_unit = simulator.dataUnit.list.FindAll(p => p.unit == "chara" && 0 < p.hp && p.hp < p.hp_max);
			if( 0 < damage_unit.Count && 20 < mp.Value)
			{
				mp.Value -= 20;
				foreach( DataUnitParam unit in damage_unit)
				{
					unit.HpHeal(10);
				}
				Debug.Log("<color=yellow>回復</color>");
			}

			Finish();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class enemy_create : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public FsmBool is_boss;
		public override void OnEnter()
		{
			base.OnEnter();

			int enemy_chara_id = 0;

			if (is_boss.Value == false) {
				Debug.Log("battle_start");

				List<MasterStageEnemyParam> appear_enemy_list = simulator.masterStageEnemy.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == wave.Value);
				if (appear_enemy_list.Count == 0)
				{
					appear_enemy_list = simulator.masterStageEnemy.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == 0);
				}
				int[] enemy_prob = new int[appear_enemy_list.Count];
				for (int i = 0; i < appear_enemy_list.Count; i++)
				{
					enemy_prob[i] = appear_enemy_list[i].prob;
				}

				int index = UtilRand.GetIndex(enemy_prob);
				// chara_id = enemy_idです
				//GameMain.Instance.battleMain.RequestBattle.Invoke(false, appear_enemy_list[index].enemy_id);
				enemy_chara_id = appear_enemy_list[index].enemy_id;
			}
			else
			{
				MasterStageParam master_stage = simulator.masterStage.list.Find(p => p.stage_id == stage_id.Value);

				enemy_chara_id = master_stage.boss_chara_id;
			}
			create_enemy(enemy_chara_id);

			Finish();
		}

		private void create_enemy( int _enemy_chara_id)
		{
			simulator.dataUnit.list.RemoveAll(p => p.unit == "enemy");
			MasterCharaParam master_enemy = simulator.masterChara.list.Find(p => p.chara_id == _enemy_chara_id);
			DataUnitParam enemy = DataUnit.MakeUnit(master_enemy,1, "enemy", 60);
			simulator.dataUnit.list.Add(enemy);

			//Debug.LogError(_enemy_chara_id);

			simulator.dataCardEnemy.list.Clear();
			int iSerial = 1;
			foreach (MasterCharaCardParam cc in simulator.masterCharaCard.list.FindAll(p => p.chara_id == _enemy_chara_id))
			{
				DataCardParam add = new DataCardParam();
				MasterCardParam master_card = simulator.masterCard.list.Find(p => p.card_id == cc.card_id);
				add.Copy(master_card, _enemy_chara_id, iSerial);
				simulator.dataCardEnemy.list.Add(add);
				iSerial += 1;
			}
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class enemy_card : GameSimulatorActionBase
	{
		public FsmInt enemy_card_id;
		public FsmInt enemy_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			if (simulator.dataCardEnemy.CardFill(2) == false)
			{
				simulator.dataCardEnemy.DeckShuffle();
				if (simulator.dataCardEnemy.CardFill(2) == false)
				{
					Debug.LogError("warning enemy deck");
				}
			}
			DataCardParam select_enemy_card = simulator.dataCardEnemy.RandomSelectFromHand();
			select_enemy_card.status = (int)DataCard.STATUS.REMOVE;
			enemy_card_id.Value = select_enemy_card.card_id;
			enemy_card_serial.Value = select_enemy_card.card_serial;
			Finish();
		}

	}


	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class player_card : GameSimulatorActionBase
	{
		public FsmInt enemy_card_id;
		public FsmInt enemy_card_serial;

		public FsmInt player_card_serial;

		public override void OnEnter()
		{
			base.OnEnter();

			DataCardParam data_enemy_card = simulator.dataCardEnemy.list.Find(p => p.card_serial == enemy_card_serial.Value);
			MasterCardParam master_enemy_card = simulator.masterCard.list.Find(p => p.card_id == enemy_card_id.Value);

			int iSerialPlayerCard = simulator.dataCard.SelectBattleCard_FromHand(
				master_enemy_card,
				simulator.masterCard.list,
				simulator.masterCardSymbol.list);

			if(iSerialPlayerCard == 0)
			{
				iSerialPlayerCard = simulator.dataCard.RandomSelectFromHand().card_serial;
			}

			player_card_serial.Value = iSerialPlayerCard;

			Finish();
		}

	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class fight : GameSimulatorActionBase
	{
		public FsmInt card_play_count;
		public FsmInt mp;
		public FsmInt player_card_serial;
		public FsmInt enemy_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			DataCardParam player_card = simulator.dataCard.list.Find(p => p.card_serial == player_card_serial.Value);
			DataCardParam enemy_card = simulator.dataCardEnemy.list.Find(p => p.card_serial == enemy_card_serial.Value);
			player_card.status = (int)DataCard.STATUS.REMOVE;

			card_play_count.Value += 1;
			mp.Value += player_card.power;
			if (30 < mp.Value )
			{
				mp.Value = 30;
			}
			DataCard.Offset(player_card, enemy_card);

			DataUnitParam player_unit = simulator.dataUnit.list.Find(p => p.chara_id == player_card.chara_id && p.unit == "chara");
			DataUnitParam enemy_unit = simulator.dataUnit.list.Find(p => p.unit == "enemy");

			player_unit.Attack_Sim(player_unit.str, 1001, player_card, enemy_unit);

			if( enemy_unit.hp <= 0 )
			{
				Fsm.Event("win");
				return;
			}
			else
			{
				enemy_unit.Attack_Sim(enemy_unit.str, 1001, enemy_card, player_unit);
			}

			if( player_unit.hp <= 0)
			{
				Fsm.Event("dead");
				return;
			}

			player_unit.Attack_Sim(player_unit.magic, 3001, player_card, enemy_unit);

			if (enemy_unit.hp <= 0)
			{
				Fsm.Event("win");
				return;
			}
			else
			{
				enemy_unit.Attack_Sim(enemy_unit.magic, 3001, enemy_card, player_unit);
			}

			if (player_unit.hp <= 0)
			{
				Fsm.Event("dead");
				return;
			}

			Finish();
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class dead_player : GameSimulatorActionBase
	{
		public FsmInt player_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			DataCardParam player_card = simulator.dataCard.list.Find(p => p.card_serial == player_card_serial.Value);

			Debug.Log(player_card.chara_id);
			DataUnitParam battle_chara = simulator.dataUnit.list.Find(p => p.chara_id == player_card.chara_id && p.unit == "chara");

			List<DataUnitParam> back_chara_list = simulator.dataUnit.list.FindAll(p => p.unit == "chara" && p.position == "back");

			bool bChangeMember = false;
			foreach (DataUnitParam back_unit in back_chara_list)
			{
				// 選手交代
				if (0 < back_unit.hp)
				{
					back_unit.position = battle_chara.position;
					battle_chara.position = "back";
					foreach (DataCardParam card in simulator.dataCard.list.FindAll(p => p.chara_id == back_unit.chara_id))
					{
						card.status = (int)DataCard.STATUS.REMOVE;
					}
					bChangeMember = true;
				}
			}
			if (bChangeMember)
			{
				Debug.Log("メンバー交代");
				//GameMain.Instance.panelStatus.Initialize(simulator.dataUnit, simulator.masterChara);
			}
			else
			{
			}

			foreach (DataCardParam card in simulator.dataCard.list.FindAll(p => p.chara_id == battle_chara.chara_id))
			{
				card.status = (int)DataCard.STATUS.NOTUSE;
			}

			if (simulator.dataUnit.IsAliveParty())
			{
				//Fsm.Event("dead");
				Finish();
			}
			else
			{
				Fsm.Event("gameover");
			}
		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class battle_bonus : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();

			List<MasterStageBattleBonusParam> stage_bb_list = simulator.masterStageBattleBonus.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == wave.Value);
			if (stage_bb_list.Count == 0)
			{
				stage_bb_list = simulator.masterStageBattleBonus.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == 0);
			}
			if (stage_bb_list.Count == 0)
			{
				stage_bb_list = simulator.masterStageBattleBonus.list.FindAll(p => p.stage_id == 0 && p.wave == wave.Value);
			}

			int[] prob_arr = new int[stage_bb_list.Count];
			for (int i = 0; i < stage_bb_list.Count; i++)
			{
				prob_arr[i] = stage_bb_list[i].prob;
				//Debug.Log(string.Format("index={0} prob={1}", i, prob_arr[i]));
			}

			foreach (DataUnitParam unit in simulator.dataUnit.list.FindAll(p => p.unit == "chara"))
			{
				int index = UtilRand.GetIndex(prob_arr);
				//Debug.Log(index);
				MasterBattleBonusParam master_bb = simulator.masterBattleBonus.list.Find(p => p.battle_bonus_id == stage_bb_list[index].battle_bonus_id);

				//Debug.Log(master_bb.battle_bonus_id);
				//Debug.Log(master_bb.field);
				//Debug.Log(master_bb.param);

				simulator.dataUnit.AddAssist(unit, "bb", "バトルボーナス",
					unit.chara_id,
					master_bb.field,
					master_bb.param,
					-1);
			}

			Finish();

		}

	}


	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class goal_check : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();

			MasterStageParam master_stage = simulator.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			if( master_stage.total_wave <= wave.Value)
			{
				Fsm.Event("boss");
			}
			else
			{
				Fsm.Event("next");
			}

		}
	}
	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class data_record : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt level;
		public FsmInt wave;
		public FsmInt play_count;
		public FsmInt reload_count;
		public FsmBool is_boss;
		public FsmBool is_clear;

		public override void OnEnter()
		{
			base.OnEnter();

			MasterStageParam master_stage = simulator.masterStage.list.Find(p => p.stage_id == stage_id.Value);

			DataSimulation dataSimulation = new DataSimulation();
			dataSimulation.SetSaveFilename("data_simulation");
			if (dataSimulation.LoadMulti())
			{
				DataSimulationParam param = dataSimulation.list.Find(p => p.stage_id == stage_id.Value && p.level == level.Value);

				if( param == null)
				{
					param = new DataSimulationParam();
					param.stage_id = stage_id.Value;
					param.level = level.Value;
					dataSimulation.list.Add(param);
				}
				param.count += 1;

				switch ( wave.Value)
				{
					case 1:
						param.wave_1 += 1;
						break;
					case 2:
						param.wave_2 += 1;
						break;
					case 3:
						param.wave_3 += 1;
						break;
					case 4:
						param.wave_4 += 1;
						break;
					case 5:
						param.wave_5 += 1;
						break;
				}
				if(is_boss.Value)
				{
					param.arrive_boss += 1;
				}

				if (is_clear.Value)
				{
					param.clear += 1;
					param.clear_play_count += play_count.Value;
					param.clear_reload_count += reload_count.Value;
				}
			}
			dataSimulation.Save();
			Finish();

		}
	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class output_status : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			List<DataUnitParam> unit_list = simulator.dataUnit.list.FindAll(p => p.unit == "chara");

			foreach( DataUnitParam unit in unit_list)
			{
				Debug.Log(string.Format("chara_id:{0} HP({1}/{2}) STR({3}) MAGIC({4}) HEAL({5})",
					unit.chara_id,
					unit.hp,
					unit.hp_max,
					unit.str,
					unit.magic,
					unit.heal));
			}



			Finish();

		}
	}




}
