using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000679 RID: 1657
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x0012B600 File Offset: 0x00129A00
		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing result;
				if (this.miniToInstall != null)
				{
					result = this.miniToInstall;
				}
				else
				{
					if (this.buildingToReinstall == null)
					{
						throw new InvalidOperationException("Nothing to install.");
					}
					result = this.buildingToReinstall;
				}
				return result;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x0012B648 File Offset: 0x00129A48
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0012B668 File Offset: 0x00129A68
		public override Graphic Graphic
		{
			get
			{
				Graphic graphic = this.ThingToInstall.def.installBlueprintDef.graphic;
				return graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x0012B6A4 File Offset: 0x00129AA4
		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0012B6BE File Offset: 0x00129ABE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x0012B6EC File Offset: 0x00129AEC
		public override ThingDef UIStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x0012B70C File Offset: 0x00129B0C
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.", false);
			return new List<ThingDefCountClass>();
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0012B734 File Offset: 0x00129B34
		protected override Thing MakeSolidThing()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (this.miniToInstall != null)
			{
				this.miniToInstall.InnerThing = null;
				this.miniToInstall.Destroy(DestroyMode.Vanish);
			}
			return thingToInstall;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x0012B778 File Offset: 0x00129B78
		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0012B7D4 File Offset: 0x00129BD4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(this.ThingToInstall.def, this.ThingToInstall.Stuff);
			if (buildCopy != null)
			{
				yield return buildCopy;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command facility in BuildFacilityCommandUtility.BuildFacilityCommands(this.ThingToInstall.def))
				{
					yield return facility;
				}
			}
			yield break;
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x0012B7FE File Offset: 0x00129BFE
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.buildingToReinstall != null)
			{
				GenDraw.DrawLineBetween(this.buildingToReinstall.TrueCenter(), this.TrueCenter());
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x0012B828 File Offset: 0x00129C28
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0012B839 File Offset: 0x00129C39
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.", false);
			}
			else
			{
				this.miniToInstall = null;
				this.buildingToReinstall = buildingToReinstall;
			}
		}

		// Token: 0x04001393 RID: 5011
		private MinifiedThing miniToInstall;

		// Token: 0x04001394 RID: 5012
		private Building buildingToReinstall;
	}
}
