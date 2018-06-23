using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004FB RID: 1275
	public class Need_Joy : Need
	{
		// Token: 0x04000D83 RID: 3459
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x04000D84 RID: 3460
		private int lastGainTick = -999;

		// Token: 0x060016EF RID: 5871 RVA: 0x000CA8EC File Offset: 0x000C8CEC
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x000CA964 File Offset: 0x000C8D64
		public JoyCategory CurCategory
		{
			get
			{
				JoyCategory result;
				if (this.CurLevel < 0.01f)
				{
					result = JoyCategory.Empty;
				}
				else if (this.CurLevel < 0.15f)
				{
					result = JoyCategory.VeryLow;
				}
				else if (this.CurLevel < 0.3f)
				{
					result = JoyCategory.Low;
				}
				else if (this.CurLevel < 0.7f)
				{
					result = JoyCategory.Satisfied;
				}
				else if (this.CurLevel < 0.85f)
				{
					result = JoyCategory.High;
				}
				else
				{
					result = JoyCategory.Extreme;
				}
				return result;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000CA9F0 File Offset: 0x000C8DF0
		private float FallPerInterval
		{
			get
			{
				float result;
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					result = 0.0015f;
					break;
				case JoyCategory.VeryLow:
					result = 0.0006f;
					break;
				case JoyCategory.Low:
					result = 0.00105f;
					break;
				case JoyCategory.Satisfied:
					result = 0.0015f;
					break;
				case JoyCategory.High:
					result = 0.0015f;
					break;
				case JoyCategory.Extreme:
					result = 0.0015f;
					break;
				default:
					throw new InvalidOperationException();
				}
				return result;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x000CAA74 File Offset: 0x000C8E74
		public override int GUIChangeArrow
		{
			get
			{
				return (!this.GainingJoy) ? -1 : 1;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000CAA9C File Offset: 0x000C8E9C
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 10;
			}
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x000CAAC6 File Offset: 0x000C8EC6
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000CAADA File Offset: 0x000C8EDA
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x000CAAF4 File Offset: 0x000C8EF4
		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (amount > 0f)
			{
				amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
				amount = Mathf.Min(amount, 1f - this.CurLevel);
				this.curLevelInt += amount;
				if (joyKind != null)
				{
					this.tolerances.Notify_JoyGained(amount, joyKind);
				}
				this.lastGainTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x000CAB67 File Offset: 0x000C8F67
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.tolerances.NeedInterval();
				if (!this.GainingJoy)
				{
					this.CurLevel -= this.FallPerInterval;
				}
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000CABA0 File Offset: 0x000C8FA0
		public override string GetTipString()
		{
			string text = base.GetTipString();
			string text2 = this.tolerances.TolerancesString();
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan != null)
			{
				float num = CaravanPawnsNeedsUtility.GetCaravanNotMovingJoyGainPerTick(this.pawn, caravan) * 2500f;
				if (num > 0f)
				{
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"\n\n",
						"GainingJoyBecauseCaravanNotMoving".Translate(),
						": +",
						num.ToStringPercent(),
						"/",
						"LetterHour".Translate()
					});
				}
			}
			return text;
		}
	}
}
