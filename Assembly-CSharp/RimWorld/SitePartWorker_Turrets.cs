using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200060E RID: 1550
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x06001F33 RID: 7987 RVA: 0x0010F11C File Offset: 0x0010D51C
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
