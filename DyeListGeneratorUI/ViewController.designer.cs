// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DyeListGeneratorUI
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField CustomerOrdersFileNameLabel { get; set; }

		[Outlet]
		AppKit.NSTextField MasterDyeListFileNameLabel { get; set; }

		[Outlet]
		AppKit.NSTextField ProgramStatusLabel { get; set; }

		[Action ("CustomerOrdersFilePickerButtonClicked:")]
		partial void CustomerOrdersFilePickerButtonClicked (Foundation.NSObject sender);

		[Action ("GenerateDyeListButtonClicked:")]
		partial void GenerateDyeListButtonClicked (Foundation.NSObject sender);

		[Action ("MasterDyeListFilePickerButtonClicked:")]
		partial void MasterDyeListFilePickerButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CustomerOrdersFileNameLabel != null) {
				CustomerOrdersFileNameLabel.Dispose ();
				CustomerOrdersFileNameLabel = null;
			}

			if (MasterDyeListFileNameLabel != null) {
				MasterDyeListFileNameLabel.Dispose ();
				MasterDyeListFileNameLabel = null;
			}

			if (ProgramStatusLabel != null) {
				ProgramStatusLabel.Dispose ();
				ProgramStatusLabel = null;
			}
		}
	}
}
