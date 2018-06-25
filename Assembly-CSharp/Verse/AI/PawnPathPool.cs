using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A96 RID: 2710
	public class PawnPathPool
	{
		// Token: 0x04002609 RID: 9737
		private Map map;

		// Token: 0x0400260A RID: 9738
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x0400260B RID: 9739
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();

		// Token: 0x06003C3B RID: 15419 RVA: 0x001FD488 File Offset: 0x001FB888
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x001FD4B4 File Offset: 0x001FB8B4
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001FD4D0 File Offset: 0x001FB8D0
		public PawnPath GetEmptyPawnPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > this.map.mapPawns.AllPawnsSpawnedCount + 2)
			{
				Log.ErrorOnce("PawnPathPool leak: more paths than spawned pawns. Force-recovering.", 664788, false);
				this.paths.Clear();
			}
			PawnPath pawnPath = new PawnPath();
			this.paths.Add(pawnPath);
			pawnPath.inUse = true;
			return pawnPath;
		}
	}
}
