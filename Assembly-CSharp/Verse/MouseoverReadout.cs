using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000E8B RID: 3723
	public class MouseoverReadout
	{
		// Token: 0x04003A1A RID: 14874
		private TerrainDef cachedTerrain;

		// Token: 0x04003A1B RID: 14875
		private string cachedTerrainString;

		// Token: 0x04003A1C RID: 14876
		private string[] glowStrings;

		// Token: 0x04003A1D RID: 14877
		private const float YInterval = 19f;

		// Token: 0x04003A1E RID: 14878
		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);

		// Token: 0x060057EA RID: 22506 RVA: 0x002D1977 File Offset: 0x002CFD77
		public MouseoverReadout()
		{
			this.MakePermaCache();
		}

		// Token: 0x060057EB RID: 22507 RVA: 0x002D1988 File Offset: 0x002CFD88
		private void MakePermaCache()
		{
			this.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				this.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x060057EC RID: 22508 RVA: 0x002D19F0 File Offset: 0x002CFDF0
		public void MouseoverReadoutOnGUI()
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (Find.MainTabsRoot.OpenTab == null)
				{
					GenUI.DrawTextWinterShadow(new Rect(256f, (float)(UI.screenHeight - 256), -256f, 256f));
					Text.Font = GameFont.Small;
					GUI.color = new Color(1f, 1f, 1f, 0.8f);
					IntVec3 c = UI.MouseCell();
					if (c.InBounds(Find.CurrentMap))
					{
						float num = 0f;
						Profiler.BeginSample("fog");
						if (c.Fogged(Find.CurrentMap))
						{
							Rect rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
							Widgets.Label(rect, "Undiscovered".Translate());
							GUI.color = Color.white;
							Profiler.EndSample();
						}
						else
						{
							Profiler.EndSample();
							Profiler.BeginSample("light");
							Rect rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
							int num2 = Mathf.RoundToInt(Find.CurrentMap.glowGrid.GameGlowAt(c, false) * 100f);
							Widgets.Label(rect, this.glowStrings[num2]);
							num += 19f;
							Profiler.EndSample();
							Profiler.BeginSample("terrain");
							rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
							TerrainDef terrain = c.GetTerrain(Find.CurrentMap);
							if (terrain != this.cachedTerrain)
							{
								string str = ((double)terrain.fertility <= 0.0001) ? "" : (" " + "FertShort".Translate() + " " + terrain.fertility.ToStringPercent());
								this.cachedTerrainString = terrain.LabelCap + ((terrain.passability == Traversability.Impassable) ? null : (" (" + "WalkSpeed".Translate(new object[]
								{
									this.SpeedPercentString((float)terrain.pathCost)
								}) + str + ")"));
								this.cachedTerrain = terrain;
							}
							Widgets.Label(rect, this.cachedTerrainString);
							num += 19f;
							Profiler.EndSample();
							Profiler.BeginSample("zone");
							Zone zone = c.GetZone(Find.CurrentMap);
							if (zone != null)
							{
								rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
								string label = zone.label;
								Widgets.Label(rect, label);
								num += 19f;
							}
							Profiler.EndSample();
							float depth = Find.CurrentMap.snowGrid.GetDepth(c);
							if (depth > 0.03f)
							{
								rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
								SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
								string label2 = SnowUtility.GetDescription(snowCategory) + " (" + "WalkSpeed".Translate(new object[]
								{
									this.SpeedPercentString((float)SnowUtility.MovementTicksAddOn(snowCategory))
								}) + ")";
								Widgets.Label(rect, label2);
								num += 19f;
							}
							Profiler.BeginSample("things");
							List<Thing> thingList = c.GetThingList(Find.CurrentMap);
							for (int i = 0; i < thingList.Count; i++)
							{
								Thing thing = thingList[i];
								if (thing.def.category != ThingCategory.Mote)
								{
									rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
									string labelMouseover = thing.LabelMouseover;
									Widgets.Label(rect, labelMouseover);
									num += 19f;
								}
							}
							Profiler.EndSample();
							Profiler.BeginSample("roof");
							RoofDef roof = c.GetRoof(Find.CurrentMap);
							if (roof != null)
							{
								rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
								Widgets.Label(rect, roof.LabelCap);
								num += 19f;
							}
							Profiler.EndSample();
							GUI.color = Color.white;
						}
					}
				}
			}
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x002D1EE4 File Offset: 0x002D02E4
		private string SpeedPercentString(float extraPathTicks)
		{
			float f = 13f / (extraPathTicks + 13f);
			return f.ToStringPercent();
		}
	}
}
