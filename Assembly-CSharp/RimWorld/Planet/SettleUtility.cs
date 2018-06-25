using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F9 RID: 1529
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x04001210 RID: 4624
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x001077FC File Offset: 0x00105BFC
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

		// Token: 0x06001E6C RID: 7788 RVA: 0x00107854 File Offset: 0x00105C54
		public static FactionBase AddNewHome(int tile, Faction faction)
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.Tile = tile;
			factionBase.SetFaction(faction);
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
			Find.WorldObjects.Add(factionBase);
			return factionBase;
		}
	}
}
