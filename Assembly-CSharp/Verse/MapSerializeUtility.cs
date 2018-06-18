using System;

namespace Verse
{
	// Token: 0x02000CA5 RID: 3237
	public static class MapSerializeUtility
	{
		// Token: 0x0600473E RID: 18238 RVA: 0x002587C0 File Offset: 0x00256BC0
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0025880C File Offset: 0x00256C0C
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
