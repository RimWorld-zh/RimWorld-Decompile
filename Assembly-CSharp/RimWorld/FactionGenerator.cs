using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class FactionGenerator
	{
		private const int MinStartVisibleFactions = 5;

		private static readonly FloatRange FactionBasesPer100kTiles = new FloatRange(75f, 85f);

		public static void GenerateFactionsIntoWorld()
		{
			int i = 0;
			foreach (FactionDef allDef in DefDatabase<FactionDef>.AllDefs)
			{
				for (int j = 0; j < allDef.requiredCountAtGameStart; j++)
				{
					Faction faction = FactionGenerator.NewGeneratedFaction(allDef);
					Find.FactionManager.Add(faction);
					if (!allDef.hidden)
					{
						i++;
					}
				}
			}
			for (; i < 5; i++)
			{
				FactionDef facDef = (from fa in DefDatabase<FactionDef>.AllDefs
				where fa.canMakeRandomly && Find.FactionManager.AllFactions.Count((Faction f) => f.def == fa) < fa.maxCountAtGameStart
				select fa).RandomElement();
				Faction faction2 = FactionGenerator.NewGeneratedFaction(facDef);
				Find.World.factionManager.Add(faction2);
			}
			int num = GenMath.RoundRandom((float)((float)Find.WorldGrid.TilesCount / 100000.0 * FactionGenerator.FactionBasesPer100kTiles.RandomInRange));
			num -= Find.WorldObjects.FactionBases.Count;
			for (int k = 0; k < num; k++)
			{
				Faction faction3 = (from x in Find.World.factionManager.AllFactionsListForReading
				where !x.def.isPlayer && !x.def.hidden
				select x).RandomElementByWeight((Faction x) => x.def.baseSelectionWeight);
				FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				factionBase.SetFaction(faction3);
				factionBase.Tile = TileFinder.RandomFactionBaseTileFor(faction3, false, null);
				factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase);
				Find.WorldObjects.Add(factionBase);
			}
		}

		public static void EnsureRequiredEnemies(Faction player)
		{
			foreach (FactionDef allDef in DefDatabase<FactionDef>.AllDefs)
			{
				if (allDef.mustStartOneEnemy && Find.World.factionManager.AllFactions.Any((Faction f) => f.def == allDef) && !Find.World.factionManager.AllFactions.Any((Faction f) => f.def == allDef && f.HostileTo(player)))
				{
					Faction faction = (from f in Find.World.factionManager.AllFactions
					where f.def == allDef
					select f).RandomElement();
					float goodwillChange = (float)((0.0 - (faction.GoodwillWith(player) + 100.0)) * Rand.Range(0.8f, 0.9f));
					faction.AffectGoodwillWith(player, goodwillChange);
					faction.SetHostileTo(player, true);
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
			foreach (Faction item in Find.FactionManager.AllFactionsListForReading)
			{
				faction.TryMakeInitialRelationsWith(item);
			}
			if (!facDef.hidden && !facDef.isPlayer)
			{
				FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				factionBase.SetFaction(faction);
				factionBase.Tile = TileFinder.RandomFactionBaseTileFor(faction, false, null);
				factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase);
				Find.WorldObjects.Add(factionBase);
			}
			faction.GenerateNewLeader();
			return faction;
		}

		private static float NewRandomColorFromSpectrum(Faction faction)
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
					if (faction2 != faction && faction2.def == faction.def)
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
	}
}
