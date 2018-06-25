using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200055B RID: 1371
	public class FactionManager : IExposable
	{
		// Token: 0x04000F2A RID: 3882
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04000F2B RID: 3883
		private Faction ofPlayer;

		// Token: 0x04000F2C RID: 3884
		private Faction ofMechanoids;

		// Token: 0x04000F2D RID: 3885
		private Faction ofInsects;

		// Token: 0x04000F2E RID: 3886
		private Faction ofAncients;

		// Token: 0x04000F2F RID: 3887
		private Faction ofAncientsHostile;

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x000E0F44 File Offset: 0x000DF344
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x000E0F60 File Offset: 0x000DF360
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x000E0F7C File Offset: 0x000DF37C
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.def.hidden
				select fa;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x000E0FBC File Offset: 0x000DF3BC
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000E0FDC File Offset: 0x000DF3DC
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x000E0FFC File Offset: 0x000DF3FC
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x000E1018 File Offset: 0x000DF418
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000E1034 File Offset: 0x000DF434
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x000E1050 File Offset: 0x000DF450
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x000E106C File Offset: 0x000DF46C
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000E1088 File Offset: 0x000DF488
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000E10D6 File Offset: 0x000DF4D6
		public void Add(Faction faction)
		{
			if (!this.allFactions.Contains(faction))
			{
				this.allFactions.Add(faction);
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000E1101 File Offset: 0x000DF501
		public void Remove(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				this.allFactions.Remove(faction);
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000E1130 File Offset: 0x000DF530
		public void FactionManagerTick()
		{
			FactionBaseProximityGoodwillUtility.CheckFactionBaseProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000E1174 File Offset: 0x000DF574
		public void FactionsDebugDrawOnMap()
		{
			if (DebugViewSettings.drawFactions)
			{
				for (int i = 0; i < this.allFactions.Count; i++)
				{
					this.allFactions[i].DebugDrawOnMap();
				}
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000E11C0 File Offset: 0x000DF5C0
		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000E1220 File Offset: 0x000DF620
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined)
		{
			IEnumerable<Faction> source = from x in this.AllFactions
			where !x.IsPlayer && !x.def.hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel)
			select x;
			return source.TryRandomElementByWeight(delegate(Faction x)
			{
				float result;
				if (tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					result = 0.1f;
				}
				else
				{
					result = 1f;
				}
				return result;
			}, out faction);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000E127C File Offset: 0x000DF67C
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction faction;
			Faction result;
			if ((from x in this.AllFactions
			where !x.IsPlayer && (allowHidden || !x.def.hidden) && (allowDefeated || !x.defeated) && (allowNonHumanlike || x.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel) && x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out faction))
			{
				result = faction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000E12E0 File Offset: 0x000DF6E0
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction faction;
			Faction result;
			if ((from x in this.AllFactions
			where !x.IsPlayer && (allowHidden || !x.def.hidden) && (allowDefeated || !x.defeated) && (allowNonHumanlike || x.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel) && !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out faction))
			{
				result = faction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000E1344 File Offset: 0x000DF744
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction faction;
			Faction result;
			if ((from x in this.AllFactions
			where !x.IsPlayer && (allowHidden || !x.def.hidden) && (allowDefeated || !x.defeated) && (allowNonHumanlike || x.def.humanlikeFaction) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel) && x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out faction))
			{
				result = faction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000E13A8 File Offset: 0x000DF7A8
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000E13F8 File Offset: 0x000DF7F8
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000E1454 File Offset: 0x000DF854
		private void RecacheFactions()
		{
			this.ofPlayer = null;
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].IsPlayer)
				{
					this.ofPlayer = this.allFactions[i];
					break;
				}
			}
			this.ofMechanoids = this.FirstFactionOfDef(FactionDefOf.Mechanoid);
			this.ofInsects = this.FirstFactionOfDef(FactionDefOf.Insect);
			this.ofAncients = this.FirstFactionOfDef(FactionDefOf.Ancients);
			this.ofAncientsHostile = this.FirstFactionOfDef(FactionDefOf.AncientsHostile);
		}
	}
}
