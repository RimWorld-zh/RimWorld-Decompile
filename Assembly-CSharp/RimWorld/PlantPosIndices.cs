using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	internal static class PlantPosIndices
	{
		// Token: 0x06002628 RID: 9768 RVA: 0x00146808 File Offset: 0x00144C08
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

		// Token: 0x06002629 RID: 9769 RVA: 0x00146890 File Offset: 0x00144C90
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x04001541 RID: 5441
		private static int[][][] rootList = null;

		// Token: 0x04001542 RID: 5442
		private const int ListCount = 8;
	}
}
