using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class ThingDefGenerator_Leather
	{
		private const float HumanlikeLeatherCommonalityFactor = 0.02f;

		private const float BaseLeatherCommonality = 1.2f;

		private static bool GeneratesLeather(ThingDef sourceDef)
		{
			return sourceDef.category == ThingCategory.Pawn && sourceDef.GetStatValueAbstract(StatDefOf.LeatherAmount, null) > 0.0;
		}

		public static IEnumerable<ThingDef> ImpliedLeatherDefs()
		{
			int numLeathers = (from def in DefDatabase<ThingDef>.AllDefs
			where ThingDefGenerator_Leather.GeneratesLeather(def)
			select def).Count();
			float eachLeatherCommonality = (float)(1.0 / (float)numLeathers);
			using (List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator())
			{
				ThingDef sourceDef;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						sourceDef = enumerator.Current;
						if (ThingDefGenerator_Leather.GeneratesLeather(sourceDef) && sourceDef.race.useLeatherFrom == null)
							break;
						continue;
					}
					yield break;
				}
				ThingDef d = new ThingDef
				{
					resourceReadoutPriority = ResourceCountPriority.Middle,
					category = ThingCategory.Item,
					thingClass = typeof(ThingWithComps),
					graphicData = new GraphicData(),
					graphicData = 
					{
						graphicClass = typeof(Graphic_Single)
					},
					useHitPoints = true,
					selectable = true
				};
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
				d.description = "LeatherDesc".Translate(sourceDef.label);
				d.useHitPoints = true;
				d.SetStatBaseValue(StatDefOf.MaxHitPoints, 60f);
				d.SetStatBaseValue(StatDefOf.MarketValue, (float)(sourceDef.race.leatherMarketValueFactor * 2.0999999046325684));
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
				d.defName = sourceDef.defName + "_Leather";
				if (!sourceDef.race.leatherLabel.NullOrEmpty())
				{
					d.label = sourceDef.race.leatherLabel;
				}
				else
				{
					d.label = "LeatherLabel".Translate(sourceDef.label);
				}
				d.stuffProps.color = sourceDef.race.leatherColor;
				d.graphicData.color = sourceDef.race.leatherColor;
				d.graphicData.colorTwo = sourceDef.race.leatherColor;
				d.stuffProps.commonality = (float)(1.2000000476837158 * eachLeatherCommonality * sourceDef.race.leatherCommonalityFactor);
				StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.Insulation_Cold, sourceDef.race.leatherInsulation);
				StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, StatDefOf.Insulation_Heat, sourceDef.race.leatherInsulation);
				List<StatModifier> sfos = sourceDef.race.leatherStatFactors;
				if (sfos != null)
				{
					foreach (StatModifier item in sfos)
					{
						StatUtility.SetStatValueInList(ref d.stuffProps.statFactors, item.stat, item.value);
					}
				}
				sourceDef.race.leatherDef = d;
				yield return d;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_05f2:
			/*Error near IL_05f3: Unexpected return in MoveNext()*/;
		}
	}
}
