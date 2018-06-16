using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058C RID: 1420
	public class WorldDynamicDrawManager
	{
		// Token: 0x06001B14 RID: 6932 RVA: 0x000E85E4 File Offset: 0x000E69E4
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + o + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.", false);
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000E8634 File Offset: 0x000E6A34
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + o + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.", false);
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000E8684 File Offset: 0x000E6A84
		public void DrawDynamicWorldObjects()
		{
			this.drawingNow = true;
			try
			{
				foreach (WorldObject worldObject in this.drawObjects)
				{
					try
					{
						if (!worldObject.def.expandingIcon || ExpandableWorldObjectsUtility.TransitionPct < 1f)
						{
							worldObject.Draw();
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception drawing ",
							worldObject,
							": ",
							ex
						}), false);
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Exception drawing dynamic world objects: " + arg, false);
			}
			this.drawingNow = false;
		}

		// Token: 0x04000FF7 RID: 4087
		private HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x04000FF8 RID: 4088
		private bool drawingNow;
	}
}
