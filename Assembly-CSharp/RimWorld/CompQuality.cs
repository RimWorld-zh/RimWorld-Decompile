using Verse;

namespace RimWorld
{
	public class CompQuality : ThingComp
	{
		private QualityCategory qualityInt = QualityCategory.Normal;

		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		public void SetQuality(QualityCategory q, ArtGenerationContext source)
		{
			this.qualityInt = q;
			CompArt compArt = base.parent.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.InitializeArt(source);
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.RandomTraderItemQuality(), ArtGenerationContext.Outsider);
		}

		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory = default(QualityCategory);
			if (other.TryGetQuality(out qualityCategory))
			{
				return this.qualityInt == qualityCategory;
			}
			return false;
		}

		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}
	}
}
