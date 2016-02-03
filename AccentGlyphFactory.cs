using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AccentOccurrences
{
	internal class AccentGlyphFactory : IGlyphFactory
	{
		const double glyphSize = 16.0;

		internal IEditorFormatMapService FormatMapService { get; set; }

		internal IWpfTextView View { get; set; }

		public UIElement GenerateGlyph(IWpfTextViewLine line, IGlyphTag tag)
		{
			// Ensure a glyph can be drawn for this marker
			if (tag == null || !(tag is AccentBoxOccurrences))
			{
				return null;
			}

			IEditorFormatMap map = FormatMapService.GetEditorFormatMap(View);
			ResourceDictionary dict = map.GetProperties(Constants.GLYPH_FORMAT_NAME);

			System.Windows.Shapes.Ellipse ellipse = new Ellipse();
			ellipse.Fill = (SolidColorBrush)dict[EditorFormatDefinition.BackgroundBrushId];
			ellipse.StrokeThickness = 2;
			ellipse.Stroke = (SolidColorBrush)dict[EditorFormatDefinition.ForegroundBrushId];
			ellipse.Height = glyphSize;
			ellipse.Width = glyphSize;

			return ellipse;
		}
	}
}