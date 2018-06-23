using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000746 RID: 1862
	public class CompQuality : ThingComp
	{
		// Token: 0x04001680 RID: 5760
		private QualityCategory qualityInt = QualityCategory.Normal;

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002944 RID: 10564 RVA: 0x0015F3B0 File Offset: 0x0015D7B0
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x0015F3CC File Offset: 0x0015D7CC
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x0015F3FA File Offset: 0x0015D7FA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0015F415 File Offset: 0x0015D815
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x0015F424 File Offset: 0x0015D824
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x0015F456 File Offset: 0x0015D856
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0015F474 File Offset: 0x0015D874
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(new object[]
			{
				this.Quality.GetLabel().CapitalizeFirst()
			});
		}
	}
}
