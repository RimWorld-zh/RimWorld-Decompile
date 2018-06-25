using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD7 RID: 3543
	public abstract class Graphic_Collection : Graphic
	{
		// Token: 0x040034BA RID: 13498
		protected Graphic[] subGraphics;

		// Token: 0x06004F57 RID: 20311 RVA: 0x0029563C File Offset: 0x00293A3C
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
	}
}
