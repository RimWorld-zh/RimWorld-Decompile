using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E13 RID: 3603
	public class DesignatorManager
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060051D5 RID: 20949 RVA: 0x0029F08C File Offset: 0x0029D48C
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x0029F0A8 File Offset: 0x0029D4A8
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x0029F0C3 File Offset: 0x0029D4C3
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x0029F0DE File Offset: 0x0029D4DE
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x0029F100 File Offset: 0x0029D500
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

		// Token: 0x060051DA RID: 20954 RVA: 0x0029F148 File Offset: 0x0029D548
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

		// Token: 0x060051DB RID: 20955 RVA: 0x0029F2D4 File Offset: 0x0029D6D4
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x0029F2F8 File Offset: 0x0029D6F8
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}

		// Token: 0x0400358F RID: 13711
		private Designator selectedDesignator;

		// Token: 0x04003590 RID: 13712
		private DesignationDragger dragger = new DesignationDragger();
	}
}
