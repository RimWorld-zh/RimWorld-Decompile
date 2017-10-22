#define ENABLE_PROFILER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public class MouseoverReadout
	{
		private TerrainDef cachedTerrain;

		private string cachedTerrainString;

		private string[] glowStrings;

		private const float YInterval = 19f;

		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);

		public MouseoverReadout()
		{
			this.MakePermaCache();
		}

		private void MakePermaCache()
		{
			this.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				this.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)((float)i / 100.0)).GetLabel() + " (" + ((float)((float)i / 100.0)).ToStringPercent() + ")";
			}
		}

		public void MouseoverReadoutOnGUI()
		{
			if (Event.current.type == EventType.Repaint && Find.MainTabsRoot.OpenTab == null)
			{
				GenUI.DrawTextWinterShadow(new Rect(256f, (float)(UI.screenHeight - 256), -256f, 256f));
				Text.Font = GameFont.Small;
				GUI.color = new Color(1f, 1f, 1f, 0.8f);
				IntVec3 c = UI.MouseCell();
				if (c.InBounds(Find.VisibleMap))
				{
					float num = 0f;
					Profiler.BeginSample("fog");
					Rect rect = default(Rect);
					if (c.Fogged(Find.VisibleMap))
					{
						Vector2 botLeft = MouseoverReadout.BotLeft;
						float x = botLeft.x;
						float num2 = (float)UI.screenHeight;
						Vector2 botLeft2 = MouseoverReadout.BotLeft;
						rect = new Rect(x, num2 - botLeft2.y - num, 999f, 999f);
						Widgets.Label(rect, "Undiscovered".Translate());
						GUI.color = Color.white;
						Profiler.EndSample();
					}
					else
					{
						Profiler.EndSample();
						Profiler.BeginSample("light");
						Vector2 botLeft3 = MouseoverReadout.BotLeft;
						float x2 = botLeft3.x;
						float num3 = (float)UI.screenHeight;
						Vector2 botLeft4 = MouseoverReadout.BotLeft;
						rect = new Rect(x2, num3 - botLeft4.y - num, 999f, 999f);
						int num4 = Mathf.RoundToInt((float)(Find.VisibleMap.glowGrid.GameGlowAt(c, false) * 100.0));
						Widgets.Label(rect, this.glowStrings[num4]);
						num = (float)(num + 19.0);
						Profiler.EndSample();
						Profiler.BeginSample("terrain");
						Vector2 botLeft5 = MouseoverReadout.BotLeft;
						float x3 = botLeft5.x;
						float num5 = (float)UI.screenHeight;
						Vector2 botLeft6 = MouseoverReadout.BotLeft;
						rect = new Rect(x3, num5 - botLeft6.y - num, 999f, 999f);
						TerrainDef terrain = c.GetTerrain(Find.VisibleMap);
						if (terrain != this.cachedTerrain)
						{
							string str = (!((double)terrain.fertility > 0.0001)) ? "" : (" " + "FertShort".Translate() + " " + terrain.fertility.ToStringPercent());
							this.cachedTerrainString = terrain.LabelCap + ((terrain.passability == Traversability.Impassable) ? null : (" (" + "WalkSpeed".Translate(this.SpeedPercentString((float)terrain.pathCost)) + str + ")"));
							this.cachedTerrain = terrain;
						}
						Widgets.Label(rect, this.cachedTerrainString);
						num = (float)(num + 19.0);
						Profiler.EndSample();
						Profiler.BeginSample("zone");
						Zone zone = c.GetZone(Find.VisibleMap);
						if (zone != null)
						{
							Vector2 botLeft7 = MouseoverReadout.BotLeft;
							float x4 = botLeft7.x;
							float num6 = (float)UI.screenHeight;
							Vector2 botLeft8 = MouseoverReadout.BotLeft;
							rect = new Rect(x4, num6 - botLeft8.y - num, 999f, 999f);
							string label = zone.label;
							Widgets.Label(rect, label);
							num = (float)(num + 19.0);
						}
						Profiler.EndSample();
						float depth = Find.VisibleMap.snowGrid.GetDepth(c);
						if (depth > 0.029999999329447746)
						{
							Vector2 botLeft9 = MouseoverReadout.BotLeft;
							float x5 = botLeft9.x;
							float num7 = (float)UI.screenHeight;
							Vector2 botLeft10 = MouseoverReadout.BotLeft;
							rect = new Rect(x5, num7 - botLeft10.y - num, 999f, 999f);
							SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
							string label2 = SnowUtility.GetDescription(snowCategory) + " (" + "WalkSpeed".Translate(this.SpeedPercentString((float)SnowUtility.MovementTicksAddOn(snowCategory))) + ")";
							Widgets.Label(rect, label2);
							num = (float)(num + 19.0);
						}
						Profiler.BeginSample("things");
						List<Thing> thingList = c.GetThingList(Find.VisibleMap);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (thing.def.category != ThingCategory.Mote)
							{
								Vector2 botLeft11 = MouseoverReadout.BotLeft;
								float x6 = botLeft11.x;
								float num8 = (float)UI.screenHeight;
								Vector2 botLeft12 = MouseoverReadout.BotLeft;
								rect = new Rect(x6, num8 - botLeft12.y - num, 999f, 999f);
								string labelMouseover = thing.LabelMouseover;
								Widgets.Label(rect, labelMouseover);
								num = (float)(num + 19.0);
							}
						}
						Profiler.EndSample();
						Profiler.BeginSample("roof");
						RoofDef roof = c.GetRoof(Find.VisibleMap);
						if (roof != null)
						{
							Vector2 botLeft13 = MouseoverReadout.BotLeft;
							float x7 = botLeft13.x;
							float num9 = (float)UI.screenHeight;
							Vector2 botLeft14 = MouseoverReadout.BotLeft;
							rect = new Rect(x7, num9 - botLeft14.y - num, 999f, 999f);
							Widgets.Label(rect, roof.LabelCap);
							num = (float)(num + 19.0);
						}
						Profiler.EndSample();
						GUI.color = Color.white;
					}
				}
			}
		}

		private string SpeedPercentString(float extraPathTicks)
		{
			float f = (float)(13.0 / (float)(extraPathTicks + 13.0));
			return f.ToStringPercent();
		}
	}
}
