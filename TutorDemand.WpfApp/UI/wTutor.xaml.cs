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
using TutorDemand.Business.Base;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.Tutor;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;

namespace TutorDemand.WpfApp.UI
{
    /// <summary>
    /// Interaction logic for wTutor.xaml
    /// </summary>
    public partial class wTutor : Window
    {
        private readonly ITutorBusiness tutorBusiness;

        public wTutor()
        {
            InitializeComponent();
            // AutoMapper
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProfilesMapper>();
            });
            tutorBusiness = new TutorBusiness(mapper.CreateMapper());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTutors();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if gender is default
                if ((cboGender.SelectedItem as ComboBoxItem)?.Tag.ToString() == "default")
                {
                    MessageBox.Show("Please select a gender.");
                    return;
                }

                // Validate email format
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }

                // Validate phone number format and length
                if (!IsValidPhoneNumber(txtPhone.Text))
                {
                    MessageBox.Show("Invalid phone number format. Phone number must contain exactly 10 digits.");
                    return;
                }

                // Creating a new TutorDto object and populating it with values from the form
                TutorDto tutor = new TutorDto
                {
                    Fullname = txtFullName.Text,
                    Email = txtEmail.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text,
                    Gender = (cboGender.SelectedItem as ComboBoxItem)?.Tag.ToString(),
                    TutorId = Guid.NewGuid() // Generating a new GUID for the TutorId
                };

                // Call the create method
                await tutorBusiness.CreateAsync(tutor);

                // Notify the user about the successful creation
                MessageBox.Show("Tutor created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadTutors();
            }
        }


        private async void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dgData.SelectedIndex);

                DataGridCell RowColumn = dataGrid.Columns[1].GetCellContent(row).Parent as DataGridCell;

                string id = ((TextBlock)RowColumn.Content).Text;

                IBusinessResult businessResult = await tutorBusiness.FindOneAsync(x => x.TutorId.Equals(Guid.Parse(id)));

                var tutor = businessResult.Data as Tutor;
                if (tutor == null)
                    return;

                txtId.Text = tutor.TutorId.ToString();
                txtFullName.Text = tutor.Fullname.ToString();
                txtEmail.Text = tutor.Email.ToString();
                txtAddress.Text = tutor?.Address?.ToString();
                txtPhone.Text = tutor?.Phone.ToString();
                foreach (ComboBoxItem item in cboGender.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == tutor.Gender)
                    {
                        cboGender.SelectedItem = item;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
               /* MessageBox.Show(ex.Message);*/
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
                    // Creating a new TutorDto object and populating it with values from the form
                    TutorDto tutor = new TutorDto
                    {
                        Fullname = txtFullName.Text,
                        Email = txtEmail.Text,
                        Address = txtAddress.Text,
                        Phone = txtPhone.Text,
                        Gender = (cboGender.SelectedItem as ComboBoxItem)?.Tag.ToString(),
                        TutorId = Guid.Parse(txtId.Text)
                    };

                    // Call the update method
                    await tutorBusiness.UpdateAsync(tutor);

                    // Notify the user about the successful update
                    MessageBox.Show("Tutor updated successfully.");
                }
                else
                {
                    MessageBox.Show("You must select a tutor!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadTutors();
            }
        }


        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtId.Text.Length > 0)
                {
                    // Show confirmation dialog
                    var result = MessageBox.Show("Are you sure you want to delete this tutor?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Proceed with deletion if user confirms
                        await tutorBusiness.DeleteAsync(Guid.Parse(txtId.Text));
                        MessageBox.Show("Tutor deleted successfully.");
                    }
                }
                else
                {
                    MessageBox.Show("You must select a tutor!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadTutors();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            resetInput();
        }


        private void resetInput()
        {
            txtId.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            cboGender.SelectedIndex = 0;
        }

        private async void LoadTutors()
        {
            try
            {
                var result = await tutorBusiness.GetAllAsync();

                if (result.Status == Const.SUCCESS_READ_CODE && result.Data != null)
                {
                    this.dgData.ItemsSource = result.Data as List<Tutor>;
                }
                else
                {
                    this.dgData.ItemsSource = new List<Tutor>();
                }
            }
            catch(Exception ex)
            {
                /*MessageBox.Show(ex.Message);*/
            }
            finally
            {
                resetInput();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Validate phone number format and length (10 digits)
        private bool IsValidPhoneNumber(string phone)
        {
            return phone.All(char.IsDigit) && phone.Length == 10;
        }
    }
}
