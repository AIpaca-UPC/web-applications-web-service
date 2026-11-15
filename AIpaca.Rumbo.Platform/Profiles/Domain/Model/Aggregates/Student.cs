using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Commands;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.ValueObjects;
using AIpaca.Rumbo.Platform.Profiles.Domain.Model.Entities;
using AIpaca.Rumbo.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace AIpaca.Rumbo.Platform.Profiles.Domain.Model.Aggregates;

public class Student : AggregateRoot
{
    public Student()
    {
        Name = new PersonName();
    }
    
    public Student(string firstName, string lastName, DateOnly birthdate, string schoolName)
    {
        Name = new PersonName(firstName, lastName);
        BirthDate = birthdate;
        SchoolName = schoolName;
        Status = EStudentStatus.Active;
    }
    
    public Student(RegisterStudentCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        BirthDate = command.BirthDate;
        SchoolName = command.SchoolName;
        Status = EStudentStatus.Active;
    }

    public StudentId Id { get; private set; }
    
    public PersonName Name { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string SchoolName { get; private set; }
    public EStudentStatus Status { get; private set; }
    
    public string FullName => Name.FullName;
}