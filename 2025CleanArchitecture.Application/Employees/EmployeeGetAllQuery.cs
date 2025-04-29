using _2025CleanArchitecture.Domain.Abstractions;
using _2025CleanArchitecture.Domain.Employees;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025CleanArchitecture.Application.Employees
{
    public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

    public sealed class EmployeeGetAllQueryResponse : EntityDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly BirtOfDate { get; set; }
        public decimal Salary { get; set; }
        public string TCNo { get; set; } = default!;
    }

    internal sealed class EmployeeGetAllQueryHandler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
    {
        public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
        {
            var response = employeeRepository.GetAll()
                .Select(s => new EmployeeGetAllQueryResponse
                {
                    BirtOfDate = s.BirtOfDate,
                    CreatedDate = s.CreatedDate,
                    DeletedDate = s.DeletedDate,
                    FirstName = s.FirstName,
                    Id = s.Id,
                    IsDeleted = s.IsDeleted,
                    LastName = s.LastName,
                    Salary = s.Salary,
                    TCNo = s.PersonalInformation.TCNo,
                    UpdatedDate = s.UpdatedDate,
                }).AsQueryable();

            return Task.FromResult(response);
        }
    }
}
