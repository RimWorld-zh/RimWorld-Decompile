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
		// Token: 0x06000E6A RID: 3690 RVA: 0x00079744 File Offset: 0x00077B44
		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00079790 File Offset: 0x00077B90
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

		// Token: 0x06000E6C RID: 3692 RVA: 0x0007981C File Offset: 0x00077C1C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.CandidateFactions(map, false).Any<Faction>();
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0007984C File Offset: 0x00077C4C
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
