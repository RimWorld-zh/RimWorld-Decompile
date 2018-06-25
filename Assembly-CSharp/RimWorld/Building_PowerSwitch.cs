using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000682 RID: 1666
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x040013C0 RID: 5056
		private bool wantsOnOld = true;

		// Token: 0x040013C1 RID: 5057
		private CompFlickable flickableComp;

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x0012E454 File Offset: 0x0012C854
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x0012E470 File Offset: 0x0012C870
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0012E490 File Offset: 0x0012C890
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0012E4A8 File Offset: 0x0012C8A8
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

		// Token: 0x06002318 RID: 8984 RVA: 0x0012E4F8 File Offset: 0x0012C8F8
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0012E54C File Offset: 0x0012C94C
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

		// Token: 0x0600231A RID: 8986 RVA: 0x0012E5E4 File Offset: 0x0012C9E4
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
	}
}
