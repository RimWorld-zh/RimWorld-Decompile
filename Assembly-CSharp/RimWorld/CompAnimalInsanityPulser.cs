using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class CompAnimalInsanityPulser : ThingComp
	{
		private int ticksToInsanityPulse;

		public CompProperties_AnimalInsanityPulser Props
		{
			get
			{
				return (CompProperties_AnimalInsanityPulser)base.props;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToInsanityPulse, "ticksToInsanityPulse", 0, false);
		}

		public override void CompTick()
		{
			if (base.parent.Spawned)
			{
				this.ticksToInsanityPulse--;
				if (this.ticksToInsanityPulse <= 0)
				{
					this.DoAnimalInsanityPulse();
					this.ticksToInsanityPulse = this.Props.pulseInterval.RandomInRange;
				}
			}
		}

		private void DoAnimalInsanityPulse()
		{
			IEnumerable<Pawn> enumerable = from p in base.parent.Map.mapPawns.AllPawnsSpawned
			where p.RaceProps.Animal && p.Position.InHorDistOf(base.parent.Position, (float)this.Props.radius)
			select p;
			bool flag = false;
			foreach (Pawn item in enumerable)
			{
				if (item.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null))
				{
					flag = true;
				}
			}
			if (flag)
			{
				Messages.Message("MessageAnimalInsanityPulse".Translate(), base.parent, MessageTypeDefOf.ThreatSmall);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(base.parent.Map);
				if (base.parent.Map == Find.VisibleMap)
				{
					Find.CameraDriver.shaker.DoShake(4f);
				}
			}
		}
	}
}
