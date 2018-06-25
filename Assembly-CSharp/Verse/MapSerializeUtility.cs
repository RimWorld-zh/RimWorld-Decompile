using System;

namespace Verse
{
	// Token: 0x02000CA5 RID: 3237
	public static class MapSerializeUtility
	{
		// Token: 0x0600474A RID: 18250 RVA: 0x00259F6C File Offset: 0x0025836C
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x00259FB8 File Offset: 0x002583B8
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
