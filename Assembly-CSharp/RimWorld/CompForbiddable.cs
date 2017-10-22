using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompForbiddable : ThingComp
	{
		private bool forbiddenInt = false;

		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value != this.forbiddenInt)
				{
					this.forbiddenInt = value;
					if (base.parent.Spawned)
					{
						if (this.forbiddenInt)
						{
							base.parent.Map.listerHaulables.Notify_Forbidden(base.parent);
						}
						else
						{
							base.parent.Map.listerHaulables.Notify_Unforbidden(base.parent);
						}
						if (base.parent is Building_Door)
						{
							base.parent.Map.reachability.ClearCache();
						}
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
				if (base.parent is Blueprint || base.parent is Frame)
				{
					if (base.parent.def.size.x > 1 || base.parent.def.size.z > 1)
					{
						base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.ForbiddenBig);
					}
					else
					{
						base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.Forbidden);
					}
				}
				else if (base.parent.def.category == ThingCategory.Building)
				{
					base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.ForbiddenBig);
				}
				else
				{
					base.parent.Map.overlayDrawer.DrawOverlay(base.parent, OverlayTypes.Forbidden);
				}
			}
		}

		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (base.parent is Building && base.parent.Faction != Faction.OfPlayer)
				yield break;
			Command_Toggle com = new Command_Toggle
			{
				hotKey = KeyBindingDefOf.CommandItemForbid,
				icon = TexCommand.Forbidden,
				isActive = (Func<bool>)(() => !((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0087: stateMachine*/)._0024this.forbiddenInt),
				defaultLabel = "CommandForbid".Translate()
			};
			if (this.forbiddenInt)
			{
				com.defaultDesc = "CommandForbiddenDesc".Translate();
			}
			else
			{
				com.defaultDesc = "CommandNotForbiddenDesc".Translate();
			}
			if (base.parent.def.IsDoor)
			{
				com.tutorTag = "ToggleForbidden-Door";
				com.toggleAction = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011d: stateMachine*/)._0024this.Forbidden = !((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_011d: stateMachine*/)._0024this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
				};
			}
			else
			{
				com.tutorTag = "ToggleForbidden";
				com.toggleAction = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_014b: stateMachine*/)._0024this.Forbidden = !((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_014b: stateMachine*/)._0024this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
				};
			}
			yield return (Gizmo)com;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
