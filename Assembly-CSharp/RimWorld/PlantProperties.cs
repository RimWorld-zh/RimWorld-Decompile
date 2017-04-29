using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class PlantProperties
	{
		public const int MaxMaxMeshCount = 25;

		public List<PlantBiomeRecord> wildBiomes;

		public float wildCommonalityMaxFraction = 1.25f;

		public IntRange wildClusterSizeRange = IntRange.one;

		public float wildClusterRadius = -1f;

		public List<string> sowTags = new List<string>();

		public float sowWork = 250f;

		public int sowMinSkill;

		public bool blockAdjacentSow;

		public List<ResearchProjectDef> sowResearchPrerequisites;

		public float harvestWork = 150f;

		public float harvestYield;

		public ThingDef harvestedThingDef;

		public string harvestTag;

		public float harvestMinGrowth = 0.65f;

		public float harvestAfterGrowth;

		public bool harvestFailable = true;

		public SoundDef soundHarvesting;

		public SoundDef soundHarvestFinish;

		public float growDays = 2f;

		public float lifespanFraction = 6f;

		public float growMinGlow = 0.5f;

		public float growOptimalGlow = 1f;

		public bool dieIfLeafless;

		public float fertilityMin = 0.9f;

		public float fertilitySensitivity = 0.5f;

		public bool reproduces = true;

		public float reproduceRadius = 20f;

		public float reproduceMtbDays = 10f;

		public float topWindExposure = 0.25f;

		public int maxMeshCount = 1;

		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		private string leaflessGraphicPath;

		[Unsaved]
		public Graphic leaflessGraphic;

		private string immatureGraphicPath;

		[Unsaved]
		public Graphic immatureGraphic;

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

		public float WildClusterRadiusActual
		{
			get
			{
				if (this.wildClusterRadius > 0f)
				{
					return this.wildClusterRadius;
				}
				return this.reproduceRadius;
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

		[DebuggerHidden]
		public IEnumerable<string> ConfigErrors()
		{
			PlantProperties.<ConfigErrors>c__Iterator83 <ConfigErrors>c__Iterator = new PlantProperties.<ConfigErrors>c__Iterator83();
			<ConfigErrors>c__Iterator.<>f__this = this;
			PlantProperties.<ConfigErrors>c__Iterator83 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			PlantProperties.<SpecialDisplayStats>c__Iterator84 <SpecialDisplayStats>c__Iterator = new PlantProperties.<SpecialDisplayStats>c__Iterator84();
			<SpecialDisplayStats>c__Iterator.<>f__this = this;
			PlantProperties.<SpecialDisplayStats>c__Iterator84 expr_0E = <SpecialDisplayStats>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
