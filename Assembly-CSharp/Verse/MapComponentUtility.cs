using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C35 RID: 3125
	public static class MapComponentUtility
	{
		// Token: 0x060044F5 RID: 17653 RVA: 0x00244A50 File Offset: 0x00242E50
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

		// Token: 0x060044F6 RID: 17654 RVA: 0x00244AB8 File Offset: 0x00242EB8
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

		// Token: 0x060044F7 RID: 17655 RVA: 0x00244B20 File Offset: 0x00242F20
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

		// Token: 0x060044F8 RID: 17656 RVA: 0x00244B88 File Offset: 0x00242F88
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

		// Token: 0x060044F9 RID: 17657 RVA: 0x00244BF0 File Offset: 0x00242FF0
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

		// Token: 0x060044FA RID: 17658 RVA: 0x00244C58 File Offset: 0x00243058
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
