using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	public sealed class MapInfo : IExposable
	{
		private IntVec3 sizeInt = default(IntVec3);

		public MapParent parent;

		public int Tile
		{
			get
			{
				return this.parent.Tile;
			}
		}

		public int NumCells
		{
			get
			{
				IntVec3 size = this.Size;
				int x = size.x;
				IntVec3 size2 = this.Size;
				int num = x * size2.y;
				IntVec3 size3 = this.Size;
				return num * size3.z;
			}
		}

		public IntVec3 Size
		{
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		public int PowerOfTwoOverMapSize
		{
			get
			{
				int num = Mathf.Max(this.sizeInt.x, this.sizeInt.z);
				int num2;
				for (num2 = 1; num2 <= num; num2 *= 2)
				{
				}
				return num2;
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}
	}
}
