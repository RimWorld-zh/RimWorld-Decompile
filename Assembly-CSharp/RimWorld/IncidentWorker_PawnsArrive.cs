using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public abstract class IncidentWorker_PawnsArrive : IncidentWorker
	{
		protected IncidentWorker_PawnsArrive()
		{
		}

		protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
		{
			return from f in Find.FactionManager.AllFactions
			where this.FactionCanBeGroupSource(f, map, desperate)
			select f;
		}

		protected virtual bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return !f.IsPlayer && !f.defeated && (desperate || (f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.OutdoorTemp) && f.def.allowedArrivalTemperatureRange.Includes(map.mapTemperature.SeasonalTemp)));
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return parms.faction != null || this.CandidateFactions(map, false).Any<Faction>();
		}

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

		[CompilerGenerated]
		private sealed class <CandidateFactions>c__AnonStorey0
		{
			internal Map map;

			internal bool desperate;

			internal IncidentWorker_PawnsArrive $this;

			public <CandidateFactions>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction f)
			{
				return this.$this.FactionCanBeGroupSource(f, this.map, this.desperate);
			}
		}
	}
}
