using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BA RID: 698
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x040006CE RID: 1742
		public PawnsArrivalModeDef def;

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000691F8 File Offset: 0x000675F8
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000692A0 File Offset: 0x000676A0
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			return this.def.selectionWeightCurve.Evaluate(parms.points);
		}

		// Token: 0x06000BB5 RID: 2997
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000692CB File Offset: 0x000676CB
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06000BB7 RID: 2999
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);
	}
}
