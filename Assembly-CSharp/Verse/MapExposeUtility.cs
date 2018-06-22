using System;

namespace Verse
{
	// Token: 0x02000CA1 RID: 3233
	public static class MapExposeUtility
	{
		// Token: 0x06004746 RID: 18246 RVA: 0x00259B70 File Offset: 0x00257F70
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
