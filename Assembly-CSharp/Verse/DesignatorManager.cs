using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E17 RID: 3607
	public class DesignatorManager
	{
		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060051C3 RID: 20931 RVA: 0x0029DACC File Offset: 0x0029BECC
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x0029DAE8 File Offset: 0x0029BEE8
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x0029DB03 File Offset: 0x0029BF03
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x0029DB1E File Offset: 0x0029BF1E
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x0029DB40 File Offset: 0x0029BF40
		private bool CheckSelectedDesignatorValid()
		{
			bool result;
			if (this.selectedDesignator == null)
			{
				result = false;
			}
			else if (!this.selectedDesignator.CanRemainSelected())
			{
				this.Deselect();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x0029DB88 File Offset: 0x0029BF88
		public void ProcessInputEvents()
		{
			if (this.CheckSelectedDesignatorValid())
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					if (this.selectedDesignator.DraggableDimensions == 0)
					{
						Designator designator = this.selectedDesignator;
						AcceptanceReport acceptanceReport = this.selectedDesignator.CanDesignateCell(UI.MouseCell());
						if (acceptanceReport.Accepted)
						{
							designator.DesignateSingleCell(UI.MouseCell());
							designator.Finalize(true);
						}
						else
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.SilentInput, false);
							this.selectedDesignator.Finalize(false);
						}
					}
					else
					{
						this.dragger.StartDrag();
					}
					Event.current.Use();
				}
				if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.Deselect();
					this.dragger.EndDrag();
					Event.current.Use();
					TutorSystem.Notify_Event("ClearDesignatorSelection");
				}
				if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
				{
					if (this.dragger.Dragging)
					{
						this.selectedDesignator.DesignateMultiCell(this.dragger.DragCells);
						this.dragger.EndDrag();
						Event.current.Use();
					}
				}
			}
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x0029DD14 File Offset: 0x0029C114
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x0029DD38 File Offset: 0x0029C138
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x0400358A RID: 13706
		private Designator selectedDesignator;

		// Token: 0x0400358B RID: 13707
		private DesignationDragger dragger = new DesignationDragger();
	}
}
