using CoreFoundation;
using Foundation;
using Security;
using System;
using UIKit;

namespace KeychainDemo
{
    public partial class ViewController : UIViewController
    {
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

        partial void ReadBtnClick(UIKit.UIButton sender)
        {
            MyLabel.Text = GetValueFromAccountAndKey("AccountName", "App.AppName");
        }

        partial void RemoveBtnClick(UIKit.UIButton sender)
        {

        }

        partial void WriteBtnClick(UIKit.UIButton sender)
        {
            SetValueForKeyAndAccount("MyValue", "AccountName", "App.AppName");
        }

        private SecRecord CreateRecordForNewKeyValue(string accountName, string value)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Service = "App.AppName",
                Account = accountName,
                ValueData = NSData.FromString(value, NSStringEncoding.UTF8),
                Accessible = SecAccessible.Always //This line of code is newly added.
            };
        }



        private SecRecord ExistingRecordForKey(string accountName)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Service = "App.AppName",
                Account = accountName,
                Accessible = SecAccessible.Always //This line of code is newly added. 
            };
        }



        public void SetValueForKeyAndAccount(string value, string accountName, string key)
        {
            var record = ExistingRecordForKey(accountName);
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(GetValueFromAccountAndKey(accountName, key)))
                        RemoveRecord(record);
                    return;
                }
                // if the key already exists, remove it before set value
                if (!string.IsNullOrEmpty(GetValueFromAccountAndKey(accountName, key)))
                    RemoveRecord(record);
            }
            catch (Exception e)
            {
                //Log exception here -("RemoveRecord Failed " + accountName, e,);
            }
            //Adding new record values to keychain
            var result = SecKeyChain.Add(CreateRecordForNewKeyValue(accountName, value));
            if (result != SecStatusCode.Success)
            {
                if (result == SecStatusCode.DuplicateItem)
                {
                    try
                    {
                        //Log exception here -("Error adding record: {0} for Account-" + accountName, result), "Try Remove account");
                        RemoveRecord(record);
                    }
                    catch (Exception e)
                    {
                        //Log exception here -("RemoveRecord Failed  after getting error SecStatusCode.DuplicateItem for Account-" + accountName, e);
                    }
                }
                else
                    throw new Exception(string.Format("Error adding record: {0} for Account-" + accountName, result));
            }
        }

        void RemoveRecord(SecRecord record)
        {
            SecKeyChain.Remove(record);
        }

        //string GetValueFromAccountAndKey(string accoundName, string service)
        //{
        //    var securityRecord = new SecRecord(SecKind.GenericPassword)
        //    {
        //        Service = service,
        //        Account = accoundName
        //    };

        //    SecStatusCode status;
        //    NSData resultData = SecKeyChain.QueryAsData(securityRecord, false, out status);

        //    var result = resultData != null ? new NSString(resultData, NSStringEncoding.UTF8) : "Not found";

        //    return result;
        //}

        public string GetValueFromAccountAndKey(string accountName, string key)
        {
            try
            {
                var record = ExistingRecordForKey(accountName);
                SecStatusCode resultCode;
                var match = SecKeyChain.QueryAsRecord(record, out resultCode);
                if (resultCode == SecStatusCode.Success)
                {
                    if (match.ValueData != null)
                    {
                        string valueData = NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
                        if (string.IsNullOrEmpty(valueData))
                            return string.Empty;
                        return valueData;
                    }
                    else if (match.Generic != null)
                    {
                        string valueData = NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
                        if (string.IsNullOrEmpty(valueData))
                            return string.Empty;
                        return valueData;
                    }
                    else
                        return string.Empty;
                }
            }
            catch (Exception e)
            {
                // Exception logged here -("iOS Keychain Error for account-" + accountName, e);
            }
            return string.Empty;
        }
    }
}