using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Joy : Need
	{
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		private int lastGainTick = -999;

		private const float BaseFallPerTick = 1.00000007E-05f;

		private const float ThreshLow = 0.15f;

		private const float ThreshSatisfied = 0.3f;

		private const float ThreshHigh = 0.7f;

		private const float ThreshVeryHigh = 0.85f;

		private const float MinDownedJoy = 0.25f;

		public JoyCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.0099999997764825821)
				{
					return JoyCategory.Empty;
				}
				if (this.CurLevel < 0.15000000596046448)
				{
					return JoyCategory.VeryLow;
				}
				if (this.CurLevel < 0.30000001192092896)
				{
					return JoyCategory.Low;
				}
				if (this.CurLevel < 0.699999988079071)
				{
					return JoyCategory.Satisfied;
				}
				if (this.CurLevel < 0.85000002384185791)
				{
					return JoyCategory.High;
				}
				return JoyCategory.Extreme;
			}
		}

		private float FallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					return 1.00000007E-05f;
				case JoyCategory.VeryLow:
					return 4.00000044E-06f;
				case JoyCategory.Low:
					return 7.00000055E-06f;
				case JoyCategory.Satisfied:
					return 1.00000007E-05f;
				case JoyCategory.High:
					return 1.00000007E-05f;
				case JoyCategory.Extreme:
					return 1.00000007E-05f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		public override int GUIChangeArrow
		{
			get
			{
				return this.GainingJoy ? 1 : (-1);
			}
		}

		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 10;
			}
		}

		public Need_Joy(Pawn pawn)
			: base(pawn)
		{
			base.threshPercents = new List<float>();
			base.threshPercents.Add(0.15f);
			base.threshPercents.Add(0.3f);
			base.threshPercents.Add(0.7f);
			base.threshPercents.Add(0.85f);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.8f);
		}

		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (joyKind == null)
			{
				Log.Error("No joyKind!");
			}
			else
			{
				amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
			}
			amount = Mathf.Min(amount, (float)(1.0 - this.CurLevel));
			base.curLevelInt += amount;
			if (joyKind != null)
			{
				this.tolerances.Notify_JoyGained(amount, joyKind);
			}
			this.lastGainTick = Find.TickManager.TicksGame;
		}

		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.tolerances.NeedInterval();
				if (!this.GainingJoy)
				{
					this.CurLevel -= (float)(this.FallPerTick * 150.0);
				}
				if (base.pawn.Downed && this.CurLevel < 0.25)
				{
					this.CurLevel = 0.25f;
				}
			}
		}

		public override string GetTipString()
		{
			return base.GetTipString() + "\n" + this.tolerances.TolerancesString();
		}
	}
}
