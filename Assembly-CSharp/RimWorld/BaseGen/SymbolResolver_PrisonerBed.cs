using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B7 RID: 951
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x0600107D RID: 4221 RVA: 0x0008B714 File Offset: 0x00089B14
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			Action<Thing> prevPostThingSpawn = resolveParams.postThingSpawn;
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				if (prevPostThingSpawn != null)
				{
					prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			};
			BaseGen.symbolStack.Push("bed", resolveParams);
		}
	}
}
