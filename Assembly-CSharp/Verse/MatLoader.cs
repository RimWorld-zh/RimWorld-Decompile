using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public static class MatLoader
	{
		private struct Request
		{
			public string path;

			public int renderQueue;

			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine(seed, this.path);
				return Gen.HashCombineInt(seed, this.renderQueue);
			}

			public override bool Equals(object obj)
			{
				return obj is Request && this.Equals((Request)obj);
			}

			public bool Equals(Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			public static bool operator ==(Request lhs, Request rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(Request lhs, Request rhs)
			{
				return !(lhs == rhs);
			}

			public override string ToString()
			{
				return "MatLoader.Request(" + this.path + ", " + this.renderQueue + ")";
			}
		}

		private static Dictionary<Request, Material> dict = new Dictionary<Request, Material>();

		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if ((Object)material == (Object)null)
			{
				Log.Warning("Could not load material " + matPath);
			}
			Request key = new Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2 = default(Material);
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = new Material(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}
	}
}
