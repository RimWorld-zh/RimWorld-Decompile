using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D6A RID: 3434
	public static class MemoryUtility
	{
		// Token: 0x06004CFE RID: 19710 RVA: 0x00282B6C File Offset: 0x00280F6C
		public static void UnloadUnusedUnityAssets()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Resources.UnloadUnusedAssets();
			});
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x00282B94 File Offset: 0x00280F94
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
