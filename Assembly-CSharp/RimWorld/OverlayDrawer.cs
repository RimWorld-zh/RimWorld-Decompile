using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		private const int AltitudeIndex_Forbidden = 4;

		private const int AltitudeIndex_BurningWick = 5;

		private const int AltitudeIndex_QuestionMark = 6;

		private const float PulseFrequency = 4f;

		private const float PulseAmplitude = 0.7f;

		private const float StackOffsetMultipiler = 0.25f;

		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		private Vector3 curOffset;

		private static readonly Material ForbiddenMat;

		private static readonly Material NeedsPowerMat;

		private static readonly Material PowerOffMat;

		private static readonly Material QuestionMarkMat;

		private static readonly Material BrokenDownMat;

		private static readonly Material OutOfFuelMat;

		private static readonly Material WickMaterialA;

		private static readonly Material WickMaterialB;

		private static float SingleCellForbiddenOffset;

		private static readonly float BaseAlt;

		static OverlayDrawer()
		{
			OverlayDrawer.ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);
			OverlayDrawer.NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);
			OverlayDrawer.PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);
			OverlayDrawer.QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);
			OverlayDrawer.BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);
			OverlayDrawer.OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);
			OverlayDrawer.WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);
			OverlayDrawer.WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);
			OverlayDrawer.SingleCellForbiddenOffset = 0.3f;
			OverlayDrawer.BaseAlt = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
		}

		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (this.overlaysToDraw.ContainsKey(t))
			{
				Dictionary<Thing, OverlayTypes> dictionary;
				Dictionary<Thing, OverlayTypes> obj = dictionary = this.overlaysToDraw;
				Thing key;
				Thing key2 = key = t;
				OverlayTypes overlayTypes = dictionary[key];
				obj[key2] = (overlayTypes | overlayType);
			}
			else
			{
				this.overlaysToDraw.Add(t, overlayType);
			}
		}

		public void DrawAllOverlays()
		{
			Dictionary<Thing, OverlayTypes>.Enumerator enumerator = this.overlaysToDraw.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Thing, OverlayTypes> current = enumerator.Current;
					this.curOffset = Vector3.zero;
					Thing key = current.Key;
					OverlayTypes value = current.Value;
					if (((int)value & 4) != 0)
					{
						this.RenderBurningWick(key);
					}
					else
					{
						OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
						int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
						float num = this.StackOffsetFor(current.Key);
						switch (bitCountOf)
						{
						case 1:
						{
							this.curOffset = Vector3.zero;
							break;
						}
						case 2:
						{
							this.curOffset = new Vector3((float)(-0.5 * num), 0f, 0f);
							break;
						}
						case 3:
						{
							this.curOffset = new Vector3((float)(-1.5 * num), 0f, 0f);
							break;
						}
						}
						if (((int)value & 1) != 0)
						{
							this.RenderNeedsPowerOverlay(key);
						}
						if (((int)value & 2) != 0)
						{
							this.RenderPowerOffOverlay(key);
						}
						if (((int)value & 64) != 0)
						{
							this.RenderBrokenDownOverlay(key);
						}
						if (((int)value & 128) != 0)
						{
							this.RenderOutOfFuelOverlay(key);
						}
					}
					if (((int)value & 16) != 0)
					{
						this.RenderForbiddenBigOverlay(key);
					}
					if (((int)value & 8) != 0)
					{
						this.RenderForbiddenOverlay(key);
					}
					if (((int)value & 32) != 0)
					{
						this.RenderQuestionMarkOverlay(key);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			this.overlaysToDraw.Clear();
		}

		private float StackOffsetFor(Thing t)
		{
			IntVec2 rotatedSize = t.RotatedSize;
			return (float)((float)rotatedSize.x * 0.25);
		}

		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2);
		}

		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3);
		}

		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4);
		}

		private void RenderOutOfFuelOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 5);
		}

		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane);
		}

		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = (float)(OverlayDrawer.BaseAlt + 0.046875 * (float)altInd);
			vector += this.curOffset;
			this.curOffset.x += this.StackOffsetFor(thing);
			this.RenderPulsingOverlay(thing, mat, vector, mesh);
		}

		private void RenderPulsingOverlay(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = (float)((Time.realtimeSinceStartup + 397.0 * (float)(thing.thingIDNumber % 571)) * 4.0);
			float num2 = (float)(((float)Math.Sin((double)num) + 1.0) * 0.5);
			num2 = (float)(0.30000001192092896 + num2 * 0.699999988079071);
			Material material = FadedMaterialPool.FadedVersionOf(mat, num2);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}

		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			IntVec2 rotatedSize = t.RotatedSize;
			if (rotatedSize.z == 1)
			{
				drawPos.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				float z = drawPos.z;
				IntVec2 rotatedSize2 = t.RotatedSize;
				drawPos.z = (float)(z - (float)rotatedSize2.z * 0.30000001192092896);
			}
			drawPos.y = (float)(OverlayDrawer.BaseAlt + 0.1875);
			Graphics.DrawMesh(MeshPool.plane05, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = (float)(OverlayDrawer.BaseAlt + 0.1875);
			Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		private void RenderBurningWick(Thing parent)
		{
			Material material = (!(Rand.Value < 0.5)) ? OverlayDrawer.WickMaterialB : OverlayDrawer.WickMaterialA;
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = (float)(OverlayDrawer.BaseAlt + 0.234375);
			Graphics.DrawMesh(MeshPool.plane20, drawPos, Quaternion.identity, material, 0);
		}

		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = (float)(OverlayDrawer.BaseAlt + 0.28125);
			if (t is Pawn)
			{
				drawPos.x += (float)((float)t.def.size.x - 0.51999998092651367);
				drawPos.z += (float)((float)t.def.size.z - 0.44999998807907104);
			}
			this.RenderPulsingOverlay(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}
	}
}
