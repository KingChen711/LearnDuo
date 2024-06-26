﻿using AutoMapper;
using System.CodeDom;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;
using TutorDemand.WpfApp.UI;

namespace TutorDemand.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _customerId;
        private readonly CustomerBusiness _customerBusiness;
        public MainWindow()
        {
            _customerBusiness = new CustomerBusiness();
            InitializeComponent();
        }

        private void Open_wSubject_Click(object sender, RoutedEventArgs e)
        {
            var p = new wSubject();
            p.Owner = this;
            p.Show();
        }

        private void Open_wTutor_Click(object sender, RoutedEventArgs e)
        {
            var p = new wTutor();
            p.Owner = this;
            p.Show();
        }
        private async void Open_wReservation_Click(object sender, RoutedEventArgs e)
        {
            var customerDatas = await _customerBusiness.GetAllAsync();
            var listData = (List<Customer>)customerDatas.Data!;
            var currentCustomer = listData.First();
            var p = new wReservation(currentCustomer.CustomerId.ToString());
            p.Owner = this;
            p.Show();
        }

        private void Open_wTeachingSchedule_Click(object sender, RoutedEventArgs e)
        {
            var p = new WTeachingSchedule();
            p.Owner = this;
            p.Show();
        }
    }
}