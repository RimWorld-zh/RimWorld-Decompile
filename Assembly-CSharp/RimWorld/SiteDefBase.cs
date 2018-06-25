using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class SiteDefBase : Def
	{
		public Type workerClass = typeof(SitePartWorker);

		[NoTranslate]
		public string siteTexture;

		[NoTranslate]
		public string expandingIconTexture;

		public bool applyFactionColorToSiteTexture;

		public bool showFactionInInspectString;

		public bool requiresFaction;

		public TechLevel minFactionTechLevel = TechLevel.Undefined;

		[MustTranslate]
		public string approachOrderString;

		[MustTranslate]
		public string approachingReportString;

		[NoTranslate]
		public List<string> tags = new List<string>();

		[MustTranslate]
		public string arrivedLetter;

		[MustTranslate]
		public string arrivedLetterLabel;

		public LetterDef arrivedLetterDef;

		[Unsaved]
		private SiteWorkerBase workerInt;

		[Unsaved]
		private Texture2D expandingIconTextureInt;

		[Unsaved]
		private List<GenStepDef> extraGenSteps;

		protected SiteDefBase()
		{
		}

		public SiteWorkerBase Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = this.CreateWorker();
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		public List<GenStepDef> ExtraGenSteps
		{
			get
			{
				if (this.extraGenSteps == null)
				{
					this.extraGenSteps = new List<GenStepDef>();
					List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].linkWithSite == this)
						{
							this.extraGenSteps.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.extraGenSteps;
			}
		}

		public virtual bool FactionCanOwn(Faction faction)
		{
			bool result;
			if (this.requiresFaction && faction == null)
			{
				result = false;
			}
			else if (this.minFactionTechLevel != TechLevel.Undefined && (faction == null || faction.def.techLevel < this.minFactionTechLevel))
			{
				result = false;
			}
			else
			{
				if (faction != null)
				{
					if (faction.IsPlayer || faction.defeated || faction.def.hidden)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		protected abstract SiteWorkerBase CreateWorker();
	}
}
