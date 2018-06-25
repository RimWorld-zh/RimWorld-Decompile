using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C14 RID: 3092
	public class CompGlower : ThingComp
	{
		// Token: 0x04002E30 RID: 11824
		private bool glowOnInt = false;

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06004399 RID: 17305 RVA: 0x0023BC70 File Offset: 0x0023A070
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x0600439A RID: 17306 RVA: 0x0023BC90 File Offset: 0x0023A090
		private bool ShouldBeLitNow
		{
			get
			{
				bool result;
				if (!this.parent.Spawned)
				{
					result = false;
				}
				else if (!FlickUtility.WantsToBeOn(this.parent))
				{
					result = false;
				}
				else
				{
					CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
					if (compPowerTrader != null && !compPowerTrader.PowerOn)
					{
						result = false;
					}
					else
					{
						CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
						result = (compRefuelable == null || compRefuelable.HasFuel);
					}
				}
				return result;
			}
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0023BD1C File Offset: 0x0023A11C
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt != shouldBeLitNow)
			{
				this.glowOnInt = shouldBeLitNow;
				if (!this.glowOnInt)
				{
					map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
					map.glowGrid.DeRegisterGlower(this);
				}
				else
				{
					map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlag.Things);
					map.glowGrid.RegisterGlower(this);
				}
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0023BDA4 File Offset: 0x0023A1A4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.ShouldBeLitNow)
			{
				this.UpdateLit(this.parent.Map);
				this.parent.Map.glowGrid.RegisterGlower(this);
			}
			else
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0023BDFC File Offset: 0x0023A1FC
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0023BE9B File Offset: 0x0023A29B
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0023BEB0 File Offset: 0x0023A2B0
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}
	}
}
