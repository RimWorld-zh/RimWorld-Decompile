using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000557 RID: 1367
	public static class FactionGenerator
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x000DFE10 File Offset: 0x000DE210
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
			num -= Find.WorldObjects.FactionBases.Count;
			for (int k = 0; k < num; k++)
			{
				Faction faction3 = (from x in Find.World.factionManager.AllFactionsListForReading
				where !x.def.isPlayer && !x.def.hidden
				select x).RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
				FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				factionBase.SetFaction(faction3);
				factionBase.Tile = TileFinder.RandomFactionBaseTileFor(faction3, false, null);
				factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
				Find.WorldObjects.Add(factionBase);
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x000DFFEC File Offset: 0x000DE3EC
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

		// Token: 0x060019BA RID: 6586 RVA: 0x000E014C File Offset: 0x000DE54C
		public static Faction NewGeneratedFaction()
		{
			return FactionGenerator.NewGeneratedFaction(DefDatabase<FactionDef>.GetRandom());
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000E016C File Offset: 0x000DE56C
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
				FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
				factionBase.SetFaction(faction);
				factionBase.Tile = TileFinder.RandomFactionBaseTileFor(faction, false, null);
				factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
				Find.WorldObjects.Add(factionBase);
			}
			faction.GenerateNewLeader();
			return faction;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000E02D4 File Offset: 0x000DE6D4
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

		// Token: 0x04000F1F RID: 3871
		private const int MinStartVisibleFactions = 5;

		// Token: 0x04000F20 RID: 3872
		private static readonly FloatRange FactionBasesPer100kTiles = new FloatRange(75f, 85f);
	}
}
