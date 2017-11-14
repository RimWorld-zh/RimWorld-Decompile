using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Leather
	{
		private const float HumanlikeLeatherCommonalityFactor = 0.02f;

		private const float BaseLeatherCommonality = 1.2f;

		[CompilerGenerated]
		private static Func<ThingDef, bool> _003C_003Ef__mg_0024cache0;

		private static bool GeneratesLeather(ThingDef sourceDef)
		{
			return sourceDef.category == ThingCategory.Pawn && sourceDef.GetStatValueAbstract(StatDefOf.LeatherAmount, null) > 0.0;
		}

		public static IEnumerable<ThingDef> ImpliedLeatherDefs()
		{
			int numLeathers = DefDatabase<ThingDef>.AllDefs.Where(ThingDefGenerator_Leather.GeneratesLeather).Count();
			float eachLeatherCommonality = (float)(1.0 / (float)numLeathers);
			foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.ToList())
			{
				if (ThingDefGenerator_Leather.GeneratesLeather(item) && item.race.useLeatherFrom == null)
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
					d.SetStatBaseValue(StatDefOf.Beauty, -20f);
					d.SetStatBaseValue(StatDefOf.DeteriorationRate, 2f);
					d.alwaysHaulable = true;
					d.drawGUIOverlay = true;
					d.rotatable = false;
					d.pathCost = 15;
					d.category = ThingCategory.Item;
					d.description = "LeatherDesc".Translate(item.label);
					d.useHitPoints = true;
					d.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
					d.SetStatBaseValue(StatDefOf.MarketValue, (float)(item.race.leatherMarketValueFactor * 2.0999999046325684));
					d.SetStatBaseValue(StatDefOf.Mass, 0.03f);
					d.SetStatBaseValue(StatDefOf.Flammability, 1f);
					if (d.thingCategories == null)
					{
						d.thingCategories = new List<ThingCategoryDef>();
					}
					DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.thingCategories, "Leathers");
					d.graphicData.texPath = "Things/Item/Resource/Cloth";
					d.stuffProps = new StuffProperties();
					DirectXmlCrossRefLoader.RegisterListWantsCrossRef(d.stuffProps.categories, "Leathery");
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.MarketValue, 1.3f);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.ArmorRating_Blunt, 1.5f);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.ArmorRating_Sharp, 1.5f);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.ArmorRating_Heat, 1.7f);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.ArmorRating_Electric, 4f);
					d.defName = item.defName + "_Leather";
					if (!item.race.leatherLabel.NullOrEmpty())
					{
						d.label = item.race.leatherLabel;
					}
					else
					{
						d.label = "LeatherLabel".Translate(item.label);
					}
					d.stuffProps.color = item.race.leatherColor;
					d.graphicData.color = item.race.leatherColor;
					d.graphicData.colorTwo = item.race.leatherColor;
					d.stuffProps.commonality = (float)(1.2000000476837158 * eachLeatherCommonality * item.race.leatherCommonalityFactor);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.Insulation_Cold, item.race.leatherInsulation);
					StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.Insulation_Heat, item.race.leatherInsulation);
					List<StatModifier> sfos = item.race.leatherStatFactors;
					if (sfos != null)
					{
						foreach (StatModifier item2 in sfos)
						{
							StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, item2.stat, item2.value);
						}
					}
					item.race.leatherDef = d;
					yield return d;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_05e9:
			/*Error near IL_05ea: Unexpected return in MoveNext()*/;
		}
	}
}
