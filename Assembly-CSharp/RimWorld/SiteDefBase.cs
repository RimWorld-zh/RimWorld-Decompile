using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class SiteDefBase : Def
	{
		public string siteTexture;

		public string expandingIconTexture;

		public bool applyFactionColorToSiteTexture;

		public bool showFactionInInspectString;

		public bool knownDanger;

		public bool requiresFaction;

		public TechLevel minFactionTechLevel = TechLevel.Undefined;

		[NoTranslate]
		public List<string> tags = new List<string>();

		[Unsaved]
		private Texture2D expandingIconTextureInt;

		[Unsaved]
		private List<GenStepDef> extraGenSteps;

		public Texture2D ExpandingIconTexture
		{
			get
			{
				if ((Object)this.expandingIconTextureInt == (Object)null)
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
			return (byte)((!this.requiresFaction || faction != null) ? ((this.minFactionTechLevel == TechLevel.Undefined || (faction != null && (int)faction.def.techLevel >= (int)this.minFactionTechLevel)) ? ((faction == null || (!faction.IsPlayer && !faction.defeated && !faction.def.hidden)) ? 1 : 0) : 0) : 0) != 0;
		}
	}
}
