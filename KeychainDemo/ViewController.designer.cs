// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace KeychainDemo
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MyLabel { get; set; }

        [Action ("ReadBtnClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ReadBtnClick (UIKit.UIButton sender);

        [Action ("RemoveBtnClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RemoveBtnClick (UIKit.UIButton sender);

        [Action ("WriteBtnClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void WriteBtnClick (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (MyLabel != null) {
                MyLabel.Dispose ();
                MyLabel = null;
            }
        }
    }
}