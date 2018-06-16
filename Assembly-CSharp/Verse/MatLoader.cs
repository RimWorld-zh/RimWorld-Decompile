using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F63 RID: 3939
	public static class MatLoader
	{
		// Token: 0x06005F27 RID: 24359 RVA: 0x00307700 File Offset: 0x00305B00
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

		// Token: 0x04003E68 RID: 15976
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x02000F64 RID: 3940
		private struct Request
		{
			// Token: 0x06005F29 RID: 24361 RVA: 0x003077B0 File Offset: 0x00305BB0
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			// Token: 0x06005F2A RID: 24362 RVA: 0x003077E4 File Offset: 0x00305BE4
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005F2B RID: 24363 RVA: 0x00307818 File Offset: 0x00305C18
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005F2C RID: 24364 RVA: 0x00307858 File Offset: 0x00305C58
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005F2D RID: 24365 RVA: 0x00307878 File Offset: 0x00305C78
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005F2E RID: 24366 RVA: 0x00307898 File Offset: 0x00305C98
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

			// Token: 0x04003E69 RID: 15977
			public string path;

			// Token: 0x04003E6A RID: 15978
			public int renderQueue;
		}
	}
}
