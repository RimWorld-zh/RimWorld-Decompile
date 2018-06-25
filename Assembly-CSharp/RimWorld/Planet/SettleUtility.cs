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
		// Token: 0x04001214 RID: 4628
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x00107A64 File Offset: 0x00105E64
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

		// Token: 0x06001E6B RID: 7787 RVA: 0x00107ABC File Offset: 0x00105EBC
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
