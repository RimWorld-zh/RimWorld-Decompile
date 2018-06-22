using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D0 RID: 720
	public abstract class SiteDefBase : Def
	{
		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00069C24 File Offset: 0x00068024
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00069C64 File Offset: 0x00068064
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00069CC4 File Offset: 0x000680C4
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

		// Token: 0x06000BEC RID: 3052 RVA: 0x00069D3C File Offset: 0x0006813C
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

		// Token: 0x06000BED RID: 3053
		protected abstract SiteWorkerBase CreateWorker();

		// Token: 0x04000722 RID: 1826
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x04000723 RID: 1827
		[NoTranslate]
		public string siteTexture;

		// Token: 0x04000724 RID: 1828
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04000725 RID: 1829
		public bool applyFactionColorToSiteTexture;

		// Token: 0x04000726 RID: 1830
		public bool showFactionInInspectString;

		// Token: 0x04000727 RID: 1831
		public bool requiresFaction;

		// Token: 0x04000728 RID: 1832
		public TechLevel minFactionTechLevel = TechLevel.Undefined;

		// Token: 0x04000729 RID: 1833
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x0400072A RID: 1834
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x0400072B RID: 1835
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x0400072C RID: 1836
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x0400072D RID: 1837
		[MustTranslate]
		public string arrivedLetterLabel;

		// Token: 0x0400072E RID: 1838
		public LetterDef arrivedLetterDef;

		// Token: 0x0400072F RID: 1839
		[Unsaved]
		private SiteWorkerBase workerInt;

		// Token: 0x04000730 RID: 1840
		[Unsaved]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04000731 RID: 1841
		[Unsaved]
		private List<GenStepDef> extraGenSteps;
	}
}
