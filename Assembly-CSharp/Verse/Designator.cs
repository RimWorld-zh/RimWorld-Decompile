using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Designator : Command
	{
		protected bool useMouseIcon;

		public SoundDef soundDragSustain;

		public SoundDef soundDragChanged;

		protected SoundDef soundSucceeded;

		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		protected bool hasDesignateAllFloatMenuOption;

		protected string designateAllLabel;

		private string cachedTutorTagSelect;

		private string cachedTutorTagDesignate;

		protected string cachedHighlightTag;

		public Designator()
		{
			this.activateSound = SoundDefOf.SelectDesignator;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagSelect == null)
				{
					this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag;
				}
				return this.cachedTutorTagSelect;
			}
		}

		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagDesignate == null)
				{
					this.cachedTutorTagDesignate = "Designate-" + this.tutorTag;
				}
				return this.cachedTutorTagDesignate;
			}
		}

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

		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption o in base.RightClickFloatMenuOptions)
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

		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
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

		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

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

		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

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

		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, string.Empty, this.iconAngle, this.iconOffset, null);
			}
		}

		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		public virtual void SelectedUpdate()
		{
		}

		public virtual void SelectedProcessInput(Event ev)
		{
		}

		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		public virtual bool CanRemainSelected()
		{
			return true;
		}

		public virtual void Selected()
		{
		}

		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<FloatMenuOption> <get_RightClickFloatMenuOptions>__BaseCallProxy0()
		{
			return base.RightClickFloatMenuOptions;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <o>__1;

			internal int <count>__2;

			internal int <count>__3;

			internal Designator $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			private Designator.<>c__Iterator0.<>c__AnonStorey3 $locvar1;

			private Designator.<>c__Iterator0.<>c__AnonStorey1 $locvar2;

			private Designator.<>c__Iterator0.<>c__AnonStorey2 $locvar3;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<get_RightClickFloatMenuOptions>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_25C;
				case 3u:
					goto IL_25C;
				case 4u:
					goto IL_420;
				case 5u:
					goto IL_420;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						o = enumerator.Current;
						this.$current = o;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.hasDesignateAllFloatMenuOption)
				{
					count = 0;
					List<Thing> things = base.Map.listerThings.AllThings;
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
						this.$current = new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(new object[]
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
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					this.$current = new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_25C:
				<>c__AnonStorey.designation = this.Designation;
				if (this.Designation != null)
				{
					count2 = 0;
					List<Designation> designations = base.Map.designationManager.allDesignations;
					for (int j = 0; j < designations.Count; j++)
					{
						if (designations[j].def == <>c__AnonStorey.designation && this.RemoveAllDesignationsAffects(designations[j].target))
						{
							count2++;
						}
					}
					if (count2 > 0)
					{
						this.$current = new FloatMenuOption(string.Concat(new object[]
						{
							"RemoveAllDesignations".Translate(),
							" (",
							count2,
							")"
						}), delegate()
						{
							for (int k = designations.Count - 1; k >= 0; k--)
							{
								if (designations[k].def == <>c__AnonStorey.designation && this.RemoveAllDesignationsAffects(designations[k].target))
								{
									this.Map.designationManager.RemoveDesignation(designations[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					this.$current = new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_420:
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Designator.<>c__Iterator0 <>c__Iterator = new Designator.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}

			private sealed class <>c__AnonStorey3
			{
				internal DesignationDef designation;

				internal Designator.<>c__Iterator0 <>f__ref$0;

				public <>c__AnonStorey3()
				{
				}
			}

			private sealed class <>c__AnonStorey1
			{
				internal List<Thing> things;

				internal Designator.<>c__Iterator0 <>f__ref$0;

				public <>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					for (int i = 0; i < this.things.Count; i++)
					{
						Thing t = this.things[i];
						if (!t.Fogged() && this.<>f__ref$0.$this.CanDesignateThing(t).Accepted)
						{
							this.<>f__ref$0.$this.DesignateThing(this.things[i]);
						}
					}
				}
			}

			private sealed class <>c__AnonStorey2
			{
				internal List<Designation> designations;

				internal Designator.<>c__Iterator0 <>f__ref$0;

				internal Designator.<>c__Iterator0.<>c__AnonStorey3 <>f__ref$3;

				public <>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					for (int i = this.designations.Count - 1; i >= 0; i--)
					{
						if (this.designations[i].def == this.<>f__ref$3.designation && this.<>f__ref$0.$this.RemoveAllDesignationsAffects(this.designations[i].target))
						{
							this.<>f__ref$0.$this.Map.designationManager.RemoveDesignation(this.designations[i]);
						}
					}
				}
			}
		}
	}
}
