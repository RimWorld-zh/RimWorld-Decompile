using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Corpses
	{
		private const float DaysToStartRot = 2.5f;

		private const float DaysToDessicate = 5f;

		private const float RotDamagePerDay = 2f;

		private const float DessicatedDamagePerDay = 0.7f;

		public static IEnumerable<ThingDef> ImpliedCorpseDefs()
		{
			List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ThingDef raceDef = enumerator.Current;
					if (raceDef.category == ThingCategory.Pawn)
					{
						ThingDef d = new ThingDef
						{
							category = ThingCategory.Item,
							thingClass = typeof(Corpse),
							selectable = true,
							tickerType = TickerType.Rare,
							altitudeLayer = AltitudeLayer.ItemImportant,
							scatterableOnMapGen = false
						};
						d.SetStatBaseValue(StatDefOf.Beauty, -150f);
						d.SetStatBaseValue(StatDefOf.DeteriorationRate, 2f);
						d.alwaysHaulable = true;
						d.soundDrop = SoundDef.Named("Corpse_Drop");
						d.pathCost = 15;
						d.socialPropernessMatters = false;
						d.tradeability = Tradeability.Never;
						d.inspectorTabs = new List<Type>();
						d.inspectorTabs.Add(typeof(ITab_Pawn_Health));
						d.inspectorTabs.Add(typeof(ITab_Pawn_Character));
						d.inspectorTabs.Add(typeof(ITab_Pawn_Gear));
						d.inspectorTabs.Add(typeof(ITab_Pawn_Social));
						d.comps.Add(new CompProperties_Forbiddable());
						d.recipes = new List<RecipeDef>();
						if (raceDef.race.IsMechanoid)
						{
							d.recipes.Add(RecipeDefOf.RemoveMechanoidBodyPart);
						}
						else
						{
							d.recipes.Add(RecipeDefOf.RemoveBodyPart);
						}
						d.defName = raceDef.defName + "_Corpse";
						d.label = "CorpseLabel".Translate(raceDef.label);
						d.description = "CorpseDesc".Translate(raceDef.label);
						d.soundImpactDefault = raceDef.soundImpactDefault;
						d.SetStatBaseValue(StatDefOf.Flammability, raceDef.GetStatValueAbstract(StatDefOf.Flammability, null));
						d.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)raceDef.BaseMaxHitPoints);
						d.SetStatBaseValue(StatDefOf.Mass, raceDef.statBases.GetStatOffsetFromList(StatDefOf.Mass));
						d.ingestible = new IngestibleProperties();
						IngestibleProperties ing = d.ingestible;
						ing.foodType = FoodTypeFlags.Corpse;
						ing.sourceDef = raceDef;
						ing.preferability = FoodPreferability.DesperateOnly;
						DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(ing, "tasteThought", ThoughtDefOf.AteCorpse.defName);
						ing.nutrition = 1f;
						ing.maxNumToIngestAtOnce = 1;
						ing.ingestEffect = EffecterDefOf.EatMeat;
						ing.ingestSound = SoundDefOf.RawMeat_Eat;
						ing.specialThoughtDirect = raceDef.race.FleshType.ateDirect;
						if (raceDef.race.IsFlesh)
						{
							CompProperties_Rottable rottable = new CompProperties_Rottable
							{
								daysToRotStart = 2.5f,
								daysToDessicated = 5f,
								rotDamagePerDay = 2f,
								dessicatedDamagePerDay = 0.7f
							};
							d.comps.Add(rottable);
							CompProperties_SpawnerFilth corpseBileSpawner = new CompProperties_SpawnerFilth
							{
								filthDef = ThingDefOf.FilthCorpseBile,
								spawnCountOnSpawn = 0,
								spawnMtbHours = 0f,
								spawnRadius = 0.1f,
								spawnEveryDays = 1f,
								requiredRotStage = new RotStage?(RotStage.Rotting)
							};
							d.comps.Add(corpseBileSpawner);
						}
						if (d.thingCategories == null)
						{
							d.thingCategories = new List<ThingCategoryDef>();
						}
						if (raceDef.race.Humanlike)
						{
							DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, ThingCategoryDefOf.CorpsesHumanlike.defName);
						}
						else
						{
							DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, raceDef.race.FleshType.corpseCategory.defName);
						}
						raceDef.race.corpseDef = d;
						yield return d;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
