using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldCameraDriver : MonoBehaviour
	{
		public Quaternion sphereRotation = Quaternion.identity;

		private Vector2 rotationVelocity;

		private Vector2 desiredRotation;

		private float desiredAltitude;

		public float altitude;

		private Camera cachedCamera;

		private Vector2 mouseDragVect;

		private bool mouseCoveredByUI;

		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		private float rotationAnimation_lerpFactor = 1f;

		private const float SphereRadius = 100f;

		private const float ScreenDollyEdgeWidth = 20f;

		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		private const float DollyRateKeys = 170f;

		private const float DollyRateMouseDrag = 25f;

		private const float DollyRateScreenEdge = 125f;

		private const float CamRotationDecayFactor = 0.9f;

		private const float RotationSpeedScale = 0.3f;

		private const float MaxXRotationAtMinAltitude = 88.6f;

		private const float MaxXRotationAtMaxAltitude = 78f;

		private const float StartingAltitude_Playing = 160f;

		private const float StartingAltitude_Entry = 550f;

		public const float MinAltitude = 125f;

		private const float MaxAltitude = 1100f;

		private const float ZoomSpeed = 2.6f;

		private const float ZoomTightness = 0.4f;

		private const float ZoomScaleFromAltDenominator = 12f;

		private const float PageKeyZoomRate = 2f;

		private const float ScrollWheelZoomRate = 0.1f;

		private Camera MyCamera
		{
			get
			{
				if ((Object)this.cachedCamera == (Object)null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				if (altitudePercent < 0.02500000037252903)
				{
					return WorldCameraZoomRange.VeryClose;
				}
				if (altitudePercent < 0.041999999433755875)
				{
					return WorldCameraZoomRange.Close;
				}
				return WorldCameraZoomRange.Far;
			}
		}

		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(125f, 1100f, this.altitude);
			}
		}

		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldRenderedNow;
			}
		}

		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		public void OnGUI()
		{
			GUI.depth = 100;
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				UnityGUIBugsFixer.OnGUI();
				this.mouseCoveredByUI = false;
				if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
				{
					this.mouseCoveredByUI = true;
				}
				if (!this.AnythingPreventsCameraMotion)
				{
					if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
					{
						this.mouseDragVect = Event.current.delta;
						Event.current.Use();
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
					}
					float num = 0f;
					if (Event.current.type == EventType.ScrollWheel)
					{
						float num2 = num;
						Vector2 delta = Event.current.delta;
						num = (float)(num2 - delta.y * 0.10000000149011612);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapZoomIn.KeyDownEvent)
					{
						num = (float)(num + 2.0);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapZoomOut.KeyDownEvent)
					{
						num = (float)(num - 2.0);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					this.desiredAltitude -= (float)(num * 2.5999999046325684 * this.altitude / 12.0);
					this.desiredAltitude = Mathf.Clamp(this.desiredAltitude, 125f, 1100f);
					this.desiredRotation = Vector2.zero;
					if (KeyBindingDefOf.MapDollyLeft.IsDown)
					{
						this.desiredRotation.x = -170f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDollyRight.IsDown)
					{
						this.desiredRotation.x = 170f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDollyUp.IsDown)
					{
						this.desiredRotation.y = 170f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (KeyBindingDefOf.MapDollyDown.IsDown)
					{
						this.desiredRotation.y = -170f;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
					}
					if (this.mouseDragVect != Vector2.zero)
					{
						this.mouseDragVect *= CameraDriver.HitchReduceFactor;
						this.mouseDragVect.x *= -1f;
						this.desiredRotation += this.mouseDragVect * 25f;
						this.mouseDragVect = Vector2.zero;
					}
				}
			}
		}

		public void Update()
		{
			if (!LongEventHandler.ShouldWaitForEvent)
			{
				if (Find.World == null)
				{
					this.MyCamera.gameObject.SetActive(false);
				}
				else
				{
					if (!Find.WorldInterface.everReset)
					{
						Find.WorldInterface.Reset();
					}
					Vector2 lhs = this.CalculateCurInputDollyVect();
					if (lhs != Vector2.zero)
					{
						float d = (float)((this.altitude - 125.0) / 975.0 * 0.85000002384185791 + 0.15000000596046448);
						this.rotationVelocity = new Vector2(lhs.x, lhs.y) * d;
					}
					if (!this.AnythingPreventsCameraMotion)
					{
						float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
						this.sphereRotation *= Quaternion.AngleAxis((float)(this.rotationVelocity.x * num * 0.30000001192092896), this.MyCamera.transform.up);
						this.sphereRotation *= Quaternion.AngleAxis((float)((0.0 - this.rotationVelocity.y) * num * 0.30000001192092896), this.MyCamera.transform.right);
					}
					if (this.rotationVelocity != Vector2.zero)
					{
						this.rotationVelocity *= 0.9f;
						if (this.rotationVelocity.magnitude < 0.05000000074505806)
						{
							this.rotationVelocity = Vector2.zero;
						}
					}
					float num2 = this.desiredAltitude - this.altitude;
					this.altitude += (float)(num2 * 0.40000000596046448);
					this.rotationAnimation_lerpFactor += (float)(Time.deltaTime * 8.0);
					if (Find.PlaySettings.lockNorthUp)
					{
						this.RotateSoNorthIsUp(false);
						this.ClampXRotation(ref this.sphereRotation);
					}
					this.ApplyPositionToGameObject();
				}
			}
		}

		private void ApplyPositionToGameObject()
		{
			Quaternion rotation = (!(this.rotationAnimation_lerpFactor < 1.0)) ? this.sphereRotation : Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			this.MyCamera.transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = this.MyCamera.transform.rotation * Vector3.forward;
			this.MyCamera.transform.position = -a * this.altitude;
		}

		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0.0 && mousePositionOnUI.x < 20.0)
					{
						zero.x -= 125f;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20.0)
					{
						zero.x += 125f;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20.0)
					{
						zero.y += 125f;
					}
					if (mousePositionOnUI.y >= 0.0 && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0.0)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.2800000011920929)
						{
							zero.y -= 125f;
						}
						flag = true;
					}
					vector += zero;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
		}

		public void JumpTo(int tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
		}

		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90.0)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360.0 - num)
			{
				eulerAngles.x = (float)(360.0 - num);
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}
	}
}
