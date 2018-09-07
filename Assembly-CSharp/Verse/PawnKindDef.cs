using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class PawnKindDef : Def
	{
		public ThingDef race;

		public FactionDef defaultFactionType;

		[NoTranslate]
		public List<string> backstoryCategories;

		[MustTranslate]
		public string labelPlural;

		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		public float backstoryCryptosleepCommonality;

		public int minGenerationAge;

		public int maxGenerationAge = 999999;

		public bool factionLeader;

		public bool destroyGearOnDrop;

		public bool isFighter = true;

		public float combatPower = -1f;

		public bool canArriveManhunter = true;

		public bool canBeSapper;

		public float baseRecruitDifficulty = 0.5f;

		public bool aiAvoidCover;

		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		public QualityCategory itemQuality = QualityCategory.Normal;

		public bool forceNormalGearQuality;

		public FloatRange gearHealthRange = FloatRange.One;

		public FloatRange weaponMoney = FloatRange.Zero;

		[NoTranslate]
		public List<string> weaponTags;

		public FloatRange apparelMoney = FloatRange.Zero;

		public List<ThingDef> apparelRequired;

		[NoTranslate]
		public List<string> apparelTags;

		public float apparelAllowHeadgearChance = 1f;

		public bool apparelIgnoreSeasons;

		public Color apparelColor = Color.white;

		public FloatRange techHediffsMoney = FloatRange.Zero;

		[NoTranslate]
		public List<string> techHediffsTags;

		public float techHediffsChance;

		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		public PawnInventoryOption inventoryOptions;

		public float invNutrition;

		public ThingDef invFoodDef;

		public float chemicalAddictionChance;

		public float combatEnhancingDrugsChance;

		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		public bool trader;

		[MustTranslate]
		public string labelMale;

		[MustTranslate]
		public string labelMalePlural;

		[MustTranslate]
		public string labelFemale;

		[MustTranslate]
		public string labelFemalePlural;

		public IntRange wildGroupSize = IntRange.one;

		public float ecoSystemWeight = 1f;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__mg$cache0;

		public PawnKindDef()
		{
		}

		public RaceProperties RaceProps
		{
			get
			{
				return this.race.race;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.lifeStages.Count; i++)
			{
				this.lifeStages[i].ResolveReferences();
			}
		}

		public string GetLabelPlural(int count = -1)
		{
			if (!this.labelPlural.NullOrEmpty())
			{
				return this.labelPlural;
			}
			return Find.ActiveLanguageWorker.Pluralize(this.label, count);
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in base.ConfigErrors())
			{
				yield return err;
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			else if (this.RaceProps.Humanlike && this.backstoryCategories.NullOrEmpty<string>())
			{
				yield return "Humanlike needs backstoryCategories.";
			}
			if (this.baseRecruitDifficulty > 1.0001f)
			{
				yield return this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
			}
			if (this.combatPower < 0f)
			{
				yield return this.defName + " has no combatPower.";
			}
			if (this.weaponMoney != FloatRange.Zero)
			{
				float minCost = 999999f;
				int i;
				for (i = 0; i < this.weaponTags.Count; i++)
				{
					IEnumerable<ThingDef> enumerable = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (enumerable.Any<ThingDef>())
					{
						float a = minCost;
						IEnumerable<ThingDef> source = enumerable;
						if (PawnKindDef.<>f__mg$cache0 == null)
						{
							PawnKindDef.<>f__mg$cache0 = new Func<ThingDef, float>(PawnWeaponGenerator.CheapestNonDerpPriceFor);
						}
						minCost = Mathf.Min(a, source.Min(PawnKindDef.<>f__mg$cache0));
					}
				}
				if (minCost > this.weaponMoney.min)
				{
					yield return string.Concat(new object[]
					{
						"Cheapest weapon with one of my weaponTags costs ",
						minCost,
						" but weaponMoney min is ",
						this.weaponMoney.min,
						", so could end up weaponless."
					});
				}
			}
			if (!this.RaceProps.Humanlike && this.lifeStages.Count != this.RaceProps.lifeStageAges.Count)
			{
				yield return string.Concat(new object[]
				{
					"PawnKindDef defines ",
					this.lifeStages.Count,
					" lifeStages while race def defines ",
					this.RaceProps.lifeStageAges.Count
				});
			}
			if (this.apparelRequired != null)
			{
				for (int k = 0; k < this.apparelRequired.Count; k++)
				{
					for (int j = k + 1; j < this.apparelRequired.Count; j++)
					{
						if (!ApparelUtility.CanWearTogether(this.apparelRequired[k], this.apparelRequired[j], this.race.race.body))
						{
							yield return string.Concat(new object[]
							{
								"required apparel can't be worn together (",
								this.apparelRequired[k],
								", ",
								this.apparelRequired[j],
								")"
							});
						}
					}
				}
			}
			yield break;
		}

		public static PawnKindDef Named(string defName)
		{
			return DefDatabase<PawnKindDef>.GetNamed(defName, true);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal float <minCost>__2;

			internal int <i>__3;

			internal int <j>__4;

			internal PawnKindDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
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
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_149;
				case 3u:
					goto IL_149;
				case 4u:
					goto IL_18D;
				case 5u:
					goto IL_1D1;
				case 6u:
					goto IL_30C;
				case 7u:
					goto IL_3B5;
				case 8u:
					IL_49E:
					j++;
					goto IL_4AC;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						err = enumerator.Current;
						this.$current = err;
						if (!this.$disposing)
						{
							this.$PC = 1;
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
				if (this.race == null)
				{
					this.$current = "no race";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				if (base.RaceProps.Humanlike && this.backstoryCategories.NullOrEmpty<string>())
				{
					this.$current = "Humanlike needs backstoryCategories.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_149:
				if (this.baseRecruitDifficulty > 1.0001f)
				{
					this.$current = this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_18D:
				if (this.combatPower < 0f)
				{
					this.$current = this.defName + " has no combatPower.";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1D1:
				if (this.weaponMoney != FloatRange.Zero)
				{
					minCost = 999999f;
					int i;
					for (i = 0; i < this.weaponTags.Count; i++)
					{
						IEnumerable<ThingDef> enumerable = from d in DefDatabase<ThingDef>.AllDefs
						where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
						select d;
						if (enumerable.Any<ThingDef>())
						{
							float a = minCost;
							IEnumerable<ThingDef> source = enumerable;
							if (PawnKindDef.<>f__mg$cache0 == null)
							{
								PawnKindDef.<>f__mg$cache0 = new Func<ThingDef, float>(PawnWeaponGenerator.CheapestNonDerpPriceFor);
							}
							minCost = Mathf.Min(a, source.Min(PawnKindDef.<>f__mg$cache0));
						}
					}
					if (minCost > this.weaponMoney.min)
					{
						this.$current = string.Concat(new object[]
						{
							"Cheapest weapon with one of my weaponTags costs ",
							minCost,
							" but weaponMoney min is ",
							this.weaponMoney.min,
							", so could end up weaponless."
						});
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
				}
				IL_30C:
				if (!base.RaceProps.Humanlike && this.lifeStages.Count != base.RaceProps.lifeStageAges.Count)
				{
					this.$current = string.Concat(new object[]
					{
						"PawnKindDef defines ",
						this.lifeStages.Count,
						" lifeStages while race def defines ",
						base.RaceProps.lifeStageAges.Count
					});
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_3B5:
				if (this.apparelRequired != null)
				{
					i = 0;
					goto IL_4D5;
				}
				goto IL_4F0;
				IL_4AC:
				if (j >= this.apparelRequired.Count)
				{
					i++;
				}
				else
				{
					if (!ApparelUtility.CanWearTogether(this.apparelRequired[i], this.apparelRequired[j], this.race.race.body))
					{
						this.$current = string.Concat(new object[]
						{
							"required apparel can't be worn together (",
							this.apparelRequired[i],
							", ",
							this.apparelRequired[j],
							")"
						});
						if (!this.$disposing)
						{
							this.$PC = 8;
						}
						return true;
					}
					goto IL_49E;
				}
				IL_4D5:
				if (i < this.apparelRequired.Count)
				{
					j = i + 1;
					goto IL_4AC;
				}
				IL_4F0:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnKindDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new PawnKindDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}

			private sealed class <ConfigErrors>c__AnonStorey1
			{
				internal int i;

				internal PawnKindDef.<ConfigErrors>c__Iterator0 <>f__ref$0;

				public <ConfigErrors>c__AnonStorey1()
				{
				}

				internal bool <>m__0(ThingDef d)
				{
					return d.weaponTags != null && d.weaponTags.Contains(this.<>f__ref$0.$this.weaponTags[this.i]);
				}
			}
		}
	}
}
