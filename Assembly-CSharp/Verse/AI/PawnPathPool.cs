using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A93 RID: 2707
	public class PawnPathPool
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x001FD030 File Offset: 0x001FB430
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x001FD05C File Offset: 0x001FB45C
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x001FD078 File Offset: 0x001FB478
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

		// Token: 0x040025F8 RID: 9720
		private Map map;

		// Token: 0x040025F9 RID: 9721
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x040025FA RID: 9722
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
