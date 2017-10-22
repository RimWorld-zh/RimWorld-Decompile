using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EdgeMannedMortar : SymbolResolver
	{
		public override bool CanResolve(ResolveParams rp)
		{
			CellRect cellRect = default(CellRect);
			return base.CanResolve(rp) && this.TryFindRandomInnerRectTouchingEdge(rp.rect, out cellRect);
		}

		public override void Resolve(ResolveParams rp)
		{
			CellRect rect = default(CellRect);
			if (this.TryFindRandomInnerRectTouchingEdge(rp.rect, out rect))
			{
				Rot4 value = (!rect.Cells.Any((Func<IntVec3, bool>)((IntVec3 x) => x.x == rp.rect.minX))) ? ((!rect.Cells.Any((Func<IntVec3, bool>)((IntVec3 x) => x.x == rp.rect.maxX))) ? ((!rect.Cells.Any((Func<IntVec3, bool>)((IntVec3 x) => x.z == rp.rect.minZ))) ? Rot4.North : Rot4.South) : Rot4.East) : Rot4.West;
				ResolveParams resolveParams = rp;
				resolveParams.rect = rect;
				resolveParams.thingRot = new Rot4?(value);
				BaseGen.symbolStack.Push("mannedMortar", resolveParams);
			}
		}

		private bool TryFindRandomInnerRectTouchingEdge(CellRect rect, out CellRect mortarRect)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec2 size = new IntVec2(3, 3);
			if (rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (Predicate<CellRect>)((CellRect x) => x.Cells.All((Func<IntVec3, bool>)((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null)) && GenConstruct.TerrainCanSupport(x, map, ThingDefOf.Turret_MortarBomb))))
			{
				return true;
			}
			if (rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (Predicate<CellRect>)((CellRect x) => x.Cells.All((Func<IntVec3, bool>)((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null)))))
			{
				return true;
			}
			return false;
		}
	}
}
