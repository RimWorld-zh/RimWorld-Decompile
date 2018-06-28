using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class FactionGenerator
	{
		private const int MinStartVisibleFactions = 5;

		private static readonly FloatRange FactionBasesPer100kTiles = new FloatRange(75f, 85f);

		[CompilerGenerated]
		private static Func<FactionDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Faction, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Faction, string> <>f__am$cache3;

		public static void GenerateFactionsIntoWorld()
		{
			int i = 0;
			foreach (FactionDef factionDef in DefDatabase<FactionDef>.AllDefs)
			{
				for (int j = 0; j < factionDef.requiredCountAtGameStart; j++)
				{
					Faction faction = FactionGenerator.NewGeneratedFaction(factionDef);
					Find.FactionManager.Add(faction);
					if (!factionDef.hidden)
					{
						i++;
					}
				}
			}
			while (i < 5)
			{
				FactionDef facDef = (from fa in DefDatabase<FactionDef>.AllDefs
				where fa.canMakeRandomly && Find.FactionManager.AllFactions.Count((Faction f) => f.def == fa) < fa.maxCountAtGameStart
				select fa).RandomElement<FactionDef>();
				Faction faction2 = FactionGenerator.NewGeneratedFaction(facDef);
				Find.World.factionManager.Add(faction2);
				i++;
			}
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * FactionGenerator.FactionBasesPer100kTiles.RandomInRange);
			num -= Find.WorldObjects.Settlements.Count;
			for (int k = 0; k < num; k++)
			{
				Faction faction3 = (from x in Find.World.factionManager.AllFactionsListForReading
				where !x.def.isPlayer && !x.def.hidden
				select x).RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction3);
				settlement.Tile = TileFinder.RandomFactionBaseTileFor(faction3, false, null);
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
		}

		public static void EnsureRequiredEnemies(Faction player)
		{
			using (IEnumerator<FactionDef> enumerator = DefDatabase<FactionDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionDef facDef = enumerator.Current;
					if (facDef.mustStartOneEnemy && Find.World.factionManager.AllFactions.Any((Faction f) => f.def == facDef) && !Find.World.factionManager.AllFactions.Any((Faction f) => f.def == facDef && f.HostileTo(player)))
					{
						Faction faction = (from f in Find.World.factionManager.AllFactions
						where f.def == facDef
						select f).RandomElement<Faction>();
						int num = faction.GoodwillWith(player);
						int randomInRange = DiplomacyTuning.ForcedStartingEnemyGoodwillRange.RandomInRange;
						int goodwillChange = randomInRange - num;
						faction.TryAffectGoodwillWith(player, goodwillChange, false, false, null, null);
						faction.TrySetRelationKind(player, FactionRelationKind.Hostile, false, null, null);
					}
				}
			}
		}

		public static Faction NewGeneratedFaction()
		{
			return FactionGenerator.NewGeneratedFaction(DefDatabase<FactionDef>.GetRandom());
		}

		public static Faction NewGeneratedFaction(FactionDef facDef)
		{
			Faction faction = new Faction();
			faction.def = facDef;
			faction.loadID = Find.UniqueIDsManager.GetNextFactionID();
			faction.colorFromSpectrum = FactionGenerator.NewRandomColorFromSpectrum(faction);
			if (!facDef.isPlayer)
			{
				if (facDef.fixedName != null)
				{
					faction.Name = facDef.fixedName;
				}
				else
				{
					faction.Name = NameGenerator.GenerateName(facDef.factionNameMaker, from fac in Find.FactionManager.AllFactionsVisible
					select fac.Name, false, null);
				}
			}
			faction.centralMelanin = Rand.Value;
			foreach (Faction other in Find.FactionManager.AllFactionsListForReading)
			{
				faction.TryMakeInitialRelationsWith(other);
			}
			if (!facDef.hidden && !facDef.isPlayer)
			{
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = TileFinder.RandomFactionBaseTileFor(faction, false, null);
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
			faction.GenerateNewLeader();
			return faction;
		}

		public static float NewRandomColorFromSpectrum(Faction faction)
		{
			float num = -1f;
			float result = 0f;
			for (int i = 0; i < 10; i++)
			{
				float value = Rand.Value;
				float num2 = 1f;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int j = 0; j < allFactionsListForReading.Count; j++)
				{
					Faction faction2 = allFactionsListForReading[j];
					if (faction2.def == faction.def)
					{
						float num3 = Mathf.Abs(value - faction2.colorFromSpectrum);
						if (num3 < num2)
						{
							num2 = num3;
						}
					}
				}
				if (num2 > num)
				{
					num = num2;
					result = value;
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FactionGenerator()
		{
		}

		[CompilerGenerated]
		private static bool <GenerateFactionsIntoWorld>m__0(FactionDef fa)
		{
			return fa.canMakeRandomly && Find.FactionManager.AllFactions.Count((Faction f) => f.def == fa) < fa.maxCountAtGameStart;
		}

		[CompilerGenerated]
		private static bool <GenerateFactionsIntoWorld>m__1(Faction x)
		{
			return !x.def.isPlayer && !x.def.hidden;
		}

		[CompilerGenerated]
		private static float <GenerateFactionsIntoWorld>m__2(Faction x)
		{
			return x.def.settlementGenerationWeight;
		}

		[CompilerGenerated]
		private static string <NewGeneratedFaction>m__3(Faction fac)
		{
			return fac.Name;
		}

		[CompilerGenerated]
		private sealed class <EnsureRequiredEnemies>c__AnonStorey2
		{
			internal Faction player;

			public <EnsureRequiredEnemies>c__AnonStorey2()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <EnsureRequiredEnemies>c__AnonStorey1
		{
			internal FactionDef facDef;

			internal FactionGenerator.<EnsureRequiredEnemies>c__AnonStorey2 <>f__ref$2;

			public <EnsureRequiredEnemies>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Faction f)
			{
				return f.def == this.facDef;
			}

			internal bool <>m__1(Faction f)
			{
				return f.def == this.facDef && f.HostileTo(this.<>f__ref$2.player);
			}

			internal bool <>m__2(Faction f)
			{
				return f.def == this.facDef;
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateFactionsIntoWorld>c__AnonStorey0
		{
			internal FactionDef fa;

			public <GenerateFactionsIntoWorld>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction f)
			{
				return f.def == this.fa;
			}
		}
	}
}
