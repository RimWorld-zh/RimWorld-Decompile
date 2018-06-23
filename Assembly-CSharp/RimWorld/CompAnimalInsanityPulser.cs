using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006FE RID: 1790
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x040015B4 RID: 5556
		private int ticksToInsanityPulse;

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x00151A04 File Offset: 0x0014FE04
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x00151A24 File Offset: 0x0014FE24
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x00151A4A File Offset: 0x0014FE4A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x00151A68 File Offset: 0x0014FE68
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				this.ticksToInsanityPulse--;
				if (this.ticksToInsanityPulse <= 0)
				{
					this.DoAnimalInsanityPulse();
					this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
				}
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x00151AC4 File Offset: 0x0014FEC4
		private void DoAnimalInsanityPulse()
		{
			IEnumerable<Pawn> enumerable = from p in this.parent.Map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.Position.InHorDistOf(this.parent.Position, (float)this.Props.radius)
			select p;
			bool flag = false;
			foreach (Pawn pawn in enumerable)
			{
				bool flag2 = pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false);
				if (flag2)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalInsanityPulse".Translate(), this.parent, MessageTypeDefOf.ThreatSmall, true);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
				if (this.parent.Map == Find.CurrentMap)
				{
					Find.CameraDriver.shaker.DoShake(4f);
				}
			}
		}
	}
}
