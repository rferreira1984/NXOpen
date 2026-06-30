using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using NXOpen.Features;

namespace Custom_Mascarello
{
    public partial class frm_Configurador_Teto : Form
    {
        
        decimal vao_disponivel = 0;
        decimal qtd_cav = 0;
        decimal qtd_cav_inicial = 0;
        decimal resto = 0;
        decimal resta_cav = 0;
        List<string> lista_cav = new List<string>(new string[] { });
        string MODELO_AC = "";
        string MODELO_CARROCERIA = "";
        string NORMA = "";
        string cav_1 = "";
        string cav_2 = "";
        string cav_3 = "";
        string cav_4 = "";
        string cav_5 = "";
        string cav_6 = "";
        string cav_7 = "";
        string cav_8 = "";
        string cav_9 = "";
        string cav_10 = "";
        string codigo_modulo_ac = "";
        
        public frm_Configurador_Teto()
        {
            InitializeComponent();
        }
        public string Modelo_AC { get; set; }
        public string carroceria { get; set; }
        public string norma { get; set; }

        private void frm_Configurador_Teto_Load(object sender, EventArgs e)
        {
          
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            Expression expression1 = (Expression)workPart.Expressions.FindObject("VAO_LIVRE");


            vao_disponivel = Convert.ToDecimal(expression1.Value);
            qtd_cav = Math.Floor(vao_disponivel / 860);

            resto = vao_disponivel - ((qtd_cav) * 860);

            qtd_cav_inicial = qtd_cav;
            this.lbl_qtd.Text = Convert.ToString(qtd_cav);
            this.lbl_resto.Text = Convert.ToString(resto);
            resta_cav = qtd_cav;
            MODELO_AC = this.Modelo_AC;
            MODELO_CARROCERIA = this.carroceria;
            NORMA = this.norma;
            //dtg_config_teto.Rows.Add();

            dtg_config_teto.Rows[0].Cells[0].Value = "1";
        }

        public void add_linha()
        {            
                int contador = dtg_config_teto.Rows.Count - 1;
                dtg_config_teto.Rows[contador].Cells[0].Value = Convert.ToString(contador + 1);
                resta_cav = resta_cav - 1;
                this.lbl_qtd.Text = Convert.ToString(resta_cav);
               
        }

