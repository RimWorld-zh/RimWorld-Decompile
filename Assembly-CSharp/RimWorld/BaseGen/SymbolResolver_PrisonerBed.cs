using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003B5 RID: 949
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		// Token: 0x06001079 RID: 4217 RVA: 0x0008B3D8 File Offset: 0x000897D8
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
