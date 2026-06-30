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
using NXOpen.UIStyler;
using NXOpen.UF;
using System.Collections;
using System.IO;

namespace Custom_Mascarello
{
    public partial class frm_Mapa_Jan_Borracha : Form
    {
        int count_le = 1;
        int count_ld = 1;
        string[] vaos_urb = { "740", "1000", "1148", "1300", "1405", "1427", "1490", "1570", "1640" };
        string[] vaos_rodo = { "740","840", "1000", "1148", "1200", "1231", "1250", "1300", "1380", "1405", "1427", "1490", "1570", "1640" };
        string[] vaos_fixo = { "305", "400", "430", "540", "561", "581", "640", "740", "1000", "1300", "1405", "1427", "1570", "1640" };
        public string inf_pt { get; set; }
        //public string jan { get; set; }
        string jan = "";

        public frm_Mapa_Jan_Borracha()
        {
            InitializeComponent();


        }
       
        private void btn_busca_Click(object sender, EventArgs e)
        {
           
            this.WindowState = FormWindowState.Minimized;
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject nullNXObject = null;
            MeasureDistanceBuilder measureDistanceBuilder1;
            measureDistanceBuilder1 = workPart.MeasureManager.CreateMeasureDistanceBuilder(nullNXObject);

            measureDistanceBuilder1.Mtype = NXOpen.MeasureDistanceBuilder.MeasureType.Minimum;

            theSession.SetUndoMarkName(markId1, "Measure Distance Dialog");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            DatumAxis datumAxis1 = (DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) Y axis");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(datumAxis1, Sense.Forward, NXOpen.SmartObject.UpdateOption.AfterModeling);

            DatumPlane datumPlane1 = (DatumPlane)workPart.Datums.FindObject("DATUM_CSYS(0) XY plane");

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face Face_1 = (Face)objects1[0];

            MeasureDistance dist_1 = workPart.MeasureManager.NewDistance(unit1, datumPlane1, Face_1, direction1);

            btn_busca.Text = dist_1.Value.ToString();
            measureDistanceBuilder1.Destroy();
            workPart.Expressions.Delete(expression1);

            this.WindowState = FormWindowState.Normal;
        }
        public static void SelectCurves(ref NXObject[] selectedObjects)
        {

            UI ui = NXOpen.UI.GetUI();

            string message = "Selecione o inicio do primeiro vão de janela";
            string title = "Selecione";

            Selection.SelectionScope scope = Selection.SelectionScope.AnyInAssembly;
            bool keepHighlighted = false;
            bool includeFeatures = false;
            Selection.Response response = default(Selection.Response);

            Selection.SelectionAction selectionAction = Selection.SelectionAction.ClearAndEnableSpecific;

            Selection.MaskTriple[] selectionMask_array = new Selection.MaskTriple[100];
            {
                selectionMask_array[0].Type = UFConstants.UF_face_type;
                //selectionMask_array[0].Subtype = UFConstants.uf_face;
                //selectionMask_array[0].SolidBodySubtype = UFConstants.UF_solid_type;
            }



            response = ui.SelectionManager.SelectObjects(message, title, scope, selectionAction, includeFeatures, keepHighlighted, selectionMask_array, out selectedObjects);

            if (response == Selection.Response.Cancel | response == Selection.Response.Back)
            {
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            count_le++;

            int x = count_le - 1;

            System.Windows.Forms.GroupBox gpr_1 = new System.Windows.Forms.GroupBox();

            gpr_1.Name = "grp_le_"+ count_le;
            gpr_1.Text = "VAO "+ count_le;
            gpr_1.Size = new Size(120, 95);
            gpr_1.BackColor =  Color.SkyBlue;
            int pos_x = 320 + (125 * (count_le-1));

           // MessageBox.Show(pos_x.ToString());
            gpr_1.Location = new System.Drawing.Point(pos_x, 70);
            
            this.Controls.Add(gpr_1);

           // System.Windows.Forms.ComboBox cmb = new System.Windows.Forms.ComboBox();
           // cmb.Name = "cmb_le_" + count_le;
           //// cmb.DataSource = vaos_urb;
           // cmb.Size = new Size(105, 21);
           // cmb.Location = new System.Drawing.Point(5, 22);
           // cmb.Enabled = false;
           // gpr_1.Controls.Add(cmb);
            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
            txt.Name = "cmb_le_" + count_le;
            // cmb.DataSource = vaos_urb;
            txt.Size = new Size(90, 20);
            txt.Location = new System.Drawing.Point(15, 20);
            txt.Enabled = false;
            gpr_1.Controls.Add(txt);

            System.Windows.Forms.CheckBox ckd = new System.Windows.Forms.CheckBox();

            ckd.Name = "ckd_le_" + count_le;
            ckd.Text = "VD FIXO";
            ckd.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

            // gpr_1.Text = "VAO " + count_le;
            ckd.Size = new Size(75,17);
            ckd.Location = new System.Drawing.Point(6, 69);
           
            gpr_1.Controls.Add(ckd);

            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();

            btn.Name = "btn_le_" + count_le;
            // gpr_1.Text = "VAO " + count_le;
            btn.Size = new Size(25, 20);
            btn.Location = new System.Drawing.Point(89, 48);
            btn.Image = Custom_Mascarello.Properties.Resources.cursor;
            btn.FlatStyle = FlatStyle.Popup;
            btn.BackColor = Color.White;
            btn.MouseClick += btn_vao;

            gpr_1.Controls.Add(btn);

            System.Windows.Forms.Button btn2 = new System.Windows.Forms.Button();
            btn2.Name = "btn_del_le_" + count_le;
            // gpr_1.Text = "VAO " + count_le;
            btn2.Size = new Size(25, 20);
            btn2.Location = new System.Drawing.Point(89, 72);
            btn2.Image = Custom_Mascarello.Properties.Resources.close;
            btn2.FlatStyle = FlatStyle.Popup;
            btn2.BackColor = Color.White;
            btn2.MouseClick += btn_del;

            gpr_1.Controls.Add(btn2);
            gpr_1.BringToFront();
        }
        private void btn_vao(object sender, EventArgs e)
        {
            
            this.WindowState = FormWindowState.Minimized;

            string name_var =((Button)sender).Name;

              
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Analysis->Measure Distance...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject nullNXObject = null;
            MeasureDistanceBuilder measureDistanceBuilder1;
            measureDistanceBuilder1 = workPart.MeasureManager.CreateMeasureDistanceBuilder(nullNXObject);

            measureDistanceBuilder1.Mtype = NXOpen.MeasureDistanceBuilder.MeasureType.Minimum;

            theSession.SetUndoMarkName(markId1, "Measure Distance Dialog");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            DatumAxis datumAxis1 = (DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) Y axis");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(datumAxis1, Sense.Forward, NXOpen.SmartObject.UpdateOption.AfterModeling);

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);

            Face face_1 = (Face)objects1[0];
            Face face_2 = (Face)objects1[1];

            MeasureDistance dist_1 = workPart.MeasureManager.NewDistance(unit1, face_2, face_1, direction1);
            measureDistanceBuilder1.Destroy();
            workPart.Expressions.Delete(expression1);

            foreach (var groupBox in Controls.OfType<System.Windows.Forms.GroupBox>())
            {
                if (groupBox.Name == "grp_" + name_var.Substring(name_var.Length - 4, 4))
                {
                    foreach (var textbox in groupBox.Controls.OfType<TextBox>())
                    {
                        decimal valor = Convert.ToDecimal(dist_1.Value.ToString());

                        valor=Math.Round(valor);
                        

                        textbox.Text = valor.ToString();
                        //writer.WriteElementString(comboBox.Name, comboBox.Text);
                    }
                }
            }
            this.WindowState = FormWindowState.Normal;

        }
        private void btn_del(object sender, EventArgs e)
        {

            //Form.ControlCollection.Controls.Clear();

            string name_var = ((Button)sender).Name;

           // MessageBox.Show(name_var);

            List<Control> toRemove = Controls.OfType<Control>().ToList();

            foreach (Control c in toRemove)
            {
                if (c.Name == "grp_" + name_var.Substring(name_var.Length - 4, 4))
                {
                    // MessageBox.Show("grp_" + name_var.Substring(name_var.Length - 4, 4));
                    Controls.Remove(c);
                    c.Dispose();
                }
                
            }
            
           

                //foreach (var foc in Controls.OfType<System.Windows.Forms.Form>())
                //{
                //    MessageBox.Show(name_var + "         " +groupBox.Name);

                //    if (groupBox.Name == "grp_" + name_var.Substring(name_var.Length - 4, 4))
                //    {//le_1
                //        MessageBox.Show("4444444444444444");
                //        groupBox.Controls.Remove(groupBox);
                //    }
                //}


            }
        private void btn_vao_tras(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Minimized;

            string name_var = ((Button)sender).Name;


            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Analysis->Measure Distance...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject nullNXObject = null;
            MeasureDistanceBuilder measureDistanceBuilder1;
            measureDistanceBuilder1 = workPart.MeasureManager.CreateMeasureDistanceBuilder(nullNXObject);

            measureDistanceBuilder1.Mtype = NXOpen.MeasureDistanceBuilder.MeasureType.Minimum;

            theSession.SetUndoMarkName(markId1, "Measure Distance Dialog");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            DatumAxis datumAxis1 = (DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) Y axis");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(datumAxis1, Sense.Forward, NXOpen.SmartObject.UpdateOption.AfterModeling);

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face face_1 = (Face)objects1[0];
            Face face_2 = (Face)objects1[1];

            MeasureDistance dist_1 = workPart.MeasureManager.NewDistance(unit1, face_2, face_1, direction1);

            double valor = Convert.ToDouble(dist_1.Value);
            measureDistanceBuilder1.Destroy();
            workPart.Expressions.Delete(expression1);

            foreach (var groupBox in Controls.OfType<System.Windows.Forms.GroupBox>())
            {
                if (groupBox.Name == "grp_t")
                {//le_1
                    foreach (var textbox in groupBox.Controls.OfType<TextBox>())
                    {
                        textbox.Text = Convert.ToString(Math.Round(valor));
                        //writer.WriteElementString(comboBox.Name, comboBox.Text);
                    }
                }
            }
            this.WindowState = FormWindowState.Normal;

        }

