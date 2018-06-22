using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000869 RID: 2153
	public class MainButtonsRoot
	{
		// Token: 0x060030F8 RID: 12536 RVA: 0x001A9D38 File Offset: 0x001A8138
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x001A9D8C File Offset: 0x001A818C
		private int VisibleButtonsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allButtonsInOrder.Count; i++)
				{
					if (this.allButtonsInOrder[i].buttonVisible)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x001A9DDC File Offset: 0x001A81DC
		public void MainButtonsOnGUI()
		{
			if (Event.current.type != EventType.Layout)
			{
				this.DoButtons();
				for (int i = 0; i < this.allButtonsInOrder.Count; i++)
				{
					if ((this.allButtonsInOrder[i].validWithoutMap || Find.CurrentMap != null) && this.allButtonsInOrder[i].hotKey != null && this.allButtonsInOrder[i].hotKey.KeyDownEvent)
					{
						Event.current.Use();
						this.allButtonsInOrder[i].Worker.InterfaceTryActivate();
						break;
					}
				}
			}
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x001A9E9C File Offset: 0x001A829C
		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldRenderedNow && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null)
			{
				if (KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					Event.current.Use();
					Find.World.renderer.wantedMode = WorldRenderMode.None;
				}
			}
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x001A9F04 File Offset: 0x001A8304
		private void DoButtons()
		{
			GUI.color = Color.white;
			int visibleButtonsCount = this.VisibleButtonsCount;
			int num = (int)((float)UI.screenWidth / (float)visibleButtonsCount);
			int num2 = this.allButtonsInOrder.FindLastIndex((MainButtonDef x) => x.buttonVisible);
			int num3 = 0;
			for (int i = 0; i < this.allButtonsInOrder.Count; i++)
			{
				if (this.allButtonsInOrder[i].buttonVisible)
				{
					int num4 = num;
					if (i == num2)
					{
						num4 = UI.screenWidth - num3;
					}
					Rect rect = new Rect((float)num3, (float)(UI.screenHeight - 35), (float)num4, 35f);
					this.allButtonsInOrder[i].Worker.DoButton(rect);
					num3 += num;
				}
			}
		}

		// Token: 0x04001A79 RID: 6777
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x04001A7A RID: 6778
		private List<MainButtonDef> allButtonsInOrder;
	}
}
