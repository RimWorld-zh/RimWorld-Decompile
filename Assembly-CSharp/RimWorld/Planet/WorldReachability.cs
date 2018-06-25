using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class WorldReachability
	{
		private int[] fields;

		private int nextFieldID;

		private int impassableFieldID;

		private int minValidFieldID;

		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		public bool CanReach(int startTile, int destTile)
		{
			bool result;
			if (startTile < 0 || startTile >= this.fields.Length || destTile < 0 || destTile >= this.fields.Length)
			{
				result = false;
			}
			else if (this.fields[startTile] == this.impassableFieldID || this.fields[destTile] == this.impassableFieldID)
			{
				result = false;
			}
			else if (this.IsValidField(this.fields[startTile]) || this.IsValidField(this.fields[destTile]))
			{
				result = (this.fields[startTile] == this.fields[destTile]);
			}
			else
			{
				this.FloodFillAt(startTile);
				result = (this.fields[startTile] != this.impassableFieldID && this.fields[startTile] == this.fields[destTile]);
			}
			return result;
		}

		private void InvalidateAllFields()
		{
			if (this.nextFieldID == 2147483646)
			{
				this.nextFieldID = 1;
			}
			this.minValidFieldID = this.nextFieldID;
			this.impassableFieldID = this.nextFieldID;
			this.nextFieldID++;
		}

		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		private void FloodFillAt(int tile)
		{
			World world = Find.World;
			if (world.Impassable(tile))
			{
				this.fields[tile] = this.impassableFieldID;
			}
			else
			{
				Find.WorldFloodFiller.FloodFill(tile, (int x) => !world.Impassable(x), delegate(int x)
				{
					this.fields[x] = this.nextFieldID;
				}, int.MaxValue, null);
				this.nextFieldID++;
			}
		}

		[CompilerGenerated]
		private sealed class <FloodFillAt>c__AnonStorey0
		{
			internal World world;

			internal WorldReachability $this;

			public <FloodFillAt>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int x)
			{
				return !this.world.Impassable(x);
			}

			internal void <>m__1(int x)
			{
				this.$this.fields[x] = this.$this.nextFieldID;
			}
		}
	}
}
