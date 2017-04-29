using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class CompForbiddable : ThingComp
	{
		private bool forbiddenInt;

		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value == this.forbiddenInt)
				{
					return;
				}
				this.forbiddenInt = value;
				if (this.parent.Spawned)
				{
					if (this.forbiddenInt)
					{
						this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
					}
					else
					{
						this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
					}
					if (this.parent is Building_Door)
					{
						this.parent.Map.reachability.ClearCache();
					}
				}
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		public override void PostDraw()
		{
			if (this.forbiddenInt)
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
					}
					else
					{
						this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
					}
				}
				else if (this.parent.def.category == ThingCategory.Building)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenBig);
				}
				else
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.Forbidden);
				}
			}
		}

		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompForbiddable.<CompGetGizmosExtra>c__Iterator163 <CompGetGizmosExtra>c__Iterator = new CompForbiddable.<CompGetGizmosExtra>c__Iterator163();
			<CompGetGizmosExtra>c__Iterator.<>f__this = this;
			CompForbiddable.<CompGetGizmosExtra>c__Iterator163 expr_0E = <CompGetGizmosExtra>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
