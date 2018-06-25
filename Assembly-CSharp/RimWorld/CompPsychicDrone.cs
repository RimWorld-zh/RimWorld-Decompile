using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000729 RID: 1833
	public class CompPsychicDrone : ThingComp
	{
		// Token: 0x04001617 RID: 5655
		private int ticksToIncreaseDroneLevel;

		// Token: 0x04001618 RID: 5656
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadLow;

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600285E RID: 10334 RVA: 0x00158EA4 File Offset: 0x001572A4
		public CompProperties_PsychicDrone Props
		{
			get
			{
				return (CompProperties_PsychicDrone)this.props;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600285F RID: 10335 RVA: 0x00158EC4 File Offset: 0x001572C4
		public PsychicDroneLevel DroneLevel
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00158EDF File Offset: 0x001572DF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x00158F15 File Offset: 0x00157315
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x00158F44 File Offset: 0x00157344
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				this.ticksToIncreaseDroneLevel--;
				if (this.ticksToIncreaseDroneLevel <= 0)
				{
					this.IncreaseDroneLevel();
					this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				}
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x00158F9C File Offset: 0x0015739C
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel != PsychicDroneLevel.BadExtreme)
			{
				this.droneLevel += 1;
				string text = "LetterPsychicDroneLevelIncreased".Translate();
				Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent, null);
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00159008 File Offset: 0x00157408
		public override string CompInspectStringExtra()
		{
			string text = "Error";
			switch (this.droneLevel)
			{
			case PsychicDroneLevel.BadLow:
				text = "PsychicDroneLevelLow".Translate();
				break;
			case PsychicDroneLevel.BadMedium:
				text = "PsychicDroneLevelMedium".Translate();
				break;
			case PsychicDroneLevel.BadHigh:
				text = "PsychicDroneLevelHigh".Translate();
				break;
			case PsychicDroneLevel.BadExtreme:
				text = "PsychicDroneLevelExtreme".Translate();
				break;
			}
			return "PsychicDroneLevel".Translate(new object[]
			{
				text
			});
		}
	}
}
