using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace RimWorld.Planet
{
	public class WorldLayer_Terrain : WorldLayer
	{
		private List<MeshCollider> meshCollidersInOrder = new List<MeshCollider>();

		private List<List<int>> triangleIndexToTileID = new List<List<int>>();

		private List<Vector3> elevationValues = new List<Vector3>();

		[DebuggerHidden]
		public override IEnumerable Regenerate()
		{
			WorldLayer_Terrain.<Regenerate>c__IteratorF6 <Regenerate>c__IteratorF = new WorldLayer_Terrain.<Regenerate>c__IteratorF6();
			<Regenerate>c__IteratorF.<>f__this = this;
			WorldLayer_Terrain.<Regenerate>c__IteratorF6 expr_0E = <Regenerate>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.meshCollidersInOrder.Count;
			while (i < count)
			{
				if (this.meshCollidersInOrder[i] == hit.collider)
				{
					return this.triangleIndexToTileID[i][hit.triangleIndex];
				}
				i++;
			}
			return -1;
		}

		[DebuggerHidden]
		private IEnumerable RegenerateMeshColliders()
		{
			WorldLayer_Terrain.<RegenerateMeshColliders>c__IteratorF7 <RegenerateMeshColliders>c__IteratorF = new WorldLayer_Terrain.<RegenerateMeshColliders>c__IteratorF7();
			<RegenerateMeshColliders>c__IteratorF.<>f__this = this;
			WorldLayer_Terrain.<RegenerateMeshColliders>c__IteratorF7 expr_0E = <RegenerateMeshColliders>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		private IEnumerable CalculateInterpolatedVerticesParams()
		{
			WorldLayer_Terrain.<CalculateInterpolatedVerticesParams>c__IteratorF8 <CalculateInterpolatedVerticesParams>c__IteratorF = new WorldLayer_Terrain.<CalculateInterpolatedVerticesParams>c__IteratorF8();
			<CalculateInterpolatedVerticesParams>c__IteratorF.<>f__this = this;
			WorldLayer_Terrain.<CalculateInterpolatedVerticesParams>c__IteratorF8 expr_0E = <CalculateInterpolatedVerticesParams>c__IteratorF;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
