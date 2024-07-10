using AutoMapper;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Business.Base;
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

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubjectCode.Text)
                || string.IsNullOrEmpty(txtName.Text)
                || string.IsNullOrEmpty(txtDuration.Text)
                || string.IsNullOrEmpty(dpStartDate.Text)
                || string.IsNullOrEmpty(dpEndDate.Text))
            {
                MessageBox.Show("Fields: Code, Name, Duration, StartDate, EndDate is required", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtDuration.Text.ToString(), out var validDuration))
            {
                MessageBox.Show("Duration must be digits", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtCostPrice.Text.ToString(), out var validCost))
            {
                MessageBox.Show("Cost price must be digits", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(dpEndDate.SelectedDate < dpStartDate.SelectedDate)
            {
                MessageBox.Show("End date must exceed than Start date", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var subjectDto = new SubjectDto()
            {
                SubjectCode = txtSubjectCode.Text.ToString(),
                Name = txtName.Text.ToString(),
                Description = txtDescription.Text.ToString(),
                Duration = decimal.Parse(txtDuration.Text.ToString()),
                CostPrice = decimal.Parse(txtCostPrice.Text.ToString()),
                StartDate = dpStartDate.SelectedDate,
                EndDate = dpEndDate.SelectedDate,
            };


            // Check if is update or add new 
            if (string.IsNullOrEmpty(txtSubjectId.Text)) // Process add new subject
            {
                // Check exist subject code 
                var subjectResult = await _subjectBusiness.GetWithConditionAysnc(x =>
                    x.SubjectCode.Equals(subjectDto.SubjectCode));

                if (subjectResult.Status == Const.SUCCESS_READ_CODE)
                {
                    var subjects = subjectResult.Data as List<Subject>;
                    MessageBox.Show($"Subject code {subjects!.First()!.SubjectCode} already exist", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                EmptyTextAndLoadSubjects();
                await _subjectBusiness.CreateAsync(subjectDto);

                MessageBox.Show("New subject successfully", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else // Process update subject
            {
                subjectDto.SubjectId = Guid.Parse(txtSubjectId.Text.ToString());
                var updateResult = await _subjectBusiness.UpdateAsync(subjectDto);

                if (updateResult.Status == Const.SUCCESS_UPDATE_CODE)
                {
                    EmptyTextAndLoadSubjects();

                    MessageBox.Show($"Update subject successfully", "Alert",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }
                MessageBox.Show("Something went wrong");
            }

            LoadGrdSubjects();
        }

        private void EmptyTextAndLoadSubjects()
        {
            txtDescription.Text = string.Empty;
            txtSubjectCode.Text = string.Empty;
            txtDuration.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSubjectId.Text = string.Empty;
            txtCostPrice.Text = string.Empty;
            dpStartDate.Text = string.Empty;
            dpEndDate.Text = string.Empty;

            LoadGrdSubjects();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) 
        {
            EmptyTextAndLoadSubjects();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e) 
        {
            EmptyTextAndLoadSubjects();
        }

        private async void GrdSubject_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;

            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator
                    .ContainerFromItem(grd.SelectedItem) as DataGridRow;

                if (row != null)
                {
                    var item = row.Item as Subject;

                    if (item != null)
                    {
                        var subjectResult = await _subjectBusiness.GetByIdAsync(item.SubjectId);

                        if (subjectResult.Status == Const.SUCCESS_READ_CODE && subjectResult.Data != null)
                        {
                            item = subjectResult.Data as Subject;
                            txtSubjectCode.Text = item.SubjectCode;
                            txtName.Text = item.Name;
                            txtDescription.Text = item.Description;
                            txtDuration.Text = item.Duration.ToString();
                            txtSubjectId.Text = item.SubjectId.ToString();
                            txtCostPrice.Text = item.CostPrice.ToString();
                            dpStartDate.SelectedDate = item.StartDate;
                            dpEndDate.SelectedDate = item.EndDate;
                        }
                    }
                }
            }
        }

        private async void GrdSubject_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button is null)
            {
                MessageBox.Show("Something went wrong");
                return;
            }

            var subjectId = button.CommandParameter.ToString();

            if (subjectId is null)
            {
                MessageBox.Show("Not found relevant subject to delete");
                return;
            }

            // Get subject by id 
            var subjectResult = await _subjectBusiness.GetWithConditionAysnc(x =>
                x.SubjectId.ToString().Equals(subjectId));
            if (subjectResult.Status == Const.SUCCESS_READ_CODE)
            {
                var subjects = subjectResult.Data as List<Subject>;

                // Process confirm pop up
                var messageBoxResult = MessageBox.Show(
                    $"Do to want to delete subject {subjects.First().Name}",
                    "Alert", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // Process delete subject 
                    var deleteResult = await _subjectBusiness.DeleteAsync(Guid.Parse(subjectId!));
                    if (deleteResult.Status == Const.SUCCESS_DELETE_CODE) // Delete success
                    {
                        MessageBox.Show(deleteResult.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                        EmptyTextAndLoadSubjects();
                    }
                    else // Cause error
                    {
                        MessageBox.Show(deleteResult.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void LoadGrdSubjects()
        {
            var result = await _subjectBusiness.GetAllAsync();
            if (result.Status == Const.SUCCESS_READ_CODE && result.Data != null)
            {
                this.grdSubject.ItemsSource = result.Data as List<Subject>;
            }
            else
            {
                this.grdSubject.ItemsSource = new List<Subject>();
            }
        }

        private async void OnSearchValueChanged(object sender, RoutedEventArgs e)
        {
            var searchTerm = txtSearch.Text.ToString();

            IBusinessResult result = null!;
            if(decimal.TryParse(searchTerm, out var decimalNum))
            {
                result = await _subjectBusiness.GetWithConditionAysnc(x =>
                    x.CostPrice == decimalNum
                 || x.Duration == decimalNum);

                if(result.Status == Const.SUCCESS_READ_CODE)
                {
                    grdSubject.ItemsSource = result.Data as List<Subject>;
                }
                return;
            }

            result = await _subjectBusiness.GetWithConditionAysnc(x => 
                x.SubjectCode.Contains(searchTerm)
             || x.Name.Contains(searchTerm)
             || x.Description.Contains(searchTerm)
             || x.StartDate.HasValue && x.StartDate.Value.ToString().Contains(searchTerm)
             || x.EndDate.HasValue && x.EndDate.Value.ToString().Contains(searchTerm));

            if (result.Status == Const.SUCCESS_READ_CODE)
            {
                grdSubject.ItemsSource = result.Data as List<Subject>;
            }
        }

        public async void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var searchTerm = txtSearch.Text.ToString();

                IBusinessResult result = null!;
                if (decimal.TryParse(searchTerm, out var decimalNum))
                {
                    result = await _subjectBusiness.GetWithConditionAysnc(x =>
                        x.CostPrice == decimalNum
                     || x.Duration == decimalNum);

                    if (result.Status == Const.SUCCESS_READ_CODE)
                    {
                        grdSubject.ItemsSource = result.Data as List<Subject>;
                    }
                    return;
                }

                if (DateTime.TryParse(searchTerm, out var dateTerm))
                {
                    result = await _subjectBusiness.GetWithConditionAysnc(x =>
                        x.StartDate.HasValue && x.EndDate.HasValue &&
                        dateTerm >= x.StartDate.Value && dateTerm <= x.EndDate.Value
                    );

                    if (result.Status == Const.SUCCESS_READ_CODE)
                    {
                        grdSubject.ItemsSource = result.Data as List<Subject>;
                    }

                    return;
                }

                result = await _subjectBusiness.GetWithConditionAysnc(x =>
                    x.SubjectCode.Contains(searchTerm)
                 || x.Name.Contains(searchTerm)
                 || x.Description.Contains(searchTerm)
                 || x.StartDate.HasValue && x.StartDate.Value.ToString().Contains(searchTerm)
                 || x.EndDate.HasValue && x.EndDate.Value.ToString().Contains(searchTerm));

                if (result.Status == Const.SUCCESS_READ_CODE)
                {
                    grdSubject.ItemsSource = result.Data as List<Subject>;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
