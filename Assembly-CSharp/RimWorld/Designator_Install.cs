using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Install : Designator_Place
	{
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
					result = ((building == null || !building.def.Minifiable) ? null : singleSelectedThing);
				}
				return result;
			}
		}

		private Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		public override string Label
		{
			get
			{
				return (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing)) ? "CommandReinstall".Translate() : "CommandInstall".Translate();
			}
		}

		public override string Desc
		{
			get
			{
				return (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing)) ? "CommandReinstallDesc".Translate() : "CommandInstallDesc".Translate();
			}
		}

		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		public Designator_Install()
		{
			base.icon = TexCommand.Install;
			base.iconProportions = new Vector2(1f, 1f);
		}

		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					base.placingRot = Rot4.North;
				}
			}
			base.ProcessInput(ev);
		}

		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result;
			if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x.Position == c && x.Rotation == base.placingRot && x.def == this.PlacingDef)) != null)
			{
				result = new AcceptanceReport("IdenticalThingExists".Translate());
			}
			else
			{
				BuildableDef placingDef = this.PlacingDef;
				IntVec3 center = c;
				Rot4 placingRot = base.placingRot;
				Map map = base.Map;
				Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
				result = GenConstruct.CanPlaceBlueprintAt(placingDef, center, placingRot, map, false, miniToInstallOrBuildingToReinstall);
			}
			return result;
		}

		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, base.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, base.placingRot, Faction.OfPlayer);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, base.placingRot, Faction.OfPlayer);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, base.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		protected override void DrawGhost(Color ghostCol)
		{
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), base.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint);
		}

		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef);
		}
	}
}
