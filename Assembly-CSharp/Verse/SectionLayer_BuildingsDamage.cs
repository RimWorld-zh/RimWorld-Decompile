using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class SectionLayer_BuildingsDamage : SectionLayer
	{
		private static List<Vector2> scratches = new List<Vector2>();

		public SectionLayer_BuildingsDamage(Section section)
			: base(section)
		{
			base.relevantChangeTypes = (MapMeshFlag.Buildings | MapMeshFlag.BuildingsDamage);
		}

		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 item in base.section.CellRect)
			{
				IntVec3 current = item;
				List<Thing> list = base.Map.thingGrid.ThingsListAt(current);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Building building = list[i] as Building;
					if (building != null && building.def.useHitPoints && building.HitPoints < building.MaxHitPoints && building.def.drawDamagedOverlay)
					{
						IntVec3 position = building.Position;
						if (position.x == current.x)
						{
							IntVec3 position2 = building.Position;
							if (position2.z == current.z)
							{
								this.PrintDamageVisualsFrom(building);
							}
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		private void PrintDamageVisualsFrom(Building b)
		{
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && !b.def.graphicData.damageData.enabled)
				return;
			this.PrintScratches(b);
			this.PrintCornersAndEdges(b);
		}

		private void PrintScratches(Building b)
		{
			int num = 0;
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			for (int i = 0; i < overlays.Count; i++)
			{
				if (overlays[i] == DamageOverlay.Scratch)
				{
					num++;
				}
			}
			if (num != 0)
			{
				Rect rect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
				float num2 = Mathf.Min((float)(0.5 * Mathf.Min(rect.width, rect.height)), 1f);
				rect = rect.ContractedBy((float)(num2 / 2.0));
				if (!(rect.width <= 0.0) && !(rect.height <= 0.0))
				{
					float num3 = (float)(Mathf.Max(rect.width, rect.height) * 0.699999988079071);
					SectionLayer_BuildingsDamage.scratches.Clear();
					Rand.PushState();
					Rand.Seed = b.thingIDNumber * 3697;
					for (int j = 0; j < num; j++)
					{
						this.AddScratch(b, rect.width, rect.height, ref num3);
					}
					Rand.PopState();
					float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
					IList<Material> scratchMats = BuildingsDamageSectionLayerUtility.GetScratchMats(b);
					Rand.PushState();
					Rand.Seed = b.thingIDNumber * 7;
					for (int k = 0; k < SectionLayer_BuildingsDamage.scratches.Count; k++)
					{
						Vector2 vector = SectionLayer_BuildingsDamage.scratches[k];
						float x = vector.x;
						Vector2 vector2 = SectionLayer_BuildingsDamage.scratches[k];
						float y = vector2.y;
						float rot = Rand.Range(0f, 360f);
						float num4 = num2;
						if (rect.width > 0.949999988079071 && rect.height > 0.949999988079071)
						{
							num4 *= Rand.Range(0.85f, 1f);
						}
						Vector3 center = new Vector3(rect.xMin + x, damageTexturesAltitude, rect.yMin + y);
						Printer_Plane.PrintPlane(this, center, new Vector2(num4, num4), scratchMats.RandomElement(), rot, false, null, null, 0f);
					}
					Rand.PopState();
				}
			}
		}

		private void AddScratch(Building b, float rectWidth, float rectHeight, ref float minDist)
		{
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			while (!flag)
			{
				for (int i = 0; i < 5; i++)
				{
					num = Rand.Value * rectWidth;
					num2 = Rand.Value * rectHeight;
					float num3 = 3.40282347E+38f;
					for (int j = 0; j < SectionLayer_BuildingsDamage.scratches.Count; j++)
					{
						float num4 = num;
						Vector2 vector = SectionLayer_BuildingsDamage.scratches[j];
						float num5 = num4 - vector.x;
						float num6 = num;
						Vector2 vector2 = SectionLayer_BuildingsDamage.scratches[j];
						float num7 = num5 * (num6 - vector2.x);
						float num8 = num2;
						Vector2 vector3 = SectionLayer_BuildingsDamage.scratches[j];
						float num9 = num8 - vector3.y;
						float num10 = num2;
						Vector2 vector4 = SectionLayer_BuildingsDamage.scratches[j];
						float num11 = num7 + num9 * (num10 - vector4.y);
						if (num11 < num3)
						{
							num3 = num11;
						}
					}
					if (num3 >= minDist * minDist)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					minDist *= 0.85f;
					if (minDist < 0.0010000000474974513)
						break;
				}
			}
			if (flag)
			{
				SectionLayer_BuildingsDamage.scratches.Add(new Vector2(num, num2));
			}
		}

		private void PrintCornersAndEdges(Building b)
		{
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 3;
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				this.DrawLinkableCornersAndEdges(b);
			}
			else
			{
				this.DrawFullThingCorners(b);
			}
			Rand.PopState();
		}

		private void DrawLinkableCornersAndEdges(Building b)
		{
			if (b.def.graphicData != null)
			{
				DamageGraphicData damageData = b.def.graphicData.damageData;
				if (damageData != null)
				{
					float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
					List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
					IntVec3 position = b.Position;
					Vector3 vector = new Vector3((float)((float)position.x + 0.5), damageTexturesAltitude, (float)((float)position.z + 0.5));
					float x = Rand.Range(0.4f, 0.6f);
					float z = Rand.Range(0.4f, 0.6f);
					float x2 = Rand.Range(0.4f, 0.6f);
					float z2 = Rand.Range(0.4f, 0.6f);
					for (int i = 0; i < overlays.Count; i++)
					{
						switch (overlays[i])
						{
						case DamageOverlay.TopEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(x, 0f, 0f), Vector2.one, damageData.edgeTopMat, 0f, false, null, null, 0f);
							break;
						case DamageOverlay.RightEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z), Vector2.one, damageData.edgeRightMat, 90f, false, null, null, 0f);
							break;
						case DamageOverlay.BotEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(x2, 0f, 0f), Vector2.one, damageData.edgeBotMat, 180f, false, null, null, 0f);
							break;
						case DamageOverlay.LeftEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z2), Vector2.one, damageData.edgeLeftMat, 270f, false, null, null, 0f);
							break;
						case DamageOverlay.TopLeftCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerTLMat, 0f, false, null, null, 0f);
							break;
						case DamageOverlay.TopRightCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerTRMat, 90f, false, null, null, 0f);
							break;
						case DamageOverlay.BotRightCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerBRMat, 180f, false, null, null, 0f);
							break;
						case DamageOverlay.BotLeftCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerBLMat, 270f, false, null, null, 0f);
							break;
						}
					}
				}
			}
		}

		private void DrawFullThingCorners(Building b)
		{
			if (b.def.graphicData != null)
			{
				DamageGraphicData damageData = b.def.graphicData.damageData;
				if (damageData != null)
				{
					Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
					float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
					float num = Mathf.Min(Mathf.Min(damageRect.width, damageRect.height), 1.5f);
					Material mat = default(Material);
					Material mat2 = default(Material);
					Material mat3 = default(Material);
					Material mat4 = default(Material);
					BuildingsDamageSectionLayerUtility.GetCornerMats(out mat, out mat2, out mat3, out mat4, b);
					float num2 = num * Rand.Range(0.9f, 1f);
					float num3 = num * Rand.Range(0.9f, 1f);
					float num4 = num * Rand.Range(0.9f, 1f);
					float num5 = num * Rand.Range(0.9f, 1f);
					List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
					for (int i = 0; i < overlays.Count; i++)
					{
						switch (overlays[i])
						{
						case DamageOverlay.TopLeftCorner:
						{
							Rect rect4 = new Rect(damageRect.xMin, damageRect.yMax - num2, num2, num2);
							Vector2 center7 = rect4.center;
							float x4 = center7.x;
							float y4 = damageTexturesAltitude;
							Vector2 center8 = rect4.center;
							Printer_Plane.PrintPlane(this, new Vector3(x4, y4, center8.y), rect4.size, mat, 0f, false, null, null, 0f);
							break;
						}
						case DamageOverlay.TopRightCorner:
						{
							Rect rect3 = new Rect(damageRect.xMax - num3, damageRect.yMax - num3, num3, num3);
							Vector2 center5 = rect3.center;
							float x3 = center5.x;
							float y3 = damageTexturesAltitude;
							Vector2 center6 = rect3.center;
							Printer_Plane.PrintPlane(this, new Vector3(x3, y3, center6.y), rect3.size, mat2, 90f, false, null, null, 0f);
							break;
						}
						case DamageOverlay.BotRightCorner:
						{
							Rect rect2 = new Rect(damageRect.xMax - num4, damageRect.yMin, num4, num4);
							Vector2 center3 = rect2.center;
							float x2 = center3.x;
							float y2 = damageTexturesAltitude;
							Vector2 center4 = rect2.center;
							Printer_Plane.PrintPlane(this, new Vector3(x2, y2, center4.y), rect2.size, mat3, 180f, false, null, null, 0f);
							break;
						}
						case DamageOverlay.BotLeftCorner:
						{
							Rect rect = new Rect(damageRect.xMin, damageRect.yMin, num5, num5);
							Vector2 center = rect.center;
							float x = center.x;
							float y = damageTexturesAltitude;
							Vector2 center2 = rect.center;
							Printer_Plane.PrintPlane(this, new Vector3(x, y, center2.y), rect.size, mat4, 270f, false, null, null, 0f);
							break;
						}
						}
					}
				}
			}
		}

		private float GetDamageTexturesAltitude(Building b)
		{
			return (float)(b.def.Altitude + 0.046875);
		}
	}
}
