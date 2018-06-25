using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class InspirationHandler : IExposable
	{
		public Pawn pawn;

		private Inspiration curState;

		private const int CheckStartInspirationIntervalTicks = 100;

		private const float MinMood = 0.5f;

		private const float StartInspirationMTBDaysAtMaxMood = 10f;

		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		public InspirationDef CurStateDef
		{
			get
			{
				return (this.curState == null) ? null : this.curState.def;
			}
		}

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

		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		public void Reset()
		{
			this.curState = null;
		}

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

		private InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		[CompilerGenerated]
		private bool <GetRandomAvailableInspirationDef>m__0(InspirationDef x)
		{
			return x.Worker.InspirationCanOccur(this.pawn);
		}

		[CompilerGenerated]
		private float <GetRandomAvailableInspirationDef>m__1(InspirationDef x)
		{
			return x.Worker.CommonalityFor(this.pawn);
		}
	}
}
