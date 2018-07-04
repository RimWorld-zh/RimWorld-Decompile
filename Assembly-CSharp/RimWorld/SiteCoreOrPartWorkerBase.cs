using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public abstract class SiteCoreOrPartWorkerBase
	{
		public SiteCoreOrPartDefBase def;

		protected SiteCoreOrPartWorkerBase()
		{
		}

		public virtual void PostMapGenerate(Map map)
		{
		}

		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		public virtual string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLabel = this.def.arrivedLetterLabel;
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}

		public virtual string GetPostProcessedDescriptionDialogue(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return this.def.descriptionDialogue;
		}

		public virtual string GetPostProcessedThreatLabel(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return this.def.label;
		}

		public virtual SiteCoreOrPartParams GenerateDefaultParams(Site site)
		{
			return new SiteCoreOrPartParams
			{
				randomValue = Rand.Int,
				threatPoints = StorytellerUtility.ThreatPointsToSiteThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.World))
			};
		}
	}
}
