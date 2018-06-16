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
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x0012DEDC File Offset: 0x0012C2DC
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0012DEF8 File Offset: 0x0012C2F8
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0012DF18 File Offset: 0x0012C318
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0012DF30 File Offset: 0x0012C330
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

		// Token: 0x0600231B RID: 8987 RVA: 0x0012DF80 File Offset: 0x0012C380
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0012DFD4 File Offset: 0x0012C3D4
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

		// Token: 0x0600231D RID: 8989 RVA: 0x0012E06C File Offset: 0x0012C46C
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
