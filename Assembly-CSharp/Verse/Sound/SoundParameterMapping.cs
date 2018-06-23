using System;

namespace Verse.Sound
{
	// Token: 0x02000B9B RID: 2971
	public class SoundParameterMapping
	{
		// Token: 0x04002B38 RID: 11064
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam = null;

		// Token: 0x04002B39 RID: 11065
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam = null;

		// Token: 0x04002B3A RID: 11066
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode = SoundParamUpdateMode.Constant;

		// Token: 0x04002B3B RID: 11067
		[EditorHidden]
		public SimpleCurve curve;

		// Token: 0x06004061 RID: 16481 RVA: 0x0021D3AC File Offset: 0x0021B7AC
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0021D418 File Offset: 0x0021B818
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			string title = ((this.inParam == null) ? "null" : this.inParam.Label) + " -> " + ((this.outParam == null) ? "null" : this.outParam.Label);
			if (widgetRow.ButtonText("Edit curve", "Edit the curve mapping the in parameter to the out parameter.", true, false))
			{
				Find.WindowStack.Add(new EditWindow_CurveEditor(this.curve, title));
			}
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0021D4A0 File Offset: 0x0021B8A0
		public void Apply(Sample samp)
		{
			if (this.inParam != null && this.outParam != null)
			{
				float num = this.inParam.ValueFor(samp);
				float value = this.curve.Evaluate(num);
				this.outParam.SetOn(samp, value);
				if (UnityData.isDebugBuild && this.curve.HasView)
				{
					this.curve.View.SetDebugInput(samp, num);
				}
			}
		}
	}
}
