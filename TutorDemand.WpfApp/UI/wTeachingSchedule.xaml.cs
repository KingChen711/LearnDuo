using AutoMapper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Dtos.TeachingSchedule;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;

namespace TutorDemand.WpfApp.UI;

public partial class WTeachingSchedule : Window
{
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
    private readonly ISubjectBusiness _subjectBusiness;
    private readonly ITutorBusiness _tutorBusiness;
    private readonly ISlotBusiness _slotBusiness;

    public bool IsInsertMode { get; private set; }
    public List<CheckBox> CheckBoxDays { get; private set; }

    public WTeachingSchedule()
    {
        InitializeComponent();

        var mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ProfilesMapper>(); });

        Console.WriteLine(CxbMonday.IsChecked);

        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
        _subjectBusiness = new SubjectBusiness(mapper.CreateMapper());
        _tutorBusiness ??= new TutorBusiness();
        _slotBusiness ??= new SlotBusiness();

        CheckBoxDays = [CxbMonday, CxbTuesday, CxbWednesday, CxbThursday, CxbFriday, CxbSaturday, CxbSunday];

        ResetInputs();
        ChangeMode("Insert");
        LoadData();
    }

    private void ChangeMode(string mode) //Insert, Update
    {
        switch (mode)
        {
            case "Insert":
                IsInsertMode = true;
                MutateButton.Content = "Insert";
                DeleteButton.IsEnabled = false;
                return;
            case "Update":
                IsInsertMode = false;
                MutateButton.Content = "Update";
                DeleteButton.IsEnabled = true;
                break;
        }
    }

    private void SetSelectedItem(TeachingSchedule selectedItem)
    {
        TtxTeachingScheduleId.Text = selectedItem.TeachingScheduleId.ToString();
        CbSubject.SelectedValue = selectedItem.Subject.SubjectId;
        CbSlot.SelectedValue = selectedItem.Slot.SlotId;
        CbTutor.SelectedValue = selectedItem.Tutor.TutorId;
        TtxGoogleMeetRoom.Text = selectedItem.MeetRoomCode;
        TxtPassword.Text = selectedItem.RoomPassword;
        DpStartDate.Text = selectedItem.StartDate.ToString();
        DpEndDate.Text = selectedItem.EndDate.ToString();

        var learnDays = selectedItem.LearnDays.Split(",");
        CheckBoxDays.ForEach(cbx => { cbx.IsChecked = learnDays.Contains(cbx.Content); });
    }

    private async void LoadData()
    {
        var teachingSchedulesResultTask = _teachingScheduleBusiness.GetAllAsync();
        var subjectsResultTask = _subjectBusiness.GetAllAsync();
        var tutorsResultTask = _tutorBusiness.GetAllAsync();
        var slotsResultTask = _slotBusiness.GetAllAsync();

        var results = await Task.WhenAll(teachingSchedulesResultTask, subjectsResultTask, tutorsResultTask,
            slotsResultTask);

        if (results.Any(r => r.Status != Const.SUCCESS_READ_CODE))
        {
            MessageBox.Show("An error occurred while read teaching schedules. Please try again later.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        GrdTeachingSchedule.ItemsSource = results[0].Data as List<TeachingSchedule>;
        CbSubject.ItemsSource = results[1].Data as List<Subject>;
        CbTutor.ItemsSource = results[2].Data as List<Tutor>;
        CbSlot.ItemsSource = results[3].Data as List<Slot>;
    }

    private void ResetInputs()
    {
        TtxTeachingScheduleId.Text = string.Empty;
        CbSubject.SelectedValue = Guid.Empty;
        CbSlot.SelectedValue = Guid.Empty;
        CbTutor.SelectedValue = Guid.Empty;
        TtxGoogleMeetRoom.Text = string.Empty;
        TxtPassword.Text = string.Empty;
        DpStartDate.Text = string.Empty;
        DpEndDate.Text = string.Empty;
        CheckBoxDays.ForEach(cbx => { cbx.IsChecked = false; });
    }

    private void GrdTeachingSchedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid { SelectedItem: TeachingSchedule selectedItem })
        {
            SetSelectedItem(selectedItem);
            ChangeMode("Update");
        }
    }

    private (bool success, string message, TeachingScheduleMutationDto? dto) ValidateInputs(string type)
    {
        if (type == "Update" && TtxTeachingScheduleId.Text.Equals(string.Empty))
        {
            return (false, "Something went wrong", null);
        }

        if (CbSubject.SelectedValue.Equals(Guid.Empty))
        {
            return (false, "Subject is required", null);
        }

        if (CbSlot.SelectedValue.Equals(Guid.Empty))
        {
            return (false, "Slot is required", null);
        }

        if (CbTutor.SelectedValue.Equals(Guid.Empty))
        {
            return (false, "Tutor is required", null);
        }

        if (TtxGoogleMeetRoom.Text.Equals(string.Empty))
        {
            return (false, "Google Meet Room is required", null);
        }

        if (TxtPassword.Text.Equals(string.Empty))
        {
            return (false, "Password is required", null);
        }

        if (DpStartDate.Text.Equals(string.Empty))
        {
            return (false, "Start date is required", null);
        }

        if (DpEndDate.Text.Equals(string.Empty))
        {
            return (false, "End date is required", null);
        }

        if (DpEndDate.SelectedDate <= DpStartDate.SelectedDate)
        {
            return (false, "End date must be after Start date", null);
        }

        var learnDays = string.Join(",", CheckBoxDays.Where(cbx => cbx.IsChecked == true).Select(cbx => cbx.Content));

        if (learnDays.IsNullOrEmpty())
        {
            return (false, "Learn Days is required", null);
        }

        var teachingSchedule = new TeachingScheduleMutationDto
        {
            SubjectId = (Guid)CbSubject.SelectedValue,
            TutorId = (Guid)CbTutor.SelectedValue,
            SlotId = (Guid)CbSlot.SelectedValue,
            MeetRoomCode = TtxGoogleMeetRoom.Text,
            RoomPassword = TxtPassword.Text,
            StartDate = DateOnly.Parse(DpStartDate.SelectedDate!.Value.ToShortDateString()),
            EndDate = DateOnly.Parse(DpEndDate.SelectedDate!.Value.ToShortDateString()),
            LearnDays = string.Join(",", CheckBoxDays.Where(cbx => cbx.IsChecked == true).Select(cbx => cbx.Content))
        };

        return (true, "", teachingSchedule);
    }

    private async void MutateButton_Click(object sender, RoutedEventArgs e)
    {
        var (success, message, dto) = ValidateInputs(IsInsertMode ? "Insert" : "Update");

        if (!success)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (IsInsertMode)
        {
            await _teachingScheduleBusiness.CreateAsync(dto!);
        }
        else
        {
            await _teachingScheduleBusiness.UpdateAsync(Guid.Parse(TtxTeachingScheduleId.Text), dto!);
        }

        ResetInputs();
        ChangeMode("Insert");
        LoadData(); //reload data
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var teachingScheduleId = Guid.Parse(TtxTeachingScheduleId.Text);

        var teachingScheduleResult =
            await _teachingScheduleBusiness.FindOneAsync(ts => ts.TeachingScheduleId == teachingScheduleId);

        if (teachingScheduleResult.Status != Const.SUCCESS_READ_CODE)
        {
            MessageBox.Show("Not found teaching schedules.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        var deleteTeachingScheduleResult = await _teachingScheduleBusiness.DeleteAsync(teachingScheduleId);

        if (deleteTeachingScheduleResult.Status != Const.SUCCESS_DELETE_CODE)
        {
            MessageBox.Show("An error occurred while delete schedules. Please try again later.", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        ResetInputs();
        ChangeMode("Insert");
        LoadData(); //reload data
    }

    private void ResetInputButton_Click(object sender, RoutedEventArgs e)
    {
        ResetInputs();
        ChangeMode("Insert");
    }
}