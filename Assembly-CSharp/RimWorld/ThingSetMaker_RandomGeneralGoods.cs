using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
	{
		private static Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[] GoodsWeights = new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>[]
		{
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Meals, 1f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood, 0.75f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs, 0.234f),
			new Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>(ThingSetMaker_RandomGeneralGoods.GoodsType.Resources, 0.234f)
		};

		[CompilerGenerated]
		private static Func<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		public ThingSetMaker_RandomGeneralGoods()
		{
		}

		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange? countRange = parms.countRange;
			IntRange intRange = (countRange == null) ? new IntRange(10, 20) : countRange.Value;
			TechLevel? techLevel = parms.techLevel;
			TechLevel techLevel2 = (techLevel == null) ? TechLevel.Undefined : techLevel.Value;
			int num = Mathf.Max(intRange.RandomInRange, 1);
			for (int i = 0; i < num; i++)
			{
				outThings.Add(this.GenerateSingle(techLevel2));
			}
		}

		private Thing GenerateSingle(TechLevel techLevel)
		{
			Thing thing = null;
			int num = 0;
			while (thing == null && num < 50)
			{
				IEnumerable<Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float>> goodsWeights = ThingSetMaker_RandomGeneralGoods.GoodsWeights;
				switch (goodsWeights.RandomElementByWeight((Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x) => x.Second).First)
				{
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Meals:
					thing = this.RandomMeals(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.RawFood:
					thing = this.RandomRawFood(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Medicine:
					thing = this.RandomMedicine(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Drugs:
					thing = this.RandomDrugs(techLevel);
					break;
				case ThingSetMaker_RandomGeneralGoods.GoodsType.Resources:
					thing = this.RandomResources(techLevel);
					break;
				default:
					throw new Exception();
				}
				num++;
			}
			return thing;
		}

		private Thing RandomMeals(TechLevel techLevel)
		{
			ThingDef thingDef;
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.Pemmican;
			}
			else
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					thingDef = ThingDefOf.MealSimple;
				}
				else if ((double)value < 0.75)
				{
					thingDef = ThingDefOf.MealFine;
				}
				else
				{
					thingDef = ThingDefOf.MealSurvivalPack;
				}
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 10);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		private Thing RandomRawFood(TechLevel techLevel)
		{
			ThingDef thingDef;
			Thing result;
			if (!this.PossibleRawFood(techLevel).TryRandomElement(out thingDef))
			{
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int max = Mathf.Min(thingDef.stackLimit, 75);
				thing.stackCount = Rand.RangeInclusive(1, max);
				result = thing;
			}
			return result;
		}

		private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
		{
			return from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= techLevel && x.ingestible.preferability < FoodPreferability.MealAwful
			select x;
		}

		private Thing RandomMedicine(TechLevel techLevel)
		{
			bool flag = Rand.Value < 0.75f && techLevel >= ThingDefOf.MedicineHerbal.techLevel;
			ThingDef thingDef;
			if (flag)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsMedicine && x.techLevel <= techLevel
				select x).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			}
			else if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x).TryRandomElement(out thingDef))
			{
				return null;
			}
			if (techLevel.IsNeolithicOrWorse())
			{
				thingDef = ThingDefOf.MedicineHerbal;
			}
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int max = Mathf.Min(thingDef.stackLimit, 20);
			thing.stackCount = Rand.RangeInclusive(1, max);
			return thing;
		}

		private Thing RandomDrugs(TechLevel techLevel)
		{
			ThingDef thingDef;
			Thing result;
			if (!(from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= techLevel
			select x).TryRandomElement(out thingDef))
			{
				result = null;
			}
			else
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int max = Mathf.Min(thingDef.stackLimit, 25);
				thing.stackCount = Rand.RangeInclusive(1, max);
				result = thing;
			}
			return result;
		}

		private Thing RandomResources(TechLevel techLevel)
		{
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel, false);
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			int num = Mathf.Min(thingDef.stackLimit, 75);
			thing.stackCount = Rand.RangeInclusive(num / 2, num);
			return thing;
		}

		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 <AllGeneratableThingsDebugSub>c__AnonStorey = new ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4();
			<AllGeneratableThingsDebugSub>c__AnonStorey.<>f__ref$0 = this;
			ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 <AllGeneratableThingsDebugSub>c__AnonStorey2 = <AllGeneratableThingsDebugSub>c__AnonStorey;
			TechLevel? techLevel = parms.techLevel;
			<AllGeneratableThingsDebugSub>c__AnonStorey2.techLevel = ((techLevel == null) ? TechLevel.Undefined : techLevel.Value);
			if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.Pemmican;
			}
			else
			{
				yield return ThingDefOf.MealSimple;
				yield return ThingDefOf.MealFine;
				yield return ThingDefOf.MealSurvivalPack;
			}
			foreach (ThingDef t in this.PossibleRawFood(<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel))
			{
				yield return t;
			}
			foreach (ThingDef t2 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsMedicine
			select x)
			{
				yield return t2;
			}
			foreach (ThingDef t3 in from x in ThingSetMakerUtility.allGeneratableItems
			where x.IsDrug && x.techLevel <= <AllGeneratableThingsDebugSub>c__AnonStorey.techLevel
			select x)
			{
				yield return t3;
			}
			if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
			{
				yield return ThingDefOf.WoodLog;
			}
			else
			{
				foreach (ThingDef t4 in from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d)
				select d)
				{
					yield return t4;
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ThingSetMaker_RandomGeneralGoods()
		{
		}

		[CompilerGenerated]
		private static float <GenerateSingle>m__0(Pair<ThingSetMaker_RandomGeneralGoods.GoodsType, float> x)
		{
			return x.Second;
		}

		[CompilerGenerated]
		private static float <RandomMedicine>m__1(ThingDef x)
		{
			return x.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
		}

		[CompilerGenerated]
		private static bool <RandomMedicine>m__2(ThingDef x)
		{
			return x.IsMedicine;
		}

		private enum GoodsType
		{
			None,
			Meals,
			RawFood,
			Medicine,
			Drugs,
			Resources
		}

		[CompilerGenerated]
		private sealed class <PossibleRawFood>c__AnonStorey1
		{
			internal TechLevel techLevel;

			public <PossibleRawFood>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && x.techLevel <= this.techLevel && x.ingestible.preferability < FoodPreferability.MealAwful;
			}
		}

		[CompilerGenerated]
		private sealed class <RandomMedicine>c__AnonStorey2
		{
			internal TechLevel techLevel;

			public <RandomMedicine>c__AnonStorey2()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.IsMedicine && x.techLevel <= this.techLevel;
			}
		}

		[CompilerGenerated]
		private sealed class <RandomDrugs>c__AnonStorey3
		{
			internal TechLevel techLevel;

			public <RandomDrugs>c__AnonStorey3()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.IsDrug && x.techLevel <= this.techLevel;
			}
		}

		[CompilerGenerated]
		private sealed class <AllGeneratableThingsDebugSub>c__Iterator0 : IEnumerable, IEnumerable<ThingDef>, IEnumerator, IDisposable, IEnumerator<ThingDef>
		{
			internal ThingSetMakerParams parms;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <t>__1;

			internal IEnumerator<ThingDef> $locvar1;

			internal ThingDef <t>__2;

			internal IEnumerator<ThingDef> $locvar2;

			internal ThingDef <t>__3;

			internal IEnumerator<ThingDef> $locvar3;

			internal ThingDef <t>__4;

			internal ThingSetMaker_RandomGeneralGoods $this;

			internal ThingDef $current;

			internal bool $disposing;

			internal int $PC;

			private ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 $locvar4;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private static Func<ThingDef, bool> <>f__am$cache1;

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
					<AllGeneratableThingsDebugSub>c__AnonStorey = new ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4();
					<AllGeneratableThingsDebugSub>c__AnonStorey.<>f__ref$0 = this;
					ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0.<AllGeneratableThingsDebugSub>c__AnonStorey4 <AllGeneratableThingsDebugSub>c__AnonStorey2 = <AllGeneratableThingsDebugSub>c__AnonStorey;
					TechLevel? techLevel = parms.techLevel;
					<AllGeneratableThingsDebugSub>c__AnonStorey2.techLevel = ((techLevel == null) ? TechLevel.Undefined : techLevel.Value);
					if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
					{
						this.$current = ThingDefOf.Pemmican;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					this.$current = ThingDefOf.MealSimple;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 1u:
					break;
				case 2u:
					this.$current = ThingDefOf.MealFine;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = ThingDefOf.MealSurvivalPack;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					break;
				case 5u:
					Block_8:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							t = enumerator.Current;
							this.$current = t;
							if (!this.$disposing)
							{
								this.$PC = 5;
							}
							flag = true;
							return true;
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
					enumerator2 = (from x in ThingSetMakerUtility.allGeneratableItems
					where x.IsMedicine
					select x).GetEnumerator();
					num = 4294967293u;
					goto Block_10;
				case 6u:
					goto IL_1F4;
				case 7u:
					goto IL_294;
				case 8u:
					goto IL_3F3;
				case 9u:
					goto IL_37A;
				default:
					return false;
				}
				enumerator = base.PossibleRawFood(<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel).GetEnumerator();
				num = 4294967293u;
				goto Block_8;
				Block_10:
				try
				{
					IL_1F4:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						t2 = enumerator2.Current;
						this.$current = t2;
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				enumerator3 = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsDrug && x.techLevel <= <AllGeneratableThingsDebugSub>c__AnonStorey.techLevel
				select x).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_294:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						t3 = enumerator3.Current;
						this.$current = t3;
						if (!this.$disposing)
						{
							this.$PC = 7;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
				}
				if (<AllGeneratableThingsDebugSub>c__AnonStorey.techLevel.IsNeolithicOrWorse())
				{
					this.$current = ThingDefOf.WoodLog;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				enumerator4 = (from d in DefDatabase<ThingDef>.AllDefsListForReading
				where BaseGenUtility.IsCheapWallStuff(d)
				select d).GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_37A:
					switch (num)
					{
					}
					if (enumerator4.MoveNext())
					{
						t4 = enumerator4.Current;
						this.$current = t4;
						if (!this.$disposing)
						{
							this.$PC = 9;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
						}
					}
				}
				IL_3F3:
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
				case 5u:
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
				case 6u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 7u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
						}
					}
					break;
				case 9u:
					try
					{
					}
					finally
					{
						if (enumerator4 != null)
						{
							enumerator4.Dispose();
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
				ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0 <AllGeneratableThingsDebugSub>c__Iterator = new ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0();
				<AllGeneratableThingsDebugSub>c__Iterator.$this = this;
				<AllGeneratableThingsDebugSub>c__Iterator.parms = parms;
				return <AllGeneratableThingsDebugSub>c__Iterator;
			}

			private static bool <>m__0(ThingDef x)
			{
				return x.IsMedicine;
			}

			private static bool <>m__1(ThingDef d)
			{
				return BaseGenUtility.IsCheapWallStuff(d);
			}

			private sealed class <AllGeneratableThingsDebugSub>c__AnonStorey4
			{
				internal TechLevel techLevel;

				internal ThingSetMaker_RandomGeneralGoods.<AllGeneratableThingsDebugSub>c__Iterator0 <>f__ref$0;

				public <AllGeneratableThingsDebugSub>c__AnonStorey4()
				{
				}

				internal bool <>m__0(ThingDef x)
				{
					return x.IsDrug && x.techLevel <= this.techLevel;
				}
			}
		}
	}
}
