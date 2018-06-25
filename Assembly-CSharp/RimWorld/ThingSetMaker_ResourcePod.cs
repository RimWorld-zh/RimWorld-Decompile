using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FB RID: 1787
	[HasDebugOutput]
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x040015AA RID: 5546
		private const int MaxStacks = 7;

		// Token: 0x040015AB RID: 5547
		private const float MaxMarketValue = 40f;

		// Token: 0x040015AC RID: 5548
		private const float MinMoney = 150f;

		// Token: 0x040015AD RID: 5549
		private const float MaxMoney = 600f;

		// Token: 0x060026ED RID: 9965 RVA: 0x0014E838 File Offset: 0x0014CC38
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

		// Token: 0x060026EE RID: 9966 RVA: 0x0014E8FC File Offset: 0x0014CCFC
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x0014E938 File Offset: 0x0014CD38
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

		// Token: 0x060026F0 RID: 9968 RVA: 0x0014E9D0 File Offset: 0x0014CDD0
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

		// Token: 0x060026F1 RID: 9969 RVA: 0x0014EA50 File Offset: 0x0014CE50
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

		// Token: 0x060026F2 RID: 9970 RVA: 0x0014EA98 File Offset: 0x0014CE98
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}
	}
}
