using System;

namespace Verse
{
	// Token: 0x02000CA6 RID: 3238
	public static class MapSerializeUtility
	{
		// Token: 0x06004740 RID: 18240 RVA: 0x002587E8 File Offset: 0x00256BE8
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x00258834 File Offset: 0x00256C34
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
