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
	public class ThingSetMaker_MarketValue : ThingSetMaker
	{
		private int nextSeed;

		[CompilerGenerated]
		private static Func<Thing, float> <>f__am$cache0;

		public ThingSetMaker_MarketValue()
		{
			this.nextSeed = Rand.Int;
		}

		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				return false;
			}
			IntRange? countRange = parms.countRange;
			if (countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
			if (totalMarketValueRange == null || parms.totalMarketValueRange.Value.max <= 0f)
			{
				return false;
			}
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
			return this.GeneratePossibleDefs(parms, out num, this.nextSeed).Any<ThingStuffPairWithQuality>();
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
			ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalValue, (Thing x) => x.MarketValue, maxMass);
			this.nextSeed++;
		}

		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
		}

		private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
		{
			return this.GetMinValue(thingStuffPair) * (float)thingStuffPair.thing.stackLimit;
		}

		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
		{
			Rand.PushState(seed);
			List<ThingStuffPairWithQuality> result = this.GeneratePossibleDefs(parms, out totalMarketValue);
			Rand.PopState();
			return result;
		}

		private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				totalMarketValue = 0f;
				return new List<ThingStuffPairWithQuality>();
			}
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
			IntRange? countRange = parms.countRange;
			IntRange intRange = (countRange == null) ? new IntRange(1, int.MaxValue) : countRange.Value;
			FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
			FloatRange floatRange = (totalMarketValueRange == null) ? FloatRange.Zero : totalMarketValueRange.Value;
			float? maxTotalMass = parms.maxTotalMass;
			float num = (maxTotalMass == null) ? float.MaxValue : maxTotalMass.Value;
			QualityGenerator? qualityGenerator = parms.qualityGenerator;
			QualityGenerator qualityGenerator2 = (qualityGenerator == null) ? QualityGenerator.BaseGen : qualityGenerator.Value;
			totalMarketValue = floatRange.RandomInRange;
			IntRange countRange2 = intRange;
			float totalValue = totalMarketValue;
			IEnumerable<ThingDef> allowed = enumerable;
			TechLevel techLevel3 = techLevel2;
			QualityGenerator qualityGenerator3 = qualityGenerator2;
			Func<ThingStuffPairWithQuality, float> getMinValue = new Func<ThingStuffPairWithQuality, float>(this.GetMinValue);
			Func<ThingStuffPairWithQuality, float> getMaxValue = new Func<ThingStuffPairWithQuality, float>(this.GetMaxValue);
			float maxMass = num;
			return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange2, totalValue, allowed, techLevel3, qualityGenerator3, getMinValue, getMaxValue, null, 100, maxMass);
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
				FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
				if (totalMarketValueRange == null || parms.totalMarketValueRange.Value.max == 3.40282347E+38f || ThingSetMakerUtility.GetMinMarketValue(t, techLevel) <= parms.totalMarketValueRange.Value.max)
				{
					yield return t;
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private static float <Generate>m__0(Thing x)
		{
			return x.MarketValue;
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebugSub>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingSetMakerParams parms;

			internal TechLevel <techLevel>__0;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <t>__1;

			internal ThingSetMaker_MarketValue $this;

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
						FloatRange? totalMarketValueRange = parms.totalMarketValueRange;
						if (totalMarketValueRange == null || parms.totalMarketValueRange.Value.max == 3.40282347E+38f || ThingSetMakerUtility.GetMinMarketValue(t, techLevel) <= parms.totalMarketValueRange.Value.max)
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
				ThingSetMaker_MarketValue.<AllGeneratableThingsDebugSub>c__Iterator0 <AllGeneratableThingsDebugSub>c__Iterator = new ThingSetMaker_MarketValue.<AllGeneratableThingsDebugSub>c__Iterator0();
				<AllGeneratableThingsDebugSub>c__Iterator.$this = this;
				<AllGeneratableThingsDebugSub>c__Iterator.parms = parms;
				return <AllGeneratableThingsDebugSub>c__Iterator;
			}
		}
	}
}
