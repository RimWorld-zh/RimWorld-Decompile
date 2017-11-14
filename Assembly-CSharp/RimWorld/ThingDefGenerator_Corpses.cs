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
			foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.ToList())
			{
				if (item.category == ThingCategory.Pawn)
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
					d.inspectorTabs.Add(typeof(ITab_Pawn_Combat));
					d.comps.Add(new CompProperties_Forbiddable());
					d.recipes = new List<RecipeDef>();
					if (item.race.IsMechanoid)
					{
						d.recipes.Add(RecipeDefOf.RemoveMechanoidBodyPart);
					}
					else
					{
						d.recipes.Add(RecipeDefOf.RemoveBodyPart);
					}
					d.defName = item.defName + "_Corpse";
					d.label = "CorpseLabel".Translate(item.label);
					d.description = "CorpseDesc".Translate(item.label);
					d.soundImpactDefault = item.soundImpactDefault;
					d.SetStatBaseValue(StatDefOf.Flammability, item.GetStatValueAbstract(StatDefOf.Flammability, null));
					d.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)item.BaseMaxHitPoints);
					d.SetStatBaseValue(StatDefOf.Mass, item.statBases.GetStatOffsetFromList(StatDefOf.Mass));
					d.ingestible = new IngestibleProperties();
					IngestibleProperties ing = d.ingestible;
					ing.foodType = FoodTypeFlags.Corpse;
					ing.sourceDef = item;
					ing.preferability = FoodPreferability.DesperateOnly;
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(ing, "tasteThought", ThoughtDefOf.AteCorpse.defName);
					ing.nutrition = 1f;
					ing.maxNumToIngestAtOnce = 1;
					ing.ingestEffect = EffecterDefOf.EatMeat;
					ing.ingestSound = SoundDefOf.RawMeat_Eat;
					ing.specialThoughtDirect = item.race.FleshType.ateDirect;
					if (item.race.IsFlesh)
					{
						CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
						compProperties_Rottable.daysToRotStart = 2.5f;
						compProperties_Rottable.daysToDessicated = 5f;
						compProperties_Rottable.rotDamagePerDay = 2f;
						compProperties_Rottable.dessicatedDamagePerDay = 0.7f;
						d.comps.Add(compProperties_Rottable);
						CompProperties_SpawnerFilth compProperties_SpawnerFilth = new CompProperties_SpawnerFilth();
						compProperties_SpawnerFilth.filthDef = ThingDefOf.FilthCorpseBile;
						compProperties_SpawnerFilth.spawnCountOnSpawn = 0;
						compProperties_SpawnerFilth.spawnMtbHours = 0f;
						compProperties_SpawnerFilth.spawnRadius = 0.1f;
						compProperties_SpawnerFilth.spawnEveryDays = 1f;
						compProperties_SpawnerFilth.requiredRotStage = RotStage.Rotting;
						d.comps.Add(compProperties_SpawnerFilth);
					}
					if (d.thingCategories == null)
					{
						d.thingCategories = new List<ThingCategoryDef>();
					}
					if (item.race.Humanlike)
					{
						DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, ThingCategoryDefOf.CorpsesHumanlike.defName);
					}
					else
					{
						DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, item.race.FleshType.corpseCategory.defName);
					}
					item.race.corpseDef = d;
					yield return d;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_057d:
			/*Error near IL_057e: Unexpected return in MoveNext()*/;
		}
	}
}
