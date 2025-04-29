using _2025CleanArchitecture.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace _2025CleanArchitecture.Application.Employees
{
    public sealed record EmployeeCreateCommand(
        string FirstName,
        string LastName,
        decimal Salary,
        DateOnly BirthOfDate,
        PersonalInformation PersonalInformation,
        Address? Address
        ) : IRequest<Result<string>>;

    public sealed class EmployeeCreateCommanadValidator : AbstractValidator<EmployeeCreateCommand>
    {
        public EmployeeCreateCommanadValidator()
        {
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("İsim alanı en az 3 karakter olmalıdır.");
            RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyisim alanı en az 3 karakter olmalıdır.");
            RuleFor(x => x.PersonalInformation.TCNo)
                .MinimumLength(11).WithMessage("Geçerli bir TCKN giriniz.")
                .MaximumLength(11).WithMessage("Geçerli bir TCKN giriniz.");
        }
    }

    internal sealed class EmployeeCreateCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
        {
            var isEmployeeExist = await employeeRepository.AnyAsync(p => p.PersonalInformation.TCNo == request.PersonalInformation.TCNo, cancellationToken);
            if (isEmployeeExist)
            {
                return Result<string>.Failure("Bu TC ile daha önce kayıt oluşturulmuş");
            }

            Employee employee = request.Adapt<Employee>();

            employeeRepository.Add(employee);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Personel Kaydı Tamamlandı";
        }
    }
}
