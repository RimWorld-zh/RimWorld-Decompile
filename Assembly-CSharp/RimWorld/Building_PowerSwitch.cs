using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000680 RID: 1664
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06002311 RID: 8977 RVA: 0x0012E09C File Offset: 0x0012C49C
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x0012E0B8 File Offset: 0x0012C4B8
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0012E0D8 File Offset: 0x0012C4D8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0012E0F0 File Offset: 0x0012C4F0
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

		// Token: 0x06002315 RID: 8981 RVA: 0x0012E140 File Offset: 0x0012C540
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0012E194 File Offset: 0x0012C594
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

		// Token: 0x06002317 RID: 8983 RVA: 0x0012E22C File Offset: 0x0012C62C
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

		// Token: 0x040013BC RID: 5052
		private bool wantsOnOld = true;

		// Token: 0x040013BD RID: 5053
		private CompFlickable flickableComp;
	}
}
