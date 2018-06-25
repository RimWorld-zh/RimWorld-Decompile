using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065D RID: 1629
	public class TaleData_Surroundings : TaleData
	{
		// Token: 0x04001354 RID: 4948
		public int tile;

		// Token: 0x04001355 RID: 4949
		public float temperature;

		// Token: 0x04001356 RID: 4950
		public float snowDepth;

		// Token: 0x04001357 RID: 4951
		public WeatherDef weather;

		// Token: 0x04001358 RID: 4952
		public RoomRoleDef roomRole;

		// Token: 0x04001359 RID: 4953
		public float roomImpressiveness;

		// Token: 0x0400135A RID: 4954
		public float roomBeauty;

		// Token: 0x0400135B RID: 4955
		public float roomCleanliness;

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x00120B04 File Offset: 0x0011EF04
		public bool Outdoors
		{
			get
			{
				return this.weather != null;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00120B28 File Offset: 0x0011EF28
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tile, "tile", 0, false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.snowDepth, "snowDepth", 0f, false);
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
			Scribe_Defs.Look<RoomRoleDef>(ref this.roomRole, "roomRole");
			Scribe_Values.Look<float>(ref this.roomImpressiveness, "roomImpressiveness", 0f, false);
			Scribe_Values.Look<float>(ref this.roomBeauty, "roomBeauty", 0f, false);
			Scribe_Values.Look<float>(ref this.roomCleanliness, "roomCleanliness", 0f, false);
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00120BD8 File Offset: 0x0011EFD8
		public override IEnumerable<Rule> GetRules()
		{
			yield return new Rule_String("BIOME", Find.WorldGrid[this.tile].biome.label);
			if (this.roomRole != null && this.roomRole != RoomRoleDefOf.None)
			{
				yield return new Rule_String("ROOM_role", this.roomRole.label);
				yield return new Rule_String("ROOM_roleDefinite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.roomRole.label));
				yield return new Rule_String("ROOM_roleIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.roomRole.label));
				RoomStatScoreStage impressiveness = RoomStatDefOf.Impressiveness.GetScoreStage(this.roomImpressiveness);
				RoomStatScoreStage beauty = RoomStatDefOf.Beauty.GetScoreStage(this.roomBeauty);
				RoomStatScoreStage cleanliness = RoomStatDefOf.Cleanliness.GetScoreStage(this.roomCleanliness);
				yield return new Rule_String("ROOM_impressiveness", impressiveness.label);
				yield return new Rule_String("ROOM_impressivenessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(impressiveness.label));
				yield return new Rule_String("ROOM_beauty", beauty.label);
				yield return new Rule_String("ROOM_beautyIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(beauty.label));
				yield return new Rule_String("ROOM_cleanliness", cleanliness.label);
				yield return new Rule_String("ROOM_cleanlinessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(cleanliness.label));
			}
			yield break;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00120C04 File Offset: 0x0011F004
		public static TaleData_Surroundings GenerateFrom(IntVec3 c, Map map)
		{
			TaleData_Surroundings taleData_Surroundings = new TaleData_Surroundings();
			taleData_Surroundings.tile = map.Tile;
			Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
			if (roomOrAdjacent != null)
			{
				if (roomOrAdjacent.PsychologicallyOutdoors)
				{
					taleData_Surroundings.weather = map.weatherManager.CurPerceivedWeather;
				}
				taleData_Surroundings.roomRole = roomOrAdjacent.Role;
				taleData_Surroundings.roomImpressiveness = roomOrAdjacent.GetStat(RoomStatDefOf.Impressiveness);
				taleData_Surroundings.roomBeauty = roomOrAdjacent.GetStat(RoomStatDefOf.Beauty);
				taleData_Surroundings.roomCleanliness = roomOrAdjacent.GetStat(RoomStatDefOf.Cleanliness);
			}
			if (!GenTemperature.TryGetTemperatureForCell(c, map, out taleData_Surroundings.temperature))
			{
				taleData_Surroundings.temperature = 21f;
			}
			taleData_Surroundings.snowDepth = map.snowGrid.GetDepth(c);
			return taleData_Surroundings;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00120CC8 File Offset: 0x0011F0C8
		public static TaleData_Surroundings GenerateRandom(Map map)
		{
			return TaleData_Surroundings.GenerateFrom(CellFinder.RandomCell(map), map);
		}
	}
}
