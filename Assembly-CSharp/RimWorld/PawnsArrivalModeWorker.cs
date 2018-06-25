using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BC RID: 700
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x040006CE RID: 1742
		public PawnsArrivalModeDef def;

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00069348 File Offset: 0x00067748
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000693F0 File Offset: 0x000677F0
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			return this.def.selectionWeightCurve.Evaluate(parms.points);
		}

		// Token: 0x06000BB9 RID: 3001
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x06000BBA RID: 3002 RVA: 0x0006941B File Offset: 0x0006781B
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06000BBB RID: 3003
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);
	}
}
