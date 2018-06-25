using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C3F RID: 3135
	internal static class MapMeshFlagUtility
	{
		// Token: 0x04002F46 RID: 12102
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();

		// Token: 0x06004513 RID: 17683 RVA: 0x00246010 File Offset: 0x00244410
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
