using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005FB RID: 1531
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x001075E0 File Offset: 0x001059E0
		public static bool PlayerHomesCountLimitReached
		{
			get
			{
				int num = 0;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						num++;
					}
				}
				return num >= Prefs.MaxNumberOfPlayerHomes;
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x00107638 File Offset: 0x00105A38
		public static FactionBase AddNewHome(int tile, Faction faction)
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.Tile = tile;
			factionBase.SetFaction(faction);
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
			Find.WorldObjects.Add(factionBase);
			return factionBase;
		}

		// Token: 0x04001213 RID: 4627
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);
	}
}
