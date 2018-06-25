using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public class MainButtonsRoot
	{
		// Token: 0x04001A79 RID: 6777
		public MainTabsRoot tabs = new MainTabsRoot();

		// Token: 0x04001A7A RID: 6778
		private List<MainButtonDef> allButtonsInOrder;

		// Token: 0x060030FC RID: 12540 RVA: 0x001A9E88 File Offset: 0x001A8288
		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x001A9EDC File Offset: 0x001A82DC
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

		// Token: 0x060030FE RID: 12542 RVA: 0x001A9F2C File Offset: 0x001A832C
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

		// Token: 0x060030FF RID: 12543 RVA: 0x001A9FEC File Offset: 0x001A83EC
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

		// Token: 0x06003100 RID: 12544 RVA: 0x001AA054 File Offset: 0x001A8454
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
	}
}
