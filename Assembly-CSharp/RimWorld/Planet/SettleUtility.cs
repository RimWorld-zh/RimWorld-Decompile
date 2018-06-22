using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F7 RID: 1527
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x001076AC File Offset: 0x00105AAC
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

		// Token: 0x06001E68 RID: 7784 RVA: 0x00107704 File Offset: 0x00105B04
		public static FactionBase AddNewHome(int tile, Faction faction)
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.Tile = tile;
			factionBase.SetFaction(faction);
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, null);
			Find.WorldObjects.Add(factionBase);
			return factionBase;
		}

		// Token: 0x04001210 RID: 4624
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);
	}
}
