using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C40 RID: 3136
	internal static class MapMeshFlagUtility
	{
		// Token: 0x06004509 RID: 17673 RVA: 0x002448AC File Offset: 0x00242CAC
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

		// Token: 0x04002F37 RID: 12087
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();
	}
}
