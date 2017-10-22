using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		protected abstract bool ShouldSkip(WorldObject worldObject);

		public override IEnumerable Regenerate()
		{
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allObjects.Count; i++)
			{
				WorldObject o = allObjects[i];
				if (!o.def.useDynamicDrawer && !this.ShouldSkip(o))
				{
					Material mat = o.Material;
					if ((Object)mat == (Object)null)
					{
						Log.ErrorOnce("World object " + o + " returned null material.", Gen.HashCombineInt(1948576891, o.ID));
					}
					else
					{
						LayerSubMesh subMesh = base.GetSubMesh(mat);
						Rand.PushState();
						Rand.Seed = o.ID;
						o.Print(subMesh);
						Rand.PopState();
					}
				}
			}
			base.FinalizeMesh(MeshParts.All, false);
		}
	}
}
