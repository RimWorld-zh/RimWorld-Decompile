using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000547 RID: 1351
	public class WorldReachability
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x000DB6FE File Offset: 0x000D9AFE
		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000DB729 File Offset: 0x000D9B29
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000DB734 File Offset: 0x000D9B34
		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000DB758 File Offset: 0x000D9B58
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

		// Token: 0x06001944 RID: 6468 RVA: 0x000DB83C File Offset: 0x000D9C3C
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

		// Token: 0x06001945 RID: 6469 RVA: 0x000DB87C File Offset: 0x000D9C7C
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x000DB8A0 File Offset: 0x000D9CA0
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

		// Token: 0x04000ED1 RID: 3793
		private int[] fields;

		// Token: 0x04000ED2 RID: 3794
		private int nextFieldID;

		// Token: 0x04000ED3 RID: 3795
		private int impassableFieldID;

		// Token: 0x04000ED4 RID: 3796
		private int minValidFieldID;
	}
}
