using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CharaControlAction {
	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class CharaControlActionBase : FsmStateAction
	{
		protected CharaControl charaControl;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl = Owner.GetComponent<CharaControl>();
		}
	}


	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Startup : CharaControlActionBase
	{

	}




	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Idle : CharaControlActionBase
	{
		public FsmInt move_num;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.m_eStatus = CharaControl.STATUS.IDLE;
			charaControl.OnMoveRequest.AddListener(OnMoveRequest);
		}

		private void OnMoveRequest(int arg0)
		{
			move_num.Value = arg0;
			Fsm.Event("move");
		}

		public override void OnExit()
		{
			base.OnExit();
			charaControl.OnMoveRequest.RemoveListener(OnMoveRequest);

		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class MoveCheck : CharaControlActionBase
	{
		public FsmInt move_num;
		public FsmInt target_corridor_index;
		public FsmVector3 target_position;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.m_eStatus = CharaControl.STATUS.MOVE;

			if ( 0 < move_num.Value)
			{

				if(charaControl.target_corridor.master.IsSingle())
				{
					// 明日はここから
					target_corridor_index.Value = charaControl.target_corridor.master.next_index;

					//Debug.Log(move_num.Value + ":" + target_corridor_index.Value);
					Fsm.Event("move");
				}
				else
				{
					Fsm.Event("select");
				}
			}
			else
			{
				Fsm.Event("end");
			}
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class MoveTargetIndex : CharaControlActionBase
	{
		public FsmInt target_corridor_index;
		public FsmVector3 target_position;

		public override void OnEnter()
		{
			base.OnEnter();
			DataCorridorParam target = DataManager.Instance.dataCorridor.list.Find(p => p.index == target_corridor_index.Value);

			target_position.Value = new Vector3(
				target.master.x,
				target.master.y + 0.85f,
				-1.0f);

			Finish();
		}

	}



	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Moved : CharaControlActionBase
	{
		public FsmInt move_num;
		public FsmInt target_corridor_index;
		public FsmVector3 target_position;

		public override void OnEnter()
		{
			base.OnEnter();
			move_num.Value -= 1;
			charaControl.target_corridor = DataManager.Instance.dataCorridor.list.Find(p => p.index == target_corridor_index.Value);

			StartCoroutine(delay_finish());
		}

		private IEnumerator delay_finish()
		{
			yield return null;
			Finish();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class RouteSelect : CharaControlActionBase
	{
		public FsmInt target_corridor_index;

		private int m_iSelectedIndex;
		private List<ArrowTargetCorridor> arrow_list = new List<ArrowTargetCorridor>();
		public override void OnEnter()
		{
			base.OnEnter();
			target_corridor_index.Value = 0;

			charaControl.m_btnGo.interactable = false;
			charaControl.m_btnGo.gameObject.SetActive(true);
			charaControl.m_btnGo.onClick.AddListener(OnDecide);

			arrow_list.Clear();

			int[] target_arr = new int[3] {
				charaControl.target_corridor.master.next_index,
				charaControl.target_corridor.master.next_index2,
				charaControl.target_corridor.master.next_index3
			};

			for( int i = 0; i < target_arr.Length; i++)
			{
				if(target_arr[i] == 0)
				{
					continue;
				}
				DataCorridorParam next_corridor = DataManager.Instance.dataCorridor.list.Find(p => p.index == target_arr[i]);

				ArrowTargetCorridor arrow = PrefabManager.Instance.MakeScript<ArrowTargetCorridor>(charaControl.m_prefArrowTargetCorridor, charaControl.m_goArrowRoot);
				arrow.Initialize(charaControl.target_corridor, next_corridor);
				arrow.SelectArrowIndex.AddListener(OnSelectArrowIndex);
				arrow_list.Add(arrow);
			}
		}

		private void OnDecide()
		{
			if( target_corridor_index.Value != 0)
			{
				bool bFinish = false;
				int key_item_id = GetKeyItemId(target_corridor_index.Value);
				if( key_item_id == 0)
				{
					bFinish = true;
				}
				else
				{
					DataItemParam item = DataManager.Instance.dataItem.list.Find(p => p.item_id == key_item_id && p.status == (int)DataItem.STATUS.STANDBY);
					if (item != null)
					{
						// ここでアイテム消費
						bFinish = true;
					}
					else
					{
						Debug.Log("アイテムがないため進行出来ません");
					}
				}
				if (bFinish)
				{
					Finish();
				}
			}
		}

		private int GetKeyItemId(int _iNextIndex )
		{
			int iRet = 0;
			MasterCorridorEventParam e = DataManager.Instance.masterCorridorEvent.list.Find(p =>
			p.corridor_event_id == charaControl.target_corridor.master.corridor_event_id);

			if(_iNextIndex != charaControl.target_corridor.master.next_index)
			{
				iRet = e.key_item_id;
			}
			return iRet;
		}

		private void OnSelectArrowIndex(int arg0)
		{
			bool bOK = false;

			int key_item_id = GetKeyItemId(arg0);

			if( key_item_id == 0)
			{
				bOK = true;
			}
			else
			{
				DataItemParam item = DataManager.Instance.dataItem.list.Find(p => p.item_id == key_item_id && p.status == (int)DataItem.STATUS.STANDBY);
				if (item != null)
				{
					bOK = true;
				}
				else
				{
					Debug.Log("アイテムがないため進行出来ません");
				}
			}
			if( bOK)
			{
				target_corridor_index.Value = arg0;
				charaControl.m_btnGo.interactable = true;
			}
		}

		public override void OnExit()
		{
			base.OnExit();

			charaControl.m_btnGo.gameObject.SetActive(false);
			foreach(ArrowTargetCorridor arrow in arrow_list)
			{
				GameObject.Destroy(arrow.gameObject);
			}
			charaControl.m_btnGo.onClick.RemoveListener(OnDecide);
		}
	}


	// -----------------------------------------
	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class CharaAnimWin : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.chara_control.m_animator.SetTrigger("win");
		}
	}


}
