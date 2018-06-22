using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C2 RID: 962
	public class SymbolResolver_EdgeMannedMortar : SymbolResolver
	{
		// Token: 0x060010A2 RID: 4258 RVA: 0x0008D4E8 File Offset: 0x0008B8E8
		public override bool CanResolve(ResolveParams rp)
		{
			CellRect cellRect;
			return base.CanResolve(rp) && this.TryFindRandomInnerRectTouchingEdge(rp.rect, out cellRect);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0008D51C File Offset: 0x0008B91C
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (this.TryFindRandomInnerRectTouchingEdge(rp.rect, out rect))
			{
				Rot4 value;
				if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.minX))
				{
					value = Rot4.West;
				}
				else if (rect.Cells.Any((IntVec3 x) => x.x == rp.rect.maxX))
				{
					value = Rot4.East;
				}
				else if (rect.Cells.Any((IntVec3 x) => x.z == rp.rect.minZ))
				{
					value = Rot4.South;
				}
				else
				{
					value = Rot4.North;
				}
				ResolveParams rp2 = rp;
				rp2.rect = rect;
				rp2.thingRot = new Rot4?(value);
				BaseGen.symbolStack.Push("mannedMortar", rp2);
			}
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0008D600 File Offset: 0x0008BA00
		private bool TryFindRandomInnerRectTouchingEdge(CellRect rect, out CellRect mortarRect)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec2 size = new IntVec2(3, 3);
			return rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (CellRect x) => x.Cells.All((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null) && GenConstruct.TerrainCanSupport(x, map, ThingDefOf.Turret_Mortar)) || rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (CellRect x) => x.Cells.All((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
		}
	}
}
