using Verse;

namespace RimWorld
{
	internal static class PlantPosIndices
	{
		private const int ListCount = 8;

		private static int[][][] rootList;

		static PlantPosIndices()
		{
			PlantPosIndices.rootList = new int[25][][];
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int num = 0; num < i; num++)
					{
						array[num] = num;
					}
					array.Shuffle();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}
	}
}
