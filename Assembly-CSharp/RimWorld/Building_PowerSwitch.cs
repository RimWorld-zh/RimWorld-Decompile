using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000684 RID: 1668
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x0012DF54 File Offset: 0x0012C354
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0012DF70 File Offset: 0x0012C370
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0012DF90 File Offset: 0x0012C390
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0012DFA8 File Offset: 0x0012C3A8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.flickableComp == null)
				{
					this.flickableComp = base.GetComp<CompFlickable>();
				}
				this.wantsOnOld = !FlickUtility.WantsToBeOn(this);
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0012DFF8 File Offset: 0x0012C3F8
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0012E04C File Offset: 0x0012C44C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("PowerSwitch_Power".Translate() + ": ");
			if (FlickUtility.WantsToBeOn(this))
			{
				stringBuilder.Append("On".Translate().ToLower());
			}
			else
			{
				stringBuilder.Append("Off".Translate().ToLower());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0012E0E4 File Offset: 0x0012C4E4
		private void UpdatePowerGrid()
		{
			if (FlickUtility.WantsToBeOn(this) != this.wantsOnOld)
			{
				if (base.Spawned)
				{
					base.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(base.PowerComp);
				}
				this.wantsOnOld = FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x040013BE RID: 5054
		private bool wantsOnOld = true;

		// Token: 0x040013BF RID: 5055
		private CompFlickable flickableComp;
	}
}
