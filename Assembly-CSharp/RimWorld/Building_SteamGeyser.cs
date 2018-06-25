using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006CC RID: 1740
	public class Building_SteamGeyser : Building
	{
		// Token: 0x04001506 RID: 5382
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04001507 RID: 5383
		public Building harvester = null;

		// Token: 0x04001508 RID: 5384
		private Sustainer spraySustainer = null;

		// Token: 0x04001509 RID: 5385
		private int spraySustainerStartTick = -999;

		// Token: 0x060025AA RID: 9642 RVA: 0x00142D2C File Offset: 0x0014112C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00142D7C File Offset: 0x0014117C
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x00142DE4 File Offset: 0x001411E4
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x00142E08 File Offset: 0x00141208
		public override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null)
			{
				if (Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
				{
					Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.", false);
					this.spraySustainer.End();
					this.spraySustainer = null;
				}
			}
		}
	}
}