        private void add_vao_ld_Click(object sender, EventArgs e)
        {
            count_ld++;

            int x = count_ld - 1;
          //  565; 376
            System.Windows.Forms.GroupBox gpr_1 = new System.Windows.Forms.GroupBox();

            gpr_1.Name = "grp_ld_" + count_ld;
            gpr_1.Text = "VAO " + count_ld;
            gpr_1.Size = new Size(120, 95);
            gpr_1.BackColor = Color.SkyBlue;
            int pos_x = 565 - (125 * (count_ld - 1));

            // MessageBox.Show(pos_x.ToString());
            gpr_1.Location = new System.Drawing.Point(pos_x, 375);

            this.Controls.Add(gpr_1);

            System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
            txt.Name = "txt_ld_" + count_ld;
            //cmb.DataSource = vaos_urb;
            txt.Size = new Size(90, 20);
            txt.Location = new System.Drawing.Point(15,20);
            txt.Enabled = false;
            gpr_1.Controls.Add(txt);

            //System.Windows.Forms.ComboBox cmb = new System.Windows.Forms.ComboBox();
            //cmb.Name = "cmb_ld_" + count_ld;
            ////cmb.DataSource = vaos_urb;
            //cmb.Size = new Size(105, 21);
            //cmb.Location = new System.Drawing.Point(5, 22);
            //cmb.Enabled = false;
            //gpr_1.Controls.Add(cmb);
            System.Windows.Forms.CheckBox ckd = new System.Windows.Forms.CheckBox();

            ckd.Name = "ckd_ld_" + count_ld;
            ckd.Text = "VD FIXO";
            ckd.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);

