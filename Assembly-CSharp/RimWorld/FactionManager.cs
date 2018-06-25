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
		// Token: 0x04000F26 RID: 3878
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04000F27 RID: 3879
		private Faction ofPlayer;

		// Token: 0x04000F28 RID: 3880
		private Faction ofMechanoids;

		// Token: 0x04000F29 RID: 3881
		private Faction ofInsects;

		// Token: 0x04000F2A RID: 3882
		private Faction ofAncients;

		// Token: 0x04000F2B RID: 3883
		private Faction ofAncientsHostile;

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x000E0CDC File Offset: 0x000DF0DC
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060019D2 RID: 6610 RVA: 0x000E0CF8 File Offset: 0x000DF0F8
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060019D3 RID: 6611 RVA: 0x000E0D14 File Offset: 0x000DF114
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
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000E0D54 File Offset: 0x000DF154
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x000E0D74 File Offset: 0x000DF174
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x000E0D94 File Offset: 0x000DF194
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000E0DB0 File Offset: 0x000DF1B0
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x000E0DCC File Offset: 0x000DF1CC
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060019D9 RID: 6617 RVA: 0x000E0DE8 File Offset: 0x000DF1E8
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x000E0E04 File Offset: 0x000DF204
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000E0E20 File Offset: 0x000DF220
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000E0E6E File Offset: 0x000DF26E
		public void Add(Faction faction)
		{
			if (!this.allFactions.Contains(faction))
			{
				this.allFactions.Add(faction);
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000E0E99 File Offset: 0x000DF299
		public void Remove(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				this.allFactions.Remove(faction);
				this.RecacheFactions();
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000E0EC8 File Offset: 0x000DF2C8
		public void FactionManagerTick()
		{
			FactionBaseProximityGoodwillUtility.CheckFactionBaseProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000E0F0C File Offset: 0x000DF30C
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

		// Token: 0x060019E0 RID: 6624 RVA: 0x000E0F58 File Offset: 0x000DF358
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

		// Token: 0x060019E1 RID: 6625 RVA: 0x000E0FB8 File Offset: 0x000DF3B8
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

		// Token: 0x060019E2 RID: 6626 RVA: 0x000E1014 File Offset: 0x000DF414
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

		// Token: 0x060019E3 RID: 6627 RVA: 0x000E1078 File Offset: 0x000DF478
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

		// Token: 0x060019E4 RID: 6628 RVA: 0x000E10DC File Offset: 0x000DF4DC
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

		// Token: 0x060019E5 RID: 6629 RVA: 0x000E1140 File Offset: 0x000DF540
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000E1190 File Offset: 0x000DF590
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000E11EC File Offset: 0x000DF5EC
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
