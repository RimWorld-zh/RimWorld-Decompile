using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_PrisonerBed : SymbolResolver
	{
		public SymbolResolver_PrisonerBed()
		{
		}

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

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey0
		{
			internal Action<Thing> prevPostThingSpawn;

			public <Resolve>c__AnonStorey0()
			{
			}

			internal void <>m__0(Thing x)
			{
				if (this.prevPostThingSpawn != null)
				{
					this.prevPostThingSpawn(x);
				}
				Building_Bed building_Bed = x as Building_Bed;
				if (building_Bed != null)
				{
					building_Bed.ForPrisoners = true;
				}
			}
		}
	}
}
