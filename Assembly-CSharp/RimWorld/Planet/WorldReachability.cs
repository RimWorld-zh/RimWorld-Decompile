using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000543 RID: 1347
	public class WorldReachability
	{
		// Token: 0x06001937 RID: 6455 RVA: 0x000DB70E File Offset: 0x000D9B0E
		public WorldReachability()
		{
			this.fields = new int[Find.WorldGrid.TilesCount];
			this.nextFieldID = 1;
			this.InvalidateAllFields();
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x000DB739 File Offset: 0x000D9B39
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000DB744 File Offset: 0x000D9B44
		public bool CanReach(Caravan c, int tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x000DB768 File Offset: 0x000D9B68
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

		// Token: 0x0600193B RID: 6459 RVA: 0x000DB84C File Offset: 0x000D9C4C
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

		// Token: 0x0600193C RID: 6460 RVA: 0x000DB88C File Offset: 0x000D9C8C
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x000DB8B0 File Offset: 0x000D9CB0
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

		// Token: 0x04000ECE RID: 3790
		private int[] fields;

		// Token: 0x04000ECF RID: 3791
		private int nextFieldID;

		// Token: 0x04000ED0 RID: 3792
		private int impassableFieldID;

		// Token: 0x04000ED1 RID: 3793
		private int minValidFieldID;
	}
}
