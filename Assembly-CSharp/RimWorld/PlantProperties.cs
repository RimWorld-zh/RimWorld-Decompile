using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PlantProperties
	{
		public List<PlantBiomeRecord> wildBiomes = null;

		public float wildCommonalityMaxFraction = 1.25f;

		public IntRange wildClusterSizeRange = IntRange.one;

		public float wildClusterRadius = -1f;

		public List<string> sowTags = new List<string>();

		public float sowWork = 250f;

		public int sowMinSkill = 0;

		public bool blockAdjacentSow = false;

		public List<ResearchProjectDef> sowResearchPrerequisites = null;

		public float harvestWork = 150f;

		public float harvestYield = 0f;

		public ThingDef harvestedThingDef = null;

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

		public bool reproduces = true;

		public float reproduceRadius = 20f;

		public float reproduceMtbDays = 10f;

		public bool dieIfLeafless = false;

		public bool neverBlightable;

		public bool cavePlant;

		public float topWindExposure = 0.25f;

		public int maxMeshCount = 1;

		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		private string leaflessGraphicPath = (string)null;

		[Unsaved]
		public Graphic leaflessGraphic = null;

		private string immatureGraphicPath = (string)null;

		[Unsaved]
		public Graphic immatureGraphic = null;

		public const int MaxMaxMeshCount = 25;

		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty();
			}
		}

		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.0010000000474974513;
			}
		}

		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0.0;
			}
		}

		public float WildClusterRadiusActual
		{
			get
			{
				return (!(this.wildClusterRadius > 0.0)) ? this.reproduceRadius : this.wildClusterRadius;
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
				return (int)(this.LifespanDays * 60000.0);
			}
		}

		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanFraction > 0.0;
			}
		}

		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate()
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate()
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
		}

		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount <= 25)
				yield break;
			yield return "maxMeshCount > MaxMaxMeshCount";
			/*Error: Unable to find new state assignment for yield return*/;
		}

		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.sowMinSkill > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), 0, "");
				/*Error: Unable to find new state assignment for yield return*/;
			}
			string attributes2 = "";
			if (this.Harvestable)
			{
				if (!attributes2.NullOrEmpty())
				{
					attributes2 += ", ";
				}
				attributes2 += "Harvestable".Translate();
			}
			if (this.LimitedLifespan)
			{
				if (!attributes2.NullOrEmpty())
				{
					attributes2 += ", ";
				}
				attributes2 += "LimitedLifespan".Translate();
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), 0, "")
			{
				overrideReportText = "GrowingTimeDesc".Translate()
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
