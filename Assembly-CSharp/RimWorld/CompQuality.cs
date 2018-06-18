using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200074A RID: 1866
	public class CompQuality : ThingComp
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x0015F1D8 File Offset: 0x0015D5D8
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x0015F1F4 File Offset: 0x0015D5F4
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0015F222 File Offset: 0x0015D622
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0015F23D File Offset: 0x0015D63D
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0015F24C File Offset: 0x0015D64C
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0015F27E File Offset: 0x0015D67E
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0015F29C File Offset: 0x0015D69C
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
