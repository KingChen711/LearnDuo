using AutoMapper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
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

        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProfilesMapper>();
        });

        Console.WriteLine(CxbMonday.IsChecked);

        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
        _subjectBusiness = new SubjectBusiness(mapper.CreateMapper());
        _tutorBusiness ??= new TutorBusiness();
        _slotBusiness ??= new SlotBusiness();

        CheckBoxDays = [CxbMonday, CxbTuesday, CxbWednesday, CxbThursday, CxbFriday, CxbSaturday, CxbSunday];

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
        CheckBoxDays.ForEach(cbx =>
        {
            cbx.IsChecked = learnDays.Contains(cbx.Content);
        });
    }

    private async void LoadData()
    {
        var teachingSchedulesResultTask = _teachingScheduleBusiness.GetAllAsync();
        var subjectsResultTask = _subjectBusiness.GetAllAsync();
        var tutorsResultTask = _tutorBusiness.GetAllAsync();
        var slotsResultTask = _slotBusiness.GetAllAsync();

        var results = await Task.WhenAll(teachingSchedulesResultTask, subjectsResultTask, tutorsResultTask, slotsResultTask);

        if (results.Any(r => r.Status != Const.SUCCESS_READ_CODE))
        {
            MessageBox.Show("An error occurred while read teaching schedules. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        CheckBoxDays.ForEach(cbx =>
        {
            cbx.IsChecked = false;
        });
    }

    private void GrdTeachingSchedule_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid { SelectedItem: TeachingSchedule selectedItem })
        {
            SetSelectedItem(selectedItem);
            ChangeMode("Update");
        }
    }

    private void MutateButton_Click(object sender, RoutedEventArgs e)
    {
        if (IsInsertMode)
        {
            //insert
            return;
        }

        //update
    }

    private async void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var teachingScheduleId = Guid.Parse(TtxTeachingScheduleId.Text);

        var teachingScheduleResult = await _teachingScheduleBusiness.FindOneAsync(ts => ts.TeachingScheduleId == teachingScheduleId);

        if (teachingScheduleResult.Status != Const.SUCCESS_READ_CODE)
        {
            MessageBox.Show("Not found teaching schedules.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        var deleteTeachingScheduleResult = await _teachingScheduleBusiness.DeleteAsync(teachingScheduleId);

        if (deleteTeachingScheduleResult.Status != Const.SUCCESS_DELETE_CODE)
        {
            MessageBox.Show("An error occurred while delete schedules. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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