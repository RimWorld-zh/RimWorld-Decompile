using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000704 RID: 1796
	public class CompArt : ThingComp
	{
		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600273C RID: 10044 RVA: 0x00151A94 File Offset: 0x0014FE94
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
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x00151AD4 File Offset: 0x0014FED4
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
		// (get) Token: 0x0600273E RID: 10046 RVA: 0x00151B18 File Offset: 0x0014FF18
		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x00151B34 File Offset: 0x0014FF34
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
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x00151BAC File Offset: 0x0014FFAC
		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x00151BD0 File Offset: 0x0014FFD0
		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00151BF0 File Offset: 0x0014FFF0
		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x00151BFB File Offset: 0x0014FFFB
		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x00151C08 File Offset: 0x00150008
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

		// Token: 0x06002745 RID: 10053 RVA: 0x00151CAD File Offset: 0x001500AD
		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.Name.ToStringFull;
			}
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x00151CCC File Offset: 0x001500CC
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

		// Token: 0x06002747 RID: 10055 RVA: 0x00151CFC File Offset: 0x001500FC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<string>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", new object[0]);
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00151D4C File Offset: 0x0015014C
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

		// Token: 0x06002749 RID: 10057 RVA: 0x00151DC7 File Offset: 0x001501C7
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00151DF4 File Offset: 0x001501F4
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

		// Token: 0x0600274B RID: 10059 RVA: 0x00151E70 File Offset: 0x00150270
		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x00151E98 File Offset: 0x00150298
		public string GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x00151EF0 File Offset: 0x001502F0
		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker));
		}

		// Token: 0x040015BA RID: 5562
		private string authorNameInt = null;

		// Token: 0x040015BB RID: 5563
		private string titleInt = null;

		// Token: 0x040015BC RID: 5564
		private TaleReference taleRef = null;
	}
}
