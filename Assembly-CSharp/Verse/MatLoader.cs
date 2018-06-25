using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F66 RID: 3942
	public static class MatLoader
	{
		// Token: 0x04003E7C RID: 15996
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x06005F58 RID: 24408 RVA: 0x00309F00 File Offset: 0x00308300
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath, false);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x02000F67 RID: 3943
		private struct Request
		{
			// Token: 0x04003E7D RID: 15997
			public string path;

			// Token: 0x04003E7E RID: 15998
			public int renderQueue;

			// Token: 0x06005F5A RID: 24410 RVA: 0x00309FB0 File Offset: 0x003083B0
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			// Token: 0x06005F5B RID: 24411 RVA: 0x00309FE4 File Offset: 0x003083E4
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005F5C RID: 24412 RVA: 0x0030A018 File Offset: 0x00308418
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005F5D RID: 24413 RVA: 0x0030A058 File Offset: 0x00308458
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005F5E RID: 24414 RVA: 0x0030A078 File Offset: 0x00308478
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005F5F RID: 24415 RVA: 0x0030A098 File Offset: 0x00308498
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue,
					")"
				});
			}
		}
	}
}
