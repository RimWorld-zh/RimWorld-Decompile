using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000727 RID: 1831
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x00158B88 File Offset: 0x00156F88
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00158BA8 File Offset: 0x00156FA8
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00158BC8 File Offset: 0x00156FC8
		public override void CompTickRare()
		{
			Thing thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
