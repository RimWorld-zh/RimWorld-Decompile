using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007E6 RID: 2022
	public class Designator_Install : Designator_Place
	{
		// Token: 0x06002CDE RID: 11486 RVA: 0x00179DC5 File Offset: 0x001781C5
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002CDF RID: 11487 RVA: 0x00179DFC File Offset: 0x001781FC
		private Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				Thing result;
				if (singleSelectedThing is MinifiedThing)
				{
					result = singleSelectedThing;
				}
				else
				{
					Building building = singleSelectedThing as Building;
					if (building != null && building.def.Minifiable)
					{
						result = singleSelectedThing;
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002CE0 RID: 11488 RVA: 0x00179E54 File Offset: 0x00178254
		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002CE1 RID: 11489 RVA: 0x00179E74 File Offset: 0x00178274
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002CE2 RID: 11490 RVA: 0x00179E8C File Offset: 0x0017828C
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002CE3 RID: 11491 RVA: 0x00179EAC File Offset: 0x001782AC
		public override string Label
		{
			get
			{
				string result;
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					result = "CommandInstall".Translate();
				}
				else
				{
					result = "CommandReinstall".Translate();
				}
				return result;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x00179EEC File Offset: 0x001782EC
		public override string Desc
		{
			get
			{
				string result;
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					result = "CommandInstallDesc".Translate();
				}
				else
				{
					result = "CommandReinstallDesc".Translate();
				}
				return result;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002CE5 RID: 11493 RVA: 0x00179F2C File Offset: 0x0017832C
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x00179F48 File Offset: 0x00178348
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x00179F7C File Offset: 0x0017837C
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00179FA0 File Offset: 0x001783A0
		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					this.placingRot = Rot4.North;
				}
			}
			base.ProcessInput(ev);
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00179FEC File Offset: 0x001783EC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!c.InBounds(base.Map))
			{
				result = false;
			}
			else if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Thing x) => x.Position == c && x.Rotation == this.placingRot && x.def == this.PlacingDef) != null)
			{
				result = new AcceptanceReport("IdenticalThingExists".Translate());
			}
			else
			{
				BuildableDef placingDef = this.PlacingDef;
				IntVec3 c2 = c;
				Rot4 placingRot = this.placingRot;
				Map map = base.Map;
				Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
				result = GenConstruct.CanPlaceBlueprintAt(placingDef, c2, placingRot, map, false, miniToInstallOrBuildingToReinstall);
			}
			return result;
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x0017A0B8 File Offset: 0x001784B8
		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, this.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, this.placingRot, Faction.OfPlayer);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x0017A164 File Offset: 0x00178564
		protected override void DrawGhost(Color ghostCol)
		{
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint);
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x0017A1A8 File Offset: 0x001785A8
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
