using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCF RID: 3535
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x040034A5 RID: 13477
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x040034A6 RID: 13478
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x040034A7 RID: 13479
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x040034A8 RID: 13480
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x040034A9 RID: 13481
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x040034AA RID: 13482
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x06004F3D RID: 20285 RVA: 0x0029475C File Offset: 0x00292B5C
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00294778 File Offset: 0x00292B78
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

		// Token: 0x06004F3F RID: 20287 RVA: 0x00294860 File Offset: 0x00292C60
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

		// Token: 0x06004F40 RID: 20288 RVA: 0x002948E8 File Offset: 0x00292CE8
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white);
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00294914 File Offset: 0x00292D14
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor);
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x0029493C File Offset: 0x00292D3C
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

		// Token: 0x02000DD0 RID: 3536
		private class HeadGraphicRecord
		{
			// Token: 0x040034AB RID: 13483
			public Gender gender;

			// Token: 0x040034AC RID: 13484
			public CrownType crownType = CrownType.Undefined;

			// Token: 0x040034AD RID: 13485
			public string graphicPath;

			// Token: 0x040034AE RID: 13486
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();

			// Token: 0x06004F44 RID: 20292 RVA: 0x00294A94 File Offset: 0x00292E94
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

			// Token: 0x06004F45 RID: 20293 RVA: 0x00294B68 File Offset: 0x00292F68
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
