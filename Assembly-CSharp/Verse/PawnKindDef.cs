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
		public ThingDef race = null;

		public FactionDef defaultFactionType;

		[NoTranslate]
		public string backstoryCategory = null;

		[MustTranslate]
		public string labelPlural = null;

		public List<PawnKindLifeStage> lifeStages = new List<PawnKindLifeStage>();

		public float backstoryCryptosleepCommonality = 0f;

		public int minGenerationAge = 0;

		public int maxGenerationAge = 999999;

		public bool factionLeader = false;

		public bool destroyGearOnDrop = false;

		public bool isFighter = true;

		public float combatPower = -1f;

		public bool canArriveManhunter = true;

		public bool canBeSapper = false;

		public float baseRecruitDifficulty = 0.5f;

		public bool aiAvoidCover = false;

		public FloatRange fleeHealthThresholdRange = new FloatRange(-0.4f, 0.4f);

		public QualityCategory itemQuality = QualityCategory.Normal;

		public bool forceNormalGearQuality = false;

		public FloatRange gearHealthRange = FloatRange.One;

		public FloatRange weaponMoney = FloatRange.Zero;

		[NoTranslate]
		public List<string> weaponTags = null;

		public FloatRange apparelMoney = FloatRange.Zero;

		public List<ThingDef> apparelRequired = null;

		[NoTranslate]
		public List<string> apparelTags = null;

		public float apparelAllowHeadgearChance = 1f;

		public bool apparelIgnoreSeasons = false;

		public FloatRange techHediffsMoney = FloatRange.Zero;

		[NoTranslate]
		public List<string> techHediffsTags = null;

		public float techHediffsChance = 0f;

		public List<ThingDefCountClass> fixedInventory = new List<ThingDefCountClass>();

		public PawnInventoryOption inventoryOptions = null;

		public float invNutrition = 0f;

		public ThingDef invFoodDef = null;

		public float chemicalAddictionChance = 0f;

		public float combatEnhancingDrugsChance = 0f;

		public IntRange combatEnhancingDrugsCount = IntRange.zero;

		public bool trader = false;

		[MustTranslate]
		public string labelMale = null;

		[MustTranslate]
		public string labelMalePlural = null;

		[MustTranslate]
		public string labelFemale = null;

		[MustTranslate]
		public string labelFemalePlural = null;

		public IntRange wildGroupSize = IntRange.one;

		public float ecoSystemWeight = 1f;

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
			string result;
			if (!this.labelPlural.NullOrEmpty())
			{
				result = this.labelPlural;
			}
			else
			{
				result = Find.ActiveLanguageWorker.Pluralize(this.label, count);
			}
			return result;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.race == null)
			{
				yield return "no race";
			}
			else if (this.RaceProps.Humanlike && this.backstoryCategory.NullOrEmpty())
			{
				yield return "Humanlike needs backstoryCategory.";
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
					IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
					where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
					select d;
					if (source.Any<ThingDef>())
					{
						minCost = Mathf.Min(minCost, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
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

			private static Func<ThingDef, float> <>f__am$cache0;

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
					goto IL_14D;
				case 3u:
					goto IL_14D;
				case 4u:
					goto IL_191;
				case 5u:
					goto IL_1D5;
				case 6u:
					goto IL_315;
				case 7u:
					IL_3BF:
					if (this.apparelRequired != null)
					{
						i = 0;
						goto IL_4E4;
					}
					goto IL_500;
				case 8u:
					IL_4AB:
					j++;
					goto IL_4BA;
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
				if (base.RaceProps.Humanlike && this.backstoryCategory.NullOrEmpty())
				{
					this.$current = "Humanlike needs backstoryCategory.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_14D:
				if (this.baseRecruitDifficulty > 1.0001f)
				{
					this.$current = this.defName + " recruitDifficulty is greater than 1. 1 means impossible to recruit.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_191:
				if (this.combatPower < 0f)
				{
					this.$current = this.defName + " has no combatPower.";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1D5:
				if (this.weaponMoney != FloatRange.Zero)
				{
					minCost = 999999f;
					int i;
					for (i = 0; i < this.weaponTags.Count; i++)
					{
						IEnumerable<ThingDef> source = from d in DefDatabase<ThingDef>.AllDefs
						where d.weaponTags != null && d.weaponTags.Contains(this.weaponTags[i])
						select d;
						if (source.Any<ThingDef>())
						{
							minCost = Mathf.Min(minCost, source.Min((ThingDef d) => PawnWeaponGenerator.CheapestNonDerpPriceFor(d)));
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
				IL_315:
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
				goto IL_3BF;
				IL_4BA:
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
					goto IL_4AB;
				}
				IL_4E4:
				if (i < this.apparelRequired.Count)
				{
					j = i + 1;
					goto IL_4BA;
				}
				IL_500:
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

			private static float <>m__0(ThingDef d)
			{
				return PawnWeaponGenerator.CheapestNonDerpPriceFor(d);
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
