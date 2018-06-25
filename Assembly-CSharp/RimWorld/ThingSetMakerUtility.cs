using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FE RID: 1790
	public static class ThingSetMakerUtility
	{
		// Token: 0x040015AE RID: 5550
		public static List<ThingDef> allGeneratableItems = new List<ThingDef>();

		// Token: 0x06002705 RID: 9989 RVA: 0x0014FF20 File Offset: 0x0014E320
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

		// Token: 0x06002706 RID: 9990 RVA: 0x0014FFA0 File Offset: 0x0014E3A0
		public static bool CanGenerate(ThingDef thingDef)
		{
			return (thingDef.category == ThingCategory.Item || thingDef.Minifiable) && (thingDef.category != ThingCategory.Item || thingDef.EverHaulable) && !thingDef.isUnfinishedThing && !thingDef.IsCorpse && thingDef.PlayerAcquirable && thingDef.graphicData != null && !typeof(MinifiedThing).IsAssignableFrom(thingDef.thingClass);
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x00150034 File Offset: 0x0014E434
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

		// Token: 0x06002708 RID: 9992 RVA: 0x001500C4 File Offset: 0x0014E4C4
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

		// Token: 0x06002709 RID: 9993 RVA: 0x0015010C File Offset: 0x0014E50C
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

		// Token: 0x0600270A RID: 9994 RVA: 0x00150168 File Offset: 0x0014E568
		public static bool PossibleToWeighNoMoreThan(ThingDef t, float maxMass, IEnumerable<ThingDef> allowedStuff)
		{
			bool result;
			if (maxMass == 3.40282347E+38f || t.category == ThingCategory.Pawn)
			{
				result = true;
			}
			else if (maxMass < 0f)
			{
				result = false;
			}
			else if (t.MadeFromStuff)
			{
				foreach (ThingDef stuff in allowedStuff)
				{
					if (t.GetStatValueAbstract(StatDefOf.Mass, stuff) <= maxMass)
					{
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = (t.GetStatValueAbstract(StatDefOf.Mass, null) <= maxMass);
			}
			return result;
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x00150230 File Offset: 0x0014E630
		public static bool TryGetRandomThingWhichCanWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, out ThingStuffPair thingStuffPair)
		{
			ThingDef thingDef;
			bool result;
			if (!(from x in candidates
			where ThingSetMakerUtility.PossibleToWeighNoMoreThan(x, maxMass, GenStuff.AllowedStuffsFor(x, stuffTechLevel))
			select x).TryRandomElement(out thingDef))
			{
				thingStuffPair = default(ThingStuffPair);
				result = false;
			}
			else
			{
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
				result = true;
			}
			return result;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x00150310 File Offset: 0x0014E710
		public static bool PossibleToWeighNoMoreThan(IEnumerable<ThingDef> candidates, TechLevel stuffTechLevel, float maxMass, int count)
		{
			bool result;
			if (maxMass == 3.40282347E+38f || count <= 0)
			{
				result = true;
			}
			else if (maxMass < 0f)
			{
				result = false;
			}
			else
			{
				float num = float.MaxValue;
				foreach (ThingDef thingDef in candidates)
				{
					num = Mathf.Min(num, ThingSetMakerUtility.GetMinMass(thingDef, stuffTechLevel));
				}
				result = (num <= maxMass * (float)count);
			}
			return result;
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x001503B4 File Offset: 0x0014E7B4
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

		// Token: 0x0600270E RID: 9998 RVA: 0x00150468 File Offset: 0x0014E868
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
	}
}
