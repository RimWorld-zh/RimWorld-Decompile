using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F67 RID: 3943
	public static class MatLoader
	{
		// Token: 0x04003E84 RID: 16004
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x06005F58 RID: 24408 RVA: 0x0030A144 File Offset: 0x00308544
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

		// Token: 0x02000F68 RID: 3944
		private struct Request
		{
			// Token: 0x04003E85 RID: 16005
			public string path;

			// Token: 0x04003E86 RID: 16006
			public int renderQueue;

			// Token: 0x06005F5A RID: 24410 RVA: 0x0030A1F4 File Offset: 0x003085F4
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			// Token: 0x06005F5B RID: 24411 RVA: 0x0030A228 File Offset: 0x00308628
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005F5C RID: 24412 RVA: 0x0030A25C File Offset: 0x0030865C
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005F5D RID: 24413 RVA: 0x0030A29C File Offset: 0x0030869C
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005F5E RID: 24414 RVA: 0x0030A2BC File Offset: 0x003086BC
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005F5F RID: 24415 RVA: 0x0030A2DC File Offset: 0x003086DC
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
