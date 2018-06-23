using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F9 RID: 1785
	[HasDebugOutput]
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x040015A6 RID: 5542
		private const int MaxStacks = 7;

		// Token: 0x040015A7 RID: 5543
		private const float MaxMarketValue = 40f;

		// Token: 0x040015A8 RID: 5544
		private const float MinMoney = 150f;

		// Token: 0x040015A9 RID: 5545
		private const float MaxMoney = 600f;

		// Token: 0x060026EA RID: 9962 RVA: 0x0014E488 File Offset: 0x0014C888
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

		// Token: 0x060026EB RID: 9963 RVA: 0x0014E54C File Offset: 0x0014C94C
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x0014E588 File Offset: 0x0014C988
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

		// Token: 0x060026ED RID: 9965 RVA: 0x0014E620 File Offset: 0x0014CA20
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

		// Token: 0x060026EE RID: 9966 RVA: 0x0014E6A0 File Offset: 0x0014CAA0
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

		// Token: 0x060026EF RID: 9967 RVA: 0x0014E6E8 File Offset: 0x0014CAE8
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}
	}
}
