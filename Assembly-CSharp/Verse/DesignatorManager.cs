using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E15 RID: 3605
	public class DesignatorManager
	{
		// Token: 0x0400358F RID: 13711
		private Designator selectedDesignator;

		// Token: 0x04003590 RID: 13712
		private DesignationDragger dragger = new DesignationDragger();

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x0029F1B8 File Offset: 0x0029D5B8
		public Designator SelectedDesignator
		{
			get
			{
				return this.selectedDesignator;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x0029F1D4 File Offset: 0x0029D5D4
		public DesignationDragger Dragger
		{
			get
			{
				return this.dragger;
			}
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x0029F1EF File Offset: 0x0029D5EF
		public void Select(Designator des)
		{
			this.Deselect();
			this.selectedDesignator = des;
			this.selectedDesignator.Selected();
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x0029F20A File Offset: 0x0029D60A
		public void Deselect()
		{
			if (this.selectedDesignator != null)
			{
				this.selectedDesignator = null;
				this.dragger.EndDrag();
			}
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x0029F22C File Offset: 0x0029D62C
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

		// Token: 0x060051DE RID: 20958 RVA: 0x0029F274 File Offset: 0x0029D674
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

		// Token: 0x060051DF RID: 20959 RVA: 0x0029F400 File Offset: 0x0029D800
		public void DesignationManagerOnGUI()
		{
			this.dragger.DraggerOnGUI();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.DrawMouseAttachments();
			}
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x0029F424 File Offset: 0x0029D824
		public void DesignatorManagerUpdate()
		{
			this.dragger.DraggerUpdate();
			if (this.CheckSelectedDesignatorValid())
			{
				this.selectedDesignator.SelectedUpdate();
			}
		}
	}
}
