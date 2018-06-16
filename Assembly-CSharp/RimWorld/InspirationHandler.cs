using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200002C RID: 44
	public class InspirationHandler : IExposable
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00010B07 File Offset: 0x0000EF07
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00010B18 File Offset: 0x0000EF18
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00010B3C File Offset: 0x0000EF3C
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00010B58 File Offset: 0x0000EF58
		public InspirationDef CurStateDef
		{
			get
			{
				return (this.curState == null) ? null : this.curState.def;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00010B8C File Offset: 0x0000EF8C
		private float StartInspirationMTBDays
		{
			get
			{
				float result;
				if (this.pawn.needs.mood == null)
				{
					result = -1f;
				}
				else
				{
					float curLevel = this.pawn.needs.mood.CurLevel;
					if (curLevel < 0.5f)
					{
						result = -1f;
					}
					else
					{
						result = GenMath.LerpDouble(0.5f, 1f, 210f, 10f, curLevel);
					}
				}
				return result;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00010C08 File Offset: 0x0000F008
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curState != null)
				{
					this.curState.pawn = this.pawn;
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00010C55 File Offset: 0x0000F055
		public void InspirationHandlerTick()
		{
			if (this.curState != null)
			{
				this.curState.InspirationTick();
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				this.CheckStartRandomInspiration();
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00010C88 File Offset: 0x0000F088
		public bool TryStartInspiration(InspirationDef def)
		{
			bool result;
			if (this.Inspired)
			{
				result = false;
			}
			else if (!def.Worker.InspirationCanOccur(this.pawn))
			{
				result = false;
			}
			else
			{
				this.curState = (Inspiration)Activator.CreateInstance(def.inspirationClass);
				this.curState.def = def;
				this.curState.pawn = this.pawn;
				this.curState.PostStart();
				result = true;
			}
			return result;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00010D0C File Offset: 0x0000F10C
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration != null)
			{
				if (this.curState != inspiration)
				{
					Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>(), false);
				}
				else
				{
					this.curState = null;
					inspiration.PostEnd();
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00010D6A File Offset: 0x0000F16A
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00010D95 File Offset: 0x0000F195
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00010DA0 File Offset: 0x0000F1A0
		private void CheckStartRandomInspiration()
		{
			if (!this.Inspired)
			{
				float startInspirationMTBDays = this.StartInspirationMTBDays;
				if (startInspirationMTBDays >= 0f)
				{
					if (Rand.MTBEventOccurs(startInspirationMTBDays, 60000f, 100f))
					{
						InspirationDef randomAvailableInspirationDef = this.GetRandomAvailableInspirationDef();
						if (randomAvailableInspirationDef != null)
						{
							this.TryStartInspiration(randomAvailableInspirationDef);
						}
					}
				}
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00010E04 File Offset: 0x0000F204
		private InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x040001A6 RID: 422
		public Pawn pawn;

		// Token: 0x040001A7 RID: 423
		private Inspiration curState;

		// Token: 0x040001A8 RID: 424
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x040001A9 RID: 425
		private const float MinMood = 0.5f;

		// Token: 0x040001AA RID: 426
		private const float StartInspirationMTBDaysAtMaxMood = 10f;
	}
}
