using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public abstract class Graphic_Collection : Graphic
	{
		protected Graphic[] subGraphics;

		[CompilerGenerated]
		private static Func<Texture2D, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Texture2D, string> <>f__am$cache1;

		protected Graphic_Collection()
		{
		}

		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (req.path.NullOrEmpty())
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<Texture2D> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
			where !x.name.EndsWith(Graphic_Single.MaskSuffix)
			orderby x.name
			select x).ToList<Texture2D>();
			if (list.NullOrEmpty<Texture2D>())
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path, false);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
			}
			else
			{
				this.subGraphics = new Graphic[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					string path = req.path + "/" + list[i].name;
					this.subGraphics[i] = GraphicDatabase.Get(typeof(Graphic_Single), path, req.shader, this.drawSize, this.color, Color.white, null, req.shaderParameters);
				}
			}
		}

		[CompilerGenerated]
		private static bool <Init>m__0(Texture2D x)
		{
			return !x.name.EndsWith(Graphic_Single.MaskSuffix);
		}

		[CompilerGenerated]
		private static string <Init>m__1(Texture2D x)
		{
			return x.name;
		}
	}
}