            ckd.Size = new Size(75, 17);
            ckd.Location = new System.Drawing.Point(6, 69);

            gpr_1.Controls.Add(ckd);

            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();

            btn.Name = "btn_ld_" + count_ld;
            btn.Size = new Size(23, 23);
            btn.Location = new System.Drawing.Point(89, 48);
            btn.Image = Custom_Mascarello.Properties.Resources.cursor;
            btn.FlatStyle = FlatStyle.Popup;
            btn.BackColor = Color.White;
            btn.MouseClick += btn_vao;
            gpr_1.Controls.Add(btn);
           
            System.Windows.Forms.Button btn2 = new System.Windows.Forms.Button();
            btn2.Name = "btn_del_ld_" + count_le;
           //gpr_1.Text = "VAO " + count_le;
            btn2.Size = new Size(25, 20);
            btn2.Location = new System.Drawing.Point(89, 72);
            btn2.Image = Custom_Mascarello.Properties.Resources.close;
            btn2.FlatStyle = FlatStyle.Popup;
            btn2.BackColor = Color.White;
            btn2.MouseClick += btn_del;

            gpr_1.Controls.Add(btn2);
            gpr_1.BringToFront();
        }

        private void btn_busca_pc_ld_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject nullNXObject = null;
            MeasureDistanceBuilder measureDistanceBuilder1;
            measureDistanceBuilder1 = workPart.MeasureManager.CreateMeasureDistanceBuilder(nullNXObject);

