using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class Dialog_DebugTables : Window
	{
		private const float RowHeight = 23f;

		private const float ColExtraWidth = 8f;

		private string[,] table;

		private Vector2 scrollPosition = Vector2.zero;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		public Dialog_DebugTables(string[,] tables)
		{
			this.table = tables;
			base.doCloseButton = true;
			base.doCloseX = true;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			inRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, (float)(inRect.width - 16.0), (float)((float)this.table.GetLength(1) * 23.0));
			Widgets.BeginScrollView(inRect, ref this.scrollPosition, viewRect, true);
			List<float> list = new List<float>();
			for (int i = 0; i < this.table.GetLength(0); i++)
			{
				float num = 0f;
				for (int j = 0; j < this.table.GetLength(1); j++)
				{
					string text = this.table[i, j];
					Vector2 vector = Text.CalcSize(text);
					float x = vector.x;
					if (x > num)
					{
						num = x;
					}
				}
				list.Add((float)(num + 8.0));
			}
			float num2 = 0f;
			for (int k = 0; k < this.table.GetLength(0); k++)
			{
				for (int l = 0; l < this.table.GetLength(1); l++)
				{
					Rect rect;
					Rect rect2 = rect = new Rect(num2, (float)((float)l * 23.0), list[k], 23f);
					rect.xMin -= 999f;
					rect.xMax += 999f;
					if (Mouse.IsOver(rect) || k % 2 == 0)
					{
						Widgets.DrawHighlight(rect2);
					}
					Widgets.Label(rect2, this.table[k, l]);
				}
				num2 += list[k];
			}
			Widgets.EndScrollView();
		}
	}
}
