using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ColonistBarDrawLocsFinder
	{
		private List<int> entriesInGroup = new List<int>();

		private List<int> horizontalSlotsPerGroup = new List<int>();

		private const float MarginTop = 21f;

		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		private static float MaxColonistBarWidth
		{
			get
			{
				return (float)((float)UI.screenWidth - 520.0);
			}
		}

		public void CalculateDrawLocs(List<Vector2> outDrawLocs, out float scale)
		{
			if (this.ColonistBar.Entries.Count == 0)
			{
				outDrawLocs.Clear();
				scale = 1f;
			}
			else
			{
				this.CalculateColonistsInGroup();
				bool onlyOneRow = default(bool);
				int maxPerGlobalRow = default(int);
				scale = this.FindBestScale(out onlyOneRow, out maxPerGlobalRow);
				this.CalculateDrawLocs(outDrawLocs, scale, onlyOneRow, maxPerGlobalRow);
			}
		}

		private void CalculateColonistsInGroup()
		{
			this.entriesInGroup.Clear();
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = this.CalculateGroupsCount();
			for (int num2 = 0; num2 < num; num2++)
			{
				this.entriesInGroup.Add(0);
			}
			for (int i = 0; i < entries.Count; i++)
			{
				List<int> list;
				List<int> obj = list = this.entriesInGroup;
				ColonistBar.Entry entry = entries[i];
				int group;
				obj[group = entry.group] = list[group] + 1;
			}
		}

		private int CalculateGroupsCount()
		{
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				int num3 = num;
				ColonistBar.Entry entry = entries[i];
				if (num3 != entry.group)
				{
					num2++;
					ColonistBar.Entry entry2 = entries[i];
					num = entry2.group;
				}
			}
			return num2;
		}

		private float FindBestScale(out bool onlyOneRow, out int maxPerGlobalRow)
		{
			float num = 1f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num2 = this.CalculateGroupsCount();
			while (true)
			{
				Vector2 baseSize = ColonistBar.BaseSize;
				float num3 = (float)((baseSize.x + 24.0) * num);
				float num4 = (float)(ColonistBarDrawLocsFinder.MaxColonistBarWidth - (float)(num2 - 1) * 25.0 * num);
				maxPerGlobalRow = Mathf.FloorToInt(num4 / num3);
				onlyOneRow = true;
				if (this.TryDistributeHorizontalSlotsBetweenGroups(maxPerGlobalRow))
				{
					int allowedRowsCountForScale = ColonistBarDrawLocsFinder.GetAllowedRowsCountForScale(num);
					bool flag = true;
					int num5 = -1;
					for (int i = 0; i < entries.Count; i++)
					{
						int num6 = num5;
						ColonistBar.Entry entry = entries[i];
						if (num6 != entry.group)
						{
							ColonistBar.Entry entry2 = entries[i];
							num5 = entry2.group;
							List<int> obj = this.entriesInGroup;
							ColonistBar.Entry entry3 = entries[i];
							float num7 = (float)obj[entry3.group];
							List<int> obj2 = this.horizontalSlotsPerGroup;
							ColonistBar.Entry entry4 = entries[i];
							int num8 = Mathf.CeilToInt(num7 / (float)obj2[entry4.group]);
							if (num8 > 1)
							{
								onlyOneRow = false;
							}
							if (num8 > allowedRowsCountForScale)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
						break;
				}
				num = (float)(num * 0.949999988079071);
			}
			return num;
		}

		private bool TryDistributeHorizontalSlotsBetweenGroups(int maxPerGlobalRow)
		{
			int num = this.CalculateGroupsCount();
			this.horizontalSlotsPerGroup.Clear();
			for (int num2 = 0; num2 < num; num2++)
			{
				this.horizontalSlotsPerGroup.Add(0);
			}
			GenMath.DHondtDistribution(this.horizontalSlotsPerGroup, (Func<int, float>)((int i) => (float)this.entriesInGroup[i]), maxPerGlobalRow);
			int num3 = 0;
			bool result;
			while (true)
			{
				if (num3 < this.horizontalSlotsPerGroup.Count)
				{
					if (this.horizontalSlotsPerGroup[num3] == 0)
					{
						int num4 = this.horizontalSlotsPerGroup.Max();
						if (num4 <= 1)
						{
							result = false;
							break;
						}
						int num5 = this.horizontalSlotsPerGroup.IndexOf(num4);
						int index;
						List<int> list;
						(list = this.horizontalSlotsPerGroup)[index = num5] = list[index] - 1;
						int index2;
						(list = this.horizontalSlotsPerGroup)[index2 = num3] = list[index2] + 1;
					}
					num3++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}

		private static int GetAllowedRowsCountForScale(float scale)
		{
			return (scale > 0.57999998331069946) ? 1 : ((!(scale > 0.41999998688697815)) ? 3 : 2);
		}

		private void CalculateDrawLocs(List<Vector2> outDrawLocs, float scale, bool onlyOneRow, int maxPerGlobalRow)
		{
			outDrawLocs.Clear();
			int num = maxPerGlobalRow;
			if (onlyOneRow)
			{
				for (int i = 0; i < this.horizontalSlotsPerGroup.Count; i++)
				{
					this.horizontalSlotsPerGroup[i] = Mathf.Min(this.horizontalSlotsPerGroup[i], this.entriesInGroup[i]);
				}
				num = this.ColonistBar.Entries.Count;
			}
			int num2 = this.CalculateGroupsCount();
			Vector2 baseSize = ColonistBar.BaseSize;
			float num3 = (float)((baseSize.x + 24.0) * scale);
			float num4 = (float)((float)num * num3 + (float)(num2 - 1) * 25.0 * scale);
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num5 = -1;
			int num6 = -1;
			float num7 = (float)(((float)UI.screenWidth - num4) / 2.0);
			for (int j = 0; j < entries.Count; j++)
			{
				int num8 = num5;
				ColonistBar.Entry entry = entries[j];
				if (num8 != entry.group)
				{
					if (num5 >= 0)
					{
						num7 = (float)(num7 + 25.0 * scale);
						float num9 = num7;
						float num10 = (float)this.horizontalSlotsPerGroup[num5] * scale;
						Vector2 baseSize2 = ColonistBar.BaseSize;
						num7 = (float)(num9 + num10 * (baseSize2.x + 24.0));
					}
					num6 = 0;
					ColonistBar.Entry entry2 = entries[j];
					num5 = entry2.group;
				}
				else
				{
					num6++;
				}
				float groupStartX = num7;
				ColonistBar.Entry entry3 = entries[j];
				Vector2 drawLoc = this.GetDrawLoc(groupStartX, 21f, entry3.group, num6, scale);
				outDrawLocs.Add(drawLoc);
			}
		}

		private Vector2 GetDrawLoc(float groupStartX, float groupStartY, int group, int numInGroup, float scale)
		{
			float num = (float)(numInGroup % this.horizontalSlotsPerGroup[group]) * scale;
			Vector2 baseSize = ColonistBar.BaseSize;
			float num2 = (float)(groupStartX + num * (baseSize.x + 24.0));
			float num3 = (float)(numInGroup / this.horizontalSlotsPerGroup[group]) * scale;
			Vector2 baseSize2 = ColonistBar.BaseSize;
			float y = (float)(groupStartY + num3 * (baseSize2.y + 32.0));
			if (numInGroup >= this.entriesInGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group])
			{
				int num4 = this.horizontalSlotsPerGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group];
				float num5 = num2;
				float num6 = (float)num4 * scale;
				Vector2 baseSize3 = ColonistBar.BaseSize;
				num2 = (float)(num5 + num6 * (baseSize3.x + 24.0) * 0.5);
			}
			return new Vector2(num2, y);
		}
	}
}
