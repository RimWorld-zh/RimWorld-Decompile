using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000244 RID: 580
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x0400045F RID: 1119
		public ChemicalDef chemical = null;

		// Token: 0x04000460 RID: 1120
		public float addictiveness = 0f;

		// Token: 0x04000461 RID: 1121
		public float minToleranceToAddict = 0f;

		// Token: 0x04000462 RID: 1122
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x04000463 RID: 1123
		public float needLevelOffset = 1f;

		// Token: 0x04000464 RID: 1124
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x04000465 RID: 1125
		public float largeOverdoseChance = 0f;

		// Token: 0x04000466 RID: 1126
		public bool isCombatEnhancingDrug = false;

		// Token: 0x04000467 RID: 1127
		public float listOrder = 0f;

		// Token: 0x06000A6F RID: 2671 RVA: 0x0005E970 File Offset: 0x0005CD70
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0005E9F0 File Offset: 0x0005CDF0
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0005EA14 File Offset: 0x0005CE14
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0005EA3C File Offset: 0x0005CE3C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0(parentDef))
			{
				yield return e;
			}
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
			yield break;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0005EA70 File Offset: 0x0005CE70
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry s in this.<SpecialDisplayStats>__BaseCallProxy1())
			{
				yield return s;
			}
			if (this.Addictive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Addictiveness".Translate(), this.addictiveness.ToStringPercent(), 0, "");
			}
			yield break;
		}
	}
}
