using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Excel2SqliteConverter
{
    public partial class Form1 : Form
    {
        private Excel2SqliteConverter Converter { get; } = new Excel2SqliteConverter(
            $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\features.db",
            "feature"
        );

        public Form1()
        {
            InitializeComponent();
        }

        private void excel2SqliteButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var dataTable = ExcelHandler.ImportExceltoDatatable(dialog.FileName);
                Converter.CreateAndFillDb(dataTable);
            }
        }
    }
}
