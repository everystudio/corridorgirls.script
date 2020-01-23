using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelGetCardAction {

	[ActionCategory("PanelGetCardAction")]
	[HutongGames.PlayMaker.Tooltip("PanelGetCardAction")]
	public class PanelGetCardActionBase : FsmStateAction
	{
		protected PanelGetCard panel;
		public override void OnEnter()
		{
			base.OnEnter();
			panel = Owner.GetComponent<PanelGetCard>();
		}
	}

	[ActionCategory("PanelGetCardAction")]
	[HutongGames.PlayMaker.Tooltip("PanelGetCardAction")]
	public class initialize : PanelGetCardActionBase
	{
		public FsmInt stage_id;
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (panel.stage_id != 0)
			{
				stage_id.Value = panel.stage_id;

				Debug.Log(stage_id.Value);
				panel.Initialize(3, stage_id.Value);
				Finish();
			}


		}
	}

	[ActionCategory("PanelGetCardAction")]
	[HutongGames.PlayMaker.Tooltip("PanelGetCardAction")]
	public class idle : PanelGetCardActionBase
	{
		private int temp_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			temp_serial = -1;
			select(temp_serial);
			foreach (Card c in panel.card_list)
			{
				c.OnClickCard.AddListener(OnClickCard);
			}
			panel.m_btnDecide.interactable = false;
			panel.m_btnDecide.onClick.AddListener(OnDecide);
		}

		private void OnDecide()
		{
			Fsm.Event("decide");
		}

		private void OnClickCard(int arg0)
		{
			if(temp_serial != arg0)
			{
				temp_serial = arg0;
				select(arg0);
			}
			panel.m_btnDecide.interactable = true;
		}

		private void select(int _iSerial)
		{
			//Debug.Log(_iSerial);
			foreach (Card c in panel.card_list)
			{
				c.m_animator.SetBool("up", temp_serial == c.data_card.card_serial);
				c.OnClickCard.AddListener(OnClickCard);
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			foreach (Card c in panel.card_list)
			{
				c.OnClickCard.RemoveListener(OnClickCard);

				if( temp_serial == c.data_card.card_serial)
				{
					panel.stage_id = 0;
					panel.OnSelectCardParam.Invoke(c.data_card);
				}
			}
			panel.m_btnDecide.onClick.RemoveListener(OnDecide);
		}
	}

}
