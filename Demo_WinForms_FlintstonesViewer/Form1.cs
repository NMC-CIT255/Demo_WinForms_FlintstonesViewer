﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_WinForms_FlintstonesViewer
{
    public partial class Form1 : Form
    {
        private List<Character> _charecters;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitalizeAndBindListOfPeople()
        {
            //_charecters = new List<Character>()
            //{
            //    new Character()
            //    {
            //        Id = 1,
            //        LastName = "Velis",
            //        FirstName = "John",
            //        Age = 60
            //    },
            //                    new Character()
            //    {
            //        Id = 2,
            //        LastName = "Velis",
            //        FirstName = "Jeff",
            //        Age = 55
            //    },
            //};

            string datafile = @"Data\FlintstoneCharacters.xml";

            IDataService dataService = new XmlDataService(datafile);
            _charecters = dataService.ReadAll();

            var bindingList = new BindingList<Character>(_charecters);
            var source = new BindingSource(bindingList, null);
            dataGridView_Characters.DataSource = source;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitalizeAndBindListOfPeople();
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
                Character person = new Character();
                person = (Character)dataGridView_Characters.CurrentRow.DataBoundItem;

                DetailForm detailForm = new DetailForm(person);
                detailForm.Show();
            }
        }
    }
}
