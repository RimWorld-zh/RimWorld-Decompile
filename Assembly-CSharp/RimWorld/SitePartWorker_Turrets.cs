using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000610 RID: 1552
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x06001F39 RID: 7993 RVA: 0x0010ED10 File Offset: 0x0010D110
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
	}
}
