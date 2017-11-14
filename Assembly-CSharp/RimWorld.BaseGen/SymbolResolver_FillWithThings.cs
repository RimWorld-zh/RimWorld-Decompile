using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_FillWithThings : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null)
			{
				return false;
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
					goto IL_009d;
				}
				return false;
			}
			goto IL_009d;
			IL_009d:
			return true;
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
				for (int i = rp.rect.minX; i <= rp.rect.maxX - size.x + 1; i += size.x + num)
				{
					for (int j = rp.rect.minZ; j <= rp.rect.maxZ - size.z + 1; j += size.z + num)
					{
						ResolveParams resolveParams = rp;
						resolveParams.rect = new CellRect(i, j, size.x, size.z);
						resolveParams.singleThingDef = thingDef;
						resolveParams.thingRot = rot;
						BaseGen.symbolStack.Push("thing", resolveParams);
					}
				}
				BaseGen.symbolStack.Push("clear", rp);
			}
		}
	}
}
