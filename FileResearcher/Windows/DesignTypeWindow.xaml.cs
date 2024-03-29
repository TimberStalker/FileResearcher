﻿using FileReading.ReadingData.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileResearcher.Windows
{
    public partial class DesignTypeWindow : Window
    {
        DataType dataType;

        public DesignTypeWindow(DataType dataType)
        {
            this.dataType = dataType;


            InitializeComponent();
            //DataTypeName.Text = dataType.Name;

            BaseTypesView.Drop += DataTypeView_Drop;


            DataTypeView.MouseMove += DataTypesView_MouseMove;

        }

        private void DataTypeView_Drop(object sender, DragEventArgs e)
        {
            e.Data.GetData(typeof(DataType));
        }

        private void DataTypesView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && BaseTypesView.SelectedItem is not null)
                DragDrop.DoDragDrop(BaseTypesView, BaseTypesView.SelectedItem, DragDropEffects.Copy);
        }
    }
}
