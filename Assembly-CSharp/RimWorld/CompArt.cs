using System;
using Verse;

namespace RimWorld
{
	public class CompArt : ThingComp
	{
		private string authorNameInt = null;

		private string titleInt = null;

		private TaleReference taleRef = null;

		public CompArt()
		{
		}

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

		public TaleReference TaleRef
		{
			get
			{
				return this.taleRef;
			}
		}

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

		public bool Active
		{
			get
			{
				return this.taleRef != null;
			}
		}

		public CompProperties_Art Props
		{
			get
			{
				return (CompProperties_Art)this.props;
			}
		}

		public void InitializeArt(ArtGenerationContext source)
		{
			this.InitializeArt(null, source);
		}

		public void InitializeArt(Thing relatedThing)
		{
			this.InitializeArt(relatedThing, ArtGenerationContext.Colony);
		}

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

		public void JustCreatedBy(Pawn pawn)
		{
			if (this.CanShowArt)
			{
				this.authorNameInt = pawn.Name.ToStringFull;
			}
		}

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

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.authorNameInt, "authorName", null, false);
			Scribe_Values.Look<string>(ref this.titleInt, "title", null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", new object[0]);
		}

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

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

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

		public override bool AllowStackWith(Thing other)
		{
			return !this.Active;
		}

		public string GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + this.parent, false);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker);
		}

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
