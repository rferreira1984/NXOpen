using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rotinas_Startup
{
    public partial class Form1 : Form
    {
        private List<string> _sheets;

       // string _shettName = "";
       // List<string> _shettName ;
        private List<string> _shettName = new List<string>();

        public List<string> ShettName
        {
            get { return _shettName; }
            set { _shettName = value; }
        }
        public Form1(List<string> shetts)
        {
            InitializeComponent();
            _sheets = shetts;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Label label1 = new Label();
            label1.Text = "Selecione uma folha para Excluir";
            label1.Font = new Font("Arial", 12, FontStyle.Bold);
            label1.AutoSize = true;
           // label1.Location = new Point(10, 10);
            label1.Dock = DockStyle.Top;

            this.Controls.Add(label1);
            ListView listView = new ListView();
            listView.View = View.Details;
            listView.Columns.Add("Sheets", -2, HorizontalAlignment.Left);

            foreach (var sheet in _sheets)
            {
                ListViewItem item = new ListViewItem(sheet);
                listView.Items.Add(item);

            }
            listView.DoubleClick += listView_DoubleClick;
            listView.Dock = DockStyle.Fill;
            this.Controls.Add(listView);
            label1.SendToBack();
        }

        //criar um metodo para duplo click e seleção do item no listView
        private void listView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control v in Controls)
            {
                if (v is ListView)
                {
                    ListView v1 = v as ListView;

                    foreach (ListViewItem item in v1.Items)
                    {
                        if (item.Selected)
                        {
                            string selectedSheet = item.Text;
                            ShettName.Add(selectedSheet);

                        }
                    }
                }

            }
            this.Close(); // fecha o formulário após a seleção
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShettName.Clear();
            this.Close(); // fecha o formulário após cancelar a seleção

        }
        // preciso retornar o nome da sheet selecionada para o modulo principal, para isso vou criar um evento e um delegate
        // public delegate void SheetSelectedEventHandler(object sender, SheetSelectedEventArgs e);
        //public event SheetSelectedEventHandler SheetSelected;

    }
}
