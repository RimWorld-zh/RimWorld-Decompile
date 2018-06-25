using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Designator_Install : Designator_Place
	{
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.order = -10f;
		}

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
					this.placingRot = Rot4.North;
				}
			}
			base.ProcessInput(ev);
		}

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

		protected override void DrawGhost(Color ghostCol)
		{
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint);
		}

		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}

		[CompilerGenerated]
		private sealed class <CanDesignateCell>c__AnonStorey0
		{
			internal IntVec3 c;

			internal Designator_Install $this;

			public <CanDesignateCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.Position == this.c && x.Rotation == this.$this.placingRot && x.def == this.$this.PlacingDef;
			}
		}
	}
}
