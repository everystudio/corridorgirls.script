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
		public FsmInt stage_id;
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
				stage_id.Value = simulator.stage_id;
				Finish();
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
					Debug.Log("hand:" + card.card_serial.ToString());
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
		public FsmInt current_index;
		public FsmString corridor_type;
		public override void OnEnter()
		{
			base.OnEnter();

			StartCoroutine(move_wait());

		}

		private IEnumerator move_wait()
		{
			yield return null;
			DataCardParam select_card = simulator.dataCard.RandomSelectFromHand();

			DataCorridorParam current_corridor = simulator.dataCorridor.list.Find(p => p.index == current_index.Value);

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
				appear_card = DataManagerGame.Instance.masterStageCard.list.FindAll(p => p.stage_id == stage_id.Value && p.wave == 0);
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


			MasterCardParam master = simulator.masterCard.list.Find(p => p.card_id == add_card.card_id);

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
		public override void OnEnter()
		{
			base.OnEnter();
			if(simulator.dataCard.CardFill(5))
			{
				simulator.dataCard.DeckShuffle();
				simulator.dataCard.CardFill(5);
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
	public class enemy_create : GameSimulatorActionBase
	{
		public FsmInt stage_id;
		public FsmInt wave;
		public override void OnEnter()
		{
			base.OnEnter();
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
			create_enemy(appear_enemy_list[index].enemy_id);
		}

		private void create_enemy( int _enemy_chara_id)
		{
			simulator.dataUnit.list.RemoveAll(p => p.unit == "enemy");
			MasterCharaParam master_enemy = simulator.masterChara.list.Find(p => p.chara_id == _enemy_chara_id);
			DataUnitParam enemy = DataUnit.MakeUnit(master_enemy, "enemy", 60);
			simulator.dataUnit.list.Add(enemy);

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
	public class battle_turn_start : GameSimulatorActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
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
			select_enemy_card.status = (int)DataCard.STATUS.PLAY;
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
		public FsmInt player_card_serial;
		public FsmInt enemy_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			DataCardParam player_card = simulator.dataCard.list.Find(p => p.card_serial == player_card_serial.Value);
			DataCardParam enemy_card = simulator.dataCardEnemy.list.Find(p => p.card_serial == enemy_card_serial.Value);

















		}



	}

	[ActionCategory("GameSimulatorAction")]
	[HutongGames.PlayMaker.Tooltip("GameSimulatorAction")]
	public class battle_result : GameSimulatorActionBase
	{
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



}
