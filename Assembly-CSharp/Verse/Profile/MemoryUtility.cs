using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D6B RID: 3435
	public static class MemoryUtility
	{
		// Token: 0x06004CE7 RID: 19687 RVA: 0x002811D0 File Offset: 0x0027F5D0
		public static void UnloadUnusedUnityAssets()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Resources.UnloadUnusedAssets();
			});
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x002811F8 File Offset: 0x0027F5F8
		public static void ClearAllMapsAndWorld()
		{
			if (Current.Game != null && Current.Game.Maps != null)
			{
				List<Map> maps = Find.Maps;
				FieldInfo[] fields = typeof(Map).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					if (fields[i].FieldType.IsClass)
					{
						for (int j = 0; j < maps.Count; j++)
						{
							fields[i].SetValue(maps[j], null);
						}
					}
				}
				maps.Clear();
				Current.Game.currentMapIndex = -1;
			}
			if (Find.World != null)
			{
				World world = Find.World;
				FieldInfo[] fields2 = typeof(World).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int k = 0; k < fields2.Length; k++)
				{
					if (fields2[k].FieldType.IsClass)
					{
						fields2[k].SetValue(world, null);
					}
				}
			}
			BillUtility.Clipboard = null;
		}
	}
}
