using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;


namespace Custom_Mascarello
{
    public partial class frm_Configurar_Bagageiros_M4 : Form
    {
        double vao_port_tras = 0;
        string vao_tampa_arla = "";
        frm_Configurador_Projeto_M4 instanciaDofrm_Configurador_Projeto_M4;
        public frm_Configurar_Bagageiros_M4()
        {
            InitializeComponent();

            

            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V" || _chassi == "IVECO - 170 S 28")
            {
                vao_tampa_arla = "1200";
            }
            else
            {
                vao_tampa_arla = "1100";
            }

           
            
        }

            public void padrao_tampas()
            {

                if (_chassi == "VW 17.260 OD - EURO V")
            {
                cmbox_vao_1_ld.Text = "BATERIA";
                cmbox_vao_1_le.Text = "BATERIA";
                cmbox_vao_1_ld.Enabled = false;
                cmbox_vao_1_le.Enabled = false;
            }
                if (_chassi == "IVECO - 170 S 28")
                {
                    cmbox_vao_1_ld.Text = "TQ_ARLA";
                    cmbox_vao_1_le.Text = "BATERIA";
                    cmbox_vao_2_ld.Text = "1100";
                    cmbox_vao_2_le.Text = "1600";
                    cmbox_vao_1_ld.Enabled = false;
                    cmbox_vao_1_le.Enabled = false;
                    cmbox_vao_2_ld.Enabled = false;
                    cmbox_vao_2_le.Enabled = false;

                }
                if (_chassi == "MBB OF-1724 EURO V")
                {
                    cmbox_vao_1_ld.Text = "TQ_ARLA";
                    cmbox_vao_1_le.Text = "1200";
                    cmbox_vao_2_ld.Text = "BATERIA";
                    cmbox_vao_2_le.Text = "1600";
                    cmbox_vao_3_ld.Text = "1000";
                    cmbox_vao_1_ld.Enabled = false;
                    cmbox_vao_1_le.Enabled = false;
                    cmbox_vao_2_ld.Enabled = false;
                    cmbox_vao_2_le.Enabled = false;
                    cmbox_vao_3_ld.Enabled = false;

                }
                if (_chassi == "VOLVO - B 270F (EURO V)")
                {
                    cmbox_vao_1_ld.Text = "TQ_ARLA";
                    cmbox_vao_1_le.Text = "1100";
                    cmbox_vao_2_ld.Text = "BATERIA";
                    cmbox_vao_2_le.Text = "1700";
                    cmbox_vao_3_ld.Text = "1100";
                    cmbox_vao_1_ld.Enabled = false;
                    cmbox_vao_1_le.Enabled = false;
                    cmbox_vao_2_ld.Enabled = false;
                    cmbox_vao_2_le.Enabled = false;
                    cmbox_vao_3_ld.Enabled = false;

                }
                if (_chassi == "VW 17.230 OD - EURO V")
                {
                    cmbox_vao_1_ld.Text = "BATERIA";
                    cmbox_vao_1_le.Text = "1600";
                    cmbox_vao_2_ld.Text = "1000";
                    
                    cmbox_vao_1_ld.Enabled = false;
                    cmbox_vao_1_le.Enabled = false;
                    cmbox_vao_2_ld.Enabled = false;

                }
            }
 

        private void frm_Configurar_Bagageiros_M4_Load(object sender, EventArgs e)
        {

            padrao_tampas();
            ckd_padrao_chassi.Checked = true;

            this.lbl_valor_chassi.Text = _chassi;
            this.lbl_valor_comp.Text = _ct;

            MessageBox.Show(_entre_eixo);
            this.lbl_valor_entre_eixo.Text = _entre_eixo;
        }

