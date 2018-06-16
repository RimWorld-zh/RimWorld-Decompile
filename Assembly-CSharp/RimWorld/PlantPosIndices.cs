using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006D9 RID: 1753
	internal static class PlantPosIndices
	{
		// Token: 0x06002626 RID: 9766 RVA: 0x00146790 File Offset: 0x00144B90
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

		// Token: 0x06002627 RID: 9767 RVA: 0x00146818 File Offset: 0x00144C18
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
