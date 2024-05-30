using TutorDemand.Business;
using TutorDemand.Data.Entities;

var subjectBusiness = new SubjectBusiness();


// Create new subject 

subjectBusiness.Create(new Subject
{
    SubjectId = Guid.Parse("8FCF5F48-1467-457A-8ECB-868861FA4C6A"),
    Name = "C#",
    SubjectCode = "SQ123D"
});
subjectBusiness.Create(new Subject
{
    SubjectId = Guid.Parse("79DC8C83-A5A6-4DF3-A46F-6626C37D1870"),
    Name = "Java",
    SubjectCode = "J123D"
});
subjectBusiness.Create(new Subject
{
    SubjectId = Guid.Parse("B2DAECA4-B090-4F12-8C79-CE6F09EFB1B0"),
    Name = "Unity",
    SubjectCode = "UN123S"
});


// Get all subject
var getAllResult = subjectBusiness.GetAll();
var result = (List<Subject>)getAllResult.Data;

Console.WriteLine($"{getAllResult.Message} - Total data: {result.Count}");


// Remove subject
var getByIdResult = subjectBusiness.GetById(1);
var subject = (Subject)getByIdResult.Data;
var deleteResult = subjectBusiness.Delete(subject.Id);
Console.WriteLine($"{deleteResult.Message}");

// Update subject
getByIdResult = subjectBusiness.GetById(3);
subject = (Subject)getByIdResult.Data;

// Update properties
subject.Name = "HEHE";

var updateResult = subjectBusiness.Update(subject);

Console.WriteLine($"{updateResult.Message}");
Console.WriteLine($"Current total data: {result.Count}");