using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059B RID: 1435
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		// Token: 0x06001B61 RID: 7009
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x06001B62 RID: 7010 RVA: 0x000EC6D4 File Offset: 0x000EAAD4
		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this.<Regenerate>__BaseCallProxy0().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allObjects.Count; i++)
			{
				WorldObject worldObject = allObjects[i];
				if (!worldObject.def.useDynamicDrawer)
				{
					if (!this.ShouldSkip(worldObject))
					{
						Material material = worldObject.Material;
						if (material == null)
						{
							Log.ErrorOnce("World object " + worldObject + " returned null material.", Gen.HashCombineInt(1948576891, worldObject.ID), false);
						}
						else
						{
							LayerSubMesh subMesh = base.GetSubMesh(material);
							Rand.PushState();
							Rand.Seed = worldObject.ID;
							worldObject.Print(subMesh);
							Rand.PopState();
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}
	}
}
