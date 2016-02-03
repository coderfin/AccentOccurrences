using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace AccentOccurrences
{
	[ContentType("any")]
	[Export(typeof(IGlyphFactoryProvider))]
	[Name("AccentGlyph")]
	[Order(After = "VsTextMarker")]
	[TagType(typeof(AccentBoxOccurrences))]
	internal sealed class AccentGlyphFactoryProvider : IGlyphFactoryProvider
	{
		[Import]
		internal IEditorFormatMapService FormatMapService { get; set; }

		public IGlyphFactory GetGlyphFactory(IWpfTextView view, IWpfTextViewMargin margin)
		{
			AccentGlyphFactory factory = new AccentGlyphFactory();
			factory.View = view;
			factory.FormatMapService = FormatMapService;

			return factory;
		}
	}
}