using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F6 RID: 1782
	public class ThingSetMaker_StackCount : ThingSetMaker
	{
		// Token: 0x060026C7 RID: 9927 RVA: 0x0014C548 File Offset: 0x0014A948
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

		// Token: 0x060026C8 RID: 9928 RVA: 0x0014C664 File Offset: 0x0014AA64
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
				int i = num2;
				while (i > 0)
				{
					ThingStuffPair thingStuffPair;
					if (!ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, stuffTechLevel, (num != 3.40282347E+38f) ? (num - num3) : 3.40282347E+38f, out thingStuffPair))
					{
						break;
					}
					Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
					ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
					int num4 = i;
					if (num != 3.40282347E+38f && !(thing is Pawn))
					{
						num4 = Mathf.Min(num4, Mathf.FloorToInt((num - num3) / thing.GetStatValue(StatDefOf.Mass, true)));
					}
					num4 = Mathf.Clamp(num4, 1, thing.def.stackLimit);
					thing.stackCount = num4;
					i -= num4;
					outThings.Add(thing);
					if (!(thing is Pawn))
					{
						num3 += thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
					}
				}
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0014C808 File Offset: 0x0014AC08
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0014C824 File Offset: 0x0014AC24
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
