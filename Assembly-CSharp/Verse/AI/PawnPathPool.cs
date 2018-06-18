using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A97 RID: 2711
	public class PawnPathPool
	{
		// Token: 0x06003C3B RID: 15419 RVA: 0x001FCD0C File Offset: 0x001FB10C
		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x001FCD38 File Offset: 0x001FB138
		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001FCD54 File Offset: 0x001FB154
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

		// Token: 0x040025FD RID: 9725
		private Map map;

		// Token: 0x040025FE RID: 9726
		private List<PawnPath> paths = new List<PawnPath>(64);

		// Token: 0x040025FF RID: 9727
		private static readonly PawnPath NotFoundPathInt = PawnPath.NewNotFound();
	}
}
