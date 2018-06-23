using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E11 RID: 3601
	public abstract class Designator : Command
	{
		// Token: 0x04003581 RID: 13697
		protected bool useMouseIcon = false;

		// Token: 0x04003582 RID: 13698
		public SoundDef soundDragSustain = null;

		// Token: 0x04003583 RID: 13699
		public SoundDef soundDragChanged = null;

		// Token: 0x04003584 RID: 13700
		protected SoundDef soundSucceeded = null;

		// Token: 0x04003585 RID: 13701
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04003586 RID: 13702
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04003587 RID: 13703
		protected string designateAllLabel;

		// Token: 0x04003588 RID: 13704
		private string cachedTutorTagSelect;

		// Token: 0x04003589 RID: 13705
		private string cachedTutorTagDesignate;

		// Token: 0x0400358A RID: 13706
		protected string cachedHighlightTag;

		// Token: 0x060051AC RID: 20908 RVA: 0x00173334 File Offset: 0x00171734
		public Designator()
		{
			this.activateSound = SoundDefOf.SelectDesignator;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x060051AD RID: 20909 RVA: 0x0017338C File Offset: 0x0017178C
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060051AE RID: 20910 RVA: 0x001733A8 File Offset: 0x001717A8
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060051AF RID: 20911 RVA: 0x001733C0 File Offset: 0x001717C0
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060051B0 RID: 20912 RVA: 0x001733D8 File Offset: 0x001717D8
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060051B1 RID: 20913 RVA: 0x001733F0 File Offset: 0x001717F0
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060051B2 RID: 20914 RVA: 0x00173408 File Offset: 0x00171808
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060051B3 RID: 20915 RVA: 0x00173424 File Offset: 0x00171824
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

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060051B4 RID: 20916 RVA: 0x00173474 File Offset: 0x00171874
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

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060051B5 RID: 20917 RVA: 0x001734C4 File Offset: 0x001718C4
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

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060051B6 RID: 20918 RVA: 0x0017350C File Offset: 0x0017190C
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

		// Token: 0x060051B7 RID: 20919 RVA: 0x00173538 File Offset: 0x00171938
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x00173574 File Offset: 0x00171974
		public override void ProcessInput(Event ev)
		{
			if (this.CheckCanInteract())
			{
				base.ProcessInput(ev);
				Find.DesignatorManager.Select(this);
			}
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x0017359C File Offset: 0x0017199C
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001735B6 File Offset: 0x001719B6
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051BB RID: 20923
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x060051BC RID: 20924 RVA: 0x001735C0 File Offset: 0x001719C0
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

		// Token: 0x060051BD RID: 20925 RVA: 0x0017368C File Offset: 0x00171A8C
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00173694 File Offset: 0x00171A94
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x001736AA File Offset: 0x00171AAA
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

		// Token: 0x060051C0 RID: 20928 RVA: 0x001736C4 File Offset: 0x00171AC4
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x001736E0 File Offset: 0x00171AE0
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

		// Token: 0x060051C2 RID: 20930 RVA: 0x00173734 File Offset: 0x00171B34
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00173750 File Offset: 0x00171B50
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x0017376C File Offset: 0x00171B6C
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x0017379C File Offset: 0x00171B9C
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x001737B4 File Offset: 0x00171BB4
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null);
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x001737F2 File Offset: 0x00171BF2
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001737F5 File Offset: 0x00171BF5
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001737F8 File Offset: 0x00171BF8
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x001737FB File Offset: 0x00171BFB
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001737FE File Offset: 0x00171BFE
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00173804 File Offset: 0x00171C04
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x0017381A File Offset: 0x00171C1A
		public virtual void Selected()
		{
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x0017381D File Offset: 0x00171C1D
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}
	}
}
