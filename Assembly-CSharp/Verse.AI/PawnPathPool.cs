using System.Collections.Generic;

namespace Verse.AI
{
	public class PawnPathPool
	{
		private Map map;

		private List<PawnPath> paths = new List<PawnPath>(64);

		private static readonly PawnPath NotFoundPathInt;

		public static PawnPath NotFoundPath
		{
			get
			{
				return PawnPathPool.NotFoundPathInt;
			}
		}

		public PawnPathPool(Map map)
		{
			this.map = map;
		}

		static PawnPathPool()
		{
			PawnPathPool.NotFoundPathInt = PawnPath.NewNotFound();
		}

		public PawnPath GetEmptyPawnPath()
		{
			int num = 0;
			PawnPath result;
			while (true)
			{
				if (num < this.paths.Count)
				{
					if (!this.paths[num].inUse)
					{
						this.paths[num].inUse = true;
						result = this.paths[num];
						break;
					}
					num++;
					continue;
				}
				if (this.paths.Count > this.map.mapPawns.AllPawnsSpawnedCount + 2)
				{
					Log.ErrorOnce("PawnPathPool leak: more paths than spawned pawns. Force-recovering.", 664788);
					this.paths.Clear();
				}
				PawnPath pawnPath = new PawnPath();
				this.paths.Add(pawnPath);
				pawnPath.inUse = true;
				result = pawnPath;
				break;
			}
			return result;
		}
	}
}
