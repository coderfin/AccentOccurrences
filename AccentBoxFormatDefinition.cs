using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace AccentOccurrences
{
	[Export(typeof(EditorFormatDefinition))]
	[Name("MarkerFormatDefinition/AccentBoxFormatDefinition")] // Constants.BOX_FORMAT_NAME
	[Order(After = Priority.High)] // Overrides any default styles for the text, this ensures that the styles listed below are applied
	[UserVisible(true)]
	internal class AccentBoxFormatDefinition : MarkerFormatDefinition
	{
		// Sets the default styles for the box (border color)
		public AccentBoxFormatDefinition()
		{
			this.DisplayName = Constants.BOX_DISPLAY_NAME;

			this.BackgroundColor = Settings.Default.Fill;
			this.ForegroundColor = Settings.Default.Border;

			this.ZOrder = Int32.MaxValue;
		}
	}
}