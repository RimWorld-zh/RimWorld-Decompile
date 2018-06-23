using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A2 RID: 674
	public class IncidentDef : Def
	{
		// Token: 0x04000617 RID: 1559
		public Type workerClass;

		// Token: 0x04000618 RID: 1560
		public IncidentCategoryDef category = null;

		// Token: 0x04000619 RID: 1561
		public List<IncidentTargetTypeDef> targetTypes = null;

		// Token: 0x0400061A RID: 1562
		public float baseChance = 0f;

		// Token: 0x0400061B RID: 1563
		public IncidentPopulationEffect populationEffect = IncidentPopulationEffect.None;

		// Token: 0x0400061C RID: 1564
		public int earliestDay = 0;

		// Token: 0x0400061D RID: 1565
		public int minPopulation = 0;

		// Token: 0x0400061E RID: 1566
		public float minRefireDays = 0f;

		// Token: 0x0400061F RID: 1567
		public int minDifficulty = 0;

		// Token: 0x04000620 RID: 1568
		public bool pointsScaleable = false;

		// Token: 0x04000621 RID: 1569
		public float minThreatPoints = -1f;

		// Token: 0x04000622 RID: 1570
		public List<BiomeDef> allowedBiomes;

		// Token: 0x04000623 RID: 1571
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04000624 RID: 1572
		[NoTranslate]
		public List<string> refireCheckTags;

		// Token: 0x04000625 RID: 1573
		public SimpleCurve chanceFactorByPopulationCurve = null;

		// Token: 0x04000626 RID: 1574
		public TaleDef tale = null;

		// Token: 0x04000627 RID: 1575
		[MustTranslate]
		public string letterText;

		// Token: 0x04000628 RID: 1576
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04000629 RID: 1577
		public LetterDef letterDef;

		// Token: 0x0400062A RID: 1578
		public GameConditionDef gameCondition;

		// Token: 0x0400062B RID: 1579
		public FloatRange durationDays;

		// Token: 0x0400062C RID: 1580
		public HediffDef diseaseIncident = null;

		// Token: 0x0400062D RID: 1581
		public FloatRange diseaseVictimFractionRange = new FloatRange(0f, 0.49f);

		// Token: 0x0400062E RID: 1582
		public int diseaseMaxVictims = 99999;

		// Token: 0x0400062F RID: 1583
		public List<BiomeDiseaseRecord> diseaseBiomeRecords = null;

		// Token: 0x04000630 RID: 1584
		public List<BodyPartDef> diseasePartsToAffect = null;

		// Token: 0x04000631 RID: 1585
		public ThingDef shipPart = null;

		// Token: 0x04000632 RID: 1586
		public List<MTBByBiome> mtbDaysByBiome;

		// Token: 0x04000633 RID: 1587
		[Unsaved]
		private IncidentWorker workerInt = null;

		// Token: 0x04000634 RID: 1588
		[Unsaved]
		private List<IncidentDef> cachedRefireCheckIncidents = null;

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00065F54 File Offset: 0x00064354
		public bool NeedsParmsPoints
		{
			get
			{
				return this.category.needsParmsPoints;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00065F74 File Offset: 0x00064374
		public IncidentWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (IncidentWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00065FC0 File Offset: 0x000643C0
		public List<IncidentDef> RefireCheckIncidents
		{
			get
			{
				List<IncidentDef> result;
				if (this.refireCheckTags == null)
				{
					result = null;
				}
				else
				{
					if (this.cachedRefireCheckIncidents == null)
					{
						this.cachedRefireCheckIncidents = new List<IncidentDef>();
						List<IncidentDef> allDefsListForReading = DefDatabase<IncidentDef>.AllDefsListForReading;
						for (int i = 0; i < allDefsListForReading.Count; i++)
						{
							if (this.ShouldDoRefireCheckWith(allDefsListForReading[i]))
							{
								this.cachedRefireCheckIncidents.Add(allDefsListForReading[i]);
							}
						}
					}
					result = this.cachedRefireCheckIncidents;
				}
				return result;
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00066048 File Offset: 0x00064448
		public static IncidentDef Named(string defName)
		{
			return DefDatabase<IncidentDef>.GetNamed(defName, true);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00066064 File Offset: 0x00064464
		private bool ShouldDoRefireCheckWith(IncidentDef other)
		{
			bool result;
			if (other.tags == null)
			{
				result = false;
			}
			else if (other == this)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < other.tags.Count; i++)
				{
					for (int j = 0; j < this.refireCheckTags.Count; j++)
					{
						if (other.tags[i] == this.refireCheckTags[j])
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00066100 File Offset: 0x00064500
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string c in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return c;
			}
			if (this.category == null)
			{
				yield return "category is undefined.";
			}
			if (this.targetTypes == null || this.targetTypes.Count == 0)
			{
				yield return "no target type";
			}
			if (this.TargetTypeAllowed(IncidentTargetTypeDefOf.World))
			{
				if (this.targetTypes.Any((IncidentTargetTypeDef tt) => tt != IncidentTargetTypeDefOf.World))
				{
					yield return "allows world target type along with other targets. World targeting incidents should only target the world.";
				}
			}
			if (this.TargetTypeAllowed(IncidentTargetTypeDefOf.World) && this.allowedBiomes != null)
			{
				yield return "world-targeting incident has a biome restriction list";
			}
			yield break;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0006612C File Offset: 0x0006452C
		public bool TargetTypeAllowed(IncidentTargetTypeDef target)
		{
			return this.targetTypes.Contains(target);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00066150 File Offset: 0x00064550
		public bool TargetAllowed(IIncidentTarget target)
		{
			return this.targetTypes.Intersect(target.AcceptedTypes()).Any<IncidentTargetTypeDef>();
		}
	}
}
