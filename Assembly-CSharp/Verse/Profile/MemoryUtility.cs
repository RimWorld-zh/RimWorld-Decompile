using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D69 RID: 3433
	public static class MemoryUtility
	{
		// Token: 0x06004CFE RID: 19710 RVA: 0x0028288C File Offset: 0x00280C8C
		public static void UnloadUnusedUnityAssets()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Resources.UnloadUnusedAssets();
			});
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x002828B4 File Offset: 0x00280CB4
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
