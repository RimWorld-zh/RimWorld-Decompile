using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058A RID: 1418
	public class WorldDynamicDrawManager
	{
		// Token: 0x04000FF4 RID: 4084
		private HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x04000FF5 RID: 4085
		private bool drawingNow;

		// Token: 0x06001B10 RID: 6928 RVA: 0x000E87F4 File Offset: 0x000E6BF4
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

		// Token: 0x06001B11 RID: 6929 RVA: 0x000E8844 File Offset: 0x000E6C44
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

		// Token: 0x06001B12 RID: 6930 RVA: 0x000E8894 File Offset: 0x000E6C94
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
	}
}
