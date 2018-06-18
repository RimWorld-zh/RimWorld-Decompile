using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F3 RID: 1779
	public class ThingSetMaker_Count : ThingSetMaker
	{
		// Token: 0x060026AC RID: 9900 RVA: 0x0014AFD0 File Offset: 0x001493D0
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			bool result;
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				result = false;
			}
			else
			{
				IntRange? countRange = parms.countRange;
				if (countRange != null && parms.countRange.Value.max <= 0)
				{
					result = false;
				}
				else
				{
					float? maxTotalMass = parms.maxTotalMass;
					if (maxTotalMass != null && parms.maxTotalMass != 3.40282347E+38f)
					{
						IEnumerable<ThingDef> candidates = this.AllowedThingDefs(parms);
						TechLevel? techLevel = parms.techLevel;
						TechLevel stuffTechLevel = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
						float value = parms.maxTotalMass.Value;
						IntRange? countRange2 = parms.countRange;
						if (!ThingSetMakerUtility.PossibleToWeighNoMoreThan(candidates, stuffTechLevel, value, (countRange2 == null) ? 1 : parms.countRange.Value.max))
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x0014B0EC File Offset: 0x001494EC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (enumerable.Any<ThingDef>())
			{
				TechLevel? techLevel = parms.techLevel;
				TechLevel stuffTechLevel = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
				IntRange? countRange = parms.countRange;
				IntRange intRange = (countRange == null) ? IntRange.one : countRange.Value;
				float? maxTotalMass = parms.maxTotalMass;
				float num = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
				int num2 = Mathf.Max(intRange.RandomInRange, 1);
				float num3 = 0f;
				for (int i = 0; i < num2; i++)
				{
					ThingStuffPair thingStuffPair;
					if (!ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, stuffTechLevel, (num != 3.40282347E+38f) ? (num - num3) : 3.40282347E+38f, out thingStuffPair))
					{
						break;
					}
					Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
					ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
					outThings.Add(thing);
					if (!(thing is Pawn))
					{
						num3 += thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
					}
				}
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x0014B234 File Offset: 0x00149634
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x0014B250 File Offset: 0x00149650
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel? techLevel2 = parms.techLevel;
			TechLevel techLevel = (techLevel2 == null) ? TechLevel.Undefined : techLevel2.Value;
			foreach (ThingDef t in this.AllowedThingDefs(parms))
			{
				float? maxTotalMass = parms.maxTotalMass;
				if (maxTotalMass != null && parms.maxTotalMass != 3.40282347E+38f)
				{
					float? maxTotalMass2 = parms.maxTotalMass;
					if (ThingSetMakerUtility.GetMinMass(t, techLevel) > maxTotalMass2)
					{
						continue;
					}
				}
				yield return t;
			}
			yield break;
		}
	}
}
