using System;

namespace Verse
{
	// Token: 0x02000CA4 RID: 3236
	public static class MapSerializeUtility
	{
		// Token: 0x0600474A RID: 18250 RVA: 0x00259C8C File Offset: 0x0025808C
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x00259CD8 File Offset: 0x002580D8
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
