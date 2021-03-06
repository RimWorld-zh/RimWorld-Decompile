﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		private const int MaxStacks = 7;

		private const float MaxMarketValue = 40f;

		private const float MinMoney = 150f;

		private const float MaxMoney = 600f;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		public ThingSetMaker_ResourcePod()
		{
		}

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

		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

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

		[Category("Incidents")]
		[DebugOutput]
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

		[Category("Incidents")]
		[DebugOutput]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef().LabelCap);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		[CompilerGenerated]
		private static bool <PossiblePodContentsDefs>m__0(ThingDef d)
		{
			return d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher));
		}

		[CompilerGenerated]
		private static bool <RandomPodContentsDef>m__1(ThingDef x)
		{
			return x.IsMeat;
		}

		[CompilerGenerated]
		private static bool <RandomPodContentsDef>m__2(ThingDef x)
		{
			return x.IsLeather;
		}

		[CompilerGenerated]
		private sealed class <RandomPodContentsDef>c__AnonStorey0
		{
			internal int numMeats;

			internal int numLeathers;

			public <RandomPodContentsDef>c__AnonStorey0()
			{
			}

			internal float <>m__0(ThingDef d)
			{
				return ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, this.numMeats, this.numLeathers);
			}
		}
	}
}
