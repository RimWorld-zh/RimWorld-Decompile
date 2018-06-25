using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BC5 RID: 3013
	public static class GameComponentUtility
	{
		// Token: 0x060041A1 RID: 16801 RVA: 0x0022A15C File Offset: 0x0022855C
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

		// Token: 0x060041A2 RID: 16802 RVA: 0x0022A1C8 File Offset: 0x002285C8
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

		// Token: 0x060041A3 RID: 16803 RVA: 0x0022A234 File Offset: 0x00228634
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

		// Token: 0x060041A4 RID: 16804 RVA: 0x0022A2A0 File Offset: 0x002286A0
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

		// Token: 0x060041A5 RID: 16805 RVA: 0x0022A30C File Offset: 0x0022870C
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

		// Token: 0x060041A6 RID: 16806 RVA: 0x0022A378 File Offset: 0x00228778
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