        private void dtg_config_teto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                string teste = dtg_config_teto.CurrentCell.Value.ToString();
                if (teste == "AR_CONDICIONADO")
                {

                    for (int i = 0; i <= 1; i++)
                    {
                        int contador = dtg_config_teto.Rows.Count;
                        dtg_config_teto.Rows.Add(Convert.ToString(contador), "AR_CONDICIONADO");

                    }

                    for (int j = 2; j <= 4; j++)
                    {

                        dtg_config_teto.Rows[dtg_config_teto.Rows.Count - j].Cells[1].ReadOnly = true;
                    }
                }
            }
            catch (Exception)
            {


            }

            

        }

        private void dtg_config_teto_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           // MessageBox.Show(resta_cav.ToString());
            //if (resta_cav > 0)
            //{
                add_linha();
            //}
            //if (resta_cav == 0)
            //{
            //    // MessageBox.Show("BLOCK");
            //    dtg_config_teto.AllowUserToAddRows = false;
            //}     
        }
        public void ver_cavernas()
        {
            for (int i = 0; i <= dtg_config_teto.Rows.Count-1; i++)
            {
              lista_cav.Add(dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString());
            }
           
        }
        
        private void btn_aplicar_Click(object sender, EventArgs e)
        {
            if(NORMA == "SPTRANS")
            {
                NORMA = NORMA;
            }
            else
            {
                NORMA = "PADRAO";
            }
            if (MODELO_CARROCERIA == "MS3")
            {
                DataSet dadosXML = new DataSet();
                dadosXML.ReadXml(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S3\xml_teto.xml");

                foreach (DataRow dRow in dadosXML.Tables["ac"].Rows)
                {
                    if (MODELO_AC == dRow["Desc_ac_tarefa"].ToString() && NORMA == dRow["Norma"].ToString())
                    {
                        codigo_modulo_ac = dRow["codigo_modulo"].ToString();
                    }
                }
            }
            else
            {
                DataSet dadosXML = new DataSet();
                dadosXML.ReadXml(@"S:\Cad.Minibuss\CONFIGCAD_2014_MRP\DLL\NX\MICRO_S4\xml_teto.xml");

                foreach (DataRow dRow in dadosXML.Tables["ac"].Rows)
                {
                    if (MODELO_AC == dRow["Desc_ac_tarefa"].ToString())
                    {
                        codigo_modulo_ac = dRow["codigo_modulo"].ToString();
                    }
                }
            }
            ver_cavernas();

            List<string> CAV_ALCAPAO = new List<string>(new string[] {});

            decimal POS_ALC_1 = 0;
            decimal POS_ALC_2 = 0;

            int cont = 0;

            //for (int i = 0; i <= lista_cav.Count-2; i++)
            //{
                
            //     if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() == "ALCAPAO")
            //    {

            //        CAV_ALCAPAO.Add(Convert.ToString(i + 1));
            //        cont++;
            //    } 
            //}

            //if (CAV_ALCAPAO[0].ToString() == "1")
            //{
            //    POS_ALC_1 = 40;
            //}
            //else
            //{
            //    POS_ALC_1 = 40 + (860 * (Convert.ToInt32(CAV_ALCAPAO[0])-1));
            //}

          
            //if (cont  == 2)
	           // {
            //     POS_ALC_2 = 40 + (860 * (Convert.ToInt32(CAV_ALCAPAO[1]) - 1));               
	           // }

            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");


            Expression expression1 = (Expression)workPart.Expressions.FindObject("QTD_CAV");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("CAV_1");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("CAV_2");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("CAV_3");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("CAV_4");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("CAV_5");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("CAV_6");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("CAV_7");
            Expression expression9 = (Expression)workPart.Expressions.FindObject("CAV_8");
            Expression expression10 = (Expression)workPart.Expressions.FindObject("CAV_9");
            Expression expression11 = (Expression)workPart.Expressions.FindObject("CAV_10");
            Expression expression12 = (Expression)workPart.Expressions.FindObject("POS_ALC_1");
            Expression expression13 = (Expression)workPart.Expressions.FindObject("POS_ALC_2");
            Expression expression14 = (Expression)workPart.Expressions.FindObject("VAO_ULTIMA_CAV");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, Convert.ToString(qtd_cav));

            try
            {
                workPart.Expressions.EditWithUnits(expression2, null, "\"" + lista_cav[0] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression2, null, "\"" +"\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression3, null, "\"" + lista_cav[1] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression3, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression4, null, "\"" + lista_cav[2] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression4, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression5, null, "\"" + lista_cav[3] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression5, null, "\"" + "\"");
            }

            try
            {
                workPart.Expressions.EditWithUnits(expression6, null, "\"" + lista_cav[4] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression6, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression7, null, "\"" + lista_cav[5] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression7, null, "\"" + "\"");
            } try
            {
                workPart.Expressions.EditWithUnits(expression8, null, "\"" + lista_cav[6] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression8, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression9, null, "\"" + lista_cav[7] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression9, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression10, null, "\"" + lista_cav[8] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression10, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression11, null, "\"" + lista_cav[9] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression11, null, "\"" +  "\"");
            }

           
            workPart.Expressions.EditWithUnits(expression12, null, Convert.ToString(POS_ALC_1));
            workPart.Expressions.EditWithUnits(expression13, null, Convert.ToString(POS_ALC_2));
            workPart.Expressions.EditWithUnits(expression14, null, Convert.ToString(resto));


            try
            {
                Expression expression29 = (Expression)workPart.Expressions.FindObject("NORMA");
                string norma = "\"" + NORMA + "\"";
                workPart.Expressions.EditWithUnits(expression29, null, norma);
            }
            catch (Exception)
            {
            }
            theSession.Preferences.Modeling.UpdatePending = false;
           
            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId2, "Update Expression Data");

            theSession.Preferences.Modeling.UpdatePending = true;


            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            theSession.UpdateManager.DoInterpartUpdate(markId2);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId2);
            Add_tubo_arco_teto();
            Add_perfil_Z_Lateral();
            Add_perfil_Z_Central();
            Add_Modulo_Alcapao();
            Add_curva_teto();
            if (MODELO_AC != "3.1.1-NAO")
            {
                Add_modulo_ac();
            }
            //Hide_Features();
            MessageBox.Show("Configurações aplicadas");
        }
        public void Add_modulo_ac()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            Expression expression_Pos_ac = (Expression)workPart.Expressions.FindObject("POS_AC");
            double pos_ac = expression_Pos_ac.Value;
                    try
                    {
                        theSession.Parts.SetNonmasterSeedPartData("@DB/"+codigo_modulo_ac+"/000");

                        BasePart basePart1;
                        PartLoadStatus partLoadStatus1;
                        basePart1 = theSession.Parts.OpenBase("@DB/" + codigo_modulo_ac + "/000", out partLoadStatus1);

                        partLoadStatus1.Dispose();
                        int nErrs1;
                    }
                    catch (Exception)
                    {
                    }
                    Point3d basePoint1 = new Point3d(0.0, pos_ac, 0.0);
                    Matrix3x3 orientation1;
                    orientation1.Xx = 1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;
                    PartLoadStatus partLoadStatus2;
                    NXOpen.Assemblies.Component component1;

                    bool rev = false;
                    int rev_num = 0;
                    while (rev == false)
                     {
                   try
                    {
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + codigo_modulo_ac + "/"+ rev_num.ToString().PadLeft(3,'0'), "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
                    partLoadStatus2.Dispose();
                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                    rev = true;
                   }
                   catch
                   {
                    rev_num++;

                   }
            }
                    
                    
        }
        public void Add_tubo_arco_teto()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Expression expression_Inicio_Vao_Livre = (Expression)workPart.Expressions.FindObject("INICIO_VAO_LIVRE");
            double pos_inicio = expression_Inicio_Vao_Livre.Value;

            for (int i = 1; i <=  dtg_config_teto.RowCount-2; i++)
            {

                if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO" && i == 0)
                {
                    try
                    {
                        theSession.Parts.SetNonmasterSeedPartData("@DB/216137/000");

                        BasePart basePart1;
                        PartLoadStatus partLoadStatus1;
                        basePart1 = theSession.Parts.OpenBase("@DB/216137/000", out partLoadStatus1);

                        partLoadStatus1.Dispose();
                        int nErrs1;
                    }
                    catch (Exception)
                    {


                    }

                    Point3d basePoint1 = new Point3d(0.0, pos_inicio+20, 0.0);
                    Matrix3x3 orientation1;
                    orientation1.Xx = 1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;
                    PartLoadStatus partLoadStatus2;
                    NXOpen.Assemblies.Component component1;
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/216137/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }

                if (i > 0)
                {
                    if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO" && dtg_config_teto.Rows[i - 1].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO")
                    {
                        try
                        {
                            theSession.Parts.SetNonmasterSeedPartData("@DB/216137/000");

                            BasePart basePart1;
                            PartLoadStatus partLoadStatus1;
                            basePart1 = theSession.Parts.OpenBase("@DB/216137/000", out partLoadStatus1);

                            partLoadStatus1.Dispose();
                            int nErrs1;
                        }
                        catch (Exception)
                        {


                        }

                        Point3d basePoint1 = new Point3d(0.0, pos_inicio + (i * 860), 0.0);
                        Matrix3x3 orientation1;
                        orientation1.Xx = 1.0;
                        orientation1.Xy = 0.0;
                        orientation1.Xz = 0.0;
                        orientation1.Yx = 0.0;
                        orientation1.Yy = 1.0;
                        orientation1.Yz = 0.0;
                        orientation1.Zx = 0.0;
                        orientation1.Zy = 0.0;
                        orientation1.Zz = 1.0;
                        PartLoadStatus partLoadStatus2;
                        NXOpen.Assemblies.Component component1;
                        component1 = workPart.ComponentAssembly.AddComponent("@DB/216137/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

                        partLoadStatus2.Dispose();

                        NXObject[] objects1 = new NXObject[0];
                        int nErrs3;
                        nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                    }
                }
                if (i == dtg_config_teto.RowCount-2)
                {
                    if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO")
                    {
                        try
                        {
                            theSession.Parts.SetNonmasterSeedPartData("@DB/216137/000");

                            BasePart basePart1;
                            PartLoadStatus partLoadStatus1;
                            basePart1 = theSession.Parts.OpenBase("@DB/216137/000", out partLoadStatus1);

                            partLoadStatus1.Dispose();
                            int nErrs1;
                        }
                        catch (Exception)
                        {


                        }

                        Point3d basePoint1 = new Point3d(0.0, pos_inicio + ((i+1) * 860), 0.0);
                        Matrix3x3 orientation1;
                        orientation1.Xx = 1.0;
                        orientation1.Xy = 0.0;
                        orientation1.Xz = 0.0;
                        orientation1.Yx = 0.0;
                        orientation1.Yy = 1.0;
                        orientation1.Yz = 0.0;
                        orientation1.Zx = 0.0;
                        orientation1.Zy = 0.0;
                        orientation1.Zz = 1.0;
                        PartLoadStatus partLoadStatus2;
                        NXOpen.Assemblies.Component component1;
                        component1 = workPart.ComponentAssembly.AddComponent("@DB/216137/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

                        partLoadStatus2.Dispose();

                        NXObject[] objects1 = new NXObject[0];
                        int nErrs3;
                        nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                    }
                }
                
            }
   
        }
        public void Add_perfil_Z_Lateral()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Expression expression_Inicio_Vao_Livre = (Expression)workPart.Expressions.FindObject("INICIO_VAO_LIVRE");
            double pos_inicio = expression_Inicio_Vao_Livre.Value+40;
            double pos_inicio_le = expression_Inicio_Vao_Livre.Value + 860;
            for (int i = 0; i <= dtg_config_teto.RowCount - 2; i++)
            {
                    if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO")
                    {
                       try
                    {
                        theSession.Parts.SetNonmasterSeedPartData("@DB/216146/000");

                        BasePart basePart1;
                        PartLoadStatus partLoadStatus1;
                        basePart1 = theSession.Parts.OpenBase("@DB/216146/000", out partLoadStatus1);

                        partLoadStatus1.Dispose();
                        int nErrs1;
                    }
                    catch (Exception)
                    {


                    }

                    Point3d basePoint1 = new Point3d(-584.43, pos_inicio + (i*860), 1928.65);
                    Point3d basePoint2 = new Point3d(584.43, pos_inicio_le + (i*860), 1928.65);


                    Matrix3x3 orientation1;
                    orientation1.Xx = 0.997907649;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.064655421;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = -0.064655421;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.997907649;

                    Matrix3x3 orientation2;
                    orientation2.Xx = -0.997907649;
                    orientation2.Xy = 0.0;
                    orientation2.Xz = 0.064655421;
                    orientation2.Yx = 0.0;
                    orientation2.Yy = -1.0;
                    orientation2.Yz = 0.0;
                    orientation2.Zx = 0.064655421;
                    orientation2.Zy = 0.0;
                    orientation2.Zz = 0.997907649;

                  
                    PartLoadStatus partLoadStatus2;
                    NXOpen.Assemblies.Component component1;
                    NXOpen.Assemblies.Component component2;
                   component1 = workPart.ComponentAssembly.AddComponent("@DB/216146/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
                   component2 = workPart.ComponentAssembly.AddComponent("@DB/216146/000", "MODEL", "", basePoint2, orientation2, -1, out partLoadStatus2, true);
                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                    }
            }

        }
        public void Add_perfil_Z_Central()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Expression expression_Inicio_Vao_Livre = (Expression)workPart.Expressions.FindObject("INICIO_VAO_LIVRE");
            double pos_inicio = expression_Inicio_Vao_Livre.Value + 40;
            double pos_inicio_le = expression_Inicio_Vao_Livre.Value + 860;
            for (int i = 0; i <= dtg_config_teto.RowCount - 2; i++)
            {
                if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "AR_CONDICIONADO" && dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() != "ALCAPAO")
                {
                    try
                    {
                        theSession.Parts.SetNonmasterSeedPartData("@DB/216146/000");

                        BasePart basePart1;
                        PartLoadStatus partLoadStatus1;
                        basePart1 = theSession.Parts.OpenBase("@DB/216146/000", out partLoadStatus1);

                        partLoadStatus1.Dispose();
                        int nErrs1;
                    }
                    catch (Exception)
                    {


                    }

                    Point3d basePoint1 = new Point3d(-103.2, pos_inicio + (i * 860), 1950.8);
                    
                    Matrix3x3 orientation1;
                    orientation1.Xx = 1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;
             
                    PartLoadStatus partLoadStatus2;
                    NXOpen.Assemblies.Component component1;
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/216146/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);
             
                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
            }

        }
        public void Add_curva_teto()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Expression expression_Inicio_Vao_Livre = (Expression)workPart.Expressions.FindObject("INICIO_VAO_LIVRE");
            double pos_inicio = expression_Inicio_Vao_Livre.Value + 20 ;

            for (int i = 1; i <= dtg_config_teto.RowCount - 1; i++)
            {
                try
                {
                    theSession.Parts.SetNonmasterSeedPartData("@DB/220911/000");

                    BasePart basePart1;
                    PartLoadStatus partLoadStatus1;
                    basePart1 = theSession.Parts.OpenBase("@DB/220911/000", out partLoadStatus1);

                    partLoadStatus1.Dispose();
                    int nErrs1;
                }
                catch (Exception)
                {


                }

                Point3d basePoint1 = new Point3d(-902.1, pos_inicio + (i * 860), 1733.4);
                Point3d basePoint2 = new Point3d(902.1, pos_inicio + (i * 860), 1733.4);

                Matrix3x3 orientation1;
                orientation1.Xx = 0.059533116;
                orientation1.Xy = 0.0;
                orientation1.Xz = 0.998226331;
                orientation1.Yx = 0.0;
                orientation1.Yy = 1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = -0.998226331;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.059533116;

                Matrix3x3 orientation2;
                orientation2.Xx = -0.059533116;
                orientation2.Xy = 0.0;
                orientation2.Xz = 0.998226331;
                orientation2.Yx = 0.0;
                orientation2.Yy = -1.0;
                orientation2.Yz = 0.0;
                orientation2.Zx = 0.998226331;
                orientation2.Zy = 0.0;
                orientation2.Zz = 0.059533116;

                PartLoadStatus partLoadStatus2;
                NXOpen.Assemblies.Component component1;
                component1 = workPart.ComponentAssembly.AddComponent("@DB/220911/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

                NXOpen.Assemblies.Component component2;
                component2 = workPart.ComponentAssembly.AddComponent("@DB/220911/000", "MODEL", "", basePoint2, orientation2, -1, out partLoadStatus2, true);
                partLoadStatus2.Dispose();

                NXObject[] objects1 = new NXObject[0];
                int nErrs3;
                nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
            } 

        }
        public void Add_Modulo_Alcapao()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            Expression expression_Inicio_Vao_Livre = (Expression)workPart.Expressions.FindObject("INICIO_VAO_LIVRE");
            double pos_inicio = expression_Inicio_Vao_Livre.Value;
           
            for (int i = 0; i <= dtg_config_teto.RowCount - 2; i++)
            {
                if ( dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() == "ALCAPAO")
                {
                    try
                    {
                        theSession.Parts.SetNonmasterSeedPartData("@DB/216559/000");

                        BasePart basePart1;
                        PartLoadStatus partLoadStatus1;
                        basePart1 = theSession.Parts.OpenBase("@DB/216559/000", out partLoadStatus1);

                        partLoadStatus1.Dispose();
                        int nErrs1;
                    }
                    catch (Exception)
                    {


                    }

                    Point3d basePoint1 = new Point3d(0.0, pos_inicio + (i * 860), 0.0);

                    Matrix3x3 orientation1;
                    orientation1.Xx = 1.0;
                    orientation1.Xy = 0.0;
                    orientation1.Xz = 0.0;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = 0.0;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 1.0;

                    PartLoadStatus partLoadStatus2;
                    NXOpen.Assemblies.Component component1;
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/216559/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus2, true);

                    partLoadStatus2.Dispose();

                    NXObject[] objects1 = new NXObject[0];
                    int nErrs3;
                    nErrs3 = theSession.UpdateManager.AddToDeleteList(objects1);
                }
            }

        }
        public void Hide_Features()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;


            DisplayableObject[] objects1 = new DisplayableObject[12];
            Sketch sketch1 = (Sketch)workPart.Sketches.FindObject("SKETCH_001");
            objects1[0] = sketch1;
            Sketch sketch2 = (Sketch)workPart.Sketches.FindObject("SKETCH_002");
            objects1[1] = sketch2;
            Sketch sketch3 = (Sketch)workPart.Sketches.FindObject("_");
            objects1[2] = sketch3;
            Sketch sketch4 = (Sketch)workPart.Sketches.FindObject("1");
            objects1[3] = sketch4;
            Sketch sketch5 = (Sketch)workPart.Sketches.FindObject("2");
            objects1[4] = sketch5;
            Sketch sketch6 = (Sketch)workPart.Sketches.FindObject("SKETCH_002_0");
            objects1[5] = sketch6;
            Sketch sketch7 = (Sketch)workPart.Sketches.FindObject("SKETCH_002_0_0");
            objects1[6] = sketch7;
            Sketch sketch8 = (Sketch)workPart.Sketches.FindObject("SKETCH_002_0_1");
            objects1[7] = sketch8;
            Sketch sketch9 = (Sketch)workPart.Sketches.FindObject("SKETCH_002_0_2");
            objects1[8] = sketch9;
            Sketch sketch10 = (Sketch)workPart.Sketches.FindObject("SKETCH_002_0_3");
            objects1[9] = sketch10;
            Sketch sketch11 = (Sketch)workPart.Sketches.FindObject("SKETCH_004");
            objects1[10] = sketch11;
            Sketch sketch12 = (Sketch)workPart.Sketches.FindObject("SKETCH_005");
            objects1[11] = sketch12;
            theSession.DisplayManager.BlankObjects(objects1);

            workPart.ModelingViews.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);
        }
        private void btn_Preview_Click(object sender, EventArgs e)
        {
            ver_cavernas();

            List<string> CAV_ALCAPAO = new List<string>(new string[] { });

            decimal POS_ALC_1 = 0;
            decimal POS_ALC_2 = 0;

            int cont = 0;

            //for (int i = 0; i <= lista_cav.Count - 2; i++)
            //{
               
            //    if (dtg_config_teto.Rows[i].Cells[1].FormattedValue.ToString() == "ALCAPAO")
            //    {

            //        CAV_ALCAPAO.Add(Convert.ToString(i + 1));
            //        cont++;
            //    }
            //}

            //if (CAV_ALCAPAO[0].ToString() == "1")
            //{
            //    POS_ALC_1 = 40;
            //}
            //else
            //{
            //    POS_ALC_1 = 40 + (860 * (Convert.ToInt32(CAV_ALCAPAO[0]) - 1));
            //}

            //if (cont == 2)
            //{


            //    POS_ALC_2 = 40 + (860 * (Convert.ToInt32(CAV_ALCAPAO[1]) - 1));

            //}
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");


            Expression expression1 = (Expression)workPart.Expressions.FindObject("QTD_CAV");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("CAV_1");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("CAV_2");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("CAV_3");
            Expression expression5 = (Expression)workPart.Expressions.FindObject("CAV_4");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("CAV_5");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("CAV_6");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("CAV_7");
            Expression expression9 = (Expression)workPart.Expressions.FindObject("CAV_8");
            Expression expression10 = (Expression)workPart.Expressions.FindObject("CAV_9");
            Expression expression11 = (Expression)workPart.Expressions.FindObject("CAV_10");
            Expression expression12 = (Expression)workPart.Expressions.FindObject("POS_ALC_1");
            Expression expression13 = (Expression)workPart.Expressions.FindObject("POS_ALC_2");
            Expression expression14 = (Expression)workPart.Expressions.FindObject("VAO_ULTIMA_CAV");
            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, Convert.ToString(qtd_cav));

            try
            {
                workPart.Expressions.EditWithUnits(expression2, null, "\"" + lista_cav[0] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression2, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression3, null, "\"" + lista_cav[1] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression3, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression4, null, "\"" + lista_cav[2] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression4, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression5, null, "\"" + lista_cav[3] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression5, null, "\"" + "\"");
            }

            try
            {
                workPart.Expressions.EditWithUnits(expression6, null, "\"" + lista_cav[4] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression6, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression7, null, "\"" + lista_cav[5] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression7, null, "\"" + "\"");
            } try
            {
                workPart.Expressions.EditWithUnits(expression8, null, "\"" + lista_cav[6] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression8, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression9, null, "\"" + lista_cav[7] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression9, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression10, null, "\"" + lista_cav[8] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression10, null, "\"" + "\"");
            }
            try
            {
                workPart.Expressions.EditWithUnits(expression11, null, "\"" + lista_cav[9] + "\"");
            }
            catch (Exception)
            {
                workPart.Expressions.EditWithUnits(expression11, null, "\"" + "\"");
            }
            workPart.Expressions.EditWithUnits(expression12, null, Convert.ToString(POS_ALC_1));
            workPart.Expressions.EditWithUnits(expression13, null, Convert.ToString(POS_ALC_2));
            workPart.Expressions.EditWithUnits(expression14, null, Convert.ToString(resto));
            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            theSession.DeleteUndoMark(markId2, "Update Expression Data");

            theSession.Preferences.Modeling.UpdatePending = true;


            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

            theSession.UpdateManager.DoInterpartUpdate(markId2);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId2);

            lista_cav.Clear();
            CAV_ALCAPAO.Clear();
            cav_1 = "";
             cav_2 = "";
             cav_3 = "";
             cav_4 = "";
             cav_5 = "";
             cav_6 = "";
             cav_7 = "";
             cav_8 = "";
             cav_9 = "";
             cav_10 = "";
            MessageBox.Show("Configurações aplicadas");
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
          
            dtg_config_teto.ClearSelection();

            this.lbl_qtd.Text = Convert.ToString(qtd_cav_inicial);

            while (dtg_config_teto.Rows.Count > 1)
            {
                int i = 0;
                dtg_config_teto.Rows.Remove(dtg_config_teto.Rows[i]);
                i++;
            }
            // resta_cav = 0;

            resta_cav = qtd_cav;

            dtg_config_teto.Rows[0].Cells[0].Value = "1";
        }   
    }
}
