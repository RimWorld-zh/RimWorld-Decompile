using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000700 RID: 1792
	public class CompArt : ThingComp
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002734 RID: 10036 RVA: 0x00151C38 File Offset: 0x00150038
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
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x00151C78 File Offset: 0x00150078
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
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x00151CBC File Offset: 0x001500BC
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x00151CD8 File Offset: 0x001500D8
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
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00151D50 File Offset: 0x00150150
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002739 RID: 10041 RVA: 0x00151D74 File Offset: 0x00150174
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x00151D94 File Offset: 0x00150194
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x00151D9F File Offset: 0x0015019F
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x00151DAC File Offset: 0x001501AC
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

		// Token: 0x0600273D RID: 10045 RVA: 0x00151E51 File Offset: 0x00150251
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.Name.ToStringFull;
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x00151E70 File Offset: 0x00150270
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

		// Token: 0x0600273F RID: 10047 RVA: 0x00151EA0 File Offset: 0x001502A0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<string>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", new object[0]);
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x00151EF0 File Offset: 0x001502F0
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

		// Token: 0x06002741 RID: 10049 RVA: 0x00151F6B File Offset: 0x0015036B
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00151F98 File Offset: 0x00150398
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

		// Token: 0x06002743 RID: 10051 RVA: 0x00152014 File Offset: 0x00150414
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x0015203C File Offset: 0x0015043C
		public string GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00152094 File Offset: 0x00150494
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		// Token: 0x040015B8 RID: 5560
		private string authorNameInt = null;

		// Token: 0x040015B9 RID: 5561
		private string titleInt = null;

		// Token: 0x040015BA RID: 5562
		private TaleReference taleRef = null;
	}
}
