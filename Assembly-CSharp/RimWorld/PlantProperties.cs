using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000271 RID: 625
	public class PlantProperties
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00061488 File Offset: 0x0005F888
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000614AC File Offset: 0x0005F8AC
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000614D0 File Offset: 0x0005F8D0
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000614F8 File Offset: 0x0005F8F8
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00061520 File Offset: 0x0005F920
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanFraction;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00061544 File Offset: 0x0005F944
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00061568 File Offset: 0x0005F968
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanFraction > 0f;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0006158C File Offset: 0x0005F98C
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x000615C4 File Offset: 0x0005F9C4
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000615E4 File Offset: 0x0005F9E4
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

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0006164C File Offset: 0x0005FA4C
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00061678 File Offset: 0x0005FA78
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

		// Token: 0x04000529 RID: 1321
		public List<PlantBiomeRecord> wildBiomes = null;

		// Token: 0x0400052A RID: 1322
		public int wildClusterRadius = -1;

		// Token: 0x0400052B RID: 1323
		public float wildClusterWeight = 15f;

		// Token: 0x0400052C RID: 1324
		public float wildOrder = 2f;

		// Token: 0x0400052D RID: 1325
		public bool wildEqualLocalDistribution = true;

		// Token: 0x0400052E RID: 1326
		public bool cavePlant;

		// Token: 0x0400052F RID: 1327
		public float cavePlantWeight = 1f;

		// Token: 0x04000530 RID: 1328
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x04000531 RID: 1329
		public float sowWork = 10f;

		// Token: 0x04000532 RID: 1330
		public int sowMinSkill = 0;

		// Token: 0x04000533 RID: 1331
		public bool blockAdjacentSow = false;

		// Token: 0x04000534 RID: 1332
		public List<ResearchProjectDef> sowResearchPrerequisites = null;

		// Token: 0x04000535 RID: 1333
		public bool mustBeWildToSow;

		// Token: 0x04000536 RID: 1334
		public float harvestWork = 10f;

		// Token: 0x04000537 RID: 1335
		public float harvestYield = 0f;

		// Token: 0x04000538 RID: 1336
		public ThingDef harvestedThingDef = null;

		// Token: 0x04000539 RID: 1337
		[NoTranslate]
		public string harvestTag;

		// Token: 0x0400053A RID: 1338
		public float harvestMinGrowth = 0.65f;

		// Token: 0x0400053B RID: 1339
		public float harvestAfterGrowth = 0f;

		// Token: 0x0400053C RID: 1340
		public bool harvestFailable = true;

		// Token: 0x0400053D RID: 1341
		public SoundDef soundHarvesting = null;

		// Token: 0x0400053E RID: 1342
		public SoundDef soundHarvestFinish = null;

		// Token: 0x0400053F RID: 1343
		public float growDays = 2f;

		// Token: 0x04000540 RID: 1344
		public float lifespanFraction = 6f;

		// Token: 0x04000541 RID: 1345
		public float growMinGlow = 0.51f;

		// Token: 0x04000542 RID: 1346
		public float growOptimalGlow = 1f;

		// Token: 0x04000543 RID: 1347
		public float fertilityMin = 0.9f;

		// Token: 0x04000544 RID: 1348
		public float fertilitySensitivity = 0.5f;

		// Token: 0x04000545 RID: 1349
		public bool dieIfLeafless = false;

		// Token: 0x04000546 RID: 1350
		public bool neverBlightable = false;

		// Token: 0x04000547 RID: 1351
		public bool interferesWithRoof = false;

		// Token: 0x04000548 RID: 1352
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x04000549 RID: 1353
		public float topWindExposure = 0.25f;

		// Token: 0x0400054A RID: 1354
		public int maxMeshCount = 1;

		// Token: 0x0400054B RID: 1355
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x0400054C RID: 1356
		[NoTranslate]
		private string leaflessGraphicPath = null;

		// Token: 0x0400054D RID: 1357
		[Unsaved]
		public Graphic leaflessGraphic = null;

		// Token: 0x0400054E RID: 1358
		[NoTranslate]
		private string immatureGraphicPath = null;

		// Token: 0x0400054F RID: 1359
		[Unsaved]
		public Graphic immatureGraphic = null;

		// Token: 0x04000550 RID: 1360
		public bool dropLeaves = false;

		// Token: 0x04000551 RID: 1361
		public const int MaxMaxMeshCount = 25;
	}
}
