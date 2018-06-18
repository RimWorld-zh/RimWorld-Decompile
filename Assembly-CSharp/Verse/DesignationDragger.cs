using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E13 RID: 3603
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x0029D138 File Offset: 0x0029B538
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x0029D154 File Offset: 0x0029B554
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x0029D174 File Offset: 0x0029B574
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x0029D198 File Offset: 0x0029B598
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x0029D1B9 File Offset: 0x0029B5B9
		public void StartDrag()
		{
			this.dragging = true;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x0029D1CE File Offset: 0x0029B5CE
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

		// Token: 0x06005193 RID: 20883 RVA: 0x0029D20C File Offset: 0x0029B60C
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

		// Token: 0x06005194 RID: 20884 RVA: 0x0029D304 File Offset: 0x0029B704
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

		// Token: 0x06005195 RID: 20885 RVA: 0x0029D444 File Offset: 0x0029B844
		private void DrawNumber(Vector2 screenPos, int number)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			rect.y += 3f;
			Widgets.Label(rect, number.ToStringCached());
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x0029D4B4 File Offset: 0x0029B8B4
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

		// Token: 0x06005197 RID: 20887 RVA: 0x0029D7C8 File Offset: 0x0029BBC8
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

		// Token: 0x04003571 RID: 13681
		private bool dragging = false;

		// Token: 0x04003572 RID: 13682
		private IntVec3 startDragCell;

		// Token: 0x04003573 RID: 13683
		private int lastFrameDragCellsDrawn = 0;

		// Token: 0x04003574 RID: 13684
		private Sustainer sustainer;

		// Token: 0x04003575 RID: 13685
		private float lastDragRealTime = -1000f;

		// Token: 0x04003576 RID: 13686
		private List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04003577 RID: 13687
		private string failureReasonInt = null;

		// Token: 0x04003578 RID: 13688
		private int lastUpdateFrame = -1;

		// Token: 0x04003579 RID: 13689
		private const int MaxSquareWidth = 50;
	}
}
