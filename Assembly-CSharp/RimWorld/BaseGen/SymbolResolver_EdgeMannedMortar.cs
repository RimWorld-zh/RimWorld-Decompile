using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_EdgeMannedMortar : SymbolResolver
	{
		public SymbolResolver_EdgeMannedMortar()
		{
		}

		public override bool CanResolve(ResolveParams rp)
		{
			CellRect cellRect;
			return base.CanResolve(rp) && this.TryFindRandomInnerRectTouchingEdge(rp.rect, out cellRect);
		}

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

		private bool TryFindRandomInnerRectTouchingEdge(CellRect rect, out CellRect mortarRect)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec2 size = new IntVec2(3, 3);
			return rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (CellRect x) => x.Cells.All((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null) && GenConstruct.TerrainCanSupport(x, map, ThingDefOf.Turret_Mortar)) || rect.TryFindRandomInnerRectTouchingEdge(size, out mortarRect, (CellRect x) => x.Cells.All((IntVec3 y) => y.Standable(map) && y.GetEdifice(map) == null));
		}

		[CompilerGenerated]
		private sealed class <Resolve>c__AnonStorey0
		{
			internal ResolveParams rp;

			public <Resolve>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.x == this.rp.rect.minX;
			}

			internal bool <>m__1(IntVec3 x)
			{
				return x.x == this.rp.rect.maxX;
			}

			internal bool <>m__2(IntVec3 x)
			{
				return x.z == this.rp.rect.minZ;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomInnerRectTouchingEdge>c__AnonStorey1
		{
			internal Map map;

			public <TryFindRandomInnerRectTouchingEdge>c__AnonStorey1()
			{
			}

			internal bool <>m__0(CellRect x)
			{
				return x.Cells.All((IntVec3 y) => y.Standable(this.map) && y.GetEdifice(this.map) == null) && GenConstruct.TerrainCanSupport(x, this.map, ThingDefOf.Turret_Mortar);
			}

			internal bool <>m__1(CellRect x)
			{
				return x.Cells.All((IntVec3 y) => y.Standable(this.map) && y.GetEdifice(this.map) == null);
			}

			internal bool <>m__2(IntVec3 y)
			{
				return y.Standable(this.map) && y.GetEdifice(this.map) == null;
			}

			internal bool <>m__3(IntVec3 y)
			{
				return y.Standable(this.map) && y.GetEdifice(this.map) == null;
			}
		}
	}
}
