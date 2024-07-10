using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Reservation;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;
using Exception = System.Exception;

namespace TutorDemand.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wReservation.xaml
    /// </summary>
    public partial class wReservation : Window
    {
        
        private readonly IReservationBusiness _reservationService;
        private readonly ITutorBusiness _tutorBusiness;
        private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
        private readonly ISubjectBusiness _subjectBusiness;
        private readonly IMapper _mapper;
        private readonly ICustomerBusiness _customerBusiness;
        private readonly string _currentCustomerId;
        public wReservation(string customerId)
        {
            InitializeComponent();
            _customerBusiness = new CustomerBusiness();
            _reservationService = new ReservationBusiness();
            _tutorBusiness = new TutorBusiness();
            _subjectBusiness = new SubjectBusiness();
            _teachingScheduleBusiness = new TeachingScheduleBusiness();
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ProfilesMapper>(); }).CreateMapper();
            _currentCustomerId = customerId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadReservation();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check combobox
                if ((cboPaymentMethod.SelectedItem as ComboBoxItem)?.Tag.ToString() == "default")
                {
                    MessageBox.Show("Please select payment method.");
                    return;
                }

                if ((cboPaymentStatus.SelectedItem as ComboBoxItem)?.Tag.ToString() == "default")
                {
                    MessageBox.Show("Please select payment status.");
                    return;
                }

                if ((cboReservationStatus.SelectedItem as ComboBoxItem)?.Tag.ToString() == "default")
                {
                    MessageBox.Show("Please select reservation status.");
                    return;
                }

                if (!isPrice(txtPrice.Text))
                {
                    MessageBox.Show("Please insert a number");
                    return;
                }

                ReservationDetails createReq = new ReservationDetails()
                {
                    PaidPrice = int.Parse(txtPrice.Text),
                    ReservationId = Guid.NewGuid(),
                    SubjectName = txtSubjectName.Text,
                    TutorName = txtTutorName.Text,
                    PaymentMethod = (cboPaymentMethod.SelectedItem as ComboBoxItem)?.Tag.ToString()!,
                    PaymentStatus = (cboPaymentStatus.SelectedItem as ComboBoxItem)?.Tag.ToString()!,
                    ReservationStatus = (cboReservationStatus.SelectedItem as ComboBoxItem)?.Tag.ToString()!
                };
                // var currentCustomerDatas = await _customerBusiness.GetAllAsync();
                // var listCustomer = (List<Customer>)currentCustomerDatas.Data!;
                // var currentCustomer = listCustomer.First()
                
                var reservation = _mapper.Map<Reservation>(createReq);
                var subject = await _subjectBusiness.GetWithConditionAysnc(s => s.Name.Equals(txtSubjectName.Text));
                if (subject.Data is null)
                {
                    MessageBox.Show("Something went wrong with the subject!");
                    return;
                }
                var subjectData = (List<Subject>)subject.Data!;
                var findingSubject = subjectData.First();

                var tutor = await _tutorBusiness.FindOneAsync(t => t.Fullname.Equals(txtTutorName.Text));

                if (tutor.Data is null)
                {
                    MessageBox.Show("Something went wrong with the tutor!");
                    return;
                }

                var findingTutor = (Tutor)tutor.Data!;
                var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(x =>
                    x.SubjectId.Equals(findingSubject.SubjectId)
                    && x.TutorId.Equals(findingTutor.TutorId));

                if (teachingSchedule.Data is null)
                {
                    MessageBox.Show("This teacher does not have the schedule for this subject");
                    return;
                }
        
                var scheduleData = (TeachingSchedule)teachingSchedule.Data!;
                reservation.TeachingScheduleId = scheduleData.TeachingScheduleId;
                reservation.CustomerId = Guid.Parse(_currentCustomerId);
                await _reservationService.CreateAsync(reservation);
                LoadReservation();
                // Notify the user about the successful creation
                MessageBox.Show("Reservation created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dgData.SelectedIndex);
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;

                string id = ((TextBlock)RowColumn.Content).Text;

                IBusinessResult reservationResult =
                    await _reservationService.FindOneAsync(r => r.ReservationId.Equals(Guid.Parse(id)));

                var reservation = (Reservation)reservationResult.Data!;

                var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(ts =>
                    ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                var teachingScheduleData = (TeachingSchedule)teachingSchedule.Data!;
                var tutor = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(teachingScheduleData.TutorId));
                var subject = await _subjectBusiness.GetByIdAsync(((TeachingSchedule)teachingSchedule.Data!).SubjectId);
                var result = _mapper.Map<ReservationDetails>(reservation);
                result.TutorName = ((Tutor)tutor.Data!).Fullname;
                result.SubjectName = ((Subject)subject.Data!).Name;

                txtId.Text = reservation.ReservationId.ToString();
                txtPrice.Text = reservation.PaidPrice.ToString();
                txtSubjectName.Text = result.SubjectName;
                txtTutorName.Text = result.TutorName;

                foreach (ComboBoxItem item in cboPaymentMethod.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == reservation.PaymentMethod)
                    {
                        cboPaymentMethod.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cboPaymentStatus.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == reservation.PaymentStatus)
                    {
                        cboPaymentStatus.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cboReservationStatus.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == reservation.ReservationStatus)
                    {
                        cboReservationStatus.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (txtId.Text.Length > 0)
                {
                    ReservationDetails req = new ReservationDetails()
                    {
                        PaidPrice = int.Parse(txtPrice.Text),
                        TutorName = txtTutorName.Text,
                        SubjectName = txtSubjectName.Text,
                        ReservationId = Guid.Parse(txtId.Text),
                        PaymentMethod = (cboPaymentMethod.SelectedItem as ComboBoxItem)?.Tag.ToString()!,
                        PaymentStatus = (cboPaymentStatus.SelectedItem as ComboBoxItem)?.Tag.ToString()!,
                        ReservationStatus = (cboReservationStatus.SelectedItem as ComboBoxItem)?.Tag.ToString()!
                    };

                    var reservation = _mapper.Map<Reservation>(req);
                    var subject = await _subjectBusiness.GetWithConditionAysnc(s => s.Name.Equals(txtSubjectName.Text));
                    if (subject.Data is null)
                    {
                        MessageBox.Show("Something went wrong with the subject!");
                        return;
                    }

                    var subjectData = (List<Subject>)subject.Data!;
                    var findingSubject = subjectData.First();
                    

                    var tutor = await _tutorBusiness.FindOneAsync(t => t.Fullname.Equals(txtTutorName.Text));

                    if (tutor.Data is null)
                    {
                        MessageBox.Show("Something went wrong with the tutor!");
                        return;
                    }

                    var findingTutor = (Tutor)tutor.Data!;

                    var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(x =>
                        x.SubjectId.Equals(findingSubject.SubjectId)
                                           && x.TutorId.Equals(findingTutor.TutorId));

                    var scheduleData = (TeachingSchedule)teachingSchedule.Data!;
                    reservation.TeachingScheduleId = scheduleData.TeachingScheduleId;
                    await _reservationService.UpdateAsync(reservation);
                    LoadReservation();
                    // Notify the user about the successful update
                    MessageBox.Show("Reservation updated successfully.");
                }
                else
                {
                    MessageBox.Show("You must select a Reservation!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtId.Text.Length > 0)
                {
                    // Show confirmation dialog
                    var result = MessageBox.Show("Are you sure you want to delete this reservation?", "Confirm Delete",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _reservationService.DeleteAsync(Guid.Parse(txtId.Text));
                        LoadReservation();
                        MessageBox.Show("Tutor deleted successfully.");
                    }
                }
                else
                {
                    MessageBox.Show("You must select a Reservation!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetInput();
        }


        private void resetInput()
        {

            txtSubjectName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtId.Text = string.Empty;
            txtTutorName.Text = string.Empty;
            cboPaymentMethod.SelectedItem = 0;
            cboPaymentStatus.SelectedItem = 0;
            cboReservationStatus.SelectedItem = 0;
        }

        private async void LoadReservation()
        {
            try
            {
                var reservations = await _reservationService.GetAllAsync();
                var reservationDatas = (List<Reservation>)reservations.Data!;
                reservationDatas.OrderBy(x => x.PaidPrice < 1);
                var resultData = new List<ReservationDetails>();

                foreach (var reservation in reservationDatas)
                {
                    var teachingSchedule = await _teachingScheduleBusiness.FindOneAsync(ts =>
                        ts.TeachingScheduleId.Equals(reservation.TeachingScheduleId));
                    var teachingScheduleData = (TeachingSchedule)teachingSchedule.Data!;
                    var tutor = await _tutorBusiness.FindOneAsync(x => x.TutorId.Equals(teachingScheduleData.TutorId));
                    var subject =
                        await _subjectBusiness.GetByIdAsync(((TeachingSchedule)teachingSchedule.Data!).SubjectId);

                    var result = _mapper.Map<ReservationDetails>(reservation);
                    result.TutorName = ((Tutor)tutor.Data!).Fullname;
                    result.SubjectName = ((Subject)subject.Data!).Name;
                    resultData.Add(result);
                }

                this.dgData.ItemsSource = resultData;
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(ex.Message);*/
            }
            finally
            {
                resetInput();
            }
        }

        public bool isPrice(string price)
        {
            var regex = new Regex("^[0-9]");
            return regex.IsMatch(price);
        }
    }
}