using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ThingSetMakerUtility
	{
		public static List<ThingDef> allGeneratableItems = new List<ThingDef>();

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

		public static void Reset()
		{
			ThingSetMakerUtility.allGeneratableItems.Clear();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (ThingSetMakerUtility.CanGenerate(thingDef))
				{
					ThingSetMakerUtility.allGeneratableItems.Add(thingDef);
				}
			}
			ThingSetMaker_Meteorite.Reset();
		}

		public static bool CanGenerate(ThingDef thingDef)
		{
			return (thingDef.category == ThingCategory.Item || thingDef.Minifiable) && (thingDef.category != ThingCategory.Item || thingDef.EverHaulable) && !thingDef.isUnfinishedThing && !thingDef.IsCorpse && thingDef.PlayerAcquirable && thingDef.graphicData != null && !typeof(MinifiedThing).IsAssignableFrom(thingDef.thingClass);
		}

		public static IEnumerable<ThingDef> GetAllowedThingDefs(ThingSetMakerParams parms)
		{
			ThingSetMakerUtility.<GetAllowedThingDefs>c__AnonStorey0 <GetAllowedThingDefs>c__AnonStorey = new ThingSetMakerUtility.<GetAllowedThingDefs>c__AnonStorey0();
			<GetAllowedThingDefs>c__AnonStorey.parms = parms;
			ThingSetMakerUtility.<GetAllowedThingDefs>c__AnonStorey0 <GetAllowedThingDefs>c__AnonStorey2 = <GetAllowedThingDefs>c__AnonStorey;
			TechLevel? techLevel = <GetAllowedThingDefs>c__AnonStorey.parms.techLevel;
			<GetAllowedThingDefs>c__AnonStorey2.techLevel = ((techLevel == null) ? TechLevel.Undefined : techLevel.Value);
			IEnumerable<ThingDef> source = <GetAllowedThingDefs>c__AnonStorey.parms.filter.AllowedThingDefs;
			if (<GetAllowedThingDefs>c__AnonStorey.techLevel != TechLevel.Undefined)
			{
				source = from x in source
				where x.techLevel <= <GetAllowedThingDefs>c__AnonStorey.techLevel
				select x;
			}
			return source.Where(delegate(ThingDef x)
			{
				if (ThingSetMakerUtility.CanGenerate(x))
				{
					float? maxThingMarketValue = <GetAllowedThingDefs>c__AnonStorey.parms.maxThingMarketValue;
					if (maxThingMarketValue != null)
					{
						float? maxThingMarketValue2 = <GetAllowedThingDefs>c__AnonStorey.parms.maxThingMarketValue;
						if (!(x.BaseMarketValue <= maxThingMarketValue2))
						{
							goto IL_7E;
						}
					}
					return <GetAllowedThingDefs>c__AnonStorey.parms.validator == null || <GetAllowedThingDefs>c__AnonStorey.parms.validator(x);
				}
				IL_7E:
				return false;
			});
		}

		public static void AssignQuality(Thing thing, QualityGenerator? qualityGenerator)
		{
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityGenerator qualityGenerator2 = (qualityGenerator == null) ? QualityGenerator.BaseGen : qualityGenerator.Value;
				QualityCategory q = QualityUtility.GenerateQuality(qualityGenerator2);
				compQuality.SetQuality(q, ArtGenerationContext.Outsider);
			}
		}

		public static float AdjustedBigCategoriesSelectionWeight(ThingDef d, int numMeats, int numLeathers)
		{
			float num = 1f;
			if (d.IsMeat)
			{
				num *= Mathf.Min(5f / (float)numMeats, 1f);
			}
			if (d.IsLeather)
			{
				num *= Mathf.Min(5f / (float)numLeathers, 1f);
			}
			return num;
		}

		public static bool PossibleToWeighNoMoreThan(ThingDef t, float maxMass, IEnumerable<ThingDef> allowedStuff)
		{
			if (maxMass == 3.40282347E+38f || t.category == ThingCategory.Pawn)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			if (t.MadeFromStuff)
			{
				foreach (ThingDef stuff in allowedStuff)
				{
					if (t.GetStatValueAbstract(StatDefOf.Mass, stuff) <= maxMass)
					{
						return true;
					}
				}
				return false;
			}
			return t.GetStatValueAbstract(StatDefOf.Mass, null) <= maxMass;
		}

		public static bool TryGetRandomThingWhichCanWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, out ThingStuffPair thingStuffPair)
		{
			ThingDef thingDef;
			if (!(from x in candidates
			where ThingSetMakerUtility.PossibleToWeighNoMoreThan(x, maxMass, GenStuff.AllowedStuffsFor(x, stuffTechLevel))
			select x).TryRandomElement(out thingDef))
			{
				thingStuffPair = default(ThingStuffPair);
				return false;
			}
			ThingDef stuff;
			if (thingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel)
				where thingDef.GetStatValueAbstract(StatDefOf.Mass, x) <= maxMass
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					thingStuffPair = default(ThingStuffPair);
					return false;
				}
			}
			else
			{
				stuff = null;
			}
			thingStuffPair = new ThingStuffPair(thingDef, stuff, 1f);
			return true;
		}

		public static bool PossibleToWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, int count)
		{
			if (maxMass == 3.40282347E+38f || count <= 0)
			{
				return true;
			}
			if (maxMass < 0f)
			{
				return false;
			}
			float num = float.MaxValue;
			foreach (ThingDef thingDef in candidates)
			{
				num = Mathf.Min(num, ThingSetMakerUtility.GetMinMass(thingDef, stuffTechLevel));
			}
			return num <= maxMass * (float)count;
		}

		public static float GetMinMass(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				foreach (ThingDef thingDef2 in GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel))
				{
					if (thingDef2.stuffProps.commonality > 0f)
					{
						num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, thingDef2));
					}
				}
			}
			else
			{
				num = Mathf.Min(num, thingDef.GetStatValueAbstract(StatDefOf.Mass, null));
			}
			return num;
		}

		public static float GetMinMarketValue(ThingDef thingDef, TechLevel stuffTechLevel)
		{
			float num = float.MaxValue;
			if (thingDef.MadeFromStuff)
			{
				foreach (ThingDef thingDef2 in GenStuff.AllowedStuffsFor(thingDef, stuffTechLevel))
				{
					if (thingDef2.stuffProps.commonality > 0f)
					{
						num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, thingDef2, QualityCategory.Awful), true));
					}
				}
			}
			else
			{
				num = Mathf.Min(num, StatDefOf.MarketValue.Worker.GetValue(StatRequest.For(thingDef, null, QualityCategory.Awful), true));
			}
			return num;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThingSetMakerUtility()
		{
		}

		[CompilerGenerated]
		private static float <TryGetRandomThingWhichCanWeighNoMoreThan>m__0(ThingDef x)
		{
			return x.stuffProps.commonality;
		}

		[CompilerGenerated]
		private sealed class <GetAllowedThingDefs>c__AnonStorey0
		{
			internal TechLevel techLevel;

			internal ThingSetMakerParams parms;

			public <GetAllowedThingDefs>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.techLevel <= this.techLevel;
			}

			internal bool <>m__1(ThingDef x)
			{
				if (ThingSetMakerUtility.CanGenerate(x))
				{
					float? maxThingMarketValue = this.parms.maxThingMarketValue;
					if (maxThingMarketValue != null)
					{
						float? maxThingMarketValue2 = this.parms.maxThingMarketValue;
						if (!(x.BaseMarketValue <= maxThingMarketValue2))
						{
							goto IL_7E;
						}
					}
					return this.parms.validator == null || this.parms.validator(x);
				}
				IL_7E:
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <TryGetRandomThingWhichCanWeighNoMoreThan>c__AnonStorey1
		{
			internal float maxMass;

			internal TechLevel stuffTechLevel;

			internal ThingDef thingDef;

			public <TryGetRandomThingWhichCanWeighNoMoreThan>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return ThingSetMakerUtility.PossibleToWeighNoMoreThan(x, this.maxMass, GenStuff.AllowedStuffsFor(x, this.stuffTechLevel));
			}

			internal bool <>m__1(ThingDef x)
			{
				return this.thingDef.GetStatValueAbstract(StatDefOf.Mass, x) <= this.maxMass;
			}
		}
	}
}
