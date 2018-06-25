using System;
using Verse;

namespace RimWorld
{
	public class SiteWorkerBase
	{
		public SiteDefBase def;

		public SiteWorkerBase()
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
	}
}
