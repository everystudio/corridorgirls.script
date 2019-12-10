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

					DataCorridorParam target = DataManager.Instance.dataCorridor.list.Find(p => p.index == target_corridor_index.Value);

					target_position.Value = new Vector3(
						target.master.x,
						target.master.y + 1.5f,
						-1.0f);

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




}
