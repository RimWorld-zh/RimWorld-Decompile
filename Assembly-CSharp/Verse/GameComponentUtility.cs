using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC6 RID: 3014
	public static class GameComponentUtility
	{
		// Token: 0x060041A1 RID: 16801 RVA: 0x0022A43C File Offset: 0x0022883C
		public static void GameComponentUpdate()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x0022A4A8 File Offset: 0x002288A8
		public static void GameComponentTick()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x0022A514 File Offset: 0x00228914
		public static void GameComponentOnGUI()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x0022A580 File Offset: 0x00228980
		public static void FinalizeInit()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x0022A5EC File Offset: 0x002289EC
		public static void StartedNewGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].StartedNewGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x0022A658 File Offset: 0x00228A58
		public static void LoadedGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].LoadedGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}
	}
}
