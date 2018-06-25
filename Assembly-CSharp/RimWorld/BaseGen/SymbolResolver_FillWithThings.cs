using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FillWithThings : SymbolResolver
	{
		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		public SymbolResolver_FillWithThings()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
			}
			else if (rp.singleThingToSpawn != null)
			{
				result = false;
			}
			else
			{
				if (rp.singleThingDef != null)
				{
					Rot4? thingRot = rp.thingRot;
					Rot4 rot = (thingRot == null) ? Rot4.North : thingRot.Value;
					IntVec3 zero = IntVec3.Zero;
					IntVec2 size = rp.singleThingDef.size;
					GenAdj.AdjustForRotation(ref zero, ref size, rot);
					if (rp.rect.Width < size.x || rp.rect.Height < size.z)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef;
			if ((thingDef = rp.singleThingDef) == null)
			{
				thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
				where x.IsWeapon || x.IsMedicine || x.IsDrug
				select x).RandomElement<ThingDef>();
			}
			ThingDef thingDef2 = thingDef;
			Rot4? thingRot = rp.thingRot;
			Rot4 rot = (thingRot == null) ? Rot4.North : thingRot.Value;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef2.size;
			int? fillWithThingsPadding = rp.fillWithThingsPadding;
			int num = (fillWithThingsPadding == null) ? 0 : fillWithThingsPadding.Value;
			if (num < 0)
			{
				num = 0;
			}
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			if (size.x <= 0 || size.z <= 0)
			{
				Log.Error("Thing has 0 size.", false);
			}
			else
			{
				for (int i = rp.rect.minX; i <= rp.rect.maxX - size.x + 1; i += size.x + num)
				{
					for (int j = rp.rect.minZ; j <= rp.rect.maxZ - size.z + 1; j += size.z + num)
					{
						ResolveParams resolveParams = rp;
						resolveParams.rect = new CellRect(i, j, size.x, size.z);
						resolveParams.singleThingDef = thingDef2;
						resolveParams.thingRot = new Rot4?(rot);
						BaseGen.symbolStack.Push("thing", resolveParams);
					}
				}
				BaseGen.symbolStack.Push("clear", rp);
			}
		}

		[CompilerGenerated]
		private static bool <Resolve>m__0(ThingDef x)
		{
			return x.IsWeapon || x.IsMedicine || x.IsDrug;
		}
	}
}
