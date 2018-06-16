using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000702 RID: 1794
	public class CompAnimalInsanityPulser : ThingComp
	{
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002733 RID: 10035 RVA: 0x001517E8 File Offset: 0x0014FBE8
		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)this.props;
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00151808 File Offset: 0x0014FC08
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x0015182E File Offset: 0x0014FC2E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0015184C File Offset: 0x0014FC4C
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

		// Token: 0x06002737 RID: 10039 RVA: 0x001518A8 File Offset: 0x0014FCA8
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

		// Token: 0x040015B6 RID: 5558
		private int ticksToInsanityPulse;
	}
}
