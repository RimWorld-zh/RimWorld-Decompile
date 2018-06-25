using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D7 RID: 1751
	internal static class PlantPosIndices
	{
		// Token: 0x04001543 RID: 5443
		private static int[][][] rootList = null;

		// Token: 0x04001544 RID: 5444
		private const int ListCount = 8;

		// Token: 0x06002623 RID: 9763 RVA: 0x00146D04 File Offset: 0x00145104
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

		// Token: 0x06002624 RID: 9764 RVA: 0x00146D8C File Offset: 0x0014518C
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}
	}
}
