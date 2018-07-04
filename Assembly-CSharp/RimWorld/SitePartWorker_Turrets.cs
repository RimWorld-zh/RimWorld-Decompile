using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class SitePartWorker_Turrets : SitePartWorker
	{
		private const float TurretPoints = 250f;

		private const int MinTurrets = 2;

		private const int MaxTurrets = 11;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		public SitePartWorker_Turrets()
		{
		}

		public override string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLabel, out preferredLetterDef, out lookTargets);
			Thing t;
			if ((t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun && x.HostileTo(Faction.OfPlayer))) == null)
			{
				t = map.listerThings.AllThings.FirstOrDefault((Thing x) => x is Building_TurretGun);
			}
			lookTargets = t;
			return arrivedLetterPart;
		}

		public override SiteCoreOrPartParams GenerateDefaultParams(Site site)
		{
			SiteCoreOrPartParams siteCoreOrPartParams = base.GenerateDefaultParams(site);
			siteCoreOrPartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			siteCoreOrPartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(siteCoreOrPartParams.threatPoints / 250f), 2, 11);
			return siteCoreOrPartParams;
		}

		public override string GetPostProcessedDescriptionDialogue(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return string.Format(base.GetPostProcessedDescriptionDialogue(site, siteCoreOrPart), this.GetTurretsCount(siteCoreOrPart.parms));
		}

		public override string GetPostProcessedThreatLabel(Site site, SiteCoreOrPartBase siteCoreOrPart)
		{
			return string.Concat(new object[]
			{
				base.GetPostProcessedThreatLabel(site, siteCoreOrPart),
				" (",
				this.GetTurretsCount(siteCoreOrPart.parms),
				")"
			});
		}

		private int GetTurretsCount(SiteCoreOrPartParams parms)
		{
			return parms.turretsCount + parms.mortarsCount;
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__0(Thing x)
		{
			return x is Building_TurretGun && x.HostileTo(Faction.OfPlayer);
		}

		[CompilerGenerated]
		private static bool <GetArrivedLetterPart>m__1(Thing x)
		{
			return x is Building_TurretGun;
		}
	}
}
