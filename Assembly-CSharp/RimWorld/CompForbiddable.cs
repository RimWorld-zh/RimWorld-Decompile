using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000714 RID: 1812
	public class CompForbiddable : ThingComp
	{
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x001542EC File Offset: 0x001526EC
		// (set) Token: 0x060027C4 RID: 10180 RVA: 0x00154308 File Offset: 0x00152708
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
					if (this.parent.Spawned)
					{
						if (this.forbiddenInt)
						{
							this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
							this.parent.Map.listerMergeables.Notify_Forbidden(this.parent);
						}
						else
						{
							this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
							this.parent.Map.listerMergeables.Notify_Unforbidden(this.parent);
						}
						if (this.parent is Building_Door)
						{
							this.parent.Map.reachability.ClearCache();
						}
					}
				}
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x001543E5 File Offset: 0x001527E5
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x001543FC File Offset: 0x001527FC
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

		// Token: 0x060027C7 RID: 10183 RVA: 0x00154506 File Offset: 0x00152906
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00154518 File Offset: 0x00152918
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent is Building && this.parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			Command_Toggle com = new Command_Toggle();
			com.hotKey = KeyBindingDefOf.Command_ItemForbid;
			com.icon = TexCommand.Forbidden;
			com.isActive = (() => !this.forbiddenInt);
			com.defaultLabel = "CommandForbid".Translate();
			if (this.forbiddenInt)
			{
				com.defaultDesc = "CommandForbiddenDesc".Translate();
			}
			else
			{
				com.defaultDesc = "CommandNotForbiddenDesc".Translate();
			}
			if (this.parent.def.IsDoor)
			{
				com.tutorTag = "ToggleForbidden-Door";
				com.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
				};
			}
			else
			{
				com.tutorTag = "ToggleForbidden";
				com.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
				};
			}
			yield return com;
			yield break;
		}

		// Token: 0x040015DE RID: 5598
		private bool forbiddenInt = false;
	}
}