        public void bagageiros()
        {
            // lbl_proccess.Text = "Configurando Bagageiros...";



            //---------------------------------------------
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            /// BAG LD
            string vao_1_ld = "";
            string vao_2_ld = "";
            string vao_3_ld = cmbox_vao_3_ld.Text;
            string vao_4_ld = cmbox_vao_4_ld.Text;
            if (cmbox_vao_1_ld.Text == "TQ_ARLA")
            {
                vao_1_ld = vao_tampa_arla;
            }
            if (cmbox_vao_1_ld.Text == "BATERIA")
            {
                vao_1_ld = "600";
            }

            if (cmbox_vao_1_ld.Text == "BATERIA" && _chassi == "VW 17.260 OD - EURO V")
            {
                vao_1_ld = "1002";
            }
            if (cmbox_vao_1_ld.Text != "BATERIA" && cmbox_vao_1_ld.Text != "TQ_ARLA")
            {
                vao_1_ld = cmbox_vao_1_ld.Text;
            }
            if (cmbox_vao_2_ld.Text == "TQ_ARLA")
            {
                vao_2_ld = vao_tampa_arla;
            }
            if (cmbox_vao_2_ld.Text == "BATERIA")
            {
                vao_2_ld = "600";
            }
            if (cmbox_vao_2_ld.Text != "BATERIA" && cmbox_vao_2_ld.Text != "TQ_ARLA")
            {
                vao_2_ld = cmbox_vao_2_ld.Text;
            }
            if (cmbox_vao_4_ld.Text == "NÃO")
            {
                vao_4_ld = "0";
            }

            if (cmbox_vao_4_ld.Text != "NÃO")
            {
                vao_4_ld = cmbox_vao_4_ld.Text;
            }
            ///BAG LE
            string vao_1_le = "";
            string vao_2_le = "";
            string vao_3_le = cmbox_vao_3_le.Text;
            string vao_4_le = cmbox_vao_4_le.Text;
            if (cmbox_vao_1_le.Text == "TQ_ARLA")
            {
                vao_1_le = vao_tampa_arla;
            }
            if (cmbox_vao_1_le.Text == "BATERIA")
            {
                vao_1_le = "600";
            }
            if (cmbox_vao_1_le.Text == "BATERIA" && _chassi == "VW 17.260 OD - EURO V")
            {
                vao_1_le = "1002";
            }
            if (cmbox_vao_1_le.Text != "BATERIA" && cmbox_vao_1_le.Text != "TQ_ARLA")
            {
                vao_1_le = cmbox_vao_1_le.Text;
            }
            if (cmbox_vao_2_le.Text == "TQ_ARLA")
            {
                vao_2_le = vao_tampa_arla;
            }
            if (cmbox_vao_2_le.Text == "BATERIA")
            {
                vao_2_le = "600";
            }
            if (cmbox_vao_2_le.Text != "BATERIA" && cmbox_vao_2_le.Text != "TQ_ARLA")
            {
                vao_2_le = cmbox_vao_2_le.Text;
            }
            if (cmbox_vao_4_le.Text == "NÃO")
            {
                vao_4_le = "0";
            }

            if (cmbox_vao_4_le.Text != "NÃO")
            {
                vao_4_le = cmbox_vao_4_le.Text;
            }

            string vao_bag_tras = "";
            if (cmbox_vao_bag_tras.Text == "NÃO")
            {
                vao_bag_tras = "0";
            }

            if (cmbox_vao_bag_tras.Text != "NÃO")
            {
                vao_bag_tras = cmbox_vao_bag_tras.Text;
            }

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");
            Expression expression1 = (Expression)workPart.Expressions.FindObject("VAO_1_LD");
            Expression expression2 = (Expression)workPart.Expressions.FindObject("VAO_2_LD");
            Expression expression3 = (Expression)workPart.Expressions.FindObject("VAO_3_LD");
            Expression expression4 = (Expression)workPart.Expressions.FindObject("VAO_4_LD");
            Expression expression11 = (Expression)workPart.Expressions.FindObject("VAO_1_LD_VALUE"); 
            Expression expression12 = (Expression)workPart.Expressions.FindObject("VAO_2_LD_VALUE");

            Expression expression5 = (Expression)workPart.Expressions.FindObject("VAO_1_LE");
            Expression expression6 = (Expression)workPart.Expressions.FindObject("VAO_2_LE");
            Expression expression7 = (Expression)workPart.Expressions.FindObject("VAO_3_LE");
            Expression expression8 = (Expression)workPart.Expressions.FindObject("VAO_4_LE");
            Expression expression13 = (Expression)workPart.Expressions.FindObject("VAO_1_LE_VALUE");
            Expression expression14 = (Expression)workPart.Expressions.FindObject("VAO_2_LE_VALUE");
            
            Expression expression9 = (Expression)workPart.Expressions.FindObject("VAO_BAG_TRAS");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");
            workPart.Expressions.EditWithUnits(expression1, unit1, vao_1_ld);
            workPart.Expressions.EditWithUnits(expression2, unit1, vao_2_ld);
            workPart.Expressions.EditWithUnits(expression3, unit1, vao_3_ld);
            workPart.Expressions.EditWithUnits(expression4, unit1, vao_4_ld);
            workPart.Expressions.EditWithUnits(expression5, unit1, vao_1_le);
            workPart.Expressions.EditWithUnits(expression6, unit1, vao_2_le);
            workPart.Expressions.EditWithUnits(expression7, unit1, vao_3_le);
            workPart.Expressions.EditWithUnits(expression8, unit1, vao_4_le);
            workPart.Expressions.EditWithUnits(expression9, unit1, vao_bag_tras);


            string VAO_1_LD_VALUE = "\"" + cmbox_vao_1_ld.Text + "\"";
            string VAO_2_LD_VALUE = "\"" + cmbox_vao_2_ld.Text + "\"";
            string VAO_1_LE_VALUE = "\"" + cmbox_vao_1_le.Text + "\"";
            string VAO_2_LE_VALUE = "\"" + cmbox_vao_2_le.Text + "\"";

            workPart.Expressions.EditWithUnits(expression11, null, VAO_1_LD_VALUE);
            workPart.Expressions.EditWithUnits(expression12, null, VAO_2_LD_VALUE);
            workPart.Expressions.EditWithUnits(expression13, null, VAO_1_LE_VALUE);
            workPart.Expressions.EditWithUnits(expression14, null, VAO_2_LE_VALUE);

            theSession.Preferences.Modeling.UpdatePending = false;

            Expression expression10 = (Expression)workPart.Expressions.FindObject("VAO_TAMPA_TRAS");
            vao_port_tras = expression10.Value;
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
            MessageBox.Show("Configurações aplicadas");
        }

