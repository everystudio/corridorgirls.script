using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCameraAction {
	
	[ActionCategory("GameCameraAction")]
	[HutongGames.PlayMaker.Tooltip("GameCameraAction")]
	public class GameCameraActionBase : FsmStateAction
	{
		protected GameCamera gameCamera;
		public override void OnEnter()
		{
			base.OnEnter();
			gameCamera = Owner.GetComponent<GameCamera>();
		}
	}

	[ActionCategory("GameCameraAction")]
	[HutongGames.PlayMaker.Tooltip("GameCameraAction")]
	public class Wait : GameCameraActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameCamera.RequestMoveStart.AddListener(() =>
			{
				Fsm.Event("move");
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			gameCamera.RequestMoveStart.RemoveAllListeners();
		}
	}

	[ActionCategory("GameCameraAction")]
	[HutongGames.PlayMaker.Tooltip("GameCameraAction")]
	public class Move : GameCameraActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameCamera.SwipeStart();
			gameCamera.RequestMoveStop.AddListener(() =>
			{
				Finish();
			});
		}
		public override void OnExit()
		{
			base.OnExit();
			gameCamera.RequestMoveStop.RemoveAllListeners();
			gameCamera.SwipeEnd();
		}
	}
	[ActionCategory("GameCameraAction")]
	[HutongGames.PlayMaker.Tooltip("GameCameraAction")]
	public class CommonRequestCameraMove : FsmStateAction
	{
		public FsmBool is_move;
		public override void OnEnter()
		{
			base.OnEnter();
			if (is_move.Value) {
				GameCamera.Instance.RequestMoveStart.Invoke();
			}
			else
			{
				GameCamera.Instance.RequestMoveStop.Invoke();
			}
		}
	}



}
