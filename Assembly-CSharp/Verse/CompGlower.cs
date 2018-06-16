using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C15 RID: 3093
	public class CompGlower : ThingComp
	{
		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x0600438F RID: 17295 RVA: 0x0023A514 File Offset: 0x00238914
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06004390 RID: 17296 RVA: 0x0023A534 File Offset: 0x00238934
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

		// Token: 0x06004391 RID: 17297 RVA: 0x0023A5C0 File Offset: 0x002389C0
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

		// Token: 0x06004392 RID: 17298 RVA: 0x0023A648 File Offset: 0x00238A48
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

		// Token: 0x06004393 RID: 17299 RVA: 0x0023A6A0 File Offset: 0x00238AA0
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0023A73F File Offset: 0x00238B3F
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0023A754 File Offset: 0x00238B54
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.UpdateLit(map);
		}

		// Token: 0x04002E21 RID: 11809
		private bool glowOnInt = false;
	}
}
