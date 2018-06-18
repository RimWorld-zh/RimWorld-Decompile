using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050D RID: 1293
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x0600172F RID: 5935 RVA: 0x000CBDDD File Offset: 0x000CA1DD
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x000CBDF1 File Offset: 0x000CA1F1
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x000CBE14 File Offset: 0x000CA214
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000CBE30 File Offset: 0x000CA230
		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.needs.RemoveAll((Need x) => x == null) != 0)
				{
					Log.Error("Pawn " + this.pawn.Label + " had some null needs after loading.", false);
				}
				this.BindDirectNeedFields();
			}
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000CBECC File Offset: 0x000CA2CC
		private void BindDirectNeedFields()
		{
			this.mood = this.TryGetNeed<Need_Mood>();
			this.food = this.TryGetNeed<Need_Food>();
			this.rest = this.TryGetNeed<Need_Rest>();
			this.joy = this.TryGetNeed<Need_Joy>();
			this.beauty = this.TryGetNeed<Need_Beauty>();
			this.comfort = this.TryGetNeed<Need_Comfort>();
			this.roomsize = this.TryGetNeed<Need_RoomSize>();
			this.outdoors = this.TryGetNeed<Need_Outdoors>();
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000CBF3C File Offset: 0x000CA33C
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

		// Token: 0x06001735 RID: 5941 RVA: 0x000CBF90 File Offset: 0x000CA390
		public T TryGetNeed<T>() where T : Need
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].GetType() == typeof(T))
				{
					return (T)((object)this.needs[i]);
				}
			}
			return (T)((object)null);
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000CC000 File Offset: 0x000CA400
		public Need TryGetNeed(NeedDef def)
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].def == def)
				{
					return this.needs[i];
				}
			}
			return null;
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000CC060 File Offset: 0x000CA460
		public void SetInitialLevels()
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000CC0A0 File Offset: 0x000CA4A0
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

		// Token: 0x06001739 RID: 5945 RVA: 0x000CC110 File Offset: 0x000CA510
		private bool ShouldHaveNeed(NeedDef nd)
		{
			bool result;
			if (this.pawn.RaceProps.intelligence < nd.minIntelligence)
			{
				result = false;
			}
			else
			{
				if (nd.colonistsOnly)
				{
					if (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer)
					{
						return false;
					}
				}
				if (nd.colonistAndPrisonersOnly)
				{
					if ((this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || this.pawn.HostFaction != Faction.OfPlayer))
					{
						return false;
					}
				}
				if (nd.onlyIfCausedByHediff)
				{
					if (!this.pawn.health.hediffSet.hediffs.Any((Hediff x) => x.def.causesNeed == nd))
					{
						return false;
					}
				}
				if (nd.neverOnPrisoner && this.pawn.IsPrisoner)
				{
					result = false;
				}
				else if (nd == NeedDefOf.Food)
				{
					result = this.pawn.RaceProps.EatsFood;
				}
				else
				{
					result = (nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest);
				}
			}
			return result;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000CC2BC File Offset: 0x000CA6BC
		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, new object[]
			{
				this.pawn
			});
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x000CC30C File Offset: 0x000CA70C
		private void RemoveNeed(NeedDef nd)
		{
			Need item = this.TryGetNeed(nd);
			this.needs.Remove(item);
			this.BindDirectNeedFields();
		}

		// Token: 0x04000DC0 RID: 3520
		private Pawn pawn;

		// Token: 0x04000DC1 RID: 3521
		private List<Need> needs = new List<Need>();

		// Token: 0x04000DC2 RID: 3522
		public Need_Mood mood;

		// Token: 0x04000DC3 RID: 3523
		public Need_Food food;

		// Token: 0x04000DC4 RID: 3524
		public Need_Rest rest;

		// Token: 0x04000DC5 RID: 3525
		public Need_Joy joy;

		// Token: 0x04000DC6 RID: 3526
		public Need_Beauty beauty;

		// Token: 0x04000DC7 RID: 3527
		public Need_RoomSize roomsize;

		// Token: 0x04000DC8 RID: 3528
		public Need_Outdoors outdoors;

		// Token: 0x04000DC9 RID: 3529
		public Need_Comfort comfort;
	}
}
