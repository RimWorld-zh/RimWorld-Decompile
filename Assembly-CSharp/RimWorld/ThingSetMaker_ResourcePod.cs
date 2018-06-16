using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FD RID: 1789
	[HasDebugOutput]
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x060026F0 RID: 9968 RVA: 0x0014E26C File Offset: 0x0014C66C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef();
			float num = Rand.Range(150f, 600f);
			do
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
			}
			while (outThings.Count < 7 && num > thingDef.BaseMarketValue);
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x0014E330 File Offset: 0x0014C730
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x0014E36C File Offset: 0x0014C76C
		private static ThingDef RandomPodContentsDef()
		{
			int numMeats = (from x in ThingSetMaker_ResourcePod.PossiblePodContentsDefs()
			where x.IsMeat
			select x).Count<ThingDef>();
			int numLeathers = (from x in ThingSetMaker_ResourcePod.PossiblePodContentsDefs()
			where x.IsLeather
			select x).Count<ThingDef>();
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs().RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers));
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x0014E404 File Offset: 0x0014C804
		[DebugOutput]
		[Category("Incidents")]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x0014E484 File Offset: 0x0014C884
		[DebugOutput]
		[Category("Incidents")]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef().LabelCap);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x0014E4CC File Offset: 0x0014C8CC
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x040015A8 RID: 5544
		private const int MaxStacks = 7;

		// Token: 0x040015A9 RID: 5545
		private const float MaxMarketValue = 40f;

		// Token: 0x040015AA RID: 5546
		private const float MinMoney = 150f;

		// Token: 0x040015AB RID: 5547
		private const float MaxMoney = 600f;
	}
}
