using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000683 RID: 1667
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x0012D9E8 File Offset: 0x0012BDE8
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (this.overlaysToDraw.ContainsKey(t))
			{
				Dictionary<Thing, OverlayTypes> dictionary;
				(dictionary = this.overlaysToDraw)[t] = (dictionary[t] | overlayType);
			}
			else
			{
				this.overlaysToDraw.Add(t, overlayType);
			}
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0012DA34 File Offset: 0x0012BE34
		public void DrawAllOverlays()
		{
			foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair in this.overlaysToDraw)
			{
				this.curOffset = Vector3.zero;
				Thing key = keyValuePair.Key;
				OverlayTypes value = keyValuePair.Value;
				if ((value & OverlayTypes.BurningWick) != (OverlayTypes)0)
				{
					this.RenderBurningWick(key);
				}
				else
				{
					OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
					int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
					float num = this.StackOffsetFor(keyValuePair.Key);
					if (bitCountOf != 1)
					{
						if (bitCountOf != 2)
						{
							if (bitCountOf == 3)
							{
								this.curOffset = new Vector3(-1.5f * num, 0f, 0f);
							}
						}
						else
						{
							this.curOffset = new Vector3(-0.5f * num, 0f, 0f);
						}
					}
					else
					{
						this.curOffset = Vector3.zero;
					}
					if ((value & OverlayTypes.NeedsPower) != (OverlayTypes)0)
					{
						this.RenderNeedsPowerOverlay(key);
					}
					if ((value & OverlayTypes.PowerOff) != (OverlayTypes)0)
					{
						this.RenderPowerOffOverlay(key);
					}
					if ((value & OverlayTypes.BrokenDown) != (OverlayTypes)0)
					{
						this.RenderBrokenDownOverlay(key);
					}
					if ((value & OverlayTypes.OutOfFuel) != (OverlayTypes)0)
					{
						this.RenderOutOfFuelOverlay(key);
					}
				}
				if ((value & OverlayTypes.ForbiddenBig) != (OverlayTypes)0)
				{
					this.RenderForbiddenBigOverlay(key);
				}
				if ((value & OverlayTypes.Forbidden) != (OverlayTypes)0)
				{
					this.RenderForbiddenOverlay(key);
				}
				if ((value & OverlayTypes.QuestionMark) != (OverlayTypes)0)
				{
					this.RenderQuestionMarkOverlay(key);
				}
			}
			this.overlaysToDraw.Clear();
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0012DBD8 File Offset: 0x0012BFD8
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x0012DC02 File Offset: 0x0012C002
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0012DC13 File Offset: 0x0012C013
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0012DC24 File Offset: 0x0012C024
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0012DC38 File Offset: 0x0012C038
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable == null) ? ThingDefOf.Chemfuel.uiIcon : compRefuelable.Props.FuelIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0012DC98 File Offset: 0x0012C098
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0012DCB8 File Offset: 0x0012C0B8
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.046875f * (float)altInd;
			vector += this.curOffset;
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0012DD1C File Offset: 0x0012C11C
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = (Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f;
			float num2 = ((float)Math.Sin((double)num) + 1f) * 0.5f;
			num2 = 0.3f + num2 * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num2);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0012DD84 File Offset: 0x0012C184
		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			if (t.RotatedSize.z == 1)
			{
				drawPos.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				drawPos.z -= (float)t.RotatedSize.z * 0.3f;
			}
			drawPos.y = OverlayDrawer.BaseAlt + 0.1875f;
			Graphics.DrawMesh(MeshPool.plane05, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0012DE10 File Offset: 0x0012C210
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.1875f;
			Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0012DE50 File Offset: 0x0012C250
		private void RenderBurningWick(Thing parent)
		{
			Material material;
			if ((parent.thingIDNumber + Find.TickManager.TicksGame) % 6 < 3)
			{
				material = OverlayDrawer.WickMaterialA;
			}
			else
			{
				material = OverlayDrawer.WickMaterialB;
			}
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.234375f;
			Graphics.DrawMesh(MeshPool.plane20, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0012DEB4 File Offset: 0x0012C2B4
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.28125f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x040013AC RID: 5036
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x040013AD RID: 5037
		private Vector3 curOffset;

		// Token: 0x040013AE RID: 5038
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x040013AF RID: 5039
		private static readonly Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B0 RID: 5040
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B1 RID: 5041
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B2 RID: 5042
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B3 RID: 5043
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B4 RID: 5044
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B5 RID: 5045
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x040013B6 RID: 5046
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x040013B7 RID: 5047
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x040013B8 RID: 5048
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x040013B9 RID: 5049
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x040013BA RID: 5050
		private const float PulseFrequency = 4f;

		// Token: 0x040013BB RID: 5051
		private const float PulseAmplitude = 0.7f;

		// Token: 0x040013BC RID: 5052
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x040013BD RID: 5053
		private const float StackOffsetMultipiler = 0.25f;
	}
}
