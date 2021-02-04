using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace EstudoGridView
{
    public partial class EstudoGridView : Form
    {
        public EstudoGridView()
        {
            InitializeComponent();
        }

        int QtdRegistros { get; set; }
        DataSet Ds;
        SqlDataAdapter sda;
        private void button1_Click(object sender, EventArgs e)
        {
            string StrSelect = textBox1.Text;
            SqlConnection con = new SqlConnection("Data Source=localhost;Initial Catalog=New_Veletrica;User id=user;Password=pwd");
            Ds = new DataSet();
            con.Open();
            sda = new SqlDataAdapter(StrSelect, con);
            sda.Fill(Ds, "Clientes");
            button1.Text = string.Concat("Conectado na Base ", con.Database);
            con.Close();
            con.Dispose();
            sda.Dispose();
        }        

        private void ContaRegistros()
        {
            QtdRegistros = this.dataGridView1.Rows.Count;
            lblRegistros.Text = QtdRegistros.ToString();
        }


        private void btCarregar_Click(object sender, EventArgs e)
        {
            int qtd = int.Parse(txtQuantidade.Text);

            DataTable dt = new DataTable();

            foreach(DataColumn OldCol in Ds.Tables[0].Columns)
            {
                DataColumn dcNew = new DataColumn();
                dcNew.ColumnName = OldCol.ColumnName;
                dcNew.DataType = OldCol.DataType;
                dcNew.Caption = OldCol.Caption;
                dcNew.MaxLength = OldCol.MaxLength;
                dt.Columns.Add(dcNew);
            }
            
            for (int i = 0; i < qtd; i++)
            {
                DataRow item = Ds.Tables[0].Rows[i];
                dt.ImportRow(item);
            }
            
            dataGridView1.DataSource = dt;
            dt.Dispose();
            ContaRegistros();
        }

        private void btFiltro_Click(object sender, EventArgs e)
        {
            DataView Dataview = new DataView();
            Dataview.Table = Ds.Tables[0];
            if (!string.IsNullOrWhiteSpace(txtFiltro.Text))
            {                
                Dataview.RowFilter = txtFiltro.Text;
                Dataview.RowStateFilter = DataViewRowState.CurrentRows;
            }
            else
            {
                Dataview.RowFilter = "";
                Dataview.RowStateFilter = DataViewRowState.OriginalRows;
            }
            this.dataGridView1.DataSource = Dataview;
            ContaRegistros();
        }
    }
}
