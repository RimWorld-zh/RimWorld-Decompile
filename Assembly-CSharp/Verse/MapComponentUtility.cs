using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C37 RID: 3127
	public static class MapComponentUtility
	{
		// Token: 0x060044F8 RID: 17656 RVA: 0x00244B2C File Offset: 0x00242F2C
		public static void MapComponentUpdate(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00244B94 File Offset: 0x00242F94
		public static void MapComponentTick(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x00244BFC File Offset: 0x00242FFC
		public static void MapComponentOnGUI(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x00244C64 File Offset: 0x00243064
		public static void FinalizeInit(Map map)
		{
			List<MapComponent> components = map.components;
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

		// Token: 0x060044FC RID: 17660 RVA: 0x00244CCC File Offset: 0x002430CC
		public static void MapGenerated(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapGenerated();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00244D34 File Offset: 0x00243134
		public static void MapRemoved(Map map)
		{
			List<MapComponent> components = map.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].MapRemoved();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}
	}
}
