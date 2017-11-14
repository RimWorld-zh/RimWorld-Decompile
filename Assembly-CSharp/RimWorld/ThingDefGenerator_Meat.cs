using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Meat
	{
		public static IEnumerable<ThingDef> ImpliedMeatDefs()
		{
			foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.ToList())
			{
				if (item.category == ThingCategory.Pawn && item.race.useMeatFrom == null)
				{
					if (item.race.IsFlesh)
					{
						ThingDef d = new ThingDef();
						d.resourceReadoutPriority = ResourceCountPriority.Middle;
						d.category = ThingCategory.Item;
						d.thingClass = typeof(ThingWithComps);
						d.graphicData = new GraphicData();
						d.graphicData.graphicClass = typeof(Graphic_Single);
						d.useHitPoints = true;
						d.selectable = true;
						d.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
						d.altitudeLayer = AltitudeLayer.Item;
						d.stackLimit = 75;
						d.comps.Add(new CompProperties_Forbiddable());
						CompProperties_Rottable rotProps = new CompProperties_Rottable
						{
							daysToRotStart = 2f,
							rotDestroys = true
						};
						d.comps.Add(rotProps);
						d.comps.Add(new CompProperties_FoodPoisoningChance());
						d.tickerType = TickerType.Rare;
						d.SetStatBaseValue(StatDefOf.Beauty, -20f);
						d.alwaysHaulable = true;
						d.rotatable = false;
						d.pathCost = 15;
						d.drawGUIOverlay = true;
						d.socialPropernessMatters = true;
						d.category = ThingCategory.Item;
						d.description = "MeatDesc".Translate(item.label);
						d.useHitPoints = true;
						d.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
						d.SetStatBaseValue(StatDefOf.DeteriorationRate, 6f);
						d.SetStatBaseValue(StatDefOf.Mass, 0.03f);
						d.SetStatBaseValue(StatDefOf.Flammability, 0.5f);
						d.BaseMarketValue = ThingDefGenerator_Meat.GetMeatMarketValue(item);
						if (d.thingCategories == null)
						{
							d.thingCategories = new List<ThingCategoryDef>();
						}
						DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, "MeatRaw");
						d.ingestible = new IngestibleProperties();
						d.ingestible.foodType = FoodTypeFlags.Meat;
						d.ingestible.preferability = FoodPreferability.RawBad;
						DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(d.ingestible, "tasteThought", ThoughtDefOf.AteRawFood.defName);
						d.ingestible.nutrition = 0.05f;
						d.ingestible.ingestEffect = EffecterDefOf.EatMeat;
						d.ingestible.ingestSound = SoundDef.Named("RawMeat_Eat");
						d.ingestible.specialThoughtDirect = item.race.FleshType.ateDirect;
						d.ingestible.specialThoughtAsIngredient = item.race.FleshType.ateAsIngredient;
						if (item.race.Humanlike)
						{
							d.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/MeatHuman";
						}
						else
						{
							if (item.race.baseBodySize < 0.699999988079071)
							{
								d.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/MeatSmall";
							}
							else
							{
								d.graphicData.texPath = "Things/Item/Resource/MeatFoodRaw/MeatBig";
							}
							d.graphicData.color = item.race.meatColor;
						}
						d.defName = item.defName + "_Meat";
						if (item.race.meatLabel.NullOrEmpty())
						{
							d.label = "MeatLabel".Translate(item.label);
						}
						else
						{
							d.label = item.race.meatLabel;
						}
						d.ingestible.sourceDef = item;
						item.race.meatDef = d;
						yield return d;
						/*Error: Unable to find new state assignment for yield return*/;
					}
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(item.race, "meatDef", "Steel");
				}
			}
			yield break;
			IL_058b:
			/*Error near IL_058c: Unexpected return in MoveNext()*/;
		}

		private static float GetMeatMarketValue(ThingDef sourceDef)
		{
			if (sourceDef.race.Humanlike)
			{
				return 0.8f;
			}
			return 2f;
		}
	}
}
