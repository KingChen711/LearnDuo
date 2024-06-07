using AutoMapper;
using System.Windows;
using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Common;
using TutorDemand.Data.Entities;
using TutorDemand.Data.Mappings;

namespace TutorDemand.WpfApp.UI;

public partial class wTeachingSchedule : Window
{
    private readonly ITeachingScheduleBusiness _teachingScheduleBusiness;
    private readonly ISubjectBusiness _subjectBusiness;
    private readonly ITutorBusiness _tutorBusiness;
    private readonly ISlotBusiness _slotBusiness;

    public wTeachingSchedule()
    {
        InitializeComponent();

        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProfilesMapper>();
        });

        _teachingScheduleBusiness ??= new TeachingScheduleBusiness();
        _subjectBusiness = new SubjectBusiness(mapper.CreateMapper());
        _tutorBusiness ??= new TutorBusiness();
        _slotBusiness ??= new SlotBusiness();

        LoadData();
    }

    private async void LoadData()
    {
        var result = await _teachingScheduleBusiness.GetAll();

        if (result.Status == Const.SUCCESS_READ_CODE && result.Data != null)
        {
            grdTeachingSchedule.ItemsSource = result.Data as List<TeachingSchedule>;
        }

        var subjectsResultTask = _subjectBusiness.GetAllAsync();
        var tutorsResultTask = _tutorBusiness.GetAllAsync();
        var slotsResultTask = _slotBusiness.GetAllAsync();

        var results = await Task.WhenAll(subjectsResultTask, tutorsResultTask, slotsResultTask);

        if (results.Any(r => r.Status != Const.SUCCESS_READ_CODE))
        {
            //Show error
        }

        CbSubject.ItemsSource = results[0].Data as List<Subject>;
        CbTutor.ItemsSource = results[1].Data as List<Tutor>;
        CbSlot.ItemsSource = results[2].Data as List<Slot>;
    }
}