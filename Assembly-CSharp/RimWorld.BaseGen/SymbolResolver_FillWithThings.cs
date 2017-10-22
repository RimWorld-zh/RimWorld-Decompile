using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FillWithThings : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			bool result;
			if (!base.CanResolve(rp))
			{
				result = false;
				goto IL_00b7;
			}
			if (rp.singleThingToSpawn != null)
			{
				result = false;
				goto IL_00b7;
			}
			if (rp.singleThingDef != null)
			{
				Rot4? thingRot = rp.thingRot;
				Rot4 rot = (!thingRot.HasValue) ? Rot4.North : thingRot.Value;
				IntVec3 zero = IntVec3.Zero;
				IntVec2 size = rp.singleThingDef.size;
				GenAdj.AdjustForRotation(ref zero, ref size, rot);
				if (rp.rect.Width >= size.x && rp.rect.Height >= size.z)
				{
					goto IL_00b0;
				}
				result = false;
				goto IL_00b7;
			}
			goto IL_00b0;
			IL_00b7:
			return result;
			IL_00b0:
			result = true;
			goto IL_00b7;
		}

		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.singleThingDef ?? (from x in ItemCollectionGeneratorUtility.allGeneratableItems
			where x.IsWeapon || x.IsMedicine || x.IsDrug
			select x).RandomElement();
			Rot4? thingRot = rp.thingRot;
			Rot4 rot = (!thingRot.HasValue) ? Rot4.North : thingRot.Value;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef.size;
			int? fillWithThingsPadding = rp.fillWithThingsPadding;
			int num = fillWithThingsPadding.HasValue ? fillWithThingsPadding.Value : 0;
			if (num < 0)
			{
				num = 0;
			}
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			if (size.x <= 0 || size.z <= 0)
			{
				Log.Error("Thing has 0 size.");
			}
			else
			{
				for (int num2 = rp.rect.minX; num2 <= rp.rect.maxX - size.x + 1; num2 += size.x + num)
				{
					for (int num3 = rp.rect.minZ; num3 <= rp.rect.maxZ - size.z + 1; num3 += size.z + num)
					{
						ResolveParams resolveParams = rp;
						resolveParams.rect = new CellRect(num2, num3, size.x, size.z);
						resolveParams.singleThingDef = thingDef;
						resolveParams.thingRot = new Rot4?(rot);
						BaseGen.symbolStack.Push("thing", resolveParams);
					}
				}
				BaseGen.symbolStack.Push("clear", rp);
			}
		}
	}
}
