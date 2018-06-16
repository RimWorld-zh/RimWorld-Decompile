using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E15 RID: 3605
	public abstract class Designator : Command
	{
		// Token: 0x0600519A RID: 20890 RVA: 0x001730C8 File Offset: 0x001714C8
		public Designator()
		{
			this.activateSound = SoundDefOf.SelectDesignator;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x00173120 File Offset: 0x00171520
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x0600519C RID: 20892 RVA: 0x0017313C File Offset: 0x0017153C
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x00173154 File Offset: 0x00171554
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x0600519E RID: 20894 RVA: 0x0017316C File Offset: 0x0017156C
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x0600519F RID: 20895 RVA: 0x00173184 File Offset: 0x00171584
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060051A0 RID: 20896 RVA: 0x0017319C File Offset: 0x0017159C
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x001731B8 File Offset: 0x001715B8
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

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x00173208 File Offset: 0x00171608
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

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x00173258 File Offset: 0x00171658
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

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060051A4 RID: 20900 RVA: 0x001732A0 File Offset: 0x001716A0
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

		// Token: 0x060051A5 RID: 20901 RVA: 0x001732CC File Offset: 0x001716CC
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00173308 File Offset: 0x00171708
		public override void ProcessInput(Event ev)
		{
			if (this.CheckCanInteract())
			{
				base.ProcessInput(ev);
				Find.DesignatorManager.Select(this);
			}
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00173330 File Offset: 0x00171730
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x0017334A File Offset: 0x0017174A
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051A9 RID: 20905
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x060051AA RID: 20906 RVA: 0x00173354 File Offset: 0x00171754
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

		// Token: 0x060051AB RID: 20907 RVA: 0x00173420 File Offset: 0x00171820
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x00173428 File Offset: 0x00171828
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x0017343E File Offset: 0x0017183E
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

		// Token: 0x060051AE RID: 20910 RVA: 0x00173458 File Offset: 0x00171858
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x00173474 File Offset: 0x00171874
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

		// Token: 0x060051B0 RID: 20912 RVA: 0x001734C8 File Offset: 0x001718C8
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001734E4 File Offset: 0x001718E4
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x00173500 File Offset: 0x00171900
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x00173530 File Offset: 0x00171930
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x00173548 File Offset: 0x00171948
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null);
			}
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00173586 File Offset: 0x00171986
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00173589 File Offset: 0x00171989
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x0017358C File Offset: 0x0017198C
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x0017358F File Offset: 0x0017198F
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00173592 File Offset: 0x00171992
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x00173598 File Offset: 0x00171998
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001735AE File Offset: 0x001719AE
		public virtual void Selected()
		{
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001735B1 File Offset: 0x001719B1
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x0400357C RID: 13692
		protected bool useMouseIcon = false;

		// Token: 0x0400357D RID: 13693
		public SoundDef soundDragSustain = null;

		// Token: 0x0400357E RID: 13694
		public SoundDef soundDragChanged = null;

		// Token: 0x0400357F RID: 13695
		protected SoundDef soundSucceeded = null;

		// Token: 0x04003580 RID: 13696
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04003581 RID: 13697
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04003582 RID: 13698
		protected string designateAllLabel;

		// Token: 0x04003583 RID: 13699
		private string cachedTutorTagSelect;

		// Token: 0x04003584 RID: 13700
		private string cachedTutorTagDesignate;

		// Token: 0x04003585 RID: 13701
		protected string cachedHighlightTag;
	}
}
