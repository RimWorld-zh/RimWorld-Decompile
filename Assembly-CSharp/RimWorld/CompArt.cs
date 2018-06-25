using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000702 RID: 1794
	public class CompArt : ThingComp
	{
		// Token: 0x040015BC RID: 5564
		private string authorNameInt = null;

		// Token: 0x040015BD RID: 5565
		private string titleInt = null;

		// Token: 0x040015BE RID: 5566
		private TaleReference taleRef = null;

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x00151FE8 File Offset: 0x001503E8
		public string AuthorName
		{
			get
			{
				string result;
				if (this.authorNameInt.NullOrEmpty())
				{
					result = "UnknownLower".Translate().CapitalizeFirst();
				}
				else
				{
					result = this.authorNameInt;
				}
				return result;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00152028 File Offset: 0x00150428
		public string Title
		{
			get
			{
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.", false);
					this.titleInt = "Error";
				}
				return this.titleInt;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x0015206C File Offset: 0x0015046C
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600273A RID: 10042 RVA: 0x00152088 File Offset: 0x00150488
		public bool CanShowArt
		{
			get
			{
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = this.parent as Building_Grave;
					if (building_Grave == null || !building_Grave.HasCorpse)
					{
						return false;
					}
				}
				QualityCategory qualityCategory;
				return !this.parent.TryGetQuality(out qualityCategory) || qualityCategory >= this.Props.minQualityForArtistic;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x00152100 File Offset: 0x00150500
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x00152124 File Offset: 0x00150524
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x00152144 File Offset: 0x00150544
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x0015214F File Offset: 0x0015054F
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x0015215C File Offset: 0x0015055C
		private void InitializeArt(Thing relatedThing, ArtGenerationContext source)
		{
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
			if (this.CanShowArt)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (relatedThing != null)
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArtConcerning(relatedThing);
					}
					else
					{
						this.taleRef = Find.TaleManager.GetRandomTaleReferenceForArt(source);
					}
				}
				else
				{
					this.taleRef = TaleReference.Taleless;
				}
				this.titleInt = this.GenerateTitle();
			}
			else
			{
				this.titleInt = null;
				this.taleRef = null;
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x00152201 File Offset: 0x00150601
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.Name.ToStringFull;
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x00152220 File Offset: 0x00150620
		public void Clear()
		{
			this.authorNameInt = null;
			this.titleInt = null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00152250 File Offset: 0x00150650
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<string>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", new object[0]);
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x001522A0 File Offset: 0x001506A0
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				string text = "Author".Translate() + ": " + this.AuthorName;
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n",
					"Title".Translate(),
					": ",
					this.Title
				});
				result = text;
			}
			return result;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0015231B File Offset: 0x0015071B
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00152348 File Offset: 0x00150748
		public override string GetDescriptionPart()
		{
			string result;
			if (!this.Active)
			{
				result = null;
			}
			else
			{
				string text = "";
				text += this.Title;
				text += "\n\n";
				text += this.GenerateImageDescription();
				text += "\n\n";
				text = text + "Author".Translate() + ": " + this.AuthorName;
				result = text;
			}
			return result;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x001523C4 File Offset: 0x001507C4
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x001523EC File Offset: 0x001507EC
		public string GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00152444 File Offset: 0x00150844
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}
	}
}
