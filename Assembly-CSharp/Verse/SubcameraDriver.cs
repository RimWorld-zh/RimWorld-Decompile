using UnityEngine;

namespace Verse
{
	public class SubcameraDriver : MonoBehaviour
	{
		private Camera[] subcameras;

		public void Init()
		{
			if (this.subcameras == null && PlayDataLoader.Loaded)
			{
				Camera camera = Find.Camera;
				this.subcameras = new Camera[DefDatabase<SubcameraDef>.DefCount];
				foreach (SubcameraDef item in DefDatabase<SubcameraDef>.AllDefsListForReading)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = item.defName;
					gameObject.transform.parent = base.transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localRotation = Quaternion.identity;
					Camera camera2 = gameObject.AddComponent<Camera>();
					camera2.orthographic = camera.orthographic;
					camera2.orthographicSize = camera.orthographicSize;
					if (item.layer.NullOrEmpty())
					{
						camera2.cullingMask = 0;
					}
					else
					{
						camera2.cullingMask = LayerMask.GetMask(item.layer);
					}
					camera2.nearClipPlane = camera.nearClipPlane;
					camera2.farClipPlane = camera.farClipPlane;
					camera2.useOcclusionCulling = camera.useOcclusionCulling;
					camera2.allowHDR = camera.allowHDR;
					camera2.renderingPath = camera.renderingPath;
					camera2.clearFlags = CameraClearFlags.Color;
					camera2.backgroundColor = new Color(0f, 0f, 0f, 0f);
					camera2.depth = (float)item.depth;
					this.subcameras[item.index] = camera2;
				}
			}
		}

		public void UpdatePositions(Camera camera)
		{
			if (this.subcameras != null)
			{
				for (int i = 0; i < this.subcameras.Length; i++)
				{
					this.subcameras[i].orthographicSize = camera.orthographicSize;
					RenderTexture renderTexture = this.subcameras[i].targetTexture;
					if ((Object)renderTexture != (Object)null && (renderTexture.width != Screen.width || renderTexture.height != Screen.height))
					{
						Object.Destroy(renderTexture);
						renderTexture = null;
					}
					if ((Object)renderTexture == (Object)null)
					{
						renderTexture = new RenderTexture(Screen.width, Screen.height, 0, DefDatabase<SubcameraDef>.AllDefsListForReading[i].BestFormat);
					}
					if (!renderTexture.IsCreated())
					{
						renderTexture.Create();
					}
					this.subcameras[i].targetTexture = renderTexture;
				}
			}
		}

		public Camera GetSubcamera(SubcameraDef def)
		{
			if (this.subcameras != null && def != null && this.subcameras.Length > def.index)
			{
				return this.subcameras[def.index];
			}
			return null;
		}
	}
}
