﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_WinForms_FlintstonesViewer
{
    public partial class ListForm : Form
    {
        private List<Character> _characters;

        public ListForm()
        {
            InitializeComponent();
        }

        private void ReadXmlFileAndBindToDataGrid()
        {
            string dataFilePath = AppConfig.dataFilePath;

            //
            // read data file
            //
            IDataService dataService = new XmlDataService(dataFilePath);
            _characters = dataService.ReadAll();

            //
            // bind list to DataGridView control
            //
            var bindingList = new BindingList<Character>(_characters);
            var source = new BindingSource(bindingList, null);
            dataGridView_Characters.DataSource = source;

            //
            // configure DataGridView control
            //
            this.dataGridView_Characters.Columns["Id"].Visible = false;
            this.dataGridView_Characters.Columns["ImageFileName"].Visible = false;
            this.dataGridView_Characters.Columns["Description"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ReadXmlFileAndBindToDataGrid();
            }
            catch (FileNotFoundException)
            {

                MessageBox.Show("Unable to locate data file.\nExiting the application.");
                this.Close();
            }

        }

        private void btn_CheckList_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_Characters.SelectedRows)
            {
                dataGridView_Characters.Rows.RemoveAt(row.Index);
            }
        }

        private void btn_DetailView_Click(object sender, EventArgs e)
        {
            if (dataGridView_Characters.SelectedRows.Count == 1)
            {
                Character character = new Character();
                character = (Character)dataGridView_Characters.CurrentRow.DataBoundItem;

                DetailForm detailForm = new DetailForm(character);
                detailForm.ShowDialog();
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            try
            {
                IDataService XmlDataService = new XmlDataService(AppConfig.dataFilePath);
                XmlDataService.WriteAll(_characters);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save current data.\nExiting the application.");
                throw;
            }
            this.Close();

        }

        private void btn_DemoSortFilterSearch_Click(object sender, EventArgs e)
        {
            //
            // Note: These examples represent a quick way to "refresh" the DataGridView.
            //       Using an ObservableCollection and implementing the INotifyPropertyChanged interface
            //       is a stronger pattern.
            //

            //
            // sort list by age and reset the DataSource
            //
            var sortedList = _characters.OrderBy(c => c.Age).ToList();
            dataGridView_Characters.DataSource = sortedList;

            //
            // filter list by gender and reset the DataSource
            //
            //var filteredList = _characters.Where(c => c.Gender == Character.GenderType.Male).ToList();
            //dataGridView_Characters.DataSource = filteredList;

            //
            // search list and reset the DataSource
            // note: searchTerm and LastName are converted to upper case for comparison
            //
            //string searchTerm = "Rubble";
            //var searchedList = _characters.Where(c => c.LastName.ToUpper().Contains(searchTerm.ToUpper())).ToList();
            //dataGridView_Characters.DataSource = searchedList;
        }
    }
}