        public void verficar_tamanho_bag_3()
        {
            string EE = "";

            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (_chassi == "VW 17.230 OD - EURO V" || _chassi == "VW 17.260 OD - EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "VOLVO - B 270F (EURO V)")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "IVECO - 170 S 28")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }


            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V" || _chassi == "IVECO - 170 S 28")
            {
                vao_tampa_arla = "1200";
            }
            else
            {
                vao_tampa_arla = "1100";
            }

            string vao_1_ld = "0";
            string vao_2_ld = "0";
            string vao_3_ld = cmbox_vao_3_ld.Text;
            string vao_4_ld = "0";

            if (cmbox_vao_1_ld.Text == "TQ_ARLA")
            {
                vao_1_ld = vao_tampa_arla;
            }
            if (cmbox_vao_1_ld.Text == "BATERIA")
            {
                vao_1_ld = "600"; ;
            }
            if (cmbox_vao_1_ld.Text != "BATERIA" && cmbox_vao_1_ld.Text != "TQ_ARLA")
            {
                vao_1_ld = cmbox_vao_1_ld.Text;
            }

            if (cmbox_vao_2_ld.Text == "TQ_ARLA")
            {
                vao_2_ld = vao_tampa_arla;
            }
            if (cmbox_vao_2_ld.Text == "BATERIA")
            {
                vao_2_ld = "600";
            }
            if (cmbox_vao_2_ld.Text != "BATERIA" && cmbox_vao_2_ld.Text != "TQ_ARLA")
            {
                vao_2_ld = cmbox_vao_2_ld.Text;
            }

            double resto_bag = (Convert.ToDouble(EE) - 1400) - (Convert.ToDouble(vao_1_ld) + Convert.ToDouble(vao_2_ld));
            // MessageBox.Show(Convert.ToString(resto_bag));
            if (resto_bag <= Convert.ToDouble(vao_3_ld))
            {
                MessageBox.Show("Vão do bagageiro 3 LD maior que espaço disponivel");
            }
        }
        public void verficar_tamanho_bag_3_le()
        {
            string EE = "";

            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (_chassi == "VW 17.230 OD - EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "VOLVO - B 270F (EURO V)")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "IVECO - 170 S 28")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
           

            string vao_1_le = "0";
            string vao_2_le = "0";
            string vao_3_le = cmbox_vao_3_le.Text;
            string vao_4_le = "0";

           
            if (cmbox_vao_1_le.Text == "TQ_ARLA")
            {
                vao_1_le = vao_tampa_arla;
            }
            if (cmbox_vao_1_le.Text == "BATERIA")
            {
                vao_1_le = "600"; ;
            }
            if (cmbox_vao_1_le.Text != "BATERIA" && cmbox_vao_1_le.Text != "TQ_ARLA")
            {
                vao_1_le = cmbox_vao_1_le.Text;
            }

            if (cmbox_vao_2_le.Text == "TQ_ARLA")
            {
                vao_2_le = vao_tampa_arla;
            }
            if (cmbox_vao_2_le.Text == "BATERIA")
            {
                vao_2_le = "600";
            }
            if (cmbox_vao_2_le.Text != "BATERIA" && cmbox_vao_2_le.Text != "TQ_ARLA")
            {
                vao_2_le = cmbox_vao_2_le.Text;
            }

            decimal resto_bag = (Convert.ToDecimal(EE) - 1400) - (Convert.ToDecimal(vao_1_le) + Convert.ToDecimal(vao_2_le));
            // MessageBox.Show(Convert.ToString(resto_bag));
            if (resto_bag <= Convert.ToDecimal(vao_3_le))
            {
                MessageBox.Show("Vão do bagageiro 3 LE maior que espaço disponivel");
            }
        }
        public void verficar_tamanho_bag_4()
        {
            string EE = "0";

            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (_chassi == "VW 17.230 OD - EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "VOLVO - B 270F (EURO V)")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "IVECO - 170 S 28")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            
            string vao_1_ld = "0";
            string vao_2_ld = "0";
            string vao_3_ld = cmbox_vao_3_ld.Text;
            string vao_4_ld = "0";
            if (cmbox_vao_1_ld.Text == "TQ_ARLA")
            {
                vao_1_ld = vao_tampa_arla;
            }
            if (cmbox_vao_1_ld.Text == "BATERIA")
            {
                vao_1_ld = "600";
            }
            if (cmbox_vao_1_ld.Text != "BATERIA" && cmbox_vao_1_ld.Text != "TQ_ARLA")
            {
                vao_1_ld = cmbox_vao_1_ld.Text;
            }
            if (cmbox_vao_2_ld.Text == "TQ_ARLA")
            {
                vao_2_ld = vao_tampa_arla;
            }
            if (cmbox_vao_2_ld.Text == "BATERIA")
            {
                vao_2_ld = "600";
            }
            if (cmbox_vao_2_ld.Text != "BATERIA" && cmbox_vao_2_ld.Text != "TQ_ARLA")
            {
                vao_2_ld = cmbox_vao_2_ld.Text;
            }
            if (cmbox_vao_4_ld.Text != "NÃO")
            {
                vao_4_ld = cmbox_vao_4_ld.Text;
            }

            double resto_bag = (Convert.ToDouble(EE) - 1400) - (Convert.ToDouble(vao_1_ld) + Convert.ToDouble(vao_2_ld) + Convert.ToDouble(vao_3_ld));
            // MessageBox.Show(Convert.ToString(resto_bag));
            if (resto_bag <= Convert.ToDouble(vao_4_ld))
            {
                MessageBox.Show("Vão do bagageiro 4 LD maior que espaço disponivel");
            }
        }
        public void verficar_tamanho_bag_4_le()
        {
            string EE = "0";

            if (_chassi == "MBB OF-1724 EURO V" || _chassi == "MBB OF-1721 EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            if (_chassi == "VW 17.230 OD - EURO V")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "VOLVO - B 270F (EURO V)")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }
            if (_chassi == "IVECO - 170 S 28")
            {

                if (Convert.ToInt32(_ct) >= 12800)
                {
                    EE = "6300";
                }
                else
                {
                    EE = "5950";
                }
            }

            string vao_1_le = "0";
            string vao_2_le = "0";
            string vao_3_le = cmbox_vao_3_le.Text;
            string vao_4_le = "0";
            if (cmbox_vao_1_le.Text == "TQ_ARLA")
            {
                vao_1_le = vao_tampa_arla;
            }
            if (cmbox_vao_1_le.Text == "BATERIA")
            {
                vao_1_le = "600";
            }
            if (cmbox_vao_1_le.Text != "BATERIA" && cmbox_vao_1_le.Text != "TQ_ARLA")
            {
                vao_1_le = cmbox_vao_1_le.Text;
            }
            if (cmbox_vao_2_le.Text == "TQ_ARLA")
            {
                vao_2_le = vao_tampa_arla;
            }
            if (cmbox_vao_2_le.Text == "BATERIA")
            {
                vao_2_le = "600";
            }
            if (cmbox_vao_2_le.Text != "BATERIA" && cmbox_vao_2_le.Text != "TQ_ARLA")
            {
                vao_2_le = cmbox_vao_2_le.Text;
            }
            if (cmbox_vao_4_le.Text != "NÃO")
            {
                vao_4_le = cmbox_vao_4_le.Text;
            }

            decimal resto_bag = (Convert.ToDecimal(EE) - 1400) - (Convert.ToDecimal(vao_1_le) + Convert.ToDecimal(vao_2_le) + Convert.ToDecimal(vao_3_le));
            // MessageBox.Show(Convert.ToString(resto_bag));
            if (resto_bag <= Convert.ToDecimal(vao_4_le))
            {
                MessageBox.Show("Vão do bagageiro 4 LE maior que espaço disponivel");
            }
        }
        private void cmbox_vao_3_ld_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbox_vao_1_ld.Text != "" && cmbox_vao_2_ld.Text != "")
            {
                verficar_tamanho_bag_3();

            }
            if (cmbox_vao_4_ld.Text != "NÃO" && cmbox_vao_4_ld.Text != "" && cmbox_vao_2_ld.Text != "" && cmbox_vao_1_ld.Text != "")
            {
                verficar_tamanho_bag_4();
            }
        }
        private void cmbox_vao_3_le_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbox_vao_1_le.Text != "" && cmbox_vao_2_le.Text != "")
            {
                verficar_tamanho_bag_3_le();
            }
            if (cmbox_vao_4_le.Text != "NÃO" && cmbox_vao_4_le.Text != "" && cmbox_vao_2_le.Text != "" && cmbox_vao_1_le.Text != "")
            {
                verficar_tamanho_bag_4_le();
            }
        }
        private void cmbox_vao_4_ld_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbox_vao_1_ld.Text != "" && cmbox_vao_2_ld.Text != "" && cmbox_vao_3_ld.Text != "")
            {
                verficar_tamanho_bag_4();
            }
        }
        private void cmbox_vao_4_le_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbox_vao_1_le.Text != "" && cmbox_vao_2_le.Text != "" && cmbox_vao_3_le.Text != "")
            {
                verficar_tamanho_bag_4_le();
            }
        }
        public string _chassi { get; set; }
        public string _entre_eixo { get; set; }
        public string _ct { get; set; }

        private void btn_aplicar_configuracoes_Click(object sender, EventArgs e)
        {
            bagageiros();
        }

        private void frm_Configurar_Bagageiros_M4_FormClosed(object sender, FormClosedEventArgs e)
        {
            bool achar1 = false;

            foreach (Form procuraForm in Application.OpenForms)
            {
                if (procuraForm is Form)
                {
                    procuraForm.WindowState = FormWindowState.Normal;
                    procuraForm.Focus();
                    achar1 = true;
                }
            }

           
        }

        private void ckd_padrao_chassi_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        private void ckd_padrao_chassi_CheckedChanged(object sender, EventArgs e)
        {
            if (ckd_padrao_chassi.Checked == false)
            {
                cmbox_vao_1_ld.Enabled = true;
                cmbox_vao_1_le.Enabled = true;
                cmbox_vao_2_ld.Enabled = true;
                cmbox_vao_2_le.Enabled = true;
                cmbox_vao_3_ld.Enabled = true;
            }
            if (ckd_padrao_chassi.Checked == true)
            {
                padrao_tampas();
            }

        }

    }
      
    
}
