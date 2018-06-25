using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002BC RID: 700
	public abstract class PawnsArrivalModeWorker
	{
		// Token: 0x040006D0 RID: 1744
		public PawnsArrivalModeDef def;

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00069344 File Offset: 0x00067744
		public virtual bool CanUseWith(IncidentParms parms)
		{
			return (parms.faction == null || this.def.minTechLevel == TechLevel.Undefined || parms.faction.def.techLevel >= this.def.minTechLevel) && (!parms.raidArrivalModeForQuickMilitaryAid || this.def.forQuickMilitaryAid) && (parms.raidStrategy == null || parms.raidStrategy.arriveModes.Contains(this.def));
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000693EC File Offset: 0x000677EC
		public virtual float GetSelectionWeight(IncidentParms parms)
		{
			return this.def.selectionWeightCurve.Evaluate(parms.points);
		}

		// Token: 0x06000BB8 RID: 3000
		public abstract void Arrive(List<Pawn> pawns, IncidentParms parms);

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00069417 File Offset: 0x00067817
		public virtual void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			throw new NotSupportedException("Traveling transport pods arrived with mode " + this.def.defName);
		}

		// Token: 0x06000BBA RID: 3002
		public abstract bool TryResolveRaidSpawnCenter(IncidentParms parms);
	}
}
