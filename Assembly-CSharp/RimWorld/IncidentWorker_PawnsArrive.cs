using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000347 RID: 839
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		// Token: 0x06000E67 RID: 3687 RVA: 0x000794DC File Offset: 0x000778DC
		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00079528 File Offset: 0x00077928
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

		// Token: 0x06000E69 RID: 3689 RVA: 0x000795B4 File Offset: 0x000779B4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.CandidateFactions(map, false).Any<Faction>();
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000795E4 File Offset: 0x000779E4
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
