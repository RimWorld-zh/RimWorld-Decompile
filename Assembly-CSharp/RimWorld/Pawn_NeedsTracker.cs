using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Pawn_NeedsTracker : IExposable
	{
		private Pawn pawn;

		private List<Need> needs = new List<Need>();

		public Need_Mood mood;

		public Need_Food food;

		public Need_Rest rest;

		public Need_Joy joy;

		public Need_Beauty beauty;

		public Need_Space space;

		public Need_Comfort comfort;

		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		public Pawn_NeedsTracker()
		{
		}

		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[1]
			{
				this.pawn
			});
			this.BindDirectNeedFields();
		}

		private void BindDirectNeedFields()
		{
			this.mood = this.TryGetNeed<Need_Mood>();
			this.food = this.TryGetNeed<Need_Food>();
			this.rest = this.TryGetNeed<Need_Rest>();
			this.joy = this.TryGetNeed<Need_Joy>();
			this.beauty = this.TryGetNeed<Need_Beauty>();
			this.comfort = this.TryGetNeed<Need_Comfort>();
			this.space = this.TryGetNeed<Need_Space>();
		}

		public void NeedsTrackerTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				for (int i = 0; i < this.needs.Count; i++)
				{
					this.needs[i].NeedInterval();
				}
			}
		}

		public T TryGetNeed<T>() where T : Need
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.needs.Count)
				{
					if (this.needs[num].GetType() == typeof(T))
					{
						result = (T)this.needs[num];
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		public Need TryGetNeed(NeedDef def)
		{
			int num = 0;
			Need result;
			while (true)
			{
				if (num < this.needs.Count)
				{
					if (this.needs[num].def == def)
					{
						result = this.needs[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		public void AddOrRemoveNeedsAsAppropriate()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				NeedDef needDef = allDefsListForReading[i];
				if (this.ShouldHaveNeed(needDef))
				{
					if (this.TryGetNeed(needDef) == null)
					{
						this.AddNeed(needDef);
					}
				}
				else if (this.TryGetNeed(needDef) != null)
				{
					this.RemoveNeed(needDef);
				}
			}
		}

		private bool ShouldHaveNeed(NeedDef nd)
		{
			return (int)this.pawn.RaceProps.intelligence >= (int)nd.minIntelligence && (!nd.colonistsOnly || (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)) && (!nd.colonistAndPrisonersOnly || (this.pawn.Faction != null && this.pawn.Faction.IsPlayer) || (this.pawn.HostFaction != null && this.pawn.HostFaction == Faction.OfPlayer)) && (!nd.onlyIfCausedByHediff || this.pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff x) => x.def.causesNeed == nd))) && (!nd.neverOnPrisoner || !this.pawn.IsPrisoner) && ((nd != NeedDefOf.Food) ? (nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest) : this.pawn.RaceProps.EatsFood);
		}

		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, this.pawn);
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}
	}
}
