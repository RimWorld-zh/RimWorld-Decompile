using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse
{
	internal static class MapMeshFlagUtility
	{
		public static List<MapMeshFlag> allFlags = new List<MapMeshFlag>();

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
