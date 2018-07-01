using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public abstract class SiteCoreOrPartWorkerBase
	{
		public SiteCoreOrPartDefBase def;

		private static readonly SimpleCurve CurrentThreatPointsToSiteThreatPoints = new SimpleCurve
		{
			{
				new CurvePoint(0f, 200f),
				true
			},
			{
				new CurvePoint(1000f, 800f),
				true
			},
			{
				new CurvePoint(4000f, 3000f),
				true
			}
		};

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

		public virtual SiteCoreOrPartParams GenerateDefaultParams()
		{
			return new SiteCoreOrPartParams
			{
				randomValue = Rand.Int,
				threatPoints = SiteCoreOrPartWorkerBase.CurrentThreatPointsToSiteThreatPoints.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.World))
			};
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SiteCoreOrPartWorkerBase()
		{
		}
	}
}
