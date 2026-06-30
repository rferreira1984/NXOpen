using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using NXOpen;
using NXOpen.UF;
using NXOpen.UIStyler;

namespace Custom_Mascarello
{
    public partial class frm_Especificos : Form
    {
        public frm_Especificos()
        {
            InitializeComponent();
        }

        private void frm_Especificos_Load(object sender, EventArgs e)
        {
            List<string> lista_item = new List<string>(new string[] { });
           

            string name_tpl = "";
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;
            NXOpen.Assemblies.Component cmp1 = workPart.ComponentAssembly.RootComponent;
            name_tpl = cmp1.DisplayName.Remove(cmp1.DisplayName.Length - 4, 4);
           

            DataSet dadosXML = new DataSet();
            dadosXML.ReadXml(@"X:\Xml\itens_config_tpl.xml");

            foreach (DataRow dRow in dadosXML.Tables[name_tpl].Rows)
            {
               
               lista_item.Add(dRow["item"].ToString() + "&" + dRow["valores"].ToString() +"&" + dRow["pergunta"].ToString());
            }

            int dim_y = 0;
            string[] quebra_opcoes;
            List<int> lista_tam = new List<int>(new int[] { });
            for (int i = 0; i <= lista_item.Count - 1; i++)
            {
                string[] quebra_tam = lista_item[i].Split('&');
                 quebra_opcoes = quebra_tam[1].Split(';');

                 dim_y = 50+(30*quebra_opcoes.Length);
                lista_tam.Add(dim_y);
            }
               
            int pos_y = 50;

           

           /// MessageBox.Show(dim_y.ToString());
            
            for (int i = 0; i <= lista_item.Count-1; i++)
            {
                /// MessageBox.Show(lista_item[i]);
                 string name_grupo = "";
                string pergunta = "";
                string[] quebra_item = lista_item[i].Split('&');
                string[] quebra_valor = quebra_item[1].Split(';');
                name_grupo = quebra_item[0];
                pergunta = quebra_item[2];
                System.Windows.Forms.Panel panel_1 = new System.Windows.Forms.Panel();

                panel_1.Name = quebra_item[0];
                panel_1.Text = "";
                panel_1.Size = new Size(650, lista_tam[i]);
                panel_1.BackColor = Color.LightCyan;
                panel_1.AutoScroll = true;
                panel_1.Location = new System.Drawing.Point(50, pos_y);

                this.Controls.Add(panel_1);

             
                Label lbl_fm = new Label();
                lbl_fm.Name = pergunta;
                lbl_fm.Text = pergunta;
                lbl_fm.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                lbl_fm.Size = new Size(600, 15);
                lbl_fm.Location = new System.Drawing.Point(5, 10);
                lbl_fm.Visible = true;

                panel_1.Controls.Add(lbl_fm);
                lbl_fm.BringToFront();
                int pos_y_check = 40;
                for (int j = 0; j <= quebra_valor.Length - 1; j++)
                {
                   RadioButton rdb_resposta = new RadioButton();
                    rdb_resposta.Name = quebra_valor[j];
                    rdb_resposta.Text = quebra_valor[j];
                    rdb_resposta.Size = new Size(300, 18);
                    rdb_resposta.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold | FontStyle.Italic);

                    rdb_resposta.Location = new System.Drawing.Point(5, pos_y_check);
                    rdb_resposta.Visible = true;

                    panel_1.Controls.Add(rdb_resposta);
                    rdb_resposta.BringToFront();
                    pos_y_check += 30;
                }
                pos_y += lista_tam[i]+10;
                
            }
            
            
        }

        private void btn_aplicar_configuracoes_Click(object sender, EventArgs e)
        {
            List<string> lista_resposta = new List<string>(new string[] { });

            foreach (var panel in Controls.OfType<Panel>())
            {
                string resp = "";

                resp = panel.Name;

                foreach (var rbd in panel.Controls.OfType<RadioButton>())
                {
                    if (rbd.Checked == true)
                    {
                        resp += ";" + rbd.Text;
                    }

                }
                lista_resposta.Add(resp);
            }
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            theSession.Preferences.Modeling.UpdatePending = false;

            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Expression");

            Unit unit1 = (Unit)workPart.UnitCollection.FindObject("MilliMeter");


            for (int i = 0; i <=lista_resposta.Count-1; i++)
            {
                string[] quebra_resp = lista_resposta[i].Split(';');
                try
                {
                   // MessageBox.Show(quebra_resp[0]);
                    Expression expression = (Expression)workPart.Expressions.FindObject(quebra_resp[0]);
                    string resp_valor = "\"" + quebra_resp[1] + "\"";
                    workPart.Expressions.EditWithUnits(expression, null, resp_valor);

            }
                catch (Exception)
            {


            }


        }
            theSession.Preferences.Modeling.UpdatePending = false;
            theSession.Preferences.Modeling.UpdatePending = true;


            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Update Expression Data");

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);
            theSession.DeleteUndoMark(markId3, "Update Expression Data");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

        }
    }
}
