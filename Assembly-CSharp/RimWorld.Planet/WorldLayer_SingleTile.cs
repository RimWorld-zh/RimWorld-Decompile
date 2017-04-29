using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace RimWorld.Planet
{
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		private int lastDrawnTile = -1;

		private List<Vector3> verts = new List<Vector3>();

		protected abstract int Tile
		{
			get;
		}

		protected abstract Material Material
		{
			get;
		}

		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_SingleTile.<Regenerate>c__IteratorEF <Regenerate>c__IteratorEF = new WorldLayer_SingleTile.<Regenerate>c__IteratorEF();
			<Regenerate>c__IteratorEF.<>f__this = this;
			WorldLayer_SingleTile.<Regenerate>c__IteratorEF expr_0E = <Regenerate>c__IteratorEF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
