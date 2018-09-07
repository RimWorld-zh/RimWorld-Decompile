using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class IngestibleProperties
	{
		[Unsaved]
		public ThingDef parent;

		public int maxNumToIngestAtOnce = 20;

		public List<IngestionOutcomeDoer> outcomeDoers;

		public int baseIngestTicks = 500;

		public float chairSearchRadius = 32f;

		public bool useEatingSpeedStat = true;

		public ThoughtDef tasteThought;

		public ThoughtDef specialThoughtDirect;

		public ThoughtDef specialThoughtAsIngredient;

		public EffecterDef ingestEffect;

		public EffecterDef ingestEffectEat;

		public SoundDef ingestSound;

		[MustTranslate]
		public string ingestCommandString;

		[MustTranslate]
		public string ingestReportString;

		[MustTranslate]
		public string ingestReportStringEat;

		public HoldOffsetSet ingestHoldOffsetStanding;

		public bool ingestHoldUsesTable = true;

		public FoodTypeFlags foodType;

		public float joy;

		public JoyKindDef joyKind;

		public ThingDef sourceDef;

		public FoodPreferability preferability;

		public bool nurseable;

		public float optimalityOffsetHumanlikes;

		public float optimalityOffsetFeedingAnimals;

		public DrugCategory drugCategory;

		[Unsaved]
		private float cachedNutrition = -1f;

		public IngestibleProperties()
		{
		}

		public JoyKindDef JoyKind
		{
			get
			{
				return (this.joyKind == null) ? JoyKindDefOf.Gluttonous : this.joyKind;
			}
		}

		public bool HumanEdible
		{
			get
			{
				return (FoodTypeFlags.OmnivoreHuman & this.foodType) != FoodTypeFlags.None;
			}
		}

		public bool IsMeal
		{
			get
			{
				return this.preferability >= FoodPreferability.MealAwful && this.preferability <= FoodPreferability.MealLavish;
			}
		}

		public float CachedNutrition
		{
			get
			{
				if (this.cachedNutrition == -1f)
				{
					this.cachedNutrition = this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null);
				}
				return this.cachedNutrition;
			}
		}

		public IEnumerable<string> ConfigErrors()
		{
			if (this.preferability == FoodPreferability.Undefined)
			{
				yield return "undefined preferability";
			}
			if (this.foodType == FoodTypeFlags.None)
			{
				yield return "no foodType";
			}
			if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
			{
				yield return string.Concat(new object[]
				{
					"Nutrition == 0 but preferability is ",
					this.preferability,
					" instead of ",
					FoodPreferability.NeverForNutrition
				});
			}
			if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
			{
				yield return "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
			}
			if (this.joy > 0f && this.joyKind == null)
			{
				yield return "joy > 0 with no joy kind";
			}
			if (this.joy == 0f && this.joyKind != null)
			{
				yield return "joy is 0 but joyKind is " + this.joyKind;
			}
			yield break;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.joy > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Joy".Translate(), this.joy.ToStringPercent("F2") + " (" + this.JoyKind.label + ")", 0, string.Empty);
			}
			if (this.outcomeDoers != null)
			{
				for (int i = 0; i < this.outcomeDoers.Count; i++)
				{
					foreach (StatDrawEntry s in this.outcomeDoers[i].SpecialDisplayStats(this.parent))
					{
						yield return s;
					}
				}
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IngestibleProperties $this;

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
				switch (num)
				{
				case 0u:
					if (this.preferability == FoodPreferability.Undefined)
					{
						this.$current = "undefined preferability";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_93;
				case 3u:
					goto IL_115;
				case 4u:
					goto IL_184;
				case 5u:
					goto IL_1C8;
				case 6u:
					goto IL_21C;
				default:
					return false;
				}
				if (this.foodType == FoodTypeFlags.None)
				{
					this.$current = "no foodType";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_93:
				if (this.parent.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f && this.preferability != FoodPreferability.NeverForNutrition)
				{
					this.$current = string.Concat(new object[]
					{
						"Nutrition == 0 but preferability is ",
						this.preferability,
						" instead of ",
						FoodPreferability.NeverForNutrition
					});
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_115:
				if (!this.parent.IsCorpse && this.preferability > FoodPreferability.DesperateOnlyForHumanlikes && !this.parent.socialPropernessMatters && this.parent.EverHaulable)
				{
					this.$current = "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_184:
				if (this.joy > 0f && this.joyKind == null)
				{
					this.$current = "joy > 0 with no joy kind";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1C8:
				if (this.joy == 0f && this.joyKind != null)
				{
					this.$current = "joy is 0 but joyKind is " + this.joyKind;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_21C:
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
				this.$disposing = true;
				this.$PC = -1;
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
				IngestibleProperties.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new IngestibleProperties.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal int <i>__1;

			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <s>__2;

			internal IngestibleProperties $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator1()
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
					if (this.joy > 0f)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Joy".Translate(), this.joy.ToStringPercent("F2") + " (" + base.JoyKind.label + ")", 0, string.Empty);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							s = enumerator.Current;
							this.$current = s;
							if (!this.$disposing)
							{
								this.$PC = 2;
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
					i++;
					goto IL_176;
				default:
					return false;
				}
				if (this.outcomeDoers == null)
				{
					goto IL_191;
				}
				i = 0;
				IL_176:
				if (i < this.outcomeDoers.Count)
				{
					enumerator = this.outcomeDoers[i].SpecialDisplayStats(this.parent).GetEnumerator();
					num = 4294967293u;
					goto Block_5;
				}
				IL_191:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				IngestibleProperties.<SpecialDisplayStats>c__Iterator1 <SpecialDisplayStats>c__Iterator = new IngestibleProperties.<SpecialDisplayStats>c__Iterator1();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}
		}
	}
}
