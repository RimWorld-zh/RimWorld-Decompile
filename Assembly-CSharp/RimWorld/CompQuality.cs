using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074A RID: 1866
	public class CompQuality : ThingComp
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x0015F144 File Offset: 0x0015D544
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0015F160 File Offset: 0x0015D560
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x0015F18E File Offset: 0x0015D58E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x0015F1A9 File Offset: 0x0015D5A9
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0015F1B8 File Offset: 0x0015D5B8
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0015F1EA File Offset: 0x0015D5EA
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0015F208 File Offset: 0x0015D608
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(new object[]
			{
				this.Quality.GetLabel().CapitalizeFirst()
			});
		}

		// Token: 0x04001682 RID: 5762
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
