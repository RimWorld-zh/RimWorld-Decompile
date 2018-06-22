using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000725 RID: 1829
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x00158A38 File Offset: 0x00156E38
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x00158A58 File Offset: 0x00156E58
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x00158A78 File Offset: 0x00156E78
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
