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
		// Token: 0x040015B4 RID: 5556
		private int ticksToInsanityPulse;

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x00151B54 File Offset: 0x0014FF54
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x00151B74 File Offset: 0x0014FF74
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x00151B9A File Offset: 0x0014FF9A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00151BB8 File Offset: 0x0014FFB8
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

		// Token: 0x06002735 RID: 10037 RVA: 0x00151C14 File Offset: 0x00150014
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
