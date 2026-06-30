using System;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Windows.Forms;
using System.IO;
using System.Text;



public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    public static Program theProgram;
    public static bool isDisposeCalled;
    public static bool IsTc = false;
    public static string DAL_Default_Item_Type = "";

    //------------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------------
    public Program()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            isDisposeCalled = true;


        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }

    //------------------------------------------------------------------------------
    //  Explicit Activation
    //      This entry point is used to activate the application explicitly
    //------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int retValue = 0;
        try
        {
            theProgram = new Program();

            
           

            
                RodaInicial();
           
            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);

        }
        return retValue;
    }
    public static void RodaInicial()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjY5MzI0QDMyMzAyZTMyMmUzMEQ4b3RhTTFtN2QzMXJSS3hHZ2FmZXBpYU1WRklKWkhQUVRlY3FDQVZqMzg9");
        Custom_Mascarello.frmPrincipal abrir = new Custom_Mascarello.frmPrincipal();
        abrir.Show();

    }


    public void Dispose()
    {
        try
        {
            if (isDisposeCalled == false)
            {
                //TODO: Add your application code here 
            }
            isDisposeCalled = true;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);

        }
    }

    public static int GetUnloadOption(string arg)
    {
        //Unloads the image explicitly, via an unload dialog
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

        //Unloads the image immediately after execution within NX
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }



}