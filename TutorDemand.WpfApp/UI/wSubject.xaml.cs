using AutoMapper;
using System;
using System.Collections.Generic;
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
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Subject;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;

namespace TutorDemand.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wSubject.xaml
    /// </summary>
    public partial class wSubject : Window
    {
        private readonly ISubjectBusiness _subjectBusiness;

        public wSubject()
        {
            InitializeComponent();

            // AutoMapper
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProfilesMapper>();
            });

            // Subject business
            _subjectBusiness = new SubjectBusiness(mapper.CreateMapper());

            // Load all subject
            this.LoadGrdSubjects();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) { }

        private void GrdSubject_MouseDouble_Click(object sender, RoutedEventArgs e) 
        {
        }

        private void GrdSubject_ButtonDelete_ClicK(object sender, RoutedEventArgs e) { }

        private async void LoadGrdSubjects() 
        {
            var result = await _subjectBusiness.GetAllAsync();

            if(result.Status == Const.SUCCESS_READ_CODE && result.Data != null)
            {
                this.grdSubject.ItemsSource = result.Data as List<Subject>;
            }
            else
            {
                this.grdSubject.ItemsSource = new List<Subject>();
            }

        }

    }
}
