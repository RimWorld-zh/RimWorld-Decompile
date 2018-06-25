using System;

namespace Verse
{
	// Token: 0x02000CA4 RID: 3236
	public static class MapExposeUtility
	{
		// Token: 0x06004749 RID: 18249 RVA: 0x00259F2C File Offset: 0x0025832C
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.ByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}
	}
}
