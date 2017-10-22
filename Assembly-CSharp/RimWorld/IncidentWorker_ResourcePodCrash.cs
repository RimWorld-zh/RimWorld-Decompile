using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		private const int MaxStacks = 7;

		private const float MaxMarketValue = 40f;

		private const float MinMoney = 150f;

		private const float MaxMoney = 600f;

		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability == Tradeability.Stockable && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1.0 && d.BaseMarketValue < 40.0 && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		private static ThingDef RandomPodContentsDef()
		{
			Func<ThingDef, bool> isLeather = (Func<ThingDef, bool>)((ThingDef d) => d.category == ThingCategory.Item && d.thingCategories != null && d.thingCategories.Contains(ThingCategoryDefOf.Leathers));
			Func<ThingDef, bool> isMeat = (Func<ThingDef, bool>)((ThingDef d) => d.category == ThingCategory.Item && d.thingCategories != null && d.thingCategories.Contains(ThingCategoryDefOf.MeatRaw));
			int numLeathers = DefDatabase<ThingDef>.AllDefs.Where(isLeather).Count();
			int numMeats = DefDatabase<ThingDef>.AllDefs.Where(isMeat).Count();
			return IncidentWorker_ResourcePodCrash.PossiblePodContentsDefs().RandomElementByWeight((Func<ThingDef, float>)delegate(ThingDef d)
			{
				float num = 100f;
				if (isLeather(d))
				{
					num = (float)(num * (5.0 / (float)numLeathers));
				}
				if (isMeat(d))
				{
					num = (float)(num * (5.0 / (float)numMeats));
				}
				return num;
			});
		}

		public override bool TryExecute(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			ThingDef thingDef = IncidentWorker_ResourcePodCrash.RandomPodContentsDef();
			List<Thing> list = new List<Thing>();
			float num = Rand.Range(150f, 600f);
			while (true)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				if ((float)num2 * thing.def.BaseMarketValue > num)
				{
					num2 = Mathf.FloorToInt(num / thing.def.BaseMarketValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				list.Add(thing);
				num -= (float)num2 * thingDef.BaseMarketValue;
				if (list.Count >= 7)
					break;
				if (num <= thingDef.BaseMarketValue)
					break;
			}
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, list, 110, false, true, true);
			Find.LetterStack.ReceiveLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.Good, new TargetInfo(intVec, map, false), (string)null);
			return true;
		}

		public static void DebugLogPossiblePodContentsDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef item in IncidentWorker_ResourcePodCrash.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(item.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		public static void DebugLogPodContentsChoices()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(IncidentWorker_ResourcePodCrash.RandomPodContentsDef().LabelCap);
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
