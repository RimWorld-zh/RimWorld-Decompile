using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_Nutrition : ThingSetMaker
	{
		private int nextSeed;

		[CompilerGenerated]
		private static Func<Thing, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingStuffPairWithQuality, float> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingStuffPairWithQuality, float> <>f__am$cache4;

		public ThingSetMaker_Nutrition()
		{
			this.nextSeed = Rand.Int;
		}

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
					FloatRange? totalNutritionRange = parms.totalNutritionRange;
					if (totalNutritionRange == null || parms.totalNutritionRange.Value.max <= 0f)
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
							if (!ThingSetMakerUtility.PossibleToWeighNoMoreThan(candidates, stuffTechLevel, value, (countRange2 == null) ? 1 : parms.countRange.Value.min))
							{
								return false;
							}
						}
						float num;
						result = this.GeneratePossibleDefs(parms, out num, this.nextSeed).Any<ThingStuffPairWithQuality>();
					}
				}
			}
			return result;
		}

		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			float? maxTotalMass = parms.maxTotalMass;
			float maxMass = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
			float totalValue;
			List<ThingStuffPairWithQuality> list = this.GeneratePossibleDefs(parms, out totalValue, this.nextSeed);
			for (int i = 0; i < list.Count; i++)
			{
				outThings.Add(list[i].MakeThing());
			}
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.GetStatValue(StatDefOf.Nutrition, true), maxMass);
			this.nextSeed++;
		}

		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalNutrition);
			Rand.PopState();
			return result;
		}

		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			List<ThingStuffPairWithQuality> result;
			if (!enumerable.Any<ThingDef>())
			{
				totalNutrition = 0f;
				result = new List<ThingStuffPairWithQuality>();
			}
			else
			{
				IntRange? countRange = parms.countRange;
				IntRange intRange = (countRange == null) ? new IntRange(1, int.MaxValue) : countRange.Value;
				FloatRange? totalNutritionRange = parms.totalNutritionRange;
				FloatRange floatRange = (totalNutritionRange == null) ? FloatRange.Zero : totalNutritionRange.Value;
				TechLevel? techLevel = parms.techLevel;
				TechLevel techLevel2 = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
				float? maxTotalMass = parms.maxTotalMass;
				float num = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
				QualityGenerator? qualityGenerator = parms.qualityGenerator;
				QualityGenerator qualityGenerator2 = (qualityGenerator == null) ? QualityGenerator.BaseGen : qualityGenerator.Value;
				totalNutrition = floatRange.RandomInRange;
				int numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
				int numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
				Func<ThingDef, float> func = (ThingDef x) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(x, numMeats, numLeathers);
				IntRange countRange2 = intRange;
				float totalValue = totalNutrition;
				IEnumerable<ThingDef> allowed = enumerable;
				TechLevel techLevel3 = techLevel2;
				QualityGenerator qualityGenerator3 = qualityGenerator2;
				Func<ThingStuffPairWithQuality, float> getMinValue = (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition);
				Func<ThingStuffPairWithQuality, float> getMaxValue = (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit;
				Func<ThingDef, float> weightSelector = func;
				float maxMass = num;
				result = ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange2, totalValue, allowed, techLevel3, qualityGenerator3, getMinValue, getMaxValue, weightSelector, 100, maxMass);
			}
			return result;
		}

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
				FloatRange? totalNutritionRange = parms.totalNutritionRange;
				if (totalNutritionRange == null || parms.totalNutritionRange.Value.max == 3.40282347E+38f || !t.IsNutritionGivingIngestible || t.ingestible.CachedNutrition <= parms.totalNutritionRange.Value.max)
				{
					yield return t;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static float <Generate>m__0(Thing x)
		{
			return x.GetStatValue(StatDefOf.Nutrition, true);
		}

		[CompilerGenerated]
		private static bool <GeneratePossibleDefs>m__1(ThingDef x)
		{
			return x.IsMeat;
		}

		[CompilerGenerated]
		private static bool <GeneratePossibleDefs>m__2(ThingDef x)
		{
			return x.IsLeather;
		}

		[CompilerGenerated]
		private static float <GeneratePossibleDefs>m__3(ThingStuffPairWithQuality x)
		{
			return x.GetStatValue(StatDefOf.Nutrition);
		}

		[CompilerGenerated]
		private static float <GeneratePossibleDefs>m__4(ThingStuffPairWithQuality x)
		{
			return x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit;
		}

		[CompilerGenerated]
		private sealed class <GeneratePossibleDefs>c__AnonStorey1
		{
			internal int numMeats;

			internal int numLeathers;

			public <GeneratePossibleDefs>c__AnonStorey1()
			{
			}

			internal float <>m__0(ThingDef x)
			{
				return ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(x, this.numMeats, this.numLeathers);
			}
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebugSub>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingSetMakerParams parms;

			internal TechLevel <techLevel>__0;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <t>__1;

			internal ThingSetMaker_Nutrition $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllGeneratableThingsDebugSub>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					TechLevel? techLevel2 = parms.techLevel;
					techLevel = ((techLevel2 == null) ? TechLevel.Undefined : techLevel2.Value);
					enumerator = this.AllowedThingDefs(parms).GetEnumerator();
					num = 4294967293u;
					break;
				}
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						t = enumerator.Current;
						float? maxTotalMass = parms.maxTotalMass;
						if (maxTotalMass != null && parms.maxTotalMass != 3.40282347E+38f)
						{
							float? maxTotalMass2 = parms.maxTotalMass;
							if (ThingSetMakerUtility.GetMinMass(t, techLevel) > maxTotalMass2)
							{
								continue;
							}
						}
						FloatRange? totalNutritionRange = parms.totalNutritionRange;
						if (totalNutritionRange == null || parms.totalNutritionRange.Value.max == 3.40282347E+38f || !t.IsNutritionGivingIngestible || t.ingestible.CachedNutrition <= parms.totalNutritionRange.Value.max)
						{
							this.$current = t;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			ThingDef IEnumerator<ThingDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.ThingDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingSetMaker_Nutrition.<AllGeneratableThingsDebugSub>c__Iterator0 <AllGeneratableThingsDebugSub>c__Iterator = new ThingSetMaker_Nutrition.<AllGeneratableThingsDebugSub>c__Iterator0();
				<AllGeneratableThingsDebugSub>c__Iterator.$this = this;
				<AllGeneratableThingsDebugSub>c__Iterator.parms = parms;
				return <AllGeneratableThingsDebugSub>c__Iterator;
			}
		}
	}
}
