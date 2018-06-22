using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005B2 RID: 1458
	public static class WorldComponentUtility
	{
		// Token: 0x06001C05 RID: 7173 RVA: 0x000F0F6C File Offset: 0x000EF36C
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

		// Token: 0x06001C06 RID: 7174 RVA: 0x000F0FD4 File Offset: 0x000EF3D4
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

		// Token: 0x06001C07 RID: 7175 RVA: 0x000F103C File Offset: 0x000EF43C
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
