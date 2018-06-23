using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C3C RID: 3132
	internal static class MapMeshFlagUtility
	{
		// Token: 0x04002F3F RID: 12095
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();

		// Token: 0x06004510 RID: 17680 RVA: 0x00245C54 File Offset: 0x00244054
		static MapMeshFlagUtility()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(MapMeshFlag)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					MapMeshFlag mapMeshFlag = (MapMeshFlag)obj;
					if (mapMeshFlag != MapMeshFlag.None)
					{
						MapMeshFlagUtility.allFlags.Add(mapMeshFlag);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
