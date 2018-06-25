using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D7 RID: 1751
	internal static class PlantPosIndices
	{
		// Token: 0x0400153F RID: 5439
		private static int[][][] rootList = null;

		// Token: 0x04001540 RID: 5440
		private const int ListCount = 8;

		// Token: 0x06002624 RID: 9764 RVA: 0x00146AA4 File Offset: 0x00144EA4
		static PlantPosIndices()
		{
			PlantPosIndices.rootList = new int[25][][];
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int k = 0; k < i; k++)
					{
						array[k] = k;
					}
					array.Shuffle<int>();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00146B2C File Offset: 0x00144F2C
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}
	}
}
