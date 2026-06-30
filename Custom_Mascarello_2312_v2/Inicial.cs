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

            bool liberacao = false;
            string mes = DateTime.Now.ToString().Substring(4, 1) + "wrf" + DateTime.Now.ToString().Substring(3, 1);
            string ano = DateTime.Now.ToString().Substring(9, 1) + "wrf" + DateTime.Now.ToString().Substring(8, 1);

            // MessageBox.Show(mes + "    "  + ano);
            string compare_mes = mes.Substring(1, 1) + "wrf" + mes.Substring(0, 1);
            string compare_ano = ano.Substring(1, 1) + "wrf" + ano.Substring(0, 1);
            StreamReader str;
            if (System.IO.File.Exists(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\CUSTOMIZACOES\new\new_in.txt"))
            {

                str = new StreamReader(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\CUSTOMIZACOES\new\new_in.txt");
                using (str)
                {
                    string linha = "";
                    while ((linha = str.ReadLine()) != null)
                    {
                        string busca_ano = linha.Substring(0, 5);
                        string busca_mes = linha.Substring(linha.Length - 5, 5);

                        string senha_in = "";
                        // MessageBox.Show(busca_mes + "   " + busca_ano);

                        if (busca_mes == mes && busca_ano == ano)
                        {

                            senha_in = linha;
                            string senha_out = BitConverter.ToString(new System.Security.Cryptography.SHA512CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(senha_in)));

                            StreamReader str_out;
                            str_out = new StreamReader(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\CUSTOMIZACOES\new\new_out.txt");
                            using (str_out)
                            {
                                string linha_out = "";
                                while ((linha_out = str_out.ReadLine()) != null)
                                {
                                    // MessageBox.Show(linha_out + "\n>>>>>" + senha_out); 
                                    if (linha_out == senha_out)
                                    {
                                        liberacao = true;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Não foi possível acessar o esta Ferramenta \nEntre em contato com o Administrador", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            if (liberacao == true)
            {
                RodaInicial();
            }
            else
            {
                MessageBox.Show("Não foi possível acessar o esta Ferramenta \nEntre em contato com o Administrador", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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