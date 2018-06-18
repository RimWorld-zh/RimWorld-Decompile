using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E14 RID: 3604
	public abstract class Designator : Command
	{
		// Token: 0x06005198 RID: 20888 RVA: 0x0017315C File Offset: 0x0017155C
		public Designator()
		{
			this.activateSound = SoundDefOf.SelectDesignator;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x001731B4 File Offset: 0x001715B4
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001731D0 File Offset: 0x001715D0
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x001731E8 File Offset: 0x001715E8
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x0600519C RID: 20892 RVA: 0x00173200 File Offset: 0x00171600
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x00173218 File Offset: 0x00171618
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x00173230 File Offset: 0x00171630
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x0017324C File Offset: 0x0017164C
		public override string TutorTagSelect
		{
			get
			{
				string result;
				if (this.tutorTag == null)
				{
					result = null;
				}
				else
				{
					if (this.cachedTutorTagSelect == null)
					{
						this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag;
					}
					result = this.cachedTutorTagSelect;
				}
				return result;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x0017329C File Offset: 0x0017169C
		public string TutorTagDesignate
		{
			get
			{
				string result;
				if (this.tutorTag == null)
				{
					result = null;
				}
				else
				{
					if (this.cachedTutorTagDesignate == null)
					{
						this.cachedTutorTagDesignate = "Designate-" + this.tutorTag;
					}
					result = this.cachedTutorTagDesignate;
				}
				return result;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x001732EC File Offset: 0x001716EC
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x00173334 File Offset: 0x00171734
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption o in this.<get_RightClickFloatMenuOptions>__BaseCallProxy0())
				{
					yield return o;
				}
				if (this.hasDesignateAllFloatMenuOption)
				{
					int count = 0;
					List<Thing> things = this.Map.listerThings.AllThings;
					for (int i = 0; i < things.Count; i++)
					{
						Thing t = things[i];
						if (!t.Fogged() && this.CanDesignateThing(t).Accepted)
						{
							count++;
						}
					}
					if (count > 0)
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(new object[]
						{
							count
						}) + ")", delegate()
						{
							for (int k = 0; k < things.Count; k++)
							{
								Thing t2 = things[k];
								if (!t2.Fogged() && this.CanDesignateThing(t2).Accepted)
								{
									this.DesignateThing(things[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				DesignationDef designation = this.Designation;
				if (this.Designation != null)
				{
					int count2 = 0;
					List<Designation> designations = this.Map.designationManager.allDesignations;
					for (int j = 0; j < designations.Count; j++)
					{
						if (designations[j].def == designation && this.RemoveAllDesignationsAffects(designations[j].target))
						{
							count2++;
						}
					}
					if (count2 > 0)
					{
						yield return new FloatMenuOption(string.Concat(new object[]
						{
							"RemoveAllDesignations".Translate(),
							" (",
							count2,
							")"
						}), delegate()
						{
							for (int k = designations.Count - 1; k >= 0; k--)
							{
								if (designations[k].def == designation && this.RemoveAllDesignationsAffects(designations[k].target))
								{
									this.Map.designationManager.RemoveDesignation(designations[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				yield break;
			}
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x00173360 File Offset: 0x00171760
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x0017339C File Offset: 0x0017179C
		public override void ProcessInput(Event ev)
		{
			if (this.CheckCanInteract())
			{
				base.ProcessInput(ev);
				Find.DesignatorManager.Select(this);
			}
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001733C4 File Offset: 0x001717C4
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001733DE File Offset: 0x001717DE
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051A7 RID: 20903
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x060051A8 RID: 20904 RVA: 0x001733E8 File Offset: 0x001717E8
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (!TutorSystem.TutorialMode || TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				bool somethingSucceeded = false;
				bool flag = false;
				foreach (IntVec3 intVec in cells)
				{
					if (this.CanDesignateCell(intVec).Accepted)
					{
						this.DesignateSingleCell(intVec);
						somethingSucceeded = true;
						if (!flag)
						{
							flag = this.ShowWarningForCell(intVec);
						}
					}
				}
				this.Finalize(somethingSucceeded);
				if (TutorSystem.TutorialMode)
				{
					TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
				}
			}
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x001734B4 File Offset: 0x001718B4
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001734BC File Offset: 0x001718BC
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001734D2 File Offset: 0x001718D2
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
			}
			else
			{
				this.FinalizeDesignationFailed();
			}
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x001734EC File Offset: 0x001718EC
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x00173508 File Offset: 0x00171908
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x0017355C File Offset: 0x0017195C
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00173578 File Offset: 0x00171978
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x00173594 File Offset: 0x00171994
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001735C4 File Offset: 0x001719C4
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001735DC File Offset: 0x001719DC
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null);
			}
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x0017361A File Offset: 0x00171A1A
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x0017361D File Offset: 0x00171A1D
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00173620 File Offset: 0x00171A20
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00173623 File Offset: 0x00171A23
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x00173626 File Offset: 0x00171A26
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x0017362C File Offset: 0x00171A2C
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00173642 File Offset: 0x00171A42
		public virtual void Selected()
		{
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00173645 File Offset: 0x00171A45
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x0400357A RID: 13690
		protected bool useMouseIcon = false;

		// Token: 0x0400357B RID: 13691
		public SoundDef soundDragSustain = null;

		// Token: 0x0400357C RID: 13692
		public SoundDef soundDragChanged = null;

		// Token: 0x0400357D RID: 13693
		protected SoundDef soundSucceeded = null;

		// Token: 0x0400357E RID: 13694
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x0400357F RID: 13695
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04003580 RID: 13696
		protected string designateAllLabel;

		// Token: 0x04003581 RID: 13697
		private string cachedTutorTagSelect;

		// Token: 0x04003582 RID: 13698
		private string cachedTutorTagDesignate;

		// Token: 0x04003583 RID: 13699
		protected string cachedHighlightTag;
	}
}
