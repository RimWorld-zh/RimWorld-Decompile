using System;

namespace Verse
{
	// Token: 0x02000CA2 RID: 3234
	public static class MapSerializeUtility
	{
		// Token: 0x06004747 RID: 18247 RVA: 0x00259BB0 File Offset: 0x00257FB0
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x00259BFC File Offset: 0x00257FFC
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
