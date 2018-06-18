using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200087B RID: 2171
	public class MainTabsRoot
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x001AD548 File Offset: 0x001AB948
		public MainButtonDef OpenTab
		{
			get
			{
				MainTabWindow mainTabWindow = Find.WindowStack.WindowOfType<MainTabWindow>();
				MainButtonDef result;
				if (mainTabWindow == null)
				{
					result = null;
				}
				else
				{
					result = mainTabWindow.def;
				}
				return result;
			}
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x001AD57C File Offset: 0x001AB97C
		public void HandleLowPriorityShortcuts()
		{
			if (this.OpenTab == MainButtonDefOf.Inspect && (Find.Selector.NumSelected == 0 || WorldRendererUtility.WorldRenderedNow))
			{
				this.EscapeCurrentTab(false);
			}
			if (Find.Selector.NumSelected == 0 && Event.current.type == EventType.MouseDown && Event.current.button == 1 && !WorldRendererUtility.WorldRenderedNow)
			{
				Event.current.Use();
				MainButtonDefOf.Architect.Worker.InterfaceTryActivate();
			}
			if (this.OpenTab != null && this.OpenTab != MainButtonDefOf.Inspect)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button != 2)
				{
					this.EscapeCurrentTab(true);
					if (Event.current.button == 0)
					{
						Find.Selector.ClearSelection();
						Find.WorldSelector.ClearSelection();
					}
				}
			}
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x001AD677 File Offset: 0x001ABA77
		public void EscapeCurrentTab(bool playSound = true)
		{
			this.SetCurrentTab(null, playSound);
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001AD682 File Offset: 0x001ABA82
		public void SetCurrentTab(MainButtonDef tab, bool playSound = true)
		{
			if (tab != this.OpenTab)
			{
				this.ToggleTab(tab, playSound);
			}
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001AD6A0 File Offset: 0x001ABAA0
		public void ToggleTab(MainButtonDef newTab, bool playSound = true)
		{
			if (this.OpenTab != null || newTab != null)
			{
				if (this.OpenTab == newTab)
				{
					Find.WindowStack.TryRemove(this.OpenTab.TabWindow, true);
					if (playSound)
					{
						SoundDefOf.TabClose.PlayOneShotOnCamera(null);
					}
				}
				else
				{
					if (this.OpenTab != null)
					{
						Find.WindowStack.TryRemove(this.OpenTab.TabWindow, true);
					}
					if (newTab != null)
					{
						Find.WindowStack.Add(newTab.TabWindow);
					}
					if (playSound)
					{
						if (newTab == null)
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
						else
						{
							SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
						}
					}
					if (TutorSystem.TutorialMode && newTab != null)
					{
						TutorSystem.Notify_Event("Open-MainTab-" + newTab.defName);
					}
				}
			}
		}
	}
}
