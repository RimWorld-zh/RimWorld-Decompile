using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD0 RID: 3536
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x040034AC RID: 13484
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x040034AD RID: 13485
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x040034AE RID: 13486
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x040034AF RID: 13487
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x040034B0 RID: 13488
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x040034B1 RID: 13489
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x06004F3D RID: 20285 RVA: 0x00294A3C File Offset: 0x00292E3C
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00294A58 File Offset: 0x00292E58
		private static void BuildDatabaseIfNecessary()
		{
			if (GraphicDatabaseHeadRecords.heads.Count <= 0 || GraphicDatabaseHeadRecords.skull == null || GraphicDatabaseHeadRecords.stump == null)
			{
				GraphicDatabaseHeadRecords.heads.Clear();
				foreach (string text in GraphicDatabaseHeadRecords.HeadsFolderPaths)
				{
					foreach (string str in GraphicDatabaseUtility.GraphicNamesInFolder(text))
					{
						GraphicDatabaseHeadRecords.heads.Add(new GraphicDatabaseHeadRecords.HeadGraphicRecord(text + "/" + str));
					}
				}
				GraphicDatabaseHeadRecords.skull = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.SkullPath);
				GraphicDatabaseHeadRecords.stump = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.StumpPath);
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x00294B40 File Offset: 0x00292F40
		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor);
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00294BC8 File Offset: 0x00292FC8
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white);
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00294BF4 File Offset: 0x00292FF4
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor);
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x00294C1C File Offset: 0x0029301C
		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			Predicate<GraphicDatabaseHeadRecords.HeadGraphicRecord> predicate = (GraphicDatabaseHeadRecords.HeadGraphicRecord head) => head.crownType == crownType && head.gender == gender;
			int num = 0;
			GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord;
			for (;;)
			{
				headGraphicRecord = GraphicDatabaseHeadRecords.heads.RandomElement<GraphicDatabaseHeadRecords.HeadGraphicRecord>();
				if (predicate(headGraphicRecord))
				{
					break;
				}
				num++;
				if (num > 40)
				{
					goto Block_2;
				}
			}
			return headGraphicRecord.GetGraphic(skinColor);
			Block_2:
			foreach (GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord2 in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(headGraphicRecord2))
				{
					return headGraphicRecord2.GetGraphic(skinColor);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor);
		}

		// Token: 0x02000DD1 RID: 3537
		private class HeadGraphicRecord
		{
			// Token: 0x040034B2 RID: 13490
			public Gender gender;

			// Token: 0x040034B3 RID: 13491
			public CrownType crownType = CrownType.Undefined;

			// Token: 0x040034B4 RID: 13492
			public string graphicPath;

			// Token: 0x040034B5 RID: 13493
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();

			// Token: 0x06004F44 RID: 20292 RVA: 0x00294D74 File Offset: 0x00293174
			public HeadGraphicRecord(string graphicPath)
			{
				this.graphicPath = graphicPath;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(graphicPath);
				string[] array = fileNameWithoutExtension.Split(new char[]
				{
					'_'
				});
				try
				{
					this.crownType = (CrownType)ParseHelper.FromString(array[array.Length - 2], typeof(CrownType));
					this.gender = (Gender)ParseHelper.FromString(array[array.Length - 3], typeof(Gender));
				}
				catch (Exception ex)
				{
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message, false);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

			// Token: 0x06004F45 RID: 20293 RVA: 0x00294E48 File Offset: 0x00293248
			public Graphic_Multi GetGraphic(Color color)
			{
				for (int i = 0; i < this.graphics.Count; i++)
				{
					if (color.IndistinguishableFrom(this.graphics[i].Key))
					{
						return this.graphics[i].Value;
					}
				}
				Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, ShaderDatabase.CutoutSkin, Vector2.one, color);
				this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
				return graphic_Multi;
			}
		}
	}
}
