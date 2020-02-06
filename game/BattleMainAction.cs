using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleMainAction
{

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class BattleMainActionBase : FsmStateAction
	{
		protected BattleMain battleMain;
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain = Owner.GetComponent<BattleMain>();
		}
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class startup : BattleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(DataManagerGame.Instance.Initialized)
			{
				Finish();
			}
		}
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class wait : BattleMainActionBase
	{
		public FsmInt enemy_chara_id;
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.RequestBattle.AddListener((int _enemy_id)=>{
				enemy_chara_id.Value = _enemy_id;
				Debug.Log(string.Format("enemy:chara_id={0}", enemy_chara_id.Value));
				Finish();
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			battleMain.RequestBattle.RemoveAllListeners();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class opening : BattleMainActionBase
	{
		public FsmInt enemy_chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			battleMain.Opening();

			battleMain.m_animChara.Play("idle");
			battleMain.m_animEnemy.Play("idle");


			if (battleMain.player_card != null)
			{
				GameObject.Destroy(battleMain.player_card.gameObject);
				battleMain.player_card = null;
			}
			if (battleMain.enemy_card != null)
			{
				GameObject.Destroy(battleMain.enemy_card.gameObject);
				battleMain.enemy_card = null;
			}
			battleMain.dataCardEnemy.list.Clear();

			BattleIcon[] arr = battleMain.m_goBattleChara.GetComponentsInChildren<BattleIcon>();
			foreach (BattleIcon c in arr)
			{
				GameObject.Destroy(c.gameObject);
			}
			arr = battleMain.m_goBattleEnemy.GetComponentsInChildren<BattleIcon>();
			foreach (BattleIcon c in arr)
			{
				GameObject.Destroy(c.gameObject);
			}

			// 敵のデッキデータ
			//Debug.LogWarning(DataManagerGame.Instance.masterCharaCard.list.FindAll(p => p.chara_id == enemy_chara_id.Value).Count);




			// 敵は１体消して新規に追加
			DataManagerGame.Instance.dataUnit.list.RemoveAll(p => p.unit == "enemy");

			MasterCharaParam master_enemy = DataManagerGame.Instance.masterChara.list.Find(p => p.chara_id == enemy_chara_id.Value);

			// 敵にこっそりテンションを入れるならここ
			DataUnitParam enemy = DataUnit.MakeUnit(master_enemy, "enemy", 60);
			DataManagerGame.Instance.dataUnit.list.Add(enemy);

			battleMain.m_sprEnemy.sprite = SpriteManager.Instance.Get(master_enemy.sprite_name);

			int iSerial = 1;
			foreach( MasterCharaCardParam cc in DataManagerGame.Instance.masterCharaCard.list.FindAll(p=>p.chara_id == enemy_chara_id.Value))
			{
				DataCardParam add = new DataCardParam();

				MasterCardParam master_card = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == cc.card_id);

				add.Copy(master_card, enemy_chara_id.Value, iSerial);

				battleMain.dataCardEnemy.list.Add(add);

				iSerial += 1;
			}

			battleMain.HpRefresh();


			battleMain.OnOpeningEnd.AddListener(() =>
			{
				battleMain.OnOpeningEnd.RemoveAllListeners();
				Finish();
			});
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TurnStart : BattleMainActionBase
	{
		public FsmInt card_fill_num;
		public override void OnEnter()
		{
			base.OnEnter();

			BattleIcon[] arr = battleMain.m_goBattleChara.GetComponentsInChildren<BattleIcon>();
			foreach (BattleIcon c in arr)
			{
				GameObject.Destroy(c.gameObject);
			}
			arr = battleMain.m_goBattleEnemy.GetComponentsInChildren<BattleIcon>();
			foreach (BattleIcon c in arr)
			{
				GameObject.Destroy(c.gameObject);
			}

			// バトルカードを消すタイミングはここでいいのかなんか不安
			// アイコンが出た後はすぐに消えてもいいのでは？
			// とりあえず証拠としてターン開始まで出しっぱなしにする
			if (battleMain.player_card != null)
			{
				GameObject.Destroy(battleMain.player_card.gameObject);
				battleMain.player_card = null;
			}
			if (battleMain.enemy_card != null)
			{
				GameObject.Destroy(battleMain.enemy_card.gameObject);
				battleMain.enemy_card = null;
			}

			int hand_card_num = DataManagerGame.Instance.dataCard.list.FindAll(p => p.status == (int)DataCard.STATUS.HAND).Count;
			Debug.Log(hand_card_num);
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

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class Idle : BattleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.gameMain.m_panelGameControlButtons.ShowButtonNum(0, null);

			battleMain.gameMain.m_panelStatus.m_btnStatus.onClick.AddListener(() =>
			{
				Fsm.Event("status");
			});
			battleMain.gameMain.m_panelStatus.m_btnItem.onClick.AddListener(() =>
			{
				Fsm.Event("item");
			});
			battleMain.gameMain.m_panelStatus.m_btnDeck.onClick.AddListener(() =>
			{
				Fsm.Event("deck");
				//m_panelPlayerDeck.Show();
			});
		}
		public override void OnExit()
		{
			base.OnExit();

			battleMain.gameMain.m_panelStatus.m_btnStatus.onClick.RemoveAllListeners();
			battleMain.gameMain.m_panelStatus.m_btnItem.onClick.RemoveAllListeners();
			battleMain.gameMain.m_panelStatus.m_btnDeck.onClick.RemoveAllListeners();

		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class ShowDeck : BattleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.gameMain.m_panelPlayerDeck.m_btnClose.gameObject.SetActive(false);
			battleMain.gameMain.m_panelPlayerDeck.Show();

			battleMain.gameMain.m_panelGameControlButtons.ShowButtonNum(1, new string[1] { "閉じる" });

			battleMain.gameMain.m_panelGameControlButtons.OnClickButton.AddListener((int _iIndex) =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			battleMain.gameMain.m_panelPlayerDeck.Close();
			battleMain.gameMain.m_panelGameControlButtons.OnClickButton.RemoveAllListeners();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class ShowItem : BattleMainActionBase
	{
		public FsmInt damage;
		public override void OnEnter()
		{
			base.OnEnter();

			ItemMain.Instance.damage = 0;

			ItemMain.Instance.RequestShow.Invoke("battle");

			ItemMain.Instance.OnClose.AddListener(() =>
			{
				Debug.Log(ItemMain.Instance.damage);
				if (0 < ItemMain.Instance.damage)
				{
					damage.Value = ItemMain.Instance.damage;
					Fsm.Event("damage");
				}
				else {
					Finish();
				}
			});

		}
		public override void OnExit()
		{
			base.OnExit();
			ItemMain.Instance.OnClose.RemoveAllListeners();
		}
	}



	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class card_select_battle : BattleMainActionBase
	{
		public FsmInt select_card_serial;
		public FsmInt select_chara_id;

		private float aging_timer;

		public override void OnEnter()
		{
			base.OnEnter();

			aging_timer = 0.0f;

			foreach (Card card in battleMain.gameMain.card_list_hand)
			{
				card.OnClickCard.AddListener(OnClickCard);
			}
		}
		private void OnClickCard(int arg0)
		{
			bool notexist = true;
			foreach (Card dc in GameMain.Instance.card_list_hand)
			{
				if (dc.data_card.card_serial == arg0)
				{
					notexist = false;
				}
			}
			if (notexist)
			{
				Debug.Log(arg0);
				foreach (Card dc in GameMain.Instance.card_list_hand)
				{
					Debug.Log(dc.data_card.card_serial);
				}
			}

			if (select_card_serial.Value == arg0)
			{
				Fsm.Event("select");
				Card selected_card = battleMain.gameMain.card_list_hand.Find(p => p.data_card.card_serial == select_card_serial.Value);

				select_chara_id.Value = selected_card.data_card.chara_id;

				selected_card.data_card.status = (int)DataCard.STATUS.PLAY;
				selected_card.m_animator.SetBool("delete", true);
				battleMain.gameMain.card_list_hand.Remove(selected_card);
				battleMain.gameMain.CardOrder();
			}
			else {
				select_card_serial.Value = arg0;
				battleMain.gameMain.CardSelectUp(select_card_serial.Value);
				DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

				battleMain.gameMain.SelectCharaId = card.chara_id;
				battleMain.HpRefresh();

				Fsm.Event("touch");
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			#region Aging
			if (DataManagerGame.Instance.IsAging)
			{
				aging_timer += Time.deltaTime;
				if (3.0f < aging_timer)
				{
					if (0 == select_card_serial.Value)
					{
						int index = UtilRand.GetRand(battleMain.gameMain.card_list_hand.Count);
						OnClickCard(battleMain.gameMain.card_list_hand[index].data_card.card_serial);
					}
					else
					{
						OnClickCard(select_card_serial.Value);
					}
				}
			}
			#endregion
		}
		public override void OnExit()
		{
			base.OnExit();
			foreach (Card card in battleMain.gameMain.card_list_hand)
			{
				card.OnClickCard.RemoveListener(OnClickCard);
			}
		}
	}


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class EnemyCard : BattleMainActionBase
	{
		public FsmInt enemy_card_id;
		public FsmInt enemy_card_serial;

		public override void OnEnter()
		{
			base.OnEnter();

			StartCoroutine(exe_main());
		}

		private IEnumerator exe_main()
		{
			yield return new WaitForSeconds(1.5f);

			battleMain.enemy_card = PrefabManager.Instance.MakeScript<Card>(
				battleMain.m_prefCard, battleMain.m_goEnemyCardRoot);

			//Debug.Log(battleMain.dataCardEnemy.list.Count);
			if (battleMain.dataCardEnemy.CardFill(2) == false)
			{
				battleMain.dataCardEnemy.DeckShuffle();
				if (battleMain.dataCardEnemy.CardFill(2) == false)
				{
					Debug.LogError("warning enemy deck");
				}
			}

			DataCardParam select_enemy_card = battleMain.dataCardEnemy.RandomSelectFromHand();

			select_enemy_card.status = (int)DataCard.STATUS.PLAY;
			enemy_card_id.Value = select_enemy_card.card_id;
			enemy_card_serial.Value = select_enemy_card.card_serial;
			//MasterCardParam master_enemy_card = DataManagerGame.Instance.masterCard.list.Find(p => p.card_id == enemy_card_id.Value);
			battleMain.enemy_card.Initialize(select_enemy_card, DataManagerGame.Instance.masterCardSymbol.list);

			Finish();


		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PlayerCard : BattleMainActionBase
	{
		public FsmInt select_card_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			if (battleMain.player_card != null)
			{
				GameObject.Destroy(battleMain.player_card.gameObject);
				battleMain.player_card = null;
			}

			battleMain.player_card = PrefabManager.Instance.MakeScript<Card>(
				battleMain.m_prefCard, battleMain.m_goPlayerCardRoot);

			DataCardParam data_card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);

			battleMain.player_card.Initialize(data_card, DataManagerGame.Instance.masterCardSymbol.list);

			Finish();
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class IconStandby : BattleMainActionBase
	{
		//public FsmInt select_card_serial;
		//public FsmInt enemy_card_id;
		private float time;

		public override void OnEnter()
		{
			base.OnEnter();
			time = 0.0f;
			battleMain.player_icon_list.Clear();
			battleMain.enemy_icon_list.Clear();

			for ( int i = 1; i <= 4; i++)
			{
				int iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.player_card.card_symbol_list.FindAll(p=>p.line == i)){
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleChara);
					icon.Initialize(sym, iIndex,true);
					battleMain.player_icon_list.Add(icon);
					iIndex += 1;
				}
				iIndex = 0;
				foreach (MasterCardSymbolParam sym in battleMain.enemy_card.card_symbol_list.FindAll(p => p.line == i))
				{
					BattleIcon icon = PrefabManager.Instance.MakeScript<BattleIcon>(battleMain.m_prefBattleIcon, battleMain.m_goBattleEnemy);
					icon.Initialize(sym, iIndex,false);
					battleMain.enemy_icon_list.Add(icon);
					iIndex += 1;
				}
			}
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			time += Time.deltaTime;
			if( 2.0f < time)
			{
				Finish();
			}
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsStart : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class SymbolOffset : BattleMainActionBase
	{
		public FsmInt symbol_id_player;
		public FsmInt symbol_id_enemy;

		public FsmInt symbol_id_player_canceler;
		public FsmInt symbol_id_enemy_canceler;

		//private bool m_bMove;
		//private float m_fTime;
		private float move_time;

		private int offset_num;
		private int offset_count;

		public override void OnEnter()
		{
			base.OnEnter();
			move_time = 0.5f;
			//m_fTime = 0.0f;
			offset_num = 0;
			offset_count = 0;
			//Debug.Log("SymbolOffset.OnEnter");

			if ( symbol_id_player_canceler.Value != 0)
			{
				BattleIcon player_icon_canceler = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_player_canceler.Value);
				if(player_icon_canceler != null)
				{
					Finish();
					return;
				}
			}
			if (symbol_id_enemy_canceler.Value != 0)
			{
				BattleIcon enemy_icon_canceler = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_enemy_canceler.Value);
				if (enemy_icon_canceler != null)
				{
					Finish();
					return;
				}
			}

			BattleIcon player_icon = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_player.Value);
			BattleIcon enemy_icon = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id_enemy.Value);

			if( player_icon != null && enemy_icon != null)
			{
				//m_bMove = true;

				player_icon.m_animator.SetTrigger("break");
				enemy_icon.m_animator.SetTrigger("break");

				offset_num += 2;

				player_icon.OnOffsetFinished.AddListener(OnIconOffsetFinished);
				enemy_icon.OnOffsetFinished.AddListener(OnIconOffsetFinished);

				List<BattleIcon> player_icon_list = battleMain.player_icon_list.FindAll(p => p.master_symbol.line == player_icon.master_symbol.line && p != player_icon);
				List<BattleIcon> enemy_icon_list = battleMain.enemy_icon_list.FindAll(p => p.master_symbol.line == enemy_icon.master_symbol.line && p != enemy_icon);

				foreach( BattleIcon icon in player_icon_list)
				{
					icon.move(move_time, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}
				foreach (BattleIcon icon in enemy_icon_list)
				{
					icon.move(move_time, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}

				battleMain.player_icon_list.Remove(player_icon);
				battleMain.enemy_icon_list.Remove(enemy_icon);

			}
			else
			{
				//m_bMove = false;
				Finish();
			}
		}

		private void OnIconOffsetFinished()
		{
			offset_count += 1;

			if( offset_num <= offset_count)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			// 多分いらないと思うけど


			/*
			if (m_bMove)
			{
				m_fTime += Time.deltaTime;
				if(move_time < m_fTime)
				{
					Fsm.Event("continue");
				}
			}
			*/
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class Attack : BattleMainActionBase
	{
		public FsmBool is_player;
		public FsmInt select_chara_id;
		public FsmInt symbol_id;

		private float m_fTime;

		private int attack_count = 0;
		private int result_count = 0;

		public override void OnEnter()
		{
			base.OnEnter();

			attack_count = 1;
			result_count = 0;

			StartCoroutine(exe_attack());
		}

		private bool act_attack()
		{
			bool bRet = false;
			BattleIcon target_icon = null;
			List<BattleIcon> target_list = null;
			if (is_player.Value)
			{
				target_icon = battleMain.player_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.player_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}
			else
			{
				target_icon = battleMain.enemy_icon_list.Find(p => p.master_symbol.card_symbol_id == symbol_id.Value);
				if (target_icon != null)
				{
					target_list = battleMain.enemy_icon_list.FindAll(p => p.master_symbol.line == target_icon.master_symbol.line && p != target_icon);
				}
			}
			if (target_icon != null)
			{
				target_icon.HitHandler.AddListener(OnHit);

				target_icon.m_animator.SetTrigger("attack");
				if (is_player.Value)
				{
					battleMain.player_icon_list.Remove(target_icon);
				}
				else
				{
					battleMain.enemy_icon_list.Remove(target_icon);
				}
				bRet = true;
			}
			if (target_list != null && 0 < target_list.Count)
			{
				foreach (BattleIcon icon in target_list)
				{
					icon.move(Defines.ICON_MOVE_TIME, icon.index - 1, icon.master_symbol.line, icon.is_left);
					icon.index -= 1;
				}
			}
			else
			{
				// ここいらない
				//bRet = false;
			}
			return bRet;
		}

		private void OnDamageFinished()
		{
			result_count += 1;
		}

		private IEnumerator DamageEffectAppear(BattleIcon arg0)
		{
			// この微妙なずらし作業がしたかっただけ
			yield return new WaitForSeconds(0.5f);

			int iDamage = 12;
			int iSwing = UtilRand.GetRand(5) - 2;

			if (is_player.Value)
			{
				DataUnitParam unit_chara = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == select_chara_id.Value);

				switch (arg0.master_symbol.card_symbol_id)
				{
					case Defines.CARD_SYMBOL_ATTACK:
						iDamage = unit_chara.str + iSwing;
						break;
					case Defines.CARD_SYMBOL_MAGIC:
						iDamage = unit_chara.magic + iSwing;
						break;
					default:
						break;
				}
			}
			else
			{
				DataUnitParam unit_enemy = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "enemy");
				switch (arg0.master_symbol.card_symbol_id)
				{
					case Defines.CARD_SYMBOL_ATTACK:
						iDamage = unit_enemy.str + iSwing;
						break;
					case Defines.CARD_SYMBOL_MAGIC:
						iDamage = unit_enemy.magic + iSwing;
						break;
					default:
						break;
				}
			}
			//Debug.Log(iDamage);

			battleMain.Damage(is_player.Value, iDamage, OnDamageFinished);

			// プレイヤー側の攻撃
			if (arg0.is_left)
			{
				DataUnitParam enemy = DataManagerGame.Instance.dataUnit.list.Find(p =>
				p.unit == "enemy");
				//Debug.Log(enemy.hp);
				enemy.hp -= iDamage;
			}
			else
			{
				DataUnitParam select_chara = DataManagerGame.Instance.dataUnit.list.Find(p =>
				p.chara_id == GameMain.Instance.SelectCharaId &&
				p.unit == "chara");

				select_chara.hp -= iDamage;
				GameMain.Instance.CharaRefresh();
			}
			battleMain.HpRefresh();
		}

		private void OnHit(BattleIcon arg0)
		{
			// アニメーションのタイミングを合わせるために階層で呼び出してます
			if (is_player.Value)
			{
				battleMain.m_animEnemy.SetBool("damage", true);
			}
			else
			{
				battleMain.m_animChara.SetBool("damage", true);
			}
			StartCoroutine(DamageEffectAppear(arg0));
		}

		private void OnAttackFinished(BattleIcon arg0)
		{
			result_count += 1;
		}

		private IEnumerator exe_attack()
		{
			while(act_attack())
			{
				attack_count += 1;
				yield return new WaitForSeconds(0.3f);
			}
			result_count += 1;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if( attack_count == result_count)
			{
				Finish();
			}
			/*
			if( m_bNext)
			{
				m_fTime += Time.deltaTime;
				if( Defines.ICON_MOVE_TIME < m_fTime)
				{
					Fsm.Event("continue");
				}
			}
			*/
		}
	}


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class AttackToEnemy : BattleMainActionBase
	{
		public FsmInt damage;
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.Damage(true, damage.Value, OnDamageFinished);

		}

		private void OnDamageFinished()
		{
			DataUnitParam enemy = DataManagerGame.Instance.dataUnit.list.Find(p =>
			p.unit == "enemy");
			Debug.Log(enemy.hp);
			enemy.hp -= damage.Value;

			battleMain.HpRefresh();

			Finish();
		}
	}




	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsPlayerAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsEnemyAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class PhysicsEnd : BattleMainActionBase
	{
	}


	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicStart : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicOffset : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicPlayerAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicEnemyAttack : BattleMainActionBase
	{
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class MagicEnd : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TurnResult : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TrunEnd : BattleMainActionBase
	{
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class CheckCharaDead : BattleMainActionBase
	{
		// 戦闘に参加したキャラだけ調べる
		public FsmInt select_chara_id;
		public override void OnEnter()
		{
			base.OnEnter();

			bool bIsDead = false;
			DataUnitParam battle_chara = DataManagerGame.Instance.dataUnit.list.Find(p => p.chara_id == select_chara_id.Value);

			if(battle_chara.hp <= 0)
			{
				bIsDead = true;

				battleMain.m_animReciverChara.OnDeadFinished.AddListener(() =>
				{
					List<DataUnitParam> back_chara_list = DataManagerGame.Instance.dataUnit.list.FindAll(p => p.unit == "chara" && p.position == "back");

					bool bChangeMember = false;
					foreach (DataUnitParam back_unit in back_chara_list)
					{
						// 選手交代
						if (0 < back_unit.hp)
						{
							back_unit.position = battle_chara.position;
							battle_chara.position = "back";
							foreach (DataCardParam card in DataManagerGame.Instance.dataCard.list.FindAll(p => p.chara_id == back_unit.chara_id))
							{
								card.status = (int)DataCard.STATUS.REMOVE;
							}
							bChangeMember = true;

							DataUnitParam other = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && (p.position == "left" || p.position == "right") && p.chara_id != back_unit.chara_id);
							if (other != null)
							{
								battleMain.m_animChara.Play("idle");
								battleMain.gameMain.SelectCharaId = other.chara_id;
								battleMain.HpRefresh();
							}
							break;
						}
					}
					if (bChangeMember)
					{
						Debug.Log("メンバー交代");
						GameMain.Instance.panelStatus.Initialize(DataManagerGame.Instance.dataUnit, DataManagerGame.Instance.masterChara);
					}
					else
					{
						// 交代メンバーがいない場合はもう片方のキャラに変更
						DataUnitParam other = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "chara" && (p.position == "left" || p.position == "right") && p.chara_id != select_chara_id.Value && 0 < p.hp);
						if (other != null)
						{
							battleMain.m_animChara.Play("idle");
							battleMain.gameMain.SelectCharaId = other.chara_id;
							battleMain.HpRefresh();
						}
					}
					if (DataManagerGame.Instance.dataUnit.IsAliveParty())
					{
						Fsm.Event("dead");
					}
					else
					{
						Fsm.Event("gameover");
					}
				});

				battleMain.m_animChara.SetTrigger("down");

				// カードをロスト
				bool bIsHand = false;
				Debug.Log(select_chara_id.Value);
				foreach( DataCardParam card in DataManagerGame.Instance.dataCard.list.FindAll(p=>p.chara_id == select_chara_id.Value)){
					if( card.status == (int)DataCard.STATUS.HAND)
					{
						Card selected_card = null;
						selected_card = GameMain.Instance.card_list_hand.Find(p => p.data_card.card_serial == card.card_serial);
						selected_card.m_animator.SetBool("delete", true);

						GameMain.Instance.card_list_hand.Remove(selected_card);

						bIsHand = true;
					}
					card.status = (int)DataCard.STATUS.NOTUSE;
				}
				if(bIsHand)
				{
					GameMain.Instance.CardOrder();
				}

			}

			// ダメージ後にフラグを戻してない場合ここを経由してないことになる
			// ダウンしててもこっちのフラグは戻してOK
			battleMain.m_animChara.SetBool("damage", false);

			if (bIsDead == true)
			{
				// アニメーション終了で反応するように変更
			}
			else
			{
				Fsm.Event("continue");
			}
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class ResultCheck : BattleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			DataUnitParam enemy = DataManagerGame.Instance.dataUnit.list.Find(p => p.unit == "enemy");

			if (enemy.hp <= 0)
			{

				battleMain.m_animReciverEnemy.OnDeadFinished.AddListener(() =>
				{
					Fsm.Event("win");
				});
				battleMain.m_animEnemy.SetTrigger("down");
				battleMain.m_animEnemy.SetBool("damage", false);
			}
			else
			{
				battleMain.m_animEnemy.SetBool("damage", false);
				Finish();
			}
		}
	}
	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class TurnEnd : BattleMainActionBase
	{
		public FsmInt select_card_serial;
		public FsmInt enemy_card_serial;

		public FsmBool is_win;
		public override void OnEnter()
		{
			base.OnEnter();

			DataCardParam card = DataManagerGame.Instance.dataCard.list.Find(p => p.card_serial == select_card_serial.Value);
			card.status = (int)DataCard.STATUS.REMOVE;

			DataCardParam select_enemy_card = battleMain.dataCardEnemy.list.Find(p => p.card_serial == enemy_card_serial.Value);
			select_enemy_card.status = (int)DataCard.STATUS.REMOVE;

			if (is_win.Value) {
				Fsm.Event("win");
			}
			else
			{
				Finish();
			}
		}
	}

	[ActionCategory("BattleMainAction")]
	[HutongGames.PlayMaker.Tooltip("BattleMainAction")]
	public class BattleFinish : BattleMainActionBase
	{
		public FsmBool battle_result;
		public override void OnEnter()
		{
			base.OnEnter();
			battleMain.OnBattleFinished.Invoke(battle_result.Value);
			Finish();
		}
	}


}
