using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E10 RID: 3600
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x04003578 RID: 13688
		private bool dragging = false;

		// Token: 0x04003579 RID: 13689
		private IntVec3 startDragCell;

		// Token: 0x0400357A RID: 13690
		private int lastFrameDragCellsDrawn = 0;

		// Token: 0x0400357B RID: 13691
		private Sustainer sustainer;

		// Token: 0x0400357C RID: 13692
		private float lastDragRealTime = -1000f;

		// Token: 0x0400357D RID: 13693
		private List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x0400357E RID: 13694
		private string failureReasonInt = null;

		// Token: 0x0400357F RID: 13695
		private int lastUpdateFrame = -1;

		// Token: 0x04003580 RID: 13696
		private const int MaxSquareWidth = 50;

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x0029E718 File Offset: 0x0029CB18
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x060051A2 RID: 20898 RVA: 0x0029E734 File Offset: 0x0029CB34
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060051A3 RID: 20899 RVA: 0x0029E754 File Offset: 0x0029CB54
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060051A4 RID: 20900 RVA: 0x0029E778 File Offset: 0x0029CB78
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x0029E799 File Offset: 0x0029CB99
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x0029E7AE File Offset: 0x0029CBAE
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x0029E7EC File Offset: 0x0029CBEC
		public void DraggerUpdate()
		{
			if (this.dragging)
			{
				List<IntVec3> list = this.DragCells;
				this.SelDes.RenderHighlight(list);
				if (list.Count != this.lastFrameDragCellsDrawn)
				{
					this.lastDragRealTime = Time.realtimeSinceStartup;
					this.lastFrameDragCellsDrawn = list.Count;
					if (this.SelDes.soundDragChanged != null)
					{
						this.SelDes.soundDragChanged.PlayOneShotOnCamera(null);
					}
				}
				if (this.sustainer == null || this.sustainer.Ended)
				{
					if (this.SelDes.soundDragSustain != null)
					{
						this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
					}
				}
				else
				{
					this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
					this.sustainer.Maintain();
				}
			}
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x0029E8E4 File Offset: 0x0029CCE4
		public void DraggerOnGUI()
		{
			if (this.dragging && this.SelDes != null && this.SelDes.DragDrawMeasurements)
			{
				IntVec3 intVec = this.startDragCell - UI.MouseCell();
				intVec.x = Mathf.Abs(intVec.x) + 1;
				intVec.z = Mathf.Abs(intVec.z) + 1;
				if (intVec.x >= 3)
				{
					Vector2 screenPos = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos.y = this.startDragCell.ToUIPosition().y;
					this.DrawNumber(screenPos, intVec.x);
				}
				if (intVec.z >= 3)
				{
					Vector2 screenPos2 = (this.startDragCell.ToUIPosition() + UI.MouseCell().ToUIPosition()) / 2f;
					screenPos2.x = this.startDragCell.ToUIPosition().x;
					this.DrawNumber(screenPos2, intVec.z);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x0029EA24 File Offset: 0x0029CE24
		private void DrawNumber(Vector2 screenPos, int number)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			rect.y += 3f;
			Widgets.Label(rect, number.ToStringCached());
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x0029EA94 File Offset: 0x0029CE94
		private void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount != this.lastUpdateFrame)
			{
				this.lastUpdateFrame = Time.frameCount;
				this.dragCells.Clear();
				this.failureReasonInt = null;
				IntVec3 intVec = this.startDragCell;
				IntVec3 intVec2 = UI.MouseCell();
				if (this.SelDes.DraggableDimensions == 1)
				{
					bool flag = true;
					if (Mathf.Abs(intVec.x - intVec2.x) < Mathf.Abs(intVec.z - intVec2.z))
					{
						flag = false;
					}
					if (flag)
					{
						int z = intVec.z;
						if (intVec.x > intVec2.x)
						{
							IntVec3 intVec3 = intVec;
							intVec = intVec2;
							intVec2 = intVec3;
						}
						for (int i = intVec.x; i <= intVec2.x; i++)
						{
							this.TryAddDragCell(new IntVec3(i, intVec.y, z));
						}
					}
					else
					{
						int x = intVec.x;
						if (intVec.z > intVec2.z)
						{
							IntVec3 intVec4 = intVec;
							intVec = intVec2;
							intVec2 = intVec4;
						}
						for (int j = intVec.z; j <= intVec2.z; j++)
						{
							this.TryAddDragCell(new IntVec3(x, intVec.y, j));
						}
					}
				}
				if (this.SelDes.DraggableDimensions == 2)
				{
					IntVec3 intVec5 = intVec;
					IntVec3 intVec6 = intVec2;
					if (intVec6.x > intVec5.x + 50)
					{
						intVec6.x = intVec5.x + 50;
					}
					if (intVec6.z > intVec5.z + 50)
					{
						intVec6.z = intVec5.z + 50;
					}
					if (intVec6.x < intVec5.x)
					{
						if (intVec6.x < intVec5.x - 50)
						{
							intVec6.x = intVec5.x - 50;
						}
						int x2 = intVec5.x;
						intVec5 = new IntVec3(intVec6.x, intVec5.y, intVec5.z);
						intVec6 = new IntVec3(x2, intVec6.y, intVec6.z);
					}
					if (intVec6.z < intVec5.z)
					{
						if (intVec6.z < intVec5.z - 50)
						{
							intVec6.z = intVec5.z - 50;
						}
						int z2 = intVec5.z;
						intVec5 = new IntVec3(intVec5.x, intVec5.y, intVec6.z);
						intVec6 = new IntVec3(intVec6.x, intVec6.y, z2);
					}
					for (int k = intVec5.x; k <= intVec6.x; k++)
					{
						for (int l = intVec5.z; l <= intVec6.z; l++)
						{
							this.TryAddDragCell(new IntVec3(k, intVec5.y, l));
						}
					}
				}
			}
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x0029EDA8 File Offset: 0x0029D1A8
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
			}
			else if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}
	}
}
