using Verse;

namespace RimWorld
{
	public class CompArt : ThingComp
	{
		private string authorNameInt = (string)null;

		private string titleInt = (string)null;

		private TaleReference taleRef = null;

		public string AuthorName
		{
			get
			{
				return (!this.authorNameInt.NullOrEmpty()) ? this.authorNameInt : "UnknownLower".Translate();
			}
		}

		public string Title
		{
			get
			{
				if (this.titleInt.NullOrEmpty())
				{
					Log.Error("CompArt got title but it wasn't configured.");
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
				bool result;
				if (this.Props.mustBeFullGrave)
				{
					Building_Grave building_Grave = base.parent as Building_Grave;
					if (building_Grave != null && building_Grave.HasCorpse)
					{
						goto IL_0037;
					}
					result = false;
					goto IL_0067;
				}
				goto IL_0037;
				IL_0037:
				QualityCategory qualityCategory = default(QualityCategory);
				result = (!((Thing)base.parent).TryGetQuality(out qualityCategory) || (int)qualityCategory >= (int)this.Props.minQualityForArtistic);
				goto IL_0067;
				IL_0067:
				return result;
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
				return (CompProperties_Art)base.props;
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
				this.titleInt = (string)null;
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
			this.authorNameInt = (string)null;
			this.titleInt = (string)null;
			if (this.taleRef != null)
			{
				this.taleRef.ReferenceDestroyed();
				this.taleRef = null;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.authorNameInt, "authorName", (string)null, false);
			Scribe_Values.Look<string>(ref this.titleInt, "title", (string)null, false);
			Scribe_Deep.Look<TaleReference>(ref this.taleRef, "taleRef", new object[0]);
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.Active)
			{
				result = (string)null;
			}
			else
			{
				string text;
				string text2 = text = "Author".Translate() + ": " + this.AuthorName;
				text2 = (result = text + "\n" + "Title".Translate() + ": " + this.Title);
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
				result = (string)null;
			}
			else
			{
				string str = "";
				str += this.Title;
				str += "\n\n";
				str += this.GenerateImageDescription();
				str += "\n\n";
				str = (result = str + "Author".Translate() + ": " + this.AuthorName);
			}
			return result;
		}

		public override bool AllowStackWith(Thing other)
		{
			return (byte)((!this.Active) ? 1 : 0) != 0;
		}

		public string GenerateImageDescription()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateImageDescription without initializing art: " + base.parent);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return this.taleRef.GenerateText(TextGenerationPurpose.ArtDescription, this.Props.descriptionMaker.RulesPlusIncludes);
		}

		private string GenerateTitle()
		{
			if (this.taleRef == null)
			{
				Log.Error("Did CompArt.GenerateTitle without initializing art: " + base.parent);
				this.InitializeArt(ArtGenerationContext.Outsider);
			}
			return GenText.CapitalizeAsTitle(this.taleRef.GenerateText(TextGenerationPurpose.ArtName, this.Props.nameMaker.RulesPlusIncludes));
		}
	}
}
