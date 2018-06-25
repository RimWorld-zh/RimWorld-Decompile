using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000748 RID: 1864
	public class CompQuality : ThingComp
	{
		// Token: 0x04001684 RID: 5764
		private QualityCategory qualityInt = QualityCategory.Normal;

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002947 RID: 10567 RVA: 0x0015F760 File Offset: 0x0015DB60
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x0015F77C File Offset: 0x0015DB7C
		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x0015F7AA File Offset: 0x0015DBAA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0015F7C5 File Offset: 0x0015DBC5
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), ArtGenerationContext.Outsider);
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x0015F7D4 File Offset: 0x0015DBD4
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x0015F806 File Offset: 0x0015DC06
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0015F824 File Offset: 0x0015DC24
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(new object[]
			{
				this.Quality.GetLabel().CapitalizeFirst()
			});
		}
	}
}
