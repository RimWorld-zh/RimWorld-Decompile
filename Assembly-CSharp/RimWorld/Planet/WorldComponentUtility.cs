using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B4 RID: 1460
	public static class WorldComponentUtility
	{
		// Token: 0x06001C09 RID: 7177 RVA: 0x000F10BC File Offset: 0x000EF4BC
		public static void WorldComponentUpdate(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000F1124 File Offset: 0x000EF524
		public static void WorldComponentTick(World world)
		{
			List<WorldComponent> components = world.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].WorldComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString(), false);
				}
			}
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000F118C File Offset: 0x000EF58C
		public static void FinalizeInit(World world)
		{
			List<WorldComponent> components = world.components;
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
	}
}
