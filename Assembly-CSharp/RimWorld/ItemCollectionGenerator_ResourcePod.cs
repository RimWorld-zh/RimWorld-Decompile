using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ItemCollectionGenerator_ResourcePod : ItemCollectionGenerator
	{
		private const int MaxStacks = 7;

		private const float MaxMarketValue = 40f;

		private const float MinMoney = 150f;

		private const float MaxMoney = 600f;

		protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ItemCollectionGenerator_ResourcePod.RandomPodContentsDef();
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
				outThings.Add(thing);
				num -= (float)num2 * thingDef.BaseMarketValue;
				if (outThings.Count >= 7)
					break;
				if (num <= thingDef.BaseMarketValue)
					break;
			}
		}

		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability == Tradeability.Stockable && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1.0 && d.BaseMarketValue < 40.0 && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		private static ThingDef RandomPodContentsDef()
		{
			int numMeats = (from x in ItemCollectionGenerator_ResourcePod.PossiblePodContentsDefs()
			where x.IsMeat
			select x).Count();
			int numLeathers = (from x in ItemCollectionGenerator_ResourcePod.PossiblePodContentsDefs()
			where x.IsLeather
			select x).Count();
			return ItemCollectionGenerator_ResourcePod.PossiblePodContentsDefs().RandomElementByWeight((ThingDef d) => ItemCollectionGeneratorUtility.AdjustedSelectionWeight(d, numMeats, numLeathers));
		}

		public static void DebugLogPossiblePodContentsDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef item in ItemCollectionGenerator_ResourcePod.PossiblePodContentsDefs())
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
				stringBuilder.AppendLine(ItemCollectionGenerator_ResourcePod.RandomPodContentsDef().LabelCap);
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
