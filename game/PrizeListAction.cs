using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrizeListAction  {
	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class PrizeListActionBase : FsmStateAction
	{
		protected PrizeList prizeList;
		public override void OnEnter()
		{
			base.OnEnter();
			prizeList = Owner.GetComponent<PrizeList>();
		}
	}
	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class Reset : PrizeListActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			prizeList.Reset();
			prizeList.NumUpdate();
			Finish();
		}
	}

	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class Show : PrizeListActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (prizeList.m_bShow)
			{
				Finish();
			}
			else {
				prizeList.OnCompleteShow.AddListener(() =>
				{
					prizeList.OnCompleteShow.RemoveAllListeners();
					Finish();
				});
				prizeList.Show();
			}
		}
	}
	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class Disable : PrizeListActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			if (!prizeList.m_bShow)
			{
				Finish();
			}
			else {
				prizeList.OnCompleteDisable.AddListener(() =>
				{
					prizeList.OnCompleteDisable.RemoveAllListeners();
					Finish();
				});
				prizeList.Disable();
			}
		}
	}

	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class Idle : PrizeListActionBase
	{
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (prizeList.m_iFood != prizeList.m_iFoodDisp)
			{
				Finish();
			}
			if (prizeList.m_iGem != prizeList.m_iGemDisp)
			{
				Finish();
			}
			if (prizeList.m_iMana != prizeList.m_iManaDisp)
			{
				Finish();
			}
			if (prizeList.m_iGold != prizeList.m_iGoldDisp)
			{
				Finish();
			}

			if(prizeList.m_bMenu)
			{
				Finish();
			}
		}
	}

	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class DiffCheck : PrizeListActionBase
	{
		private float timer;

		public override void OnEnter()
		{
			base.OnEnter();
			timer = 0;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			timer += Time.deltaTime;
			if( 0.05f < timer)
			{
				timer -= 0.05f;

				bool bChange = false;
				if (prizeList.m_iFoodDisp <prizeList.m_iFood)
				{
					prizeList.m_iFoodDisp += 1;
					bChange = true;
				}
				if (prizeList.m_iGemDisp <prizeList.m_iGem)
				{
					prizeList.m_iGemDisp += 1;
					bChange = true;
				}
				if (prizeList.m_iManaDisp <prizeList.m_iMana )
				{
					prizeList.m_iManaDisp += 1;
					bChange = true;
				}
				if (prizeList.m_iGoldDisp < prizeList.m_iGold )
				{
					prizeList.m_iGoldDisp += 1;
					bChange = true;
				}
				if( bChange)
				{
					prizeList.NumUpdate();
				}
				else
				{
					if (prizeList.m_bMenu )
					{
						Fsm.Event("menu");
					}
					else
					{
						Finish();
					}
				}
			}
		}
	}
	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class DelayClose : PrizeListActionBase
	{
		private float timer;
		public override void OnEnter()
		{
			base.OnEnter();
			timer = 0;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			timer += Time.deltaTime;
			if( 3.0f < timer)
			{
				Finish();
			}
		}

	}


	[ActionCategory("PrizeListAction")]
	[HutongGames.PlayMaker.Tooltip("PrizeListAction")]
	public class MenuWait : PrizeListActionBase
	{
		public override void OnUpdate()
		{
			base.OnUpdate();


			if (prizeList.m_iFood != prizeList.m_iFoodDisp)
			{
				Fsm.Event("diff");
			}
			if (prizeList.m_iGem != prizeList.m_iGemDisp)
			{
				Fsm.Event("diff");
			}
			if (prizeList.m_iMana != prizeList.m_iManaDisp)
			{
				Fsm.Event("diff");
			}
			if (prizeList.m_iGold != prizeList.m_iGoldDisp)
			{
				Fsm.Event("diff");
			}

			if (!prizeList.m_bMenu)
			{
				Finish();
			}
		}
	}



}
