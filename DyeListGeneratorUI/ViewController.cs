using System;
using AppKit;
using Foundation;
using System.IO;

namespace DyeListGeneratorUI
{
    public partial class ViewController : NSViewController
    {
        public FileInfo CustomerOrdersFile { private set; get; } = null;
        public FileInfo MasterDyeListFile { private set; get; } = null;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get { return base.RepresentedObject; }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void CustomerOrdersFilePickerButtonClicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = new NSOpenPanel();
            openPanel.AllowsMultipleSelection = false;
            openPanel.CanChooseDirectories = false;
            openPanel.CanCreateDirectories = false;
            openPanel.CanChooseFiles = true;

            long userInput = openPanel.RunModal();
            if (userInput == (long)NSModalResponse.OK)
            {
                String fileName = openPanel.Filenames[0];
                CustomerOrdersFile = new FileInfo(fileName);

                CustomerOrdersFileNameLabel.StringValue = CustomerOrdersFile.Name;
            }
        }

        partial void MasterDyeListFilePickerButtonClicked(Foundation.NSObject sender)
        {
            NSOpenPanel openPanel = new NSOpenPanel();
            openPanel.AllowsMultipleSelection = false;
            openPanel.CanChooseDirectories = false;
            openPanel.CanCreateDirectories = false;
            openPanel.CanChooseFiles = true;

            long userInput = openPanel.RunModal();
            if (userInput == (long)NSModalResponse.OK)
            {
                String fileName = openPanel.Filenames[0];
                MasterDyeListFile = new FileInfo(fileName);

                MasterDyeListFileNameLabel.StringValue = MasterDyeListFile.Name;
            }
        }

        partial void GenerateDyeListButtonClicked(Foundation.NSObject sender)
        {
            if (CustomerOrdersFile != null && MasterDyeListFile != null)
            {
                DyeListGenerator.DyeListGenerator.GenerateDyeList(new FileStream(CustomerOrdersFile.FullName, FileMode.Open), new FileStream(MasterDyeListFile.FullName, FileMode.Open));
            }
            else
            {
                ProgramStatusLabel.StringValue = "Error! Missing File";
                //ProgramStatusLabel.TextColor;
            }
        }
    }
}