using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class FactionManager : IExposable
	{
		private List<Faction> allFactions = new List<Faction>();

		private Faction ofPlayer;

		private Faction ofMechanoids;

		private Faction ofInsects;

		private Faction ofAncients;

		private Faction ofAncientsHostile;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Faction, float> <>f__am$cache2;

		public FactionManager()
		{
		}

		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.def.hidden
				select fa;
			}
		}

		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheFactions();
			}
		}

		public void Add(Faction faction)
		{
			if (!this.allFactions.Contains(faction))
			{
				this.allFactions.Add(faction);
				this.RecacheFactions();
			}
		}

		public void Remove(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				this.allFactions.Remove(faction);
				this.RecacheFactions();
			}
		}

		public void FactionManagerTick()
		{
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
		}

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

		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:", false);
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

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

		[CompilerGenerated]
		private static bool <get_AllFactionsVisible>m__0(Faction fa)
		{
			return !fa.def.hidden;
		}

		[CompilerGenerated]
		private static bool <GetInViewOrder>m__1(Faction x)
		{
			return x.defeated;
		}

		[CompilerGenerated]
		private static float <GetInViewOrder>m__2(Faction x)
		{
			return x.def.listOrderPriority;
		}

		[CompilerGenerated]
		private sealed class <TryGetRandomNonColonyHumanlikeFaction>c__AnonStorey0
		{
			internal bool allowDefeated;

			internal TechLevel minTechLevel;

			internal bool tryMedievalOrBetter;

			public <TryGetRandomNonColonyHumanlikeFaction>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return !x.IsPlayer && !x.def.hidden && x.def.humanlikeFaction && (this.allowDefeated || !x.defeated) && (this.minTechLevel == TechLevel.Undefined || x.def.techLevel >= this.minTechLevel);
			}

			internal float <>m__1(Faction x)
			{
				float result;
				if (this.tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					result = 0.1f;
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <RandomEnemyFaction>c__AnonStorey1
		{
			internal bool allowHidden;

			internal bool allowDefeated;

			internal bool allowNonHumanlike;

			internal TechLevel minTechLevel;

			public <RandomEnemyFaction>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return !x.IsPlayer && (this.allowHidden || !x.def.hidden) && (this.allowDefeated || !x.defeated) && (this.allowNonHumanlike || x.def.humanlikeFaction) && (this.minTechLevel == TechLevel.Undefined || x.def.techLevel >= this.minTechLevel) && x.HostileTo(Faction.OfPlayer);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomNonHostileFaction>c__AnonStorey2
		{
			internal bool allowHidden;

			internal bool allowDefeated;

			internal bool allowNonHumanlike;

			internal TechLevel minTechLevel;

			public <RandomNonHostileFaction>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return !x.IsPlayer && (this.allowHidden || !x.def.hidden) && (this.allowDefeated || !x.defeated) && (this.allowNonHumanlike || x.def.humanlikeFaction) && (this.minTechLevel == TechLevel.Undefined || x.def.techLevel >= this.minTechLevel) && !x.HostileTo(Faction.OfPlayer);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomAlliedFaction>c__AnonStorey3
		{
			internal bool allowHidden;

			internal bool allowDefeated;

			internal bool allowNonHumanlike;

			internal TechLevel minTechLevel;

			public <RandomAlliedFaction>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Faction x)
			{
				return !x.IsPlayer && (this.allowHidden || !x.def.hidden) && (this.allowDefeated || !x.defeated) && (this.allowNonHumanlike || x.def.humanlikeFaction) && (this.minTechLevel == TechLevel.Undefined || x.def.techLevel >= this.minTechLevel) && x.PlayerRelationKind == FactionRelationKind.Ally;
			}
		}
	}
}
