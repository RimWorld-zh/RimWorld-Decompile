using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class MainButtonsRoot
	{
		public MainTabsRoot tabs = new MainTabsRoot();

		private List<MainButtonDef> allButtonsInOrder;

		[CompilerGenerated]
		private static Func<MainButtonDef, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<MainButtonDef> <>f__am$cache1;

		public MainButtonsRoot()
		{
			this.allButtonsInOrder = (from x in DefDatabase<MainButtonDef>.AllDefs
			orderby x.order
			select x).ToList<MainButtonDef>();
		}

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

		public void MainButtonsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
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

		public void HandleLowPriorityShortcuts()
		{
			this.tabs.HandleLowPriorityShortcuts();
			if (WorldRendererUtility.WorldRenderedNow && Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null && KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Event.current.Use();
				Find.World.renderer.wantedMode = WorldRenderMode.None;
			}
		}

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

		[CompilerGenerated]
		private static int <MainButtonsRoot>m__0(MainButtonDef x)
		{
			return x.order;
		}

		[CompilerGenerated]
		private static bool <DoButtons>m__1(MainButtonDef x)
		{
			return x.buttonVisible;
		}
	}
}
