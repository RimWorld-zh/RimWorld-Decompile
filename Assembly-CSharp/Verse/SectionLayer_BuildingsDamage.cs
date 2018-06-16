using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C47 RID: 3143
	public class SectionLayer_BuildingsDamage : SectionLayer
	{
		// Token: 0x06004531 RID: 17713 RVA: 0x002464DD File Offset: 0x002448DD
		public SectionLayer_BuildingsDamage(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.Buildings | MapMeshFlag.BuildingsDamage);
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x002464F4 File Offset: 0x002448F4
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 c in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Building building = list[i] as Building;
					if (building != null && building.def.useHitPoints && building.HitPoints < building.MaxHitPoints && building.def.drawDamagedOverlay)
					{
						if (building.Position.x == c.x && building.Position.z == c.z)
						{
							this.PrintDamageVisualsFrom(building);
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x00246628 File Offset: 0x00244A28
		private void PrintDamageVisualsFrom(Building b)
		{
			if (b.def.graphicData == null || b.def.graphicData.damageData == null || b.def.graphicData.damageData.enabled)
			{
				this.PrintScratches(b);
				this.PrintCornersAndEdges(b);
			}
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00246688 File Offset: 0x00244A88
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
				float num2 = Mathf.Min(0.5f * Mathf.Min(rect.width, rect.height), 1f);
				rect = rect.ContractedBy(num2 / 2f);
				if (rect.width > 0f && rect.height > 0f)
				{
					float num3 = Mathf.Max(rect.width, rect.height) * 0.7f;
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
						float x = SectionLayer_BuildingsDamage.scratches[k].x;
						float y = SectionLayer_BuildingsDamage.scratches[k].y;
						float rot = Rand.Range(0f, 360f);
						float num4 = num2;
						if (rect.width > 0.95f && rect.height > 0.95f)
						{
							num4 *= Rand.Range(0.85f, 1f);
						}
						Vector3 center = new Vector3(rect.xMin + x, damageTexturesAltitude, rect.yMin + y);
						Printer_Plane.PrintPlane(this, center, new Vector2(num4, num4), scratchMats.RandomElement<Material>(), rot, false, null, null, 0f, 0f);
					}
					Rand.PopState();
				}
			}
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x002468AC File Offset: 0x00244CAC
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
					float num3 = float.MaxValue;
					for (int j = 0; j < SectionLayer_BuildingsDamage.scratches.Count; j++)
					{
						float num4 = (num - SectionLayer_BuildingsDamage.scratches[j].x) * (num - SectionLayer_BuildingsDamage.scratches[j].x) + (num2 - SectionLayer_BuildingsDamage.scratches[j].y) * (num2 - SectionLayer_BuildingsDamage.scratches[j].y);
						if (num4 < num3)
						{
							num3 = num4;
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
					if (minDist < 0.001f)
					{
						break;
					}
				}
			}
			if (flag)
			{
				SectionLayer_BuildingsDamage.scratches.Add(new Vector2(num, num2));
			}
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x002469E2 File Offset: 0x00244DE2
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

		// Token: 0x06004537 RID: 17719 RVA: 0x00246A1C File Offset: 0x00244E1C
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
					Vector3 vector = new Vector3((float)position.x + 0.5f, damageTexturesAltitude, (float)position.z + 0.5f);
					float x = Rand.Range(0.4f, 0.6f);
					float z = Rand.Range(0.4f, 0.6f);
					float x2 = Rand.Range(0.4f, 0.6f);
					float z2 = Rand.Range(0.4f, 0.6f);
					for (int i = 0; i < overlays.Count; i++)
					{
						switch (overlays[i])
						{
						case DamageOverlay.TopLeftCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerTLMat, 0f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.TopRightCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerTRMat, 90f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.BotLeftCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerBLMat, 270f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.BotRightCorner:
							Printer_Plane.PrintPlane(this, vector, Vector2.one, damageData.cornerBRMat, 180f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.LeftEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z2), Vector2.one, damageData.edgeLeftMat, 270f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.RightEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z), Vector2.one, damageData.edgeRightMat, 90f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.TopEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(x, 0f, 0f), Vector2.one, damageData.edgeTopMat, 0f, false, null, null, 0f, 0f);
							break;
						case DamageOverlay.BotEdge:
							Printer_Plane.PrintPlane(this, vector + new Vector3(x2, 0f, 0f), Vector2.one, damageData.edgeBotMat, 180f, false, null, null, 0f, 0f);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00246CD8 File Offset: 0x002450D8
		private void DrawFullThingCorners(Building b)
		{
			if (b.def.graphicData != null)
			{
				if (b.def.graphicData.damageData != null)
				{
					Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
					float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
					float num = Mathf.Min(Mathf.Min(damageRect.width, damageRect.height), 1.5f);
					Material mat;
					Material mat2;
					Material mat3;
					Material mat4;
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
							Rect rect = new Rect(damageRect.xMin, damageRect.yMax - num2, num2, num2);
							Printer_Plane.PrintPlane(this, new Vector3(rect.center.x, damageTexturesAltitude, rect.center.y), rect.size, mat, 0f, false, null, null, 0f, 0f);
							break;
						}
						case DamageOverlay.TopRightCorner:
						{
							Rect rect2 = new Rect(damageRect.xMax - num3, damageRect.yMax - num3, num3, num3);
							Printer_Plane.PrintPlane(this, new Vector3(rect2.center.x, damageTexturesAltitude, rect2.center.y), rect2.size, mat2, 90f, false, null, null, 0f, 0f);
							break;
						}
						case DamageOverlay.BotLeftCorner:
						{
							Rect rect3 = new Rect(damageRect.xMin, damageRect.yMin, num5, num5);
							Printer_Plane.PrintPlane(this, new Vector3(rect3.center.x, damageTexturesAltitude, rect3.center.y), rect3.size, mat4, 270f, false, null, null, 0f, 0f);
							break;
						}
						case DamageOverlay.BotRightCorner:
						{
							Rect rect4 = new Rect(damageRect.xMax - num4, damageRect.yMin, num4, num4);
							Printer_Plane.PrintPlane(this, new Vector3(rect4.center.x, damageTexturesAltitude, rect4.center.y), rect4.size, mat3, 180f, false, null, null, 0f, 0f);
							break;
						}
						}
					}
				}
			}
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x00246F90 File Offset: 0x00245390
		private float GetDamageTexturesAltitude(Building b)
		{
			return b.def.Altitude + 0.046875f;
		}

		// Token: 0x04002F52 RID: 12114
		private static List<Vector2> scratches = new List<Vector2>();
	}
}
