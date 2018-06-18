using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000729 RID: 1833
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x0015887C File Offset: 0x00156C7C
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x0015889C File Offset: 0x00156C9C
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x001588BC File Offset: 0x00156CBC
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
