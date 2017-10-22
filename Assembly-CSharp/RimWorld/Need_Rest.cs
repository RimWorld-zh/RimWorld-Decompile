using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Need_Rest : Need
	{
		private const float FullSleepHours = 10.5f;

		public const float BaseRestGainPerTick = 3.809524E-05f;

		private const float BaseRestFallPerTick = 1.58333332E-05f;

		public const float ThreshTired = 0.28f;

		public const float ThreshVeryTired = 0.14f;

		public const float DefaultFallAsleepMaxLevel = 0.75f;

		public const float DefaultNaturalWakeThreshold = 1f;

		public const float CanWakeThreshold = 0.2f;

		private const float BaseInvoluntarySleepMTBDays = 0.25f;

		private int lastRestTick = -999;

		private float lastRestEffectiveness = 1f;

		private int ticksAtZero;

		public RestCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.0099999997764825821)
				{
					return RestCategory.Exhausted;
				}
				if (this.CurLevel < 0.14000000059604645)
				{
					return RestCategory.VeryTired;
				}
				if (this.CurLevel < 0.2800000011920929)
				{
					return RestCategory.Tired;
				}
				return RestCategory.Rested;
			}
		}

		public float RestFallPerTick
		{
			get
			{
				switch (this.CurCategory)
				{
				case RestCategory.Rested:
				{
					return (float)(1.5833333236514591E-05 * this.RestFallFactor);
				}
				case RestCategory.Tired:
				{
					return (float)(1.5833333236514591E-05 * this.RestFallFactor * 0.699999988079071);
				}
				case RestCategory.VeryTired:
				{
					return (float)(1.5833333236514591E-05 * this.RestFallFactor * 0.30000001192092896);
				}
				case RestCategory.Exhausted:
				{
					return (float)(1.5833333236514591E-05 * this.RestFallFactor * 0.60000002384185791);
				}
				default:
				{
					return 999f;
				}
				}
			}
		}

		private float RestFallFactor
		{
			get
			{
				float num = 1f;
				List<Hediff> hediffs = base.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				return num;
			}
		}

		public override int GUIChangeArrow
		{
			get
			{
				if (this.Resting)
				{
					return 1;
				}
				return -1;
			}
		}

		public int TicksAtZero
		{
			get
			{
				return this.ticksAtZero;
			}
		}

		private bool Resting
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastRestTick + 2;
			}
		}

		public Need_Rest(Pawn pawn) : base(pawn)
		{
			base.threshPercents = new List<float>();
			base.threshPercents.Add(0.28f);
			base.threshPercents.Add(0.14f);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAtZero, "ticksAtZero", 0, false);
		}

		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.9f, 1f);
		}

		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				if (this.Resting)
				{
					this.CurLevel += (float)(0.0057142861187458038 * this.lastRestEffectiveness);
				}
				else
				{
					this.CurLevel -= (float)(this.RestFallPerTick * 150.0);
				}
			}
			if (this.CurLevel < 9.9999997473787516E-05)
			{
				this.ticksAtZero += 150;
			}
			else
			{
				this.ticksAtZero = 0;
			}
			if (this.ticksAtZero > 1000 && base.pawn.Spawned)
			{
				float mtb = (float)((this.ticksAtZero >= 15000) ? ((this.ticksAtZero >= 30000) ? ((this.ticksAtZero >= 45000) ? 0.0625 : 0.0833333358168602) : 0.125) : 0.25);
				if (Rand.MTBEventOccurs(mtb, 60000f, 150f))
				{
					base.pawn.jobs.StartJob(new Job(JobDefOf.LayDown, base.pawn.Position), JobCondition.InterruptForced, null, false, true, null, default(JobTag?));
					if (PawnUtility.ShouldSendNotificationAbout(base.pawn))
					{
						Messages.Message("MessageInvoluntarySleep".Translate(base.pawn.LabelShort), (Thing)base.pawn, MessageSound.Negative);
					}
				}
			}
		}

		public void TickResting(float restEffectiveness)
		{
			this.lastRestTick = Find.TickManager.TicksGame;
			this.lastRestEffectiveness = restEffectiveness;
		}
	}
}
