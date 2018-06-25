using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000349 RID: 841
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x0007973C File Offset: 0x00077B3C
		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00079788 File Offset: 0x00077B88
		protected virtual bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			bool result;
			if (f.IsPlayer)
			{
				result = false;
			}
			else if (f.defeated)
			{
				result = false;
			}
			else
			{
				if (!desperate)
				{
					if (!f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) || !f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00079814 File Offset: 0x00077C14
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.CandidateFactions(map, false).Any<Faction>();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00079844 File Offset: 0x00077C44
		public string DebugListingOfGroupSources()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				stringBuilder.Append(faction.Name);
				if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, false))
				{
					stringBuilder.Append("    YES");
				}
				else if (this.FactionCanBeGroupSource(faction, Find.CurrentMap, true))
				{
					stringBuilder.Append("    YES-DESPERATE");
				}
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
