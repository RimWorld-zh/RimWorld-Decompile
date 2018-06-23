using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200041B RID: 1051
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x04000B0E RID: 2830
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04000B0F RID: 2831
		private Building_SteamGeyser geyser = null;

		// Token: 0x0600123C RID: 4668 RVA: 0x0009E4EA File Offset: 0x0009C8EA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0009E508 File Offset: 0x0009C908
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0009E585 File Offset: 0x0009C985
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}
	}
}
