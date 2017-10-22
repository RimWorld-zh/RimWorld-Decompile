using Verse;

namespace RimWorld
{
	public class CompPowerPlantSteam : CompPowerPlant
	{
		private IntermittentSteamSprayer steamSprayer;

		private Building_SteamGeyser geyser;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(base.parent);
		}

		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)base.parent.Map.thingGrid.ThingAt(base.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)base.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

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
