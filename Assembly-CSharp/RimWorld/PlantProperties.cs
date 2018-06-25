using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class PlantProperties
	{
		public List<PlantBiomeRecord> wildBiomes = null;

		public int wildClusterRadius = -1;

		public float wildClusterWeight = 15f;

		public float wildOrder = 2f;

		public bool wildEqualLocalDistribution = true;

		public bool cavePlant;

		public float cavePlantWeight = 1f;

		[NoTranslate]
		public List<string> sowTags = new List<string>();

		public float sowWork = 10f;

		public int sowMinSkill = 0;

		public bool blockAdjacentSow = false;

		public List<ResearchProjectDef> sowResearchPrerequisites = null;

		public bool mustBeWildToSow;

		public float harvestWork = 10f;

		public float harvestYield = 0f;

		public ThingDef harvestedThingDef = null;

		[NoTranslate]
		public string harvestTag;

		public float harvestMinGrowth = 0.65f;

		public float harvestAfterGrowth = 0f;

		public bool harvestFailable = true;

		public SoundDef soundHarvesting = null;

		public SoundDef soundHarvestFinish = null;

		public float growDays = 2f;

		public float lifespanFraction = 6f;

		public float growMinGlow = 0.51f;

		public float growOptimalGlow = 1f;

		public float fertilityMin = 0.9f;

		public float fertilitySensitivity = 0.5f;

		public bool dieIfLeafless = false;

		public bool neverBlightable = false;

		public bool interferesWithRoof = false;

		public PlantPurpose purpose = PlantPurpose.Misc;

		public float topWindExposure = 0.25f;

		public int maxMeshCount = 1;

		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		[NoTranslate]
		private string leaflessGraphicPath = null;

		[Unsaved]
		public Graphic leaflessGraphic = null;

		[NoTranslate]
		private string immatureGraphicPath = null;

		[Unsaved]
		public Graphic immatureGraphic = null;

		public bool dropLeaves = false;

		public const int MaxMaxMeshCount = 25;

		public PlantProperties()
		{
		}

		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanFraction;
			}
		}

		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanFraction > 0f;
			}
		}

		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
		}

		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.sowMinSkill > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), 0, "");
			}
			string attributes = "";
			if (this.Harvestable)
			{
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
				}
				attributes += "Harvestable".Translate();
			}
			if (this.LimitedLifespan)
			{
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
				}
				attributes += "LimitedLifespan".Translate();
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), 0, "")
			{
				overrideReportText = "GrowingTimeDesc".Translate()
			};
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), this.fertilityMin.ToStringPercent(), 0, "");
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), this.fertilitySensitivity.ToStringPercent(), 0, "");
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), this.growMinGlow.ToStringPercent(), 0, "");
			if (!attributes.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), attributes, 0, "");
			}
			if (this.LimitedLifespan)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), this.LifespanDays.ToString("0.##") + " " + "Days".Translate(), 0, "");
			}
			if (this.harvestYield > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), this.harvestYield.ToString("F0"), 0, "");
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <PostLoadSpecial>c__AnonStorey2
		{
			internal ThingDef parentDef;

			internal PlantProperties $this;

			public <PostLoadSpecial>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.$this.leaflessGraphic = GraphicDatabase.Get(this.parentDef.graphicData.graphicClass, this.$this.leaflessGraphicPath, this.parentDef.graphic.Shader, this.parentDef.graphicData.drawSize, this.parentDef.graphicData.color, this.parentDef.graphicData.colorTwo);
			}

			internal void <>m__1()
			{
				this.$this.immatureGraphic = GraphicDatabase.Get(this.parentDef.graphicData.graphicClass, this.$this.immatureGraphicPath, this.parentDef.graphic.Shader, this.parentDef.graphicData.drawSize, this.parentDef.graphicData.color, this.parentDef.graphicData.colorTwo);
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal PlantProperties $this;

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
					if (this.maxMeshCount > 25)
					{
						this.$current = "maxMeshCount > MaxMaxMeshCount";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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
				PlantProperties.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new PlantProperties.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal string <attributes>__0;

			internal StatDrawEntry <gt>__0;

			internal PlantProperties $this;

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
				switch (num)
				{
				case 0u:
					if (this.sowMinSkill > 0)
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), 0, "");
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
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), this.fertilityMin.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), this.fertilitySensitivity.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), this.growMinGlow.ToStringPercent(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					if (!attributes.NullOrEmpty())
					{
						this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), attributes, 0, "");
						if (!this.$disposing)
						{
							this.$PC = 6;
						}
						return true;
					}
					goto IL_2E0;
				case 6u:
					goto IL_2E0;
				case 7u:
					goto IL_352;
				case 8u:
					goto IL_3B2;
				default:
					return false;
				}
				attributes = "";
				if (base.Harvestable)
				{
					if (!attributes.NullOrEmpty())
					{
						attributes += ", ";
					}
					attributes += "Harvestable".Translate();
				}
				if (base.LimitedLifespan)
				{
					if (!attributes.NullOrEmpty())
					{
						attributes += ", ";
					}
					attributes += "LimitedLifespan".Translate();
				}
				StatDrawEntry gt = new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), 0, "");
				gt.overrideReportText = "GrowingTimeDesc".Translate();
				this.$current = gt;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_2E0:
				if (base.LimitedLifespan)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), base.LifespanDays.ToString("0.##") + " " + "Days".Translate(), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_352:
				if (this.harvestYield > 0f)
				{
					this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), this.harvestYield.ToString("F0"), 0, "");
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				}
				IL_3B2:
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PlantProperties.<SpecialDisplayStats>c__Iterator1 <SpecialDisplayStats>c__Iterator = new PlantProperties.<SpecialDisplayStats>c__Iterator1();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}
		}
	}
}
