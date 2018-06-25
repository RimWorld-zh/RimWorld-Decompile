using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000700 RID: 1792
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x040015B8 RID: 5560
		private int ticksToInsanityPulse;

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x00151DB4 File Offset: 0x001501B4
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x00151DD4 File Offset: 0x001501D4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x00151DFA File Offset: 0x001501FA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x00151E18 File Offset: 0x00150218
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

		// Token: 0x06002734 RID: 10036 RVA: 0x00151E74 File Offset: 0x00150274
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