            measureDistanceBuilder1.Mtype = NXOpen.MeasureDistanceBuilder.MeasureType.Minimum;

            theSession.SetUndoMarkName(markId1, "Measure Distance Dialog");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            DatumAxis datumAxis1 = (DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) Y axis");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(datumAxis1, Sense.Forward, NXOpen.SmartObject.UpdateOption.AfterModeling);

            DatumPlane datumPlane1 = (DatumPlane)workPart.Datums.FindObject("DATUM_CSYS(0) XY plane");

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face Face_1 = (Face)objects1[0];

            MeasureDistance dist_1 = workPart.MeasureManager.NewDistance(unit1, datumPlane1, Face_1, direction1);

            btn_busca_pc_ld.Text = dist_1.Value.ToString(); 
            measureDistanceBuilder1.Destroy();
            workPart.Expressions.Delete(expression1);

            this.WindowState = FormWindowState.Normal;
        }

        private void frm_Mapa_Jan_Borracha_Load(object sender, EventArgs e)
        {
           
            if (inf_pt == "1.4.1 - Não")
            {
                pictbox_ld.Image = Custom_Mascarello.Properties.Resources.LD_S3;
                grp_t.Visible = false;
                panel8.Visible = false;
                btn_busca_pt_ld.Visible = false;
                panel7.Visible = false;
                
            }
            else
            {              
                pictbox_ld.Image = Custom_Mascarello.Properties.Resources.LD_PT_S31;
            }
            string[] vaos_final;

            if (jan == "9.2.1 - Rodoviárias Com Vidros Fume")
            {

                vaos_final = vaos_rodo;
            }
            else
            {
                btn_aplicar_jan.Enabled = false;
                vaos_final = vaos_urb;
            }
        }
        private void trocar_vaos(object sender, EventArgs e)
        {
           
        }
        private void btn_aplicar_jan_Click(object sender, EventArgs e)
        {
           
            StreamReader str;
            str = new StreamReader(@"c:\temp\jan.txt");

            
            using (str)
            {

                string linha;
                while ((linha = str.ReadLine()) != null)
                {
                    jan = linha;
                }
            }
            str.Close();
            //MessageBox.Show(jan);
             this.WindowState = FormWindowState.Minimized;         
            add_jan_LE();
            add_jan_LD();
            if (inf_pt != "1.4.1 - Não")
            {
                add_jan_LD_PT();
            }
            this.WindowState = FormWindowState.Normal;
        }
        private void add_jan_LE()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            double puxador = 1;
            if (rbd_simples.Checked == true)
            {
                puxador = 0;
            }
            List<String> lista_le = new List<String>();
            foreach (var groupBox in Controls.OfType<System.Windows.Forms.GroupBox>())
            {
                if (groupBox.Name.Contains("grp_le"))
                {
                    foreach (var textbox in groupBox.Controls.OfType<TextBox>())
                    {
                        foreach (var check in groupBox.Controls.OfType<CheckBox>())
                        {
                            lista_le.Add(groupBox.Name + ";" + textbox.Text + ";" + check.CheckState.ToString());
                        }

                    }
                }
            }
            lista_le.Sort();

            double pos_le_ini = Convert.ToDouble(btn_busca.Text);
            double pos_le = Convert.ToDouble(btn_busca.Text);
            double pos_jan = 0;


            for (int i = 0; i <= lista_le.Count - 1; i++)
            {
                double pos_x = 1108.6511;
                double pos_z = 1215.9;

                string arquivo = "";
                string[] quebra = lista_le[i].Split(';');

                if (i == 0)
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = 1173.215832307;
                        pos_z = 749.744807188;
                        pos_jan = pos_le_ini+22.5;
                        pos_le_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_le_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_le_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }

                else
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = 1173.215832307;
                        pos_z = 749.744807188;
                        pos_jan = pos_le_ini+22.5;
                        pos_le_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_le_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_le_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }
                string cod_fam = "";
                if (jan == "9.2.8 - Com Vidros Fume Superiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "265549";
                    // MessageBox.Show("0");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (quebra[2] == "Checked")
                {
                    cod_fam = "265624";

                    // MessageBox.Show("2");
                    string[] expressao = { "VAO_JAN", "COR_VD" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                {
                    cod_fam = "265680";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (arquivo != "")
                {
                    Point3d basePoint1 = new Point3d(pos_x, pos_jan, pos_z);
                    Matrix3x3 orientation1;

                    orientation1.Xx = 0.996600608;
                    orientation1.Xy = 0.000000000;
                    orientation1.Xz = 0.082384633;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = 1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = -0.082384633;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.996600608;

                    PartLoadStatus partLoadStatus3;
                    NXOpen.Assemblies.Component component1;
                    try
                    {
                        component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }
                    catch
                    {

                        component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }


                    partLoadStatus3.Dispose();
                }
                else
                {
                    MessageBox.Show("Não existe janela para o vão e configuração solicitada"+"\n"+" VÃO: "+ quebra[1], "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }      
        }
        private void add_jan_LD()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            double puxador = 1;
            if (rbd_simples.Checked == true)
            {
                puxador = 0;
            }
            /// MessageBox.Show("LE");
            // jan le
            List<String> lista_ld = new List<String>();
            foreach (var groupBox in Controls.OfType<System.Windows.Forms.GroupBox>())
            {
                if (groupBox.Name.Contains("grp_ld"))
                {
                    foreach (var textbox in groupBox.Controls.OfType<TextBox>())
                    {
                        foreach (var check in groupBox.Controls.OfType<CheckBox>())
                        {
                            lista_ld.Add(groupBox.Name + ";" + textbox.Text + ";" + check.CheckState.ToString());
                            //writer.WriteElementString(comboBox.Name, comboBox.Text);
                        }

                    }
                }
            }
            lista_ld.Sort();

            double pos_ld_ini = Convert.ToDouble(btn_busca_pc_ld.Text); 
            double pos_ld = Convert.ToDouble(btn_busca_pc_ld.Text);
            double pos_jan = 0;


            for (int i = 0; i <= lista_ld.Count - 1; i++)
            {
                double pos_x = -1108.6511;
                double pos_z = 1215.9;

                string arquivo = "";
                string[] quebra = lista_ld[i].Split(';');

                if (i == 0)
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = -1173.149060208;
                        pos_z = 750.508024957;
                        pos_jan = pos_ld_ini + Convert.ToDouble(quebra[1])-22.5;
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_ld_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }

                else
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = -1173.149060208;
                        pos_z = 750.508024957;
                        pos_jan = pos_ld_ini + Convert.ToDouble(quebra[1])-22.5;
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_ld_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }
                // MessageBox.Show(lista_le[i]);
                string cod_fam = "";
                if (jan == "9.2.8 - Com Vidros Fume Superiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "265549";
                    // MessageBox.Show("0");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (quebra[2] == "Checked")
                {
                    cod_fam = "265624";

                    // MessageBox.Show("2");
                    string[] expressao = { "VAO_JAN", "COR_VD" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                {
                    cod_fam = "265680";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }


                if (arquivo != "")
                {

                    Point3d basePoint1 = new Point3d(pos_x, pos_jan, pos_z);
                    Matrix3x3 orientation1;

                    orientation1.Xx = -0.996600608;
                    orientation1.Xy = 0.000000000;
                    orientation1.Xz = 0.082384633;
                    orientation1.Yx = 0.0;
                    orientation1.Yy = -1.0;
                    orientation1.Yz = 0.0;
                    orientation1.Zx = -0.082384633;
                    orientation1.Zy = 0.0;
                    orientation1.Zz = 0.996600608;

                    PartLoadStatus partLoadStatus3;
                    NXOpen.Assemblies.Component component1;
                    try
                    {
                        component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }
                    catch
                    {

                        component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                    }
                

                partLoadStatus3.Dispose();
            }
                 else
                {
                    MessageBox.Show("Não existe janela para o vão e configuração solicitada" + "\n" + " VÃO: " + quebra[1], "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
        private void add_jan_LD_PT()
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            double puxador = 1;
            if (rbd_simples.Checked == true)
            {
                puxador = 0;
            }
            List<String> lista_ld = new List<String>();
            foreach (var groupBox in Controls.OfType<System.Windows.Forms.GroupBox>())
            {
                if (groupBox.Name.Contains("grp_t"))
                {
                    foreach (var textbox in groupBox.Controls.OfType<TextBox>())
                    {
                        foreach (var check in groupBox.Controls.OfType<CheckBox>())
                        {
                            lista_ld.Add(groupBox.Name + ";" + textbox.Text + ";" + check.CheckState.ToString());
                        }

                    }
                }
            }
            lista_ld.Sort();

            double pos_ld_ini = Convert.ToDouble(btn_busca_pt_ld.Text); 
            double pos_ld = Convert.ToDouble(btn_busca_pt_ld.Text);
            double pos_jan = 0;


            for (int i = 0; i <= lista_ld.Count - 1; i++)
            {
                double pos_x = -1108.6511;
                double pos_z = 1215.9;

                string arquivo = "";
                string[] quebra = lista_ld[i].Split(';');

                if (i == 0)
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = -1173.149060208;
                        pos_z = 750.508024957;
                        pos_jan = pos_ld_ini + Convert.ToDouble(quebra[1])-22.5;
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_ld_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }

                else
                {
                    if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                    {
                        pos_x = -1173.149060208;
                        pos_z = 750.508024957;
                        pos_jan = pos_ld_ini + Convert.ToDouble(quebra[1])-22.5;
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                    else
                    {
                        pos_jan = pos_ld_ini + (Convert.ToDouble(quebra[1]) / 2);
                        pos_ld_ini += (Convert.ToDouble(quebra[1]) + 60);
                    }
                }
                // MessageBox.Show(lista_le[i]);
                string cod_fam = "";
                if (jan == "9.2.8 - Com Vidros Fume Superiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "265549";
                    // MessageBox.Show("0");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis" && quebra[2] != "Checked")
                {
                    cod_fam = "264867";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN", "VD_COR", "PUXADOR" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 0, 1, puxador };
                    double[] tolerancia = { 0.1, 0.1, 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (quebra[2] == "Checked")
                {
                    cod_fam = "265624";

                    // MessageBox.Show("2");
                    string[] expressao = { "VAO_JAN", "COR_VD" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }
                if (jan == "9.2.1 - Rodoviárias Com Vidros Fume" && quebra[2] != "Checked")
                {
                    cod_fam = "265680";
                    ///MessageBox.Show("1");

                    string[] expressao = { "VAO_JAN", "LADO_JAN" };
                    double[] valor = { Convert.ToDouble(quebra[1]), 1 };
                    double[] tolerancia = { 0.1, 0.1 };
                    arquivo = Custom_Mascarello.Search.CreateMember(cod_fam, "", expressao, valor, 0, tolerancia, "", "");

                }


                if(arquivo!= "")
                { 

                Point3d basePoint1 = new Point3d(pos_x, pos_jan, pos_z);
                Matrix3x3 orientation1;

                orientation1.Xx = -0.996600608;
                orientation1.Xy = 0.000000000;
                orientation1.Xz = 0.082384633;
                orientation1.Yx = 0.0;
                orientation1.Yy = -1.0;
                orientation1.Yz = 0.0;
                orientation1.Zx = -0.082384633;
                orientation1.Zy = 0.0;
                orientation1.Zz = 0.996600608;

                PartLoadStatus partLoadStatus3;
                NXOpen.Assemblies.Component component1;
                try
                {
                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo, "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }
                catch
                {

                    component1 = workPart.ComponentAssembly.AddComponent("@DB/" + arquivo + "/000", "MODEL", "", basePoint1, orientation1, -1, out partLoadStatus3, true);
                }


                partLoadStatus3.Dispose();
            }

                 else
                {
                    MessageBox.Show("Não existe janela para o vão e configuração solicitada" + "\n" + " VÃO: " + quebra[1], "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void btn_busca_pt_ld_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            NXObject nullNXObject = null;
            MeasureDistanceBuilder measureDistanceBuilder1;
            measureDistanceBuilder1 = workPart.MeasureManager.CreateMeasureDistanceBuilder(nullNXObject);

            measureDistanceBuilder1.Mtype = NXOpen.MeasureDistanceBuilder.MeasureType.Minimum;

            theSession.SetUndoMarkName(markId1, "Measure Distance Dialog");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            Expression expression1;
            expression1 = workPart.Expressions.CreateSystemExpressionWithUnits("0", unit1);

            DatumAxis datumAxis1 = (DatumAxis)workPart.Datums.FindObject("DATUM_CSYS(0) Y axis");
            Direction direction1;
            direction1 = workPart.Directions.CreateDirection(datumAxis1, Sense.Forward, NXOpen.SmartObject.UpdateOption.AfterModeling);

            DatumPlane datumPlane1 = (DatumPlane)workPart.Datums.FindObject("DATUM_CSYS(0) XY plane");

            NXObject[] objects1 = null;
            SelectCurves(ref objects1);
            Face Face_1 = (Face)objects1[0];

            MeasureDistance dist_1 = workPart.MeasureManager.NewDistance(unit1, datumPlane1, Face_1, direction1);


            btn_busca_pt_ld.Text = dist_1.Value.ToString();
            measureDistanceBuilder1.Destroy();
            workPart.Expressions.Delete(expression1);

            this.WindowState = FormWindowState.Normal;

        }

        private void rbd_simples_CheckedChanged(object sender, EventArgs e)
        {
            btn_aplicar_jan.Enabled = true;
        }

        private void rbd_trava_CheckedChanged(object sender, EventArgs e)
        {
            btn_aplicar_jan.Enabled = true;
        }
    }
}
//9.2.5 - Com Vidros Fume Fixos Inteiros sem Janelinhas Basculantes - em Borracha(Somente Carros com Ar Condicionado)
//9.2.8 - Com Vidros Fume Superiores Móveis
//9.2.9 - Com Vidros Fume Superiores e Inferiores Móveis
//9.2.10 - Com Vidros Fume Superiores Móveis(Abertura Máxima 100mm)
//9.2.11 - Com Vidros Fume Superiores e Inferiores Móveis(Abertura Máxima 150mm )
//9.2.19 - Com Vidros Colados com Janelas Padrão Urbana 50% de Janela e o Restante de Vidro Transparente Com Vidros Fume
//9.2.20 - Com Vidros Fume Colados com Janelinhas Basculantes Conforme Planta Oficial