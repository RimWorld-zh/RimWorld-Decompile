using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000E8B RID: 3723
	public class MouseoverReadout
	{
		// Token: 0x060057C8 RID: 22472 RVA: 0x002CFC3B File Offset: 0x002CE03B
		public MouseoverReadout()
		{
			this.MakePermaCache();
		}

		// Token: 0x060057C9 RID: 22473 RVA: 0x002CFC4C File Offset: 0x002CE04C
		private void MakePermaCache()
		{
			this.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				this.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x002CFCB4 File Offset: 0x002CE0B4
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

		// Token: 0x060057CB RID: 22475 RVA: 0x002D01A8 File Offset: 0x002CE5A8
		private string SpeedPercentString(float extraPathTicks)
		{
			float f = 13f / (extraPathTicks + 13f);
			return f.ToStringPercent();
		}

		// Token: 0x04003A0C RID: 14860
		private TerrainDef cachedTerrain;

		// Token: 0x04003A0D RID: 14861
		private string cachedTerrainString;

		// Token: 0x04003A0E RID: 14862
		private string[] glowStrings;

		// Token: 0x04003A0F RID: 14863
		private const float YInterval = 19f;

		// Token: 0x04003A10 RID: 14864
		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);
	}
}
