
using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
namespace Custom_Mascarello
{
    public class Update_external
    {
        // class members
        static Session theSession = Session.GetSession();
        static UFSession theUFSession = UFSession.GetUFSession();
        static BasePart workPart = theSession.Parts.BaseWork;


        static void UpdateForExternalChange()
        {
            string ruleName = "";
            try
            {
               
                theUFSession.Cfi.GetUniqueFilename(out ruleName);
               // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ruleName);
                workPart.RuleManager.CreateDynamicRule("root:", ruleName,
                    "Any", "%ug_updateForExternalChange(false)", "");
                workPart.RuleManager.Evaluate(ruleName + ":");
                workPart.RuleManager.DeleteDynamicRule("root:", ruleName);
            }
            catch
            {

               
            }
            
          
        }
        public static string Main(string teste)
        {
           
            if (workPart != null)
            {
                UpdateForExternalChange();
               
            }

            for (int ii = 0; ii < 1; ii++)
            {
                //Echo("Processing: " + 1);
                try
                {
                    PartLoadStatus loadStatus;
                    workPart = theSession.Parts.OpenBaseDisplay("1", out loadStatus);
                    reportPartLoadStatus(loadStatus);

                    UpdateForExternalChange();

                    bool anyModified;
                    PartSaveStatus status;
                    theSession.Parts.SaveAll(out anyModified, out status);

                    workPart.Close(BasePart.CloseWholeTree.True,
                        BasePart.CloseModified.CloseModified, null);
                }
                catch
                {
                   /// Echo("   " + ex.Message);
                }
            }
            return "1";
        }
        static void reportPartSaveStatus(PartSaveStatus status)
        {
            if (status.NumberUnsavedParts == 0) return;

            Echo("  Save Notes:");

            for (int ii = 0; ii < status.NumberUnsavedParts; ii++)
            {
                NXException ex = NXException.Create(status.GetStatus(ii));
                Echo("  " + status.GetPart(ii) + " - " + ex.Message);
            }
        }
        static void Echo(string output)
        {
            theSession.ListingWindow.Open();
            theSession.ListingWindow.WriteLine(output);
            theSession.LogFile.WriteLine(output);
        }
        static void reportPartLoadStatus(PartLoadStatus load_status)
        {
            if (load_status.NumberUnloadedParts == 0) return;

            Echo("  Load notes:");

            for (int ii = 0; ii < load_status.NumberUnloadedParts; ii++)
            {
                Echo("  " + load_status.GetPartName(ii) + " - "
                        + load_status.GetStatusDescription(ii));
            }
        }
      

        public static int GetUnloadOption(string arg)
        {
            //Unloads the image explicitly, via an unload dialog
            //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

            //Unloads the image immediately after execution within NX
            return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

            //Unloads the image when the NX session terminates
            // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
        }
    }
}
