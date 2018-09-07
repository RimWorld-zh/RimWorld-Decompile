using System;
using RimWorld.Planet;

namespace RimWorld
{
	public abstract class BiomeWorker
	{
		protected BiomeWorker()
		{
		}

		public abstract float GetScore(Tile tile, int tileID);
	}
}
