using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D8 RID: 728
	public class StatDef : Def
	{
		// Token: 0x0400073A RID: 1850
		public StatCategoryDef category = null;

		// Token: 0x0400073B RID: 1851
		public Type workerClass = typeof(StatWorker);

		// Token: 0x0400073C RID: 1852
		public float hideAtValue = -2.14748365E+09f;

		// Token: 0x0400073D RID: 1853
		public bool alwaysHide;

		// Token: 0x0400073E RID: 1854
		public bool showNonAbstract = true;

		// Token: 0x0400073F RID: 1855
		public bool showIfUndefined = true;

		// Token: 0x04000740 RID: 1856
		public bool showOnPawns = true;

		// Token: 0x04000741 RID: 1857
		public bool showOnHumanlikes = true;

		// Token: 0x04000742 RID: 1858
		public bool showOnNonWildManHumanlikes = true;

		// Token: 0x04000743 RID: 1859
		public bool showOnAnimals = true;

		// Token: 0x04000744 RID: 1860
		public bool showOnMechanoids = true;

		// Token: 0x04000745 RID: 1861
		public bool showOnNonWorkTables = true;

		// Token: 0x04000746 RID: 1862
		public bool neverDisabled = false;

		// Token: 0x04000747 RID: 1863
		public int displayPriorityInCategory = 0;

		// Token: 0x04000748 RID: 1864
		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		// Token: 0x04000749 RID: 1865
		public ToStringStyle toStringStyle = ToStringStyle.Integer;

		// Token: 0x0400074A RID: 1866
		public ToStringStyle? toStringStyleUnfinalized;

		// Token: 0x0400074B RID: 1867
		[MustTranslate]
		public string formatString = null;

		// Token: 0x0400074C RID: 1868
		public float defaultBaseValue = 1f;

		// Token: 0x0400074D RID: 1869
		public List<SkillNeed> skillNeedOffsets = null;

		// Token: 0x0400074E RID: 1870
		public float noSkillOffset = 0f;

		// Token: 0x0400074F RID: 1871
		public List<PawnCapacityOffset> capacityOffsets = null;

		// Token: 0x04000750 RID: 1872
		public List<StatDef> statFactors = null;

		// Token: 0x04000751 RID: 1873
		public bool applyFactorsIfNegative = true;

		// Token: 0x04000752 RID: 1874
		public List<SkillNeed> skillNeedFactors = null;

		// Token: 0x04000753 RID: 1875
		public float noSkillFactor = 1f;

		// Token: 0x04000754 RID: 1876
		public List<PawnCapacityFactor> capacityFactors = null;

		// Token: 0x04000755 RID: 1877
		public SimpleCurve postProcessCurve = null;

		// Token: 0x04000756 RID: 1878
		public float minValue = -9999999f;

		// Token: 0x04000757 RID: 1879
		public float maxValue = 9999999f;

		// Token: 0x04000758 RID: 1880
		public bool roundValue = false;

		// Token: 0x04000759 RID: 1881
		public float roundToFiveOver = float.MaxValue;

		// Token: 0x0400075A RID: 1882
		public bool minifiedThingInherits;

		// Token: 0x0400075B RID: 1883
		public bool scenarioRandomizable = false;

		// Token: 0x0400075C RID: 1884
		public List<StatPart> parts = null;

		// Token: 0x0400075D RID: 1885
		[Unsaved]
		private StatWorker workerInt = null;

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x0006A92C File Offset: 0x00068D2C
		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x0006A9B4 File Offset: 0x00068DB4
		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				ToStringStyle? toStringStyle = this.toStringStyleUnfinalized;
				return (toStringStyle == null) ? this.toStringStyle : this.toStringStyleUnfinalized.Value;
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0006A9F4 File Offset: 0x00068DF4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.capacityFactors != null)
			{
				foreach (PawnCapacityFactor afac in this.capacityFactors)
				{
					if (afac.weight > 1f)
					{
						yield return this.defName + " has activity factor with weight > 1";
					}
				}
			}
			if (this.parts != null)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					foreach (string err2 in this.parts[i].ConfigErrors())
					{
						yield return string.Concat(new string[]
						{
							this.defName,
							" has error in StatPart ",
							this.parts[i].ToString(),
							": ",
							err2
						});
					}
				}
			}
			yield break;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0006AA20 File Offset: 0x00068E20
		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			return this.Worker.ValueToString(val, true, numberSense);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0006AA44 File Offset: 0x00068E44
		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0006AA60 File Offset: 0x00068E60
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.parts != null)
			{
				List<StatPart> partsCopy = this.parts.ToList<StatPart>();
				this.parts.SortBy((StatPart x) => -x.priority, (StatPart x) => partsCopy.IndexOf(x));
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0006AACC File Offset: 0x00068ECC
		public T GetStatPart<T>() where T : StatPart
		{
			return this.parts.OfType<T>().FirstOrDefault<T>();
		}
	}
}
